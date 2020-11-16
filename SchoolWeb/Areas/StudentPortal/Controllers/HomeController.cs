using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolWeb.DataAccess.Repository;
using SchoolWeb.Models;
using SchoolWeb.Models.ViewModels;

namespace SchoolWeb.Areas.StudentPortal.Controllers
{
    [Area("StudentPortal")]
    public class HomeController : Controller
    {



        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        /********************************** profile ***********************************/
        // profile page
        public IActionResult Index()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claims.Value;
            var sutdent = _unitOfWork.Student.GetFirstOrDefault(std => std.Id == userId);

            return View(sutdent);
        }

        public IActionResult AccountStatment()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claims.Value;

            var student = _unitOfWork.Student
                .GetFirstOrDefault(s => s.Id == userId
                , includeProperities: "StudentFee,StudentFee.MonthlyPayments");

            var studentFee = student.StudentFee;


            return View(studentFee.MonthlyPayments);
        }

        /********************************** Class Schedule ***********************************/

        public IActionResult ClassSchedule()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claims.Value;
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


            return View();
        }

    }
}
