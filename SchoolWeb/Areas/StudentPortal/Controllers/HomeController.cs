using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SchoolWeb.DataAccess.Repository;
using SchoolWeb.Models;
using SchoolWeb.Models.ViewModels;
using SchoolWeb.Utility;

namespace SchoolWeb.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    [Authorize(Roles = StaticData.Role_Student)]

    public class HomeController : Controller
    {



        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        private string getCurrentStudentId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claims.Value;

        }

        /********************************** profile ***********************************/
        // profile page
        public IActionResult Index()
        {

            var userId = getCurrentStudentId();
            var sutdent = _unitOfWork.Student.GetFirstOrDefault(std => std.Id == userId);

            return View(sutdent);
        }

        public IActionResult AccountStatment()
        {

            var userId = getCurrentStudentId();

            var student = _unitOfWork.Student
                .GetFirstOrDefault(s => s.Id == userId
                , includeProperities: "StudentFee,StudentFee.MonthlyPayments");

            var studentFee = student.StudentFee;


            return View(studentFee.MonthlyPayments);
        }

        /********************************** Class Schedule ***********************************/

        public IActionResult ClassSchedule()
        {
            var userId = getCurrentStudentId();
            var student = _unitOfWork
                            .Student
                            .GetFirstOrDefault(std => std.Id == userId);

            var Classes = _unitOfWork
                            .Class
                            .GetAll(c => c.SectionId == student.SectionId, includeProperities: "Teacher,Course");

            List<Teacher> TeatchersList = new List<Teacher>();

            foreach (var Class in Classes)
            {
                if (Class.Teacher != null) TeatchersList.Add(Class.Teacher);

            }


            ClassScheduleVM classSchedule = new ClassScheduleVM()
            {
                Classes = Classes,
                Teachers = TeatchersList.Distinct().ToList()
            };

            return View(classSchedule);
        }


        /********************************** marks ***********************************/
        public IActionResult Marks()
        {

            var userId = getCurrentStudentId();
            var marks = _unitOfWork.Mark
                            .GetAll(mark => mark.StudentId == userId,
                                    includeProperities: "Course,Student");

            List<SelectListItem> grades = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "--الصف--", Value="none", Selected = true}
            };

            foreach (var mark in marks)
            {
                if (mark.Course != null)
                {

                    string grade = mark.Course.Grade;
                    grades.Add(new SelectListItem { Text = StaticFunctions.GetGrade(grade), Value = grade });

                }
            }

            StudentMarksVM studentMarksVM = new StudentMarksVM()
            {
                Grades = grades.Distinct().ToList()
            };

            return View(studentMarksVM);
        }


        #region API

        public IActionResult PopulateMarksTable(string grade, string semester)
        {
            var studentId = getCurrentStudentId();
            var student = _unitOfWork.Student
               .GetFirstOrDefault(s => s.Id == studentId
               , includeProperities: "Section,Section.Classes");


            var classes = student.Section.Classes.GroupBy(c => c.CourseId)
                .Select(c => c.First()).ToList();

            var marks = Enumerable.Empty<Mark>();



            marks = _unitOfWork.Mark.GetAll(
                    m => m.StudentId == studentId
                    && m.Course.Grade == grade
                    && m.Course.Semester.ToString() == semester
                    , includeProperities: "Course");


            return PartialView("~/Areas/Public/Views/Partials/StudentsMarks/_CoursesMarksTable.cshtml"
                , marks);

        }


        #endregion

    }
}
