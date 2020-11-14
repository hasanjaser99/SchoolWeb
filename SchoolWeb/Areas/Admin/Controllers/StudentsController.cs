using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


        public StudentsController(IUnitOfWork unitOfWork,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> StudentsInfo()
        {

            var identityUsers = await _userManager.GetUsersInRoleAsync(StaticData.Role_Student);


            var studentsInfoVM = new AdminStudentsInfoVM()
            {
                Students = _unitOfWork.Student.GetAll().Where(
                    s => s.Id == getIdentityId(s.Id, identityUsers)),

                Grades = StaticData.SelectedGradesList,

            };

            return View(studentsInfoVM);
        }

        //populate sections when grades value changed
        [HttpPost]
        public IActionResult PopulateSections(string grade)

        {
            var sections = new List<SelectListItem>()
            {
                new SelectListItem{ Text = "جميع الشعب", Value="All"}
            };

            sections.AddRange(_unitOfWork.Section.GetAll()
                .Where(s => s.Grade == grade)
                .Select(s => new SelectListItem
                {
                    Text = s.Name,
                    Value = s.Id.ToString(),

                }));

            return PartialView("~/Areas/Public/Views/Partials/AdminStudents/_SectionsDropDownList.cshtml"
                , sections);

        }

        //populate table when dropdownlist value changed
        [HttpPost]
        public IActionResult PopulateStudentsTable(string grade, string section)

        {
            //initialize empty students
            var students = Enumerable.Empty<Student>();

            if (grade == "All")
            {
                students = _unitOfWork.Student.GetAll();
            }

            else if (section == "All")
            {
                students = _unitOfWork.Student.GetAll()
                    .Where(s => s.Grade == grade);
            }
            else
            {
                students = _unitOfWork.Student.GetAll()
                    .Where(s => s.Grade == grade && s.SectionId.ToString() == section);
            }


            return PartialView("~/Areas/Public/Views/Partials/AdminStudents/_StudentsTable.cshtml"
                , students);

        }

        private string getIdentityId(string studentId, IList<IdentityUser> identityUsers)
        {
            var identityUser = identityUsers.FirstOrDefault(i => i.Id == studentId);

            if (identityUser != null)
            {
                return identityUser.Id;
            }

            return "";
        }

    }




}
