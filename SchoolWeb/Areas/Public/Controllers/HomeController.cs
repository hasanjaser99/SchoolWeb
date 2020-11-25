using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolWeb.DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolWeb.Models;
using SchoolWeb.Models.ViewModels;

namespace SchoolWeb.Areas.Public.Controllers
{
    [Area("Public")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: HomeController
        public IActionResult Index()
        {
            var ListOfNews = _unitOfWork
                            .News
                            .GetAll(includeProperities: "NewsImages")
                            .ToList()
                            .OrderBy(i => i.Date)
                            .Take(3)
                            .OrderByDescending(i => i.Date);

            var ListOfActivities = _unitOfWork
                            .Activity
                            .GetAll(includeProperities: "ActivityImages")
                            .ToList()
                            .OrderBy(i => i.Date)
                            .Take(3)
                            .OrderByDescending(i => i.Date);

            var newsAndActivities = new NewsAndActivitiesVM()
            {
                ListOfActivities = ListOfActivities,
                ListOfNews = ListOfNews
            };

            return View(newsAndActivities);
        }

    }
}
