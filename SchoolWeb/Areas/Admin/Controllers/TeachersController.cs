using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolWeb.DataAccess.Repository;

namespace SchoolWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeachersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeachersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Get All Teachers
        public IActionResult Index()
        {
            var teachers = _unitOfWork.Teacher.GetAll();

            return View(teachers);
        }

    }
}
