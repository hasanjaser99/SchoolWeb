using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class StudentRegisterVM
    {
        public Student Student { get; set; }

        [Required(ErrorMessage = "يرجى إدخال كلمة المرور")]
        [StringLength(100, ErrorMessage = "كلمة المرور يجب ان تتكون من 6 حروف الى 100 حرف", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "الرجاء التأكد من تطابق كلمة المرور")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<SelectListItem> Grades { get; set; }
        public IEnumerable<SelectListItem> BussStates { get; set; }

        [Required(ErrorMessage = "يرجى إرفاق صورة عن شهادة الميلاد")]
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }


    }
}
