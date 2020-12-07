using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SchoolWeb.DataAccess.Repository;
using SchoolWeb.Models;
using SchoolWeb.Models.ViewModels;
using SchoolWeb.Utility;

namespace SchoolWeb.Areas.TeacherPortal.Controllers
{
    [Area("TeacherPortal")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        private string getCurrentTeacherId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claims.Value;

        }

        public IActionResult Index()
        {
        
        var userId = getCurrentTeacherId();
        
        var Teacher = _unitOfWork.Teacher.GetFirstOrDefault(t => t.Id == userId);
            

            return View(Teacher);
        }

        public IActionResult ClassSchedule()
        {
            
            var userId = getCurrentTeacherId();
        
            var Classes = _unitOfWork
                            .Class
                            .GetAll(c => c.TeacherId == userId, includeProperities: "Course,Section");

            return View(Classes);
        }

        public IActionResult StudentMarks()
        {

            var userId = getCurrentTeacherId();

            var teacher = _unitOfWork.Teacher
                            .GetFirstOrDefault(t => t.Id == userId,
                                    includeProperities: "CourseTeachers,CourseTeachers.Course");

            // create courses list
            List<SelectListItem> courses = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "--المادة--", Value="none", Selected = true}
            };

            IDictionary<string, string> Filtering = new Dictionary<string, string>();

            foreach (var course in teacher.CourseTeachers)
            {
                string courseName = course.Course.Name;
                try
                {
                    Filtering.Add(courseName, courseName);
                    courses.Add(new SelectListItem { Text = courseName, Value = course.CourseId.ToString() });
                }
                catch (Exception)
                {

                    continue;
                }
    
            }



            // create Semesters list
            List<SelectListItem> Semesters = new List<SelectListItem>() {
                        new SelectListItem{ Text = "--الفصل الدراسي--", Value="none"
                             , Selected = true}
                    };
            Semesters.AddRange(StaticData.SemestersList);

            // create Sections list
            List<SelectListItem> Sections = new List<SelectListItem>()
            {
                new SelectListItem { Text = "-- الشعبة --", Value = "none", Selected = true }
            };

            // create Grades list
            List<SelectListItem> Grades = new List<SelectListItem>()
            {
               new SelectListItem { Text = "-- الصف --", Value = "none", Selected = true }
            };

            MarksOfStudentsDDListsVM lists = new MarksOfStudentsDDListsVM()
            {
                CoursesList= courses,
                SemestersList= Semesters,
                SectionsList= Sections,
                GradesList= Grades
            };



            return View(lists);
        }


        [HttpGet]
        public IActionResult EditMark(string id)
        {
            var mark = _unitOfWork
                        .Mark
                        .GetFirstOrDefault(m => m.Id.ToString() == id,
                                            includeProperities: "Student,Course,Course.CourseTeachers");
            if (mark == null)
            {
                // there is no mark with this id
               
            }

            var CurrentTeacherId = getCurrentTeacherId();
            var CurrentTeacher = mark
                                .Course
                                .CourseTeachers
                                .FirstOrDefault(ct => ct.TeacherId == CurrentTeacherId);

            if (CurrentTeacher == null)
            {
                // user not autherize
            }

            EditMarkVM editableMark = new EditMarkVM()
            {
                Id =mark.Id ,
                FirstMark = mark.FirstMark,
                SecondMark = mark.SecondMark,
                AssignmentsMark = mark.AssignmentsMark,
                FinalMark=mark.FinalMark,
                StudentName = mark.Student.ArabicName
            };

            return View(editableMark);
        }


        [HttpPost]
        public IActionResult EditMark(EditMarkVM mark)
        {
            if (ModelState.IsValid)
            {
                var MarkItem = _unitOfWork
                                .Mark
                                .GetFirstOrDefault(m => m.Id == mark.Id);


                    MarkItem.Id = mark.Id;
                    MarkItem.FirstMark = mark.FirstMark;
                    MarkItem.SecondMark = mark.SecondMark;
                    MarkItem.AssignmentsMark = mark.AssignmentsMark;
                    MarkItem.FinalMark = mark.FinalMark;
               

                _unitOfWork.Mark.Update(MarkItem);
                _unitOfWork.Save();

                return RedirectToAction(nameof(StudentMarks));
            }

            return View(mark);
        }


        #region API

        public IActionResult PopulateMarksTable(string courseId, string grade, string sectionId, string semester)
        {

            if (grade == "none" || courseId == "none" || sectionId == "none" || semester == "none")
                return Json(new { });

            var course = _unitOfWork.Course
                            .GetFirstOrDefault(c => c.Id.ToString() == courseId);

            var marks = _unitOfWork.Mark.GetAll(
                    m => m.Course.Name == course.Name
                    && m.Course.Grade == grade
                    && m.Course.Semester.ToString() == semester
                    && m.Student.SectionId.ToString() == sectionId
                    , includeProperities: "Course,Student");


            return Json(new { data = marks });

        }



        public IActionResult PopulateGradesList(string courseId)
        {
            // create Grades list
            List<SelectListItem> Grades = new List<SelectListItem>()
            {
               new SelectListItem { Text = "-- الصف --", Value = "none", Selected = true }
            };

            var course = _unitOfWork.Course
                            .GetFirstOrDefault(c => c.Id.ToString() == courseId);

            if (course == null)
            {
                return PartialView("~/Areas/Public/Views/Partials/MarksOfStudents/_GradesDropDownList.cshtml"
                , Grades);
            }

            var courses = _unitOfWork.Course
                            .GetAll(c => c.Name == course.Name);

            


            foreach (var item in courses)
            {
                var grade = item.Grade;
                Grades.Add(new SelectListItem { Text = StaticFunctions.GetGrade(grade), Value = grade });
                

            }

            return PartialView("~/Areas/Public/Views/Partials/MarksOfStudents/_GradesDropDownList.cshtml"
                , Grades);
            
        }


        public IActionResult PopulateSectionsList(string grade,string courseId)
        {
            // create Grades list
            List<SelectListItem> SectionsList = new List<SelectListItem>()
            {
               new SelectListItem { Text = "-- الشعبة --", Value = "none", Selected = true }
            };

            // getting course name
            var course = _unitOfWork.Course
                            .GetFirstOrDefault(c => c.Id.ToString() == courseId);
            if (course == null)
            {
                return PartialView("~/Areas/Public/Views/Partials/MarksOfStudents/_SectionsDropDownList.cshtml"
                , SectionsList);
            }

            var courseName = course.Name;

            // getting teacher Id
            var teacherId = getCurrentTeacherId();

            // getting calsses
            var classes = _unitOfWork.Class
                            .GetAll(c => c.Course.Name == courseName
                                    && c.TeacherId == teacherId
                                    && c.Section.Grade==grade,
                            includeProperities: "Section,Course");




            //if there is no sections met the conditions return defalut List
            if (classes.Count() == 0)
            {
                return PartialView("~/Areas/Public/Views/Partials/MarksOfStudents/_SectionsDropDownList.cshtml"
                , SectionsList);
            }

            IDictionary<string, string> Filtering = new Dictionary<string, string>();

            foreach (var item in classes)
            {
                try
                {
                    var id = item.Section.Id.ToString();
                    var name = item.Section.Name;
                    Filtering.Add(name, item.Section.Id.ToString());
                    SectionsList.Add(new SelectListItem { Text = name, Value = id });


                }
                catch (Exception)
                {

                    continue;
                }

                

            }

            return PartialView("~/Areas/Public/Views/Partials/MarksOfStudents/_SectionsDropDownList.cshtml"
                , SectionsList);

        }
       
        
        
     
        
        #endregion


    }
}
