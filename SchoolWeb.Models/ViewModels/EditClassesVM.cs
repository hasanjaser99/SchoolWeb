using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class EditClassesVM
    {
        public IEnumerable<Class> Classes { get; set; }

        [DisplayName("رقم الحصة")]
        public IEnumerable<SelectListItem> ClassNumbers { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال رقم الحصة")]
        public int SelectedClassNumber { get; set; }

        [DisplayName("اليوم")]
        public IEnumerable<SelectListItem> Days { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال اليوم")]
        public int SelectedDay { get; set; }

        [DisplayName("المعلم")]
        public IEnumerable<SelectListItem> Teachers { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال المعلم")]
        public string SelectedTeacher { get; set; }

        [DisplayName("المادة")]
        public IEnumerable<SelectListItem> Courses { get; set; }

        [Required(ErrorMessage = "الرجاء ادخال المادة")]
        public int SelectedCourse { get; set; }

        public int SectionId { get; set; }


    }
}
