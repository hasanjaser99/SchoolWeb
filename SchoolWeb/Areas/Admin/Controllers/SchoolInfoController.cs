using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolWeb.DataAccess.Repository;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using SchoolWeb.Models;
using SchoolWeb.Models.ViewModels;

namespace SchoolWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SchoolInfoController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public SchoolInfoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: HomeController
        // for FeesOfRegistration page
        public IActionResult Index()
        {   ///
            //var ListOfNews = _unitOfWork
            //                .News
            //                .GetAll(includeProperities: "NewsImages")
            //                .ToList()
            //                .OrderBy(i => i.Date)
            //                .Take(3);

            //var ListOfActivities = _unitOfWork
            //                .Activity
            //                .GetAll(includeProperities: "ActivityImages")
            //                .ToList()
            //                .OrderBy(i=>i.Date)
            //                .Take(3);

            //var newsAndActivities =new NewsAndActivitiesVM()
            //{
            //    ListOfActivities= ListOfActivities,
            //    ListOfNews= ListOfNews
            //};
            
            return View();
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
        public IActionResult Activities()
        {
            var ListOfActivities = _unitOfWork
                            .Activity
                            .GetAll(includeProperities: "ActivityImages")
                            .ToList()
                            .OrderBy(i => i.Date);

            return View(ListOfActivities);
        }


        //public IActionResult RegisterationDetails()
        //{
        //    return View();
        //}

    }
}
