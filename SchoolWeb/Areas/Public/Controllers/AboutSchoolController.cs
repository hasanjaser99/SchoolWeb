using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolWeb.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;

namespace SchoolWeb.Areas.Public.Controllers
{
    [Area("Public")]
    public class AboutSchoolController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public AboutSchoolController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        
        // for activites page
        public IActionResult Index()
        {           
            
            var ListOfActivities = _unitOfWork
                            .Activity
                            .GetAll(includeProperities: "ActivityImages")
                            .ToList()
                            .OrderBy(i => i.Date);

            return View(ListOfActivities);
        }

        public IActionResult News()
        {
            var ListOfNews = _unitOfWork
                             .News
                             .GetAll(includeProperities: "NewsImages")
                             .ToList()
                             .OrderBy(i => i.Date);

            return View(ListOfNews);
        }


        public IActionResult AboutUs()
        {

            return View();
        }

        public IActionResult Teachers()
        {

            return View();
        }
        

    }
}
