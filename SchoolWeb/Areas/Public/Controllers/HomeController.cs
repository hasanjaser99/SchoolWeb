using System.Linq;
using SchoolWeb.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using SchoolWeb.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SchoolWeb.Areas.Public.Controllers
{
    [Area("Public")]
    [AllowAnonymous]

    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(IUnitOfWork unitOfWork, SignInManager<IdentityUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
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


        //Post logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
