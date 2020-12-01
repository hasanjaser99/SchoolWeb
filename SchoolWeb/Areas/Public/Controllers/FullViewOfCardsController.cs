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
    public class FullViewOfCardsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FullViewOfCardsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        //Get viewNewsCard
        public IActionResult FullViewOfNewsItem(int ItemId)
        {
            var newsItem= _unitOfWork
                            .News
                            .GetFirstOrDefault(news => news.Id == ItemId,
                                                includeProperities: "NewsImages");
            return View(newsItem);
        }

        public IActionResult FullViewOfActivity(int ItemId)
        {
            var ActivityItem = _unitOfWork
                            .Activity
                            .GetFirstOrDefault(activity => activity.Id == ItemId,
                                                includeProperities: "ActivityImages");
            return View(ActivityItem);
        }

    }
}
