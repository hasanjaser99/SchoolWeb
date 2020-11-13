using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolWeb.DataAccess.Repository;

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
        

        public IActionResult Index()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims= claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claims.Value;
            var sutdent = _unitOfWork.Student.GetFirstOrDefault(std => std.Id == userId);
            
            return View(sutdent);
        }
    }
}
