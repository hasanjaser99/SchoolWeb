using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolWeb.Models;
using SchoolWeb.Models.ViewModels;
using SchoolWeb.Utility;

namespace SchoolWeb.Areas.Public.Controllers
{
    [Area("Public")]
    public class FeesAndRegisterationController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;


        [BindProperty]
        public StudentRegisterVM studentRegisterVM { get; set; }


        public FeesAndRegisterationController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        // Get RegisterationDetails
        public IActionResult RegisterationDetails()
        {
            return View();
        }


        // Get schoolFees
        public IActionResult SchoolFees()
        {
            var schoolFees = _unitOfWork.SchoolFee.GetAll();

            return View(schoolFees);
        }

        //Get Student Register
        public IActionResult Register()
        {

            studentRegisterVM = new StudentRegisterVM()
            {
                Grades = StaticData.GradesList,

                BussStates = StaticData.BussStatesList
            };


            return View(studentRegisterVM);
        }

        //Post Student Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Register")]
        public async Task<IActionResult> PostRegister()
        {
            if (ModelState.IsValid)
            {
                //create asp.net identity user
                var identityUser = new IdentityUser
                {
                    Email = studentRegisterVM.Student.ParentEmail,
                    UserName = studentRegisterVM.Student.ParentEmail,
                    PhoneNumber = studentRegisterVM.Student.ParentPhoneNumber,
                };

                var result = await _userManager.CreateAsync(identityUser,
                             studentRegisterVM.Password);


                if (result.Succeeded)
                {

                    // add roles to database if not exists (just for development phase)

                    if (!await _roleManager.RoleExistsAsync(StaticData.Role_Admin))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(
                              StaticData.Role_Admin));
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticData.Role_Student))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(
                              StaticData.Role_Student));
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticData.Role_Teacher))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(
                              StaticData.Role_Teacher));
                    }

                    // assign user a student role
                    await _userManager.AddToRoleAsync(identityUser, StaticData.Role_Student);


                    //get born certificate image
                    string webRootPath = _hostEnvironment.WebRootPath;
                    var files = HttpContext.Request.Form.Files;
                    var imageUrl = "";

                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(webRootPath, @"images\students");
                        var extention = Path.GetExtension(files[0].FileName);

                        using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extention), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        imageUrl = @"\images\students\" + fileName + extention;
                    }

                    //create student user
                    var student = new Student()
                    {
                        Id = identityUser.Id,
                        ArabicName = studentRegisterVM.Student.ArabicName,
                        EnglishName = studentRegisterVM.Student.EnglishName,
                        Address = studentRegisterVM.Student.EnglishName,
                        ParentEmail = studentRegisterVM.Student.ParentEmail,
                        ParentPhoneNumber = studentRegisterVM.Student.ParentPhoneNumber,
                        BornCertificateImage = imageUrl,
                        BussState = studentRegisterVM.Student.BussState,
                        Grade = studentRegisterVM.Student.Grade,
                    };

                    _unitOfWork.Student.Add(student);

                    _unitOfWork.Save();

                    return RedirectToAction(nameof(RegisterationDetails));

                }
                else
                {
                    // handle validation if errors occurs when add identity user 
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "PasswordRequiresNonAlphanumeric")
                        {
                            ModelState.AddModelError(string.Empty,
                                "كلمة المرور يجب ان تحتوي على رمز واحد على الأقل");

                        }

                        if (error.Code == "PasswordRequiresUpper")
                        {
                            ModelState.AddModelError(string.Empty,
                                "كلمة المرور يجب ان تحتوي على حرف كبير واحد على الأقل");
                        }
                    }
                }

            }

            /*if model state is not valid return same page with 
            filled lists (grades and bussStates) */
            var studentRegister = new StudentRegisterVM()
            {
                Grades = StaticData.GradesList,

                BussStates = StaticData.BussStatesList
            };


            return View(studentRegister);
        }

    }

}



