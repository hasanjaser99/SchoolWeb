using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchoolWeb.DataAccess.Repository;
using SchoolWeb.Models;
using SchoolWeb.Models.ViewModels;
using SchoolWeb.Utility;

namespace SchoolWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.Role_Admin)]

    public class TeachersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public UpsertTeacherVM UpsertTeacherVM { get; set; }

        public TeachersController(UserManager<IdentityUser> userManager,
            IUnitOfWork unitOfWork,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Get All Teachers
        public IActionResult Index()
        {
            var teachers = _unitOfWork.Teacher.GetAll();

            return View(teachers);
        }

        // Get Add/Edit Teacher Page
        public IActionResult Upsert(string teacherId)
        {
            UpsertTeacherVM = new UpsertTeacherVM()
            {
                Teacher = new Teacher()
            };

            if (!String.IsNullOrEmpty(teacherId))
            {
                //this is for edit teacher

                UpsertTeacherVM.Teacher = _unitOfWork.Teacher
                    .GetFirstOrDefault(t => t.Id == teacherId);
            }

            return View(UpsertTeacherVM);

        }

        // Post Add/Edit Teacher Page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {
            if (!String.IsNullOrEmpty(UpsertTeacherVM.Teacher.Id))
            {

                ModelState["Password"].ValidationState = ModelValidationState.Valid;

            }

            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(UpsertTeacherVM.Teacher.Id))
                {
                    //update teacher data

                    _unitOfWork.Teacher.Update(UpsertTeacherVM.Teacher);

                    _unitOfWork.Save();

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    //create asp.net identity user
                    var identityUser = new IdentityUser
                    {
                        Email = UpsertTeacherVM.Teacher.Email,
                        UserName = UpsertTeacherVM.Teacher.Email,
                        PhoneNumber = UpsertTeacherVM.Teacher.PhoneNumber,
                    };

                    var result = await _userManager.CreateAsync(identityUser,
                                 UpsertTeacherVM.Password);


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
                        if (!await _roleManager.RoleExistsAsync(StaticData.Role_Waiting))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(
                                  StaticData.Role_Waiting));
                        }

                        if (!await _roleManager.RoleExistsAsync(StaticData.Role_Teacher))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(
                                  StaticData.Role_Teacher));
                        }

                        // assign user a teacher role
                        await _userManager.AddToRoleAsync(identityUser, StaticData.Role_Teacher);

                        //create teacher user
                        var teacher = new Teacher()
                        {
                            Id = identityUser.Id,
                            Email = UpsertTeacherVM.Teacher.Email,
                            Name = UpsertTeacherVM.Teacher.Name,
                            PhoneNumber = UpsertTeacherVM.Teacher.PhoneNumber,
                            Address = UpsertTeacherVM.Teacher.Address,
                            Experience = UpsertTeacherVM.Teacher.Experience,
                            Field = UpsertTeacherVM.Teacher.Field,
                            ImageUrl = "\\images\\teachers\\deafultTeacher.svg",

                        };

                        _unitOfWork.Teacher.Add(teacher);

                        _unitOfWork.Save();

                        return RedirectToAction(nameof(Index));

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

                            if (error.Code == "DuplicateUserName")
                            {
                                ModelState.AddModelError(string.Empty,
                                    "البريد الاكتروني مسجل من قبل , يرجى استخدام بريد إلكتروني آخر");
                            }
                        }
                    }
                }
            }

            return View(UpsertTeacherVM);

        }


        #region API_CALLS

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var teacher = _unitOfWork.Teacher.GetFirstOrDefault(t => t.Id == id);

            if (teacher == null)
            {
                return Json(new { success = false, message = "حدث خطأ في عملية الحذف" });
            }

            //remove teacher from identity user
            var identityUser = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(identityUser);


            //remove teacher from teachers table
            _unitOfWork.Teacher.Remove(teacher);
            _unitOfWork.Save();

            return Json(new { success = true, message = "تم حذف المعلم بنجاح" });
        }

        #endregion

    }
}
