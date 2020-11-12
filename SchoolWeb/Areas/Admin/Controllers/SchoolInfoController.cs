using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolWeb.DataAccess.Repository;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;
using SchoolWeb.Models;
using SchoolWeb.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolWeb.Utility;

namespace SchoolWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SchoolInfoController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SchoolInfoController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment; 
            _unitOfWork = unitOfWork;
        }

        /**************************************** Functions **********************************/

        void uploadAndCreateImage(
            string webRootPath, IFormFileCollection files, IFormFile file, int tableItemId, string imgTable)
        {
            string folderPath;
            string imgUrlFilePath;
            var imageUrl = "";

            if (imgTable == "News")
            {
                folderPath = @"images\news";
                imgUrlFilePath = @"\images\news\";
            }
            else
            {
                folderPath = @"images\activities";
                imgUrlFilePath = @"\images\activities\";
            }

            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(webRootPath, folderPath);
            var extention = Path.GetExtension(files[0].FileName);

            using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
            {
                files[0].CopyTo(fileStreams);
            }

            imageUrl = imgUrlFilePath + fileName + extention;

            if (imgTable == "News")
            {
                var img = new NewsImages()
                {
                    ImageUrl = imageUrl,
                    NewsId = tableItemId
                };
                // adding image
                _unitOfWork.NewsImages.Add(img);


            }
            else
            {
                var img = new ActivityImages()
                {
                    ImageUrl = imageUrl,
                    ActivityId = tableItemId
                };
                // adding image
                _unitOfWork.ActivityImages.Add(img);


            }

        }

        /*************************************************************************************/

        void removeImageFromServer(string ImageUrl)
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }



        //________________________________ actions ___________________________________

        // GET: HomeController
        // for FeesOfRegistration page
        public IActionResult Index()
        {
            var schoolFees = _unitOfWork.SchoolFee.GetAll();

            var feesOfRegisterationVM = new FeesOfRegisterationVM()
            {
                SchoolFees= schoolFees,
                Grades = StaticData.GradesList
            };

            return View(feesOfRegisterationVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(FeesOfRegisterationVM feesOfRegisterationVM)
        {
            var schoolFee = feesOfRegisterationVM.schoolFeeItem;

            var schoolFeeItem = _unitOfWork
                                .SchoolFee
                                .GetFirstOrDefault(sf => sf.Grade.Equals(schoolFee.Grade));

            if (schoolFeeItem==null)
            {
                _unitOfWork.SchoolFee.Add(schoolFee);
            }
            else
            {
                schoolFeeItem.SchoolFees = schoolFee.SchoolFees;
                _unitOfWork.SchoolFee.Update(schoolFeeItem);
            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));

        }


        #region API CALLS

        [HttpDelete]
        public IActionResult deleteSchoolFees(string g)
        {
            try
            {
                var schoolFeeItem = _unitOfWork
                                .SchoolFee
                                .GetFirstOrDefault(sf => sf.Grade.Equals(g));

                _unitOfWork.SchoolFee.Remove(schoolFeeItem);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });

            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Delete falid" });

            }



        }
        #endregion

        //________________________________ news related actions___________________________________
        public IActionResult News()
        {
            var ListOfNews = _unitOfWork
                             .News
                             .GetAll(includeProperities: "NewsImages")
                             .ToList()
                             .OrderBy(i => i.Date);

            return View(ListOfNews);
        }


        /*************************************************************************************/

        [HttpGet]
        public IActionResult UpsertNews(int? id)
        {
            if (id != null) //in edit state
            {

                var newsItem = _unitOfWork
                                .News
                                .GetFirstOrDefault(
                                                    u=> u.Id == id , 
                                                    includeProperities: "NewsImages"
                                                    );


                
                 var newsWithImages = new NewsWithImagesVM()
                {
                    Id = newsItem.Id,
                    Description = newsItem.Description,
                    Date = newsItem.Date,
                    Title = newsItem.Title,
                    NewsImages= newsItem.NewsImages,
                    
                };
                
                return View(newsWithImages);
            }
            else // adding newsItem
            {
                var newsWithImages = new NewsWithImagesVM()
                {
                    Id = 0
                };
                return View(newsWithImages);
            }
        }
        /*************************************************************************************/
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertNews(NewsWithImagesVM newsWithImagesVM)
        {
            //check if page is used for update news
            bool isUpdate = !!(newsWithImagesVM.Id != 0);

            IEnumerable<NewsImages> images=new List<NewsImages>();

            if (isUpdate)
            {
                 images = _unitOfWork.NewsImages.GetAll(i => i.NewsId == newsWithImagesVM.Id);
                
            }

            // if admin didn't upload new images delete modelState - NewsImagesFiles errors
            if (isUpdate & ModelState["NewsImagesFiles"]!=null& images.Count()!=0)
            {
                ModelState["NewsImagesFiles"].Errors.Clear();
                ModelState["NewsImagesFiles"].ValidationState = ModelValidationState.Valid;
                
            }

            

            if (ModelState.IsValid)
            {
                if (isUpdate)
                {


                    var newsItem = new News()
                    {
                        Id= newsWithImagesVM.Id,
                        Title = newsWithImagesVM.Title,
                        Date = newsWithImagesVM.Date,
                        Description = newsWithImagesVM.Description,
                        NewsImages= newsWithImagesVM.NewsImages

                    };

                    string webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;

                    if (files.Count > 0)
                    {
                        
                        // upload images 
                        foreach (var file in files)
                        {
                            uploadAndCreateImage(webRootPath, files, file, newsItem.Id,"News");
                        }
                       
                    }
                    _unitOfWork.News.Update(newsItem);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(News));
                }
                else
                {// page is used to add news
                

                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                
                

                    if (files.Count > 0) //  new images uploaded
                    {
                        // create news Item to get its id
                        var newsItem = new News()
                        {
                            Title = newsWithImagesVM.Title,
                            Date = newsWithImagesVM.Date,
                            Description = newsWithImagesVM.Description

                        };

                        _unitOfWork.News.Add(newsItem);
                        _unitOfWork.Save();

                        // upload images 
                        foreach (var file in files)
                        {
                            uploadAndCreateImage(webRootPath, files, file, newsItem.Id,"News");
                        }

                    _unitOfWork.Save();
                     return RedirectToAction(nameof(News));
                    }

                

                }
            }

            // there are some errors 
            if (!isUpdate) newsWithImagesVM.Id = 0;
            newsWithImagesVM.NewsImages = images;
            return View(newsWithImagesVM);
        }

        /*************************************************************************************/
        public IActionResult DeleteNews(int id)
        {
            // getting newsItem
            var newsItem = _unitOfWork
                                 .News
                                 .GetFirstOrDefault(u => u.Id == id,includeProperities : "NewsImages");


            
            //deleting images from server
            foreach (var image in newsItem.NewsImages)
            {
                // delete file from server
                removeImageFromServer(image.ImageUrl);
                
            }
            
            _unitOfWork.News.Remove(newsItem);
            _unitOfWork.Save();

            return RedirectToAction(nameof(News));
        }

        /*************************************************************************************/

        #region API CALLS

        [HttpDelete]
        public IActionResult DeleteNewsImage(int id)
        {
            try
            {
                var newsImage = _unitOfWork.NewsImages.GetFirstOrDefault(img => img.Id == id);

            // delete file from server
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, newsImage.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            else
            {
                return Json(new { success = false, message = "Delete falid" });
            }

            // delete newsImage 
            _unitOfWork.NewsImages.Remove(newsImage);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });


            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Delete falid" });
            }



        }

        #endregion
       
        //________________________________ Activities related actions___________________________________

        public IActionResult Activities()
        {
            var ListOfActivities = _unitOfWork
                            .Activity
                            .GetAll(includeProperities: "ActivityImages")
                            .ToList()
                            .OrderBy(i => i.Date);

            return View(ListOfActivities);
        }


        /*************************************************************************************/

        [HttpGet]
        public IActionResult UpsertActivities(int? id)
        {
            if (id != null) //in edit state
            {

                var activityItem = _unitOfWork
                                .Activity
                                .GetFirstOrDefault(
                                    u => u.Id == id,includeProperities: "ActivityImages");



                var activityWithImages = new ActivityWithImagesVM()
                {
                    Id = activityItem.Id,
                    Description = activityItem.Description,
                    Date = activityItem.Date,
                    Title = activityItem.Title,
                    ActivityImages = activityItem.ActivityImages,

                };

                return View(activityWithImages);
            }
            else // adding activity item
            {
                var activityWithImages = new ActivityWithImagesVM()
                {
                    Id = 0
                };
                return View(activityWithImages);
            }
        }
        /*************************************************************************************/
       

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpsertActivities(ActivityWithImagesVM activityWithImagesVM)
        {
            //check if page is used for update activities
            bool isUpdate = !!(activityWithImagesVM.Id != 0);

            IEnumerable<ActivityImages> images = new List<ActivityImages>();

            if (isUpdate)
            {
                images = _unitOfWork.ActivityImages.GetAll(i => i.ActivityId == activityWithImagesVM.Id);

            }

            //if admin didn't upload new images delete modelState - ActivityImagesFiles errors
            if (isUpdate & ModelState["ActivityImagesFiles"] != null & images.Count() != 0)
            {
                ModelState["ActivityImagesFiles"].Errors.Clear();
                ModelState["ActivityImagesFiles"].ValidationState = ModelValidationState.Valid;

            }



            if (ModelState.IsValid)
            {
                if (isUpdate)
                {


                    var activityItem = new Activity()
                    {
                        Id = activityWithImagesVM.Id,
                        Title = activityWithImagesVM.Title,
                        Date = activityWithImagesVM.Date,
                        Description = activityWithImagesVM.Description,
                        ActivityImages = activityWithImagesVM.ActivityImages

                    };

                    string webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;

                    if (files.Count > 0)
                    {

                        // upload images 
                        foreach (var file in files)
                        {
                            uploadAndCreateImage(webRootPath, files, file, activityItem.Id, "Activity");
                        }

                    }
                    _unitOfWork.Activity.Update(activityItem);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(Activities));
                }
                else
                {// page is used to add activities


                    string webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;



                    if (files.Count > 0) //  new images uploaded
                    {
                        // create activity Item to get its id
                        var activityItem = new Activity()
                        {
                            Title = activityWithImagesVM.Title,
                            Date = activityWithImagesVM.Date,
                            Description = activityWithImagesVM.Description

                        };

                        _unitOfWork.Activity.Add(activityItem);
                        _unitOfWork.Save();

                        // upload images 
                        foreach (var file in files)
                        {
                            uploadAndCreateImage(webRootPath, files, file, activityItem.Id, "Activity");
                        }

                        _unitOfWork.Save();
                        return RedirectToAction(nameof(Activities));
                    }



                }
            }

            // there are some errors 
            if (!isUpdate) activityWithImagesVM.Id = 0;
            activityWithImagesVM.ActivityImages = images;
            return View(activityWithImagesVM);
        }

        /*************************************************************************************/
        public IActionResult DeleteActivity(int id)
        {
            // getting activityItem
            var activityItem = _unitOfWork
                                 .Activity
                                 .GetFirstOrDefault(u => u.Id == id, includeProperities: "ActivityImages");



            //deleting images from server
            foreach (var image in activityItem.ActivityImages)
            {
                // delete file from server
                removeImageFromServer(image.ImageUrl);

            }

            _unitOfWork.Activity.Remove(activityItem);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Activities));
        }

        /*************************************************************************************/

        #region API CALLS

        [HttpDelete]
        public IActionResult DeleteActivityImage(int id)
        {
            try
            {
                var activityImg = _unitOfWork.ActivityImages.GetFirstOrDefault(img => img.Id == id);

                // delete file from server
                string webRootPath = _hostEnvironment.WebRootPath;
                var imagePath = Path.Combine(webRootPath, activityImg.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
                else
                {
                    return Json(new { success = false, message = "Delete falid" });
                }

                // delete newsImage 
                _unitOfWork.ActivityImages.Remove(activityImg);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete Successful" });


            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Delete falid" });
            }



        }

        #endregion


    }
}
