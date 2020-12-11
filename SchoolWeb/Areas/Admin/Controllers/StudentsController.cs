using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SchoolWeb.Data;
using SchoolWeb.DataAccess.Repository;
using SchoolWeb.Models;
using SchoolWeb.Models.ViewModels;
using SchoolWeb.Utility;

namespace SchoolWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StudentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostEnvironment;


        public StudentsController(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult StudentsInfo()
        {

            var studentsInfoVM = new AdminStudentsInfoVM()
            {
                Grades = StaticData.SelectedGradesList,
            };

            return View(studentsInfoVM);
        }

        //populate sections when grades value changed or when use modal in registers requests
        [HttpPost]
        public IActionResult PopulateSections(string grade, string operation)

        {
            var sections = new List<SelectListItem>();

            if (grade != null && operation == null)
            {
                sections.Add(new SelectListItem { Text = "جميع الشعب", Value = "All" });
            }

            sections.AddRange(_unitOfWork.Section.GetAll()
                .Where(s => s.Grade == grade)
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),

                }));


            if (operation == "Upsert" || operation == "AcceptRequest")
            {
                ViewBag.sectionsDropDownWidth = "100%";

            }

            if (operation == "AcceptRequest")
            {
                ViewBag.acceptRequest = true;
            }

            return PartialView("~/Areas/Public/Views/Partials/AdminStudents/_SectionsDropDownList.cshtml"
                , sections);

        }


        public IActionResult UpsertStudent(string id)
        {
            var adminUpsertStudentVM = new AdminUpsertStudentVM();

            if (!String.IsNullOrEmpty(id))
            {
                //edit student

                var student = _unitOfWork.Student.GetFirstOrDefault(
                    s => s.Id == id, includeProperities: "StudentFee");

                adminUpsertStudentVM.Student = student;

                adminUpsertStudentVM.Discount = student.StudentFee.Discount;

                adminUpsertStudentVM.BusFees = student.StudentFee.BusFees;

                adminUpsertStudentVM.Sections = _unitOfWork.Section.GetAll()
                    .Where(s => s.Grade == adminUpsertStudentVM.Student.Grade)
                    .Select(s => new SelectListItem
                    {
                        Text = s.Name,
                        Value = s.Id.ToString(),
                        Selected = s.Id == adminUpsertStudentVM.Student.SectionId
                    });

                adminUpsertStudentVM.SelectedSection =
                    (int)adminUpsertStudentVM.Student.SectionId;

            }
            else
            {
                adminUpsertStudentVM.Student = new Student();
            }

            adminUpsertStudentVM.Grades = StaticData.GradesList;

            adminUpsertStudentVM.BussStates = StaticData.BussStatesList;


            return View(adminUpsertStudentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpsertStudent(AdminUpsertStudentVM adminUpsertStudentVM)
        {
            if (!String.IsNullOrEmpty(adminUpsertStudentVM.Student.Id))
            {
                ModelState["Password"].ValidationState = ModelValidationState.Valid;
                ModelState["Photo"].ValidationState = ModelValidationState.Valid;

            }
            if (ModelState.IsValid)
            {
                //get born certificate image
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                var imageUrl = "";

                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\students");
                    var extention = Path.GetExtension(files[0].FileName);

                    if (!String.IsNullOrEmpty(adminUpsertStudentVM.Student.Id))
                    {
                        //edit and image changed, we need to remove old image

                        var imagePath = Path.Combine(webRootPath, adminUpsertStudentVM.Student
                            .BornCertificateImage.TrimStart('\\'));

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    imageUrl = @"\images\students\" + fileName + extention;
                }
                else
                {
                    // edit but image not changed
                    imageUrl = adminUpsertStudentVM.Student.BornCertificateImage;
                }

                if (!String.IsNullOrEmpty(adminUpsertStudentVM.Student.Id))
                {
                    //edit student

                    var student = _unitOfWork.Student.GetFirstOrDefault
                        (s => s.Id == adminUpsertStudentVM.Student.Id,
                        includeProperities: "StudentFee,Section");

                    var studentFee = student.StudentFee;

                    var paiedMonths = _unitOfWork.MonthlyPayment
                        .GetAll().Where(m => m.StudentFeeId == studentFee.Id
                        && m.IsPaied == true).Count();

                    if (adminUpsertStudentVM.Student.Grade != student.Grade
                        && paiedMonths > 0)
                    {
                        // if grade change after paied

                        ModelState.AddModelError(string.Empty,
                            "لا يمكنك تغيير الصف بعد مرور شهر او اكثر من الدوام");

                        adminUpsertStudentVM.Grades = StaticData.GradesList;

                        adminUpsertStudentVM.BussStates = StaticData.BussStatesList;

                        adminUpsertStudentVM.Sections = _unitOfWork.Section.GetAll()
                            .Where(s => s.Grade == adminUpsertStudentVM.Student.Grade)
                            .Select(s => new SelectListItem
                            {
                                Text = s.Name,
                                Value = s.Id.ToString(),
                                Selected = s.Id == adminUpsertStudentVM.SelectedSection
                            });


                        return View(adminUpsertStudentVM);
                    }

                    // update student marks if section changed

                    if (student.SectionId != adminUpsertStudentVM.SelectedSection)
                    {
                        //get all student marks

                        var StudentMarks = _unitOfWork
                                        .Mark
                                        .GetAll(m => m.StudentId == student.Id
                                                && m.FinalMark == 0
                                                , includeProperities: "Course");



                        // get new section 
                        var newSection = _unitOfWork.Section
                         .GetFirstOrDefault(s => s.Id == adminUpsertStudentVM.SelectedSection,
                         includeProperities: "Classes,Classes.Course");

                        var classes = newSection.Classes.GroupBy(c => c.CourseId)
                            .Select(c => c.First()).ToList();

                        classes = classes.FindAll(c => c.Course != null);


                        foreach (var m in StudentMarks)
                        {
                            if (m.Course == null)
                                continue;

                            var classItem = classes.FirstOrDefault(c => c.Course.Name == m.Course.Name);


                            if (classItem == null)
                            {

                                _unitOfWork.Mark.Remove(m);
                            }
                            else
                            {
                                m.CourseId = classItem.CourseId;
                                _unitOfWork.Mark.Update(m);
                                classes.Remove(classItem);
                            }


                        }

                        foreach (var c in classes)
                        {
                            Mark mark = new Mark()
                            {
                                FirstMark = 0,
                                SecondMark = 0,
                                AssignmentsMark = 0,
                                FinalMark = 0,
                                CourseId = c.Course.Id,
                                StudentId = student.Id
                            };

                            _unitOfWork.Mark.Add(mark);
                        }




                    }



                    // Update Student Info
                    student.Address = adminUpsertStudentVM.Student.Address;
                    student.ArabicName = adminUpsertStudentVM.Student.ArabicName;
                    student.EnglishName = adminUpsertStudentVM.Student.EnglishName;
                    student.BornCertificateImage = imageUrl;
                    student.BussState = adminUpsertStudentVM.Student.BussState;
                    student.Grade = adminUpsertStudentVM.Student.Grade;
                    student.SectionId = adminUpsertStudentVM.SelectedSection;
                    student.ParentPhoneNumber = adminUpsertStudentVM.Student.ParentPhoneNumber;

                    _unitOfWork.Student.Update(student);



                    if (adminUpsertStudentVM.BusFees != studentFee.BusFees
                        || adminUpsertStudentVM.Discount != studentFee.Discount)
                    {
                        //discount or buss fees changed

                        //remaining Monthly Payments
                        var monthlyPayments = _unitOfWork.MonthlyPayment.GetAll()
                            .Where(m => m.IsPaied == false
                            && m.StudentFeeId == studentFee.Id);

                        // number of remaining Months
                        var remainingMonths = monthlyPayments.Count();

                        // All months (thats represnt StaticData.NumberOfMonths .. but if changed later
                        // we need to consider old value

                        var numberOfMonths = paiedMonths + remainingMonths;

                        if (adminUpsertStudentVM.BusFees != studentFee.BusFees
                            && adminUpsertStudentVM.Discount != studentFee.Discount)
                        {
                            //discount and buss fees changed

                            var newBussFees = adminUpsertStudentVM.BusFees;

                            var schoolFees = _unitOfWork.SchoolFee.GetFirstOrDefault
                                (s => s.Grade == adminUpsertStudentVM.Student.Grade).SchoolFees;

                            var newSchoolFees = schoolFees - (schoolFees * adminUpsertStudentVM.Discount / 100);

                            foreach (var monthlyPayment in monthlyPayments)
                            {
                                monthlyPayment.BusFeesAmount =
                                    newBussFees / numberOfMonths;

                                monthlyPayment.SchoolFeesAmount = newSchoolFees / numberOfMonths;


                                _unitOfWork.MonthlyPayment.Update(monthlyPayment);
                            }

                            studentFee.BusFees = adminUpsertStudentVM.BusFees;
                            studentFee.Discount = adminUpsertStudentVM.Discount;

                        }
                        else if (adminUpsertStudentVM.BusFees != studentFee.BusFees)
                        {
                            //buss fees changed

                            var newBussFees = adminUpsertStudentVM.BusFees;

                            foreach (var monthlyPayment in monthlyPayments)
                            {
                                monthlyPayment.BusFeesAmount =
                                    newBussFees / numberOfMonths;


                                _unitOfWork.MonthlyPayment.Update(monthlyPayment);
                            }

                            studentFee.BusFees = adminUpsertStudentVM.BusFees;

                        }
                        else if (adminUpsertStudentVM.Discount != studentFee.Discount)
                        {
                            //discount changed

                            var schoolFees = _unitOfWork.SchoolFee.GetFirstOrDefault
                                (s => s.Grade == adminUpsertStudentVM.Student.Grade).SchoolFees;

                            double newSchoolFees = schoolFees - (schoolFees * adminUpsertStudentVM.Discount / 100);

                            foreach (var monthlyPayment in monthlyPayments)
                            {
                                monthlyPayment.SchoolFeesAmount = newSchoolFees / numberOfMonths;


                                _unitOfWork.MonthlyPayment.Update(monthlyPayment);
                            }

                            studentFee.Discount = adminUpsertStudentVM.Discount;

                        }


                        _unitOfWork.StudentFee.Update(studentFee);


                    }

                    _unitOfWork.Save();

                    return RedirectToAction(nameof(StudentsInfo));

                }

                //create new student

                //create asp.net identity user
                var identityUser = new IdentityUser
                {
                    Email = adminUpsertStudentVM.Student.ParentEmail,
                    UserName = adminUpsertStudentVM.Student.ParentEmail,
                    PhoneNumber = adminUpsertStudentVM.Student.ParentPhoneNumber,
                };

                var result = await _userManager.CreateAsync(identityUser,
                             adminUpsertStudentVM.Password);


                if (result.Succeeded)
                {

                    // add roles to database if not exists (just for development phase)

                    if (!await _roleManager.RoleExistsAsync(StaticData.Role_Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(
                              StaticData.Role_Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticData.Role_Student))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(
                              StaticData.Role_Student));
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticData.Role_Waiting))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(
                              StaticData.Role_Waiting));
                    }

                    if (!await _roleManager.RoleExistsAsync(StaticData.Role_Teacher))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(
                              StaticData.Role_Teacher));
                    }

                    // assign user a student role
                    await _userManager.AddToRoleAsync(identityUser, StaticData.Role_Student);


                    //create student user
                    var student = new Student()
                    {
                        Id = identityUser.Id,
                        ArabicName = adminUpsertStudentVM.Student.ArabicName,
                        EnglishName = adminUpsertStudentVM.Student.EnglishName,
                        Address = adminUpsertStudentVM.Student.Address,
                        ParentEmail = adminUpsertStudentVM.Student.ParentEmail,
                        ParentPhoneNumber = adminUpsertStudentVM.Student.ParentPhoneNumber,
                        BornCertificateImage = imageUrl,
                        BussState = adminUpsertStudentVM.Student.BussState,
                        Grade = adminUpsertStudentVM.Student.Grade,
                        SectionId = adminUpsertStudentVM.SelectedSection,
                    };

                    // create student fee
                    var studentFee = new StudentFee()
                    {
                        Discount = adminUpsertStudentVM.Discount,
                        BusFees = adminUpsertStudentVM.BusFees,
                    };

                    _unitOfWork.Student.Add(student);

                    _unitOfWork.StudentFee.Add(studentFee);

                    _unitOfWork.Save();

                    student.StudentFeeId = studentFee.Id;

                    _unitOfWork.Student.Update(student);


                    //get school fees for selected grade
                    var schoolFee = _unitOfWork.SchoolFee
                        .GetFirstOrDefault(s => s.Grade == student.Grade).SchoolFees;

                    // calculate discount if exists
                    var discountedSchoolFee = schoolFee -
                        (schoolFee * studentFee.Discount / 100);



                    // create monthlyPayments for user

                    //StaticData.startMonth - 1, we substract 1  beacause we will add months in loop in first iteration
                    var currentDate = new DateTime(DateTime.Now.Year, StaticData.startMonth - 1, 1);

                    //StaticData.NumberOfMonths + 1 , we add one because we will skip one month (holiday)
                    for (int month = 1; month <= StaticData.NumberOfMonths + 1; month++)
                    {
                        //skip holiday month after create half of months
                        if (month == StaticData.NumberOfMonths / 2 + 1) continue;

                        var date = currentDate.AddMonths(month);

                        var monthlyPayment = new MonthlyPayment()
                        {
                            StudentFeeId = studentFee.Id,
                            IsPaied = false,
                            BusFeesAmount = studentFee.BusFees / StaticData.NumberOfMonths,
                            Month = month > StaticData.NumberOfMonths / 2 + 1 ? month - 1 : month,
                            Date = date,
                            SchoolFeesAmount = discountedSchoolFee / StaticData.NumberOfMonths

                        };

                        _unitOfWork.MonthlyPayment.Add(monthlyPayment);
                    }


                    var section = _unitOfWork.Section
                        .GetFirstOrDefault(s => s.Id == student.SectionId,
                        includeProperities: "Classes");

                    var classes = section.Classes.GroupBy(c => c.CourseId)
                        .Select(c => c.First()).ToList();

                    foreach (var claSs in classes)
                    {
                        var mark = new Mark()
                        {
                            FirstMark = 0,
                            SecondMark = 0,
                            AssignmentsMark = 0,
                            FinalMark = 0,
                            StudentId = student.Id,
                            CourseId = claSs.CourseId
                        };

                        _unitOfWork.Mark.Add(mark);
                    }


                    _unitOfWork.Save();

                    return RedirectToAction(nameof(StudentsInfo));

                }
                else
                {
                    // handle validation if errors occurs when add identity user 
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "PasswordRequiresNonAlphanumeric")
                        {
                            ModelState.AddModelError(string.Empty,
                                "كلمة المرور يجب ان تحتوي على رمز واحد على الأقل");

                        }

                        if (error.Code == "PasswordRequiresUpper")
                        {
                            ModelState.AddModelError(string.Empty,
                                "كلمة المرور يجب ان تحتوي على حرف كبير واحد على الأقل");
                        }

                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError(string.Empty,
                                "البريد الاكتروني مسجل من قبل , يرجى استخدام بريد إلكتروني آخر");
                        }
                    }
                }

            }

            //if model state is not valid we need to populate lists
            adminUpsertStudentVM.Grades = StaticData.GradesList;

            adminUpsertStudentVM.BussStates = StaticData.BussStatesList;

            adminUpsertStudentVM.Sections = _unitOfWork.Section.GetAll()
                .Where(s => s.Grade == adminUpsertStudentVM.Student.Grade)
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),
                });


            return View(adminUpsertStudentVM);


        }



        public IActionResult StudentDetails(string id, string from)
        {
            var adminStudentDetailsVM = new AdminStudentDetailsVM();

            var student = new Student();

            if (from == "Requests")
            {
                student = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == id);
            }
            else
            {

                student = _unitOfWork.Student
                    .GetFirstOrDefault(s => s.Id == id
                    , includeProperities: "StudentFee,StudentFee.MonthlyPayments,Section,Section.Classes");

                var monthlyPayments = student.StudentFee.MonthlyPayments.OrderBy(m => m.Month);


                var classes = student.Section.Classes
                    .Where(c => c.CourseId != null)
                    .GroupBy(c => c.CourseId)
                    .Select(c => c.First()).ToList();

                adminStudentDetailsVM.Semesters = StaticData.SelectedSemestersList;

                adminStudentDetailsVM.Marks = _unitOfWork.Mark.GetAll(includeProperities: "Course")
                    .Where(m => m.StudentId == id
                    && m.CourseId == getCourseIdFromClass((int)m.CourseId, classes));

                adminStudentDetailsVM.MonthlyPayments = monthlyPayments;
            }


            adminStudentDetailsVM.Student = student;



            return View(adminStudentDetailsVM);
        }

        //populate marks table when semester dropdownlist value changed
        [HttpPost]
        public IActionResult PopulateMarksTable(string semester, string studentId)

        {
            var student = _unitOfWork.Student
                .GetFirstOrDefault(s => s.Id == studentId
                , includeProperities: "Section,Section.Classes");


            var classes = student.Section.Classes.GroupBy(c => c.CourseId)
                .Select(c => c.First()).ToList();

            var marks = Enumerable.Empty<Mark>();


            if (semester == "All")
            {
                marks = _unitOfWork.Mark.GetAll(includeProperities: "Course")
                .Where(m => m.StudentId == studentId
                && m.CourseId == getCourseIdFromClass((int)m.CourseId, classes));
            }
            else
            {
                marks = _unitOfWork.Mark.GetAll(includeProperities: "Course")
                    .Where(m => m.StudentId == studentId
                    && m.CourseId == getCourseIdFromClass((int)m.CourseId, classes)
                    && m.Course.Semester.ToString() == semester);
            }


            return PartialView("~/Areas/Public/Views/Partials/AdminStudents/_StudentMarksTable.cshtml"
                , marks);

        }

        public IActionResult RegisterRequests()
        {

            var registerRequestsVM = new RegisterRequestsVM()
            {

                Grades = StaticData.GradesList,

                Sections = _unitOfWork.Section.GetAll()
            };


            return View(registerRequestsVM);
        }

        //populate grades when modal opened
        [HttpPost]
        public IActionResult PopulateGrades(string studentId)

        {
            var studentGrade = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == studentId).Grade;


            var grades = StaticData.GradesList.Select(s => new SelectListItem
            {
                Text = s.Text,
                Value = s.Value,
                Selected = s.Value == studentGrade
            });


            return PartialView("~/Areas/Public/Views/Partials/AdminStudents/_GradesDropDownList.cshtml"
                , grades);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptStudent(RegisterRequestsVM registerRequestsVM)
        {
            if (ModelState.IsValid)
            {

                var student = _unitOfWork.Student
                    .GetFirstOrDefault(s => s.Id == registerRequestsVM.StudentId);

                // create student fee
                var studentFee = new StudentFee()
                {
                    Discount = registerRequestsVM.Discount,
                    BusFees = registerRequestsVM.BusFees,
                };

                _unitOfWork.StudentFee.Add(studentFee);

                _unitOfWork.Save();

                student.StudentFeeId = studentFee.Id;
                student.SectionId = registerRequestsVM.SelectedSection;
                student.Grade = registerRequestsVM.SelectedGrade;

                _unitOfWork.Student.Update(student);


                //get school fees for selected grade
                var schoolFee = _unitOfWork.SchoolFee
                    .GetFirstOrDefault(s => s.Grade == student.Grade).SchoolFees;

                // calculate discount if exists
                var discountedSchoolFee = schoolFee -
                    (schoolFee * studentFee.Discount / 100);



                // create monthlyPayments for user

                //StaticData.startMonth - 1, we substract 1  beacause we will add months in loop in first iteration
                var currentDate = new DateTime(DateTime.Now.Year, StaticData.startMonth - 1, 1);

                //StaticData.NumberOfMonths + 1 , we add one because we will skip one month (holiday)
                for (int month = 1; month <= StaticData.NumberOfMonths + 1; month++)
                {
                    //skip holiday month after create half of months
                    if (month == StaticData.NumberOfMonths / 2 + 1) continue;

                    var date = currentDate.AddMonths(month);

                    var monthlyPayment = new MonthlyPayment()
                    {
                        StudentFeeId = studentFee.Id,
                        IsPaied = false,
                        BusFeesAmount = studentFee.BusFees / StaticData.NumberOfMonths,
                        Month = month > StaticData.NumberOfMonths / 2 + 1 ? month - 1 : month,
                        Date = date,
                        SchoolFeesAmount = discountedSchoolFee / StaticData.NumberOfMonths

                    };

                    _unitOfWork.MonthlyPayment.Add(monthlyPayment);
                }

                var section = _unitOfWork.Section
                    .GetFirstOrDefault(s => s.Id == student.SectionId,
                    includeProperities: "Classes");

                var classes = section.Classes.GroupBy(c => c.CourseId)
                    .Select(c => c.First()).ToList();

                foreach (var claSs in classes)
                {
                    var mark = new Mark()
                    {
                        FirstMark = 0,
                        SecondMark = 0,
                        AssignmentsMark = 0,
                        FinalMark = 0,
                        StudentId = student.Id,
                        CourseId = claSs.CourseId
                    };

                    _unitOfWork.Mark.Add(mark);
                }


                _unitOfWork.Save();

                //change role from waiting to student
                var identityUser = await _userManager.FindByIdAsync(student.Id);
                await _userManager.RemoveFromRoleAsync(identityUser, StaticData.Role_Waiting);
                await _userManager.AddToRoleAsync(identityUser, StaticData.Role_Student);

                return RedirectToAction(nameof(RegisterRequests));
            }

            return Json(new { success = true, message = "تم قبول الطالب بنجاح" });

        }


        //get finanical claims for all students
        public IActionResult FinanicalClaims()
        {

            return View(StaticData.SelectedGradesList);

        }

        // get finanical claims details for specific student
        public IActionResult FinanicalDetails(string studentId)
        {
            var student = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == studentId,
                includeProperities: "StudentFee,StudentFee.MonthlyPayments");

            return View(student.StudentFee.MonthlyPayments);
        }


        #region API_CALLS

        [HttpGet]
        public async Task<IActionResult> GetStudents(string grade, string section)
        {
            var identityUsers = await _userManager.GetUsersInRoleAsync(StaticData.Role_Student);


            //initialize empty students
            var students = Enumerable.Empty<Student>();

            if ((String.IsNullOrEmpty(grade) && String.IsNullOrEmpty(section)) || grade == "All")
            {
                students = _unitOfWork.Student.GetAll().Where(
                    s => s.Id == getIdentityId(s.Id, identityUsers));
            }

            else if (section == "All")
            {
                students = _unitOfWork.Student.GetAll()
                    .Where(s => s.Grade == grade
                    && s.Id == getIdentityId(s.Id, identityUsers));
            }
            else
            {
                students = _unitOfWork.Student.GetAll()
                    .Where(s => s.Grade == grade
                    && s.SectionId.ToString() == section
                    && s.Id == getIdentityId(s.Id, identityUsers));
            }


            return Json(new { data = students });
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var identityUsers = await _userManager.GetUsersInRoleAsync(StaticData.Role_Waiting);

            var students = _unitOfWork.Student.GetAll(includeProperities: "Section")
                    .Where(s => s.Id == getIdentityId(s.Id, identityUsers));

            return Json(new { data = students });
        }


        [HttpGet]
        public async Task<IActionResult> GetFinanicalClaimStudents(string grade)
        {
            var identityUsers = await _userManager.GetUsersInRoleAsync(StaticData.Role_Student);

            var students = _unitOfWork.Student.GetAll(
                includeProperities: "Section,StudentFee,StudentFee.MonthlyPayments")
                .Where(s => s.Id == getIdentityId(s.Id, identityUsers));

            if (!String.IsNullOrEmpty(grade) && grade != "All")
            {
                students = students.Where(s => s.Grade == grade);
            }

            var currentDate = DateTime.Now;

            var claimsStudents = new List<FinancialClaimStudent>();

            foreach (var student in students)
            {
                var price = 0.0;

                foreach (var monthlyPayment in student.StudentFee.MonthlyPayments)
                {
                    var date = monthlyPayment.Date;

                    if (currentDate >= date && monthlyPayment.IsPaied == false)
                    {
                        price += monthlyPayment.BusFeesAmount + monthlyPayment.SchoolFeesAmount;
                    }


                }

                if ((int)price != 0)
                {
                    claimsStudents.Add(new FinancialClaimStudent()
                    {
                        Student = student,
                        TotalFees = price,
                    });

                }

            }


            return Json(new { data = claimsStudents });
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            var student = _unitOfWork.Student
                .GetFirstOrDefault(s => s.Id == id, includeProperities: "StudentFee");

            if (student == null)
            {
                return Json(new { success = false, message = "حدث خطأ في عملية الحذف" });
            }

            //remove student from identity user
            var identityUser = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(identityUser);

            //remove student fee
            _unitOfWork.StudentFee.Remove(student.StudentFee);

            //remove student from students table
            _unitOfWork.Student.Remove(student);

            _unitOfWork.Save();

            return Json(new { success = true, message = "تم حذف الطالب بنجاح" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRequest(string id)
        {
            var student = _unitOfWork.Student.GetFirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return Json(new { success = false, message = "حدث خطأ في عملية الحذف" });
            }

            //remove student from identity user
            var identityUser = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(identityUser);

            //remove student from students table
            _unitOfWork.Student.Remove(student);

            _unitOfWork.Save();

            return Json(new { success = true, message = "تم حذف طلب التسجيل بنجاح" });
        }


        #endregion

        //helpers functions
        private string getIdentityId(string studentId, IList<IdentityUser> identityUsers)
        {
            var identityUser = identityUsers.FirstOrDefault(i => i.Id == studentId);

            if (identityUser != null)
            {
                return identityUser.Id;
            }

            return "";
        }


        private int getCourseIdFromClass(int courseId, IEnumerable<Class> classes)
        {
            var claSs = classes.FirstOrDefault(c => c.CourseId == courseId);

            if (claSs != null)
            {
                return (int)claSs.CourseId;
            }

            return -1;
        }

    }


}
