using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class AdminUpsertStudentVM
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

        public IEnumerable<SelectListItem> Sections { get; set; }

        [Required(ErrorMessage = "يرجى ادخال الشعبة")]
        public int SelectedSection { get; set; }

        [Display(Name = "نسبة الخصم بالنسبة المئوية (اختياري)")]
        [Range(0, 100, ErrorMessage = "نسبة الخصم بين 0% الى 100%")]
        public int Discount { get; set; }

        [Required(ErrorMessage = "يرجى إرفاق صورة عن شهادة الميلاد")]
        [DataType(DataType.Upload)]
        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رسوم المواصلات")]
        [Range(0, 1000, ErrorMessage = "رسوم المواصلات بين 0 دينار الى 1000 دينار")]
        [DisplayName("رسوم المواصلات")]
        public int BusFees { get; set; }


    }
}
