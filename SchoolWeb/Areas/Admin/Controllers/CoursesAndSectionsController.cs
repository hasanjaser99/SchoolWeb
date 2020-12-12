using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolWeb.Data;
using SchoolWeb.DataAccess.Repository;
using SchoolWeb.Models;
using SchoolWeb.Models.ViewModels;
using SchoolWeb.Utility;

namespace SchoolWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]
    public class CoursesAndSectionsController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CoursesAndSectionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /////////// Courses /////////////////

        //Get courses
        public IActionResult Courses()
        {
            var coursesVM = new CoursesIndexVM()
            {
                Courses = _unitOfWork.Course
                        .GetAll(includeProperities: "CourseTeachers,CourseTeachers.Teacher"),

                Grades = StaticData.SelectedGradesList
            };


            return View(coursesVM);
        }

        //populate table when dropdownlist value changed
        [HttpPost]
        public IActionResult PopulateCoursesTable(string grade)

        {
            //initialize empty courses
            var courses = Enumerable.Empty<Course>();

            if (grade == "All")
            {
                courses = _unitOfWork.Course
                    .GetAll(includeProperities: "CourseTeachers,CourseTeachers.Teacher");
            }
            else
            {
                courses = _unitOfWork.Course
                    .GetAll(c => c.Grade == grade,
                    includeProperities: "CourseTeachers,CourseTeachers.Teacher");
            }


            return PartialView("~/Areas/Public/Views/Partials/CoursesAndSections/_CoursesTable.cshtml"
                , courses);

        }

        // Get Add/Edit Course Page
        public IActionResult UpsertCourse(int id)
        {
            //initialize view model
            var upsertCourseVM = new UpsertCourseVM();

            //initialize dropdown teachers with all teachers in db
            upsertCourseVM.Teachers = _unitOfWork.Teacher.GetAll()
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.Id,

                });

            if (id != 0)
            {
                //Edit Course

                //initialize course
                upsertCourseVM.Course = _unitOfWork.Course
                    .GetFirstOrDefault(c => c.Id == id,
                    includeProperities: "CourseTeachers,CourseTeachers.Teacher");


                var courseTeachers = upsertCourseVM.Course.CourseTeachers;

                //edit dropdown teachers  to make courseTeachers selected
                upsertCourseVM.Teachers = _unitOfWork.Teacher.GetAll()
                    .Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.Id,
                        Selected = isSelected(t.Id, courseTeachers)

                    });

            }
            else
            {
                // Add course
                upsertCourseVM.Course = new Course();

            }

            //initialize grades and semesters
            upsertCourseVM.Grades = StaticData.GradesList;
            upsertCourseVM.Semesters = StaticData.SemestersList;
            upsertCourseVM.StaticCoursesName = StaticData.CoursesNamesList;

            return View(upsertCourseVM);
        }

        public Boolean isSelected(string teacherId, IEnumerable<CourseTeachers> courseTeachers)
        {
            var teacher = courseTeachers.FirstOrDefault(
                ct => ct.TeacherId == teacherId);

            if (teacher != null)
                return true;

            return false;
        }

        // Post Add/Edit Course Page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertCourse(UpsertCourseVM upsertCourseVM)
        {
            if (ModelState.IsValid)
            {

                if (upsertCourseVM.Course.Id == 0)
                {
                    //Add Course


                    //add course to db
                    var course = new Course()
                    {
                        Name = upsertCourseVM.Course.Name,
                        Grade = upsertCourseVM.Course.Grade,
                        Semester = upsertCourseVM.Course.Semester,
                    };

                    _unitOfWork.Course.Add(course);

                    _unitOfWork.Save();


                    //Add teachers to course
                    foreach (var teacherId in upsertCourseVM.SelectedTeachers)
                    {
                        var courseTeachers = new CourseTeachers()
                        {
                            CourseId = course.Id,
                            TeacherId = teacherId,
                        };

                        _unitOfWork.CourseTeachers.Add(courseTeachers);
                    }

                    _unitOfWork.Save();


                }
                else
                {
                    // Edit Course

                    //Empty newCourseTeachers
                    var newCourseTeachers = Enumerable.Empty<CourseTeachers>();

                    // filling newCourseTeachers with selected teachers from dropdown teachers
                    foreach (var teacherId in upsertCourseVM.SelectedTeachers)
                    {
                        var courseTeacher = new CourseTeachers()
                        {
                            TeacherId = teacherId,
                            CourseId = upsertCourseVM.Course.Id
                        };
                        newCourseTeachers = newCourseTeachers.Append(courseTeacher).ToList();
                    }

                    var oldCourse = _unitOfWork.Course.GetFirstOrDefault(
                        c => c.Id == upsertCourseVM.Course.Id,
                    includeProperities: "CourseTeachers,CourseTeachers.Teacher");

                    oldCourse.Name = upsertCourseVM.Course.Name;
                    oldCourse.Grade = upsertCourseVM.Course.Grade;
                    oldCourse.Semester = upsertCourseVM.Course.Semester;
                    oldCourse.CourseTeachers = newCourseTeachers;

                    _unitOfWork.Save();

                }
                return RedirectToAction(nameof(Courses));

            }
            return View(upsertCourseVM);
        }

        /////////// End Courses /////////////////



        /////////// Sections /////////////////


        // Get Sections
        public IActionResult Sections()
        {
            var sectionsIndexVM = new SectionsIndexVM()
            {
                Sections = _unitOfWork.Section.GetAll(
                    includeProperities: "Teacher,Students"),

                Grades = StaticData.SelectedGradesList
            };

            return View(sectionsIndexVM);
        }

        //populate table when dropdownlist value changed
        [HttpPost]
        public IActionResult PopulateSectionsTable(string grade)

        {
            //initialize empty sections
            var sections = Enumerable.Empty<Section>();

            if (grade == "All")
            {
                sections = _unitOfWork.Section.GetAll(
                    includeProperities: "Teacher,Students");
            }
            else
            {
                sections = _unitOfWork.Section
                    .GetAll(c => c.Grade == grade,
                    includeProperities: "Teacher,Students");
            }


            return PartialView("~/Areas/Public/Views/Partials/CoursesAndSections/_SectionsTable.cshtml"
                , sections);

        }

        //Get Add/Edit Section
        public IActionResult UpsertSection(int id)
        {
            var upsertSectionVM = new UpsertSectionVM();

            if (id != 0)
            {
                // Edit Section

                upsertSectionVM.Section = _unitOfWork.Section
                    .GetFirstOrDefault(s => s.Id == id,
                    includeProperities: "Teacher");
            }
            else
            {
                //Add Section

                upsertSectionVM.Section = new Section();

            }

            upsertSectionVM.Grades = StaticData.GradesList;
            upsertSectionVM.Teachers = _unitOfWork.Teacher.GetAll()
                .Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.Id,

                });

            return View(upsertSectionVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertSection(UpsertSectionVM upsertSectionVM)
        {
            if (ModelState.IsValid)
            {

                if (upsertSectionVM.Section.Id == 0)
                {
                    //Add section
                    _unitOfWork.Section.Add(upsertSectionVM.Section);

                    _unitOfWork.Save();

                    //Add classes to section
                    for (int d = 1; d <= StaticData.Days.Length; d++)
                    {
                        for (int c = 1; c <= StaticData.Classes.Length; c++)
                        {
                            var claSs = new Class()
                            {
                                ClassNumber = c,
                                Day = d,
                                SectionId = upsertSectionVM.Section.Id,

                            };

                            _unitOfWork.Class.Add(claSs);

                        }

                    }

                    _unitOfWork.Save();

                }
                else
                {
                    //update section

                    _unitOfWork.Section.Update(upsertSectionVM.Section);

                    _unitOfWork.Save();

                }


                return RedirectToAction(nameof(Sections));
            }

            return View(upsertSectionVM);

        }





        /////////// End Sections /////////////////



        /////////// Classes /////////////////

        // Get Classes
        public IActionResult Classes(int sectionId)
        {
            var classes = _unitOfWork.Class.GetAll(c => c.SectionId == sectionId,
                includeProperities: "Teacher,Course");

            return View(classes);
        }

        // Get Edit Classes
        public IActionResult EditClasses(int sectionId)
        {
            var editClassesVM = new EditClassesVM()
            {
                Classes = _unitOfWork.Class.GetAll(c => c.SectionId == sectionId,
                includeProperities: "Teacher,Course"),

                Courses = _unitOfWork.Course.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),

                }),

                ClassNumbers = StaticData.ClassNumbersList,

                Days = StaticData.DaysList,

                SectionId = sectionId, //just to pass it to post action
            };

            return View(editClassesVM);
        }

        // Post Edit Class

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditClasses(EditClassesVM editClassesVM)
        {
            if (ModelState.IsValid)
            {

                // to check if this class is the 1st class inserted of this course
                var classItem = _unitOfWork.Class
                                .GetFirstOrDefault(c => c.SectionId == editClassesVM.SectionId
                                && c.TeacherId == editClassesVM.SelectedTeacher
                                && c.CourseId == editClassesVM.SelectedCourse);



                var claSs = _unitOfWork.Class
                    .GetFirstOrDefault(c => c.SectionId == editClassesVM.SectionId
                    && c.Day == editClassesVM.SelectedDay
                    && c.ClassNumber == editClassesVM.SelectedClassNumber);

                claSs.TeacherId = editClassesVM.SelectedTeacher;
                claSs.CourseId = editClassesVM.SelectedCourse;



                if (classItem == null)
                {
                    // this is the 1st class

                    var students = _unitOfWork.Student
                                            .GetAll(std => std.SectionId == editClassesVM.SectionId);

                    foreach (var student in students)
                    {
                        Mark newMark = new Mark()
                        {
                            FirstMark = 0,
                            SecondMark = 0,
                            AssignmentsMark = 0,
                            FinalMark = 0,
                            CourseId = editClassesVM.SelectedCourse,
                            StudentId = student.Id
                        };

                        _unitOfWork.Mark.Add(newMark);
                    }

                }




                _unitOfWork.Save();

                return RedirectToAction("EditClasses", "CoursesAndSections",
                    new { sectionId = editClassesVM.SectionId });
            }



            // if fail or error occured
            if (editClassesVM.SelectedTeacher == null)
            {
                ModelState.AddModelError(string.Empty, "يرجى اختيار المعلم");
            }

            editClassesVM.Classes = _unitOfWork.Class.GetAll(c => c.SectionId == editClassesVM.SectionId,
                includeProperities: "Teacher,Course");

            editClassesVM.Courses = _unitOfWork.Course.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),

            });

            var courseTeachers = _unitOfWork.CourseTeachers
                .GetAll(includeProperities: "Teacher")
                .Where(ct => ct.CourseId == editClassesVM.SelectedCourse);


            editClassesVM.Teachers = courseTeachers.Select(ct => new SelectListItem
            {
                Text = ct.Teacher.Name,
                Value = ct.Teacher.Id,
            });

            editClassesVM.ClassNumbers = StaticData.ClassNumbersList;

            editClassesVM.Days = StaticData.DaysList;



            return View(editClassesVM);
        }

        public IActionResult PopulateTeachers(int courseId)
        {
            var courseTeachers = _unitOfWork.CourseTeachers
                .GetAll(includeProperities: "Teacher")
                .Where(ct => ct.CourseId == courseId);


            var teachers = courseTeachers.Select(ct => new SelectListItem
            {
                Text = ct.Teacher.Name,
                Value = ct.Teacher.Id,
            });


            return PartialView("~/Areas/Public/Views/Partials/CoursesAndSections/_TeachersDropDownList.cshtml"
                , teachers);

        }



        #region API_CALLS

        [HttpDelete]
        public IActionResult DeleteCourse(int id)
        {
            var course = _unitOfWork.Course.GetFirstOrDefault(c => c.Id == id);

            if (course == null)
            {
                return Json(new { success = false, message = "حدث خطأ في عملية الحذف" });
            }

            _unitOfWork.Course.Remove(course);
            _unitOfWork.Save();

            return Json(new { success = true, message = "تم حذف المادة بنجاح" });
        }

        [HttpDelete]
        public IActionResult DeleteSection(int id)
        {
            var section = _unitOfWork.Section.GetFirstOrDefault(s => s.Id == id);

            if (section == null)
            {
                return Json(new { success = false, message = "حدث خطأ في عملية الحذف" });
            }

            _unitOfWork.Section.Remove(section);
            _unitOfWork.Save();

            return Json(new { success = true, message = "تم حذف الشعبة بنجاح" });
        }

        #endregion

    }
}
