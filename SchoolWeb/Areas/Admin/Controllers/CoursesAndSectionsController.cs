using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class CoursesAndSectionsController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CoursesAndSectionsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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


                    //if(oldCourseTeachers.Contains("as"))

                    //upsertCourseVM.Course.CourseTeachers.
                }
                return RedirectToAction(nameof(Courses));

            }
            return View(upsertCourseVM);
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

        #endregion

    }
}
