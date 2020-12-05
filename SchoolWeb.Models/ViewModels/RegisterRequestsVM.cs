using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class RegisterRequestsVM
    {

        // For Accept Modal
        public IEnumerable<SelectListItem> Grades { get; set; }

        public IEnumerable<Section> Sections { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الشعبة")]
        public int SelectedSection { get; set; }

        [Required(ErrorMessage = "يرجى إدخال المستوى الدراسي")]
        public string SelectedGrade { get; set; }

        [Required]
        public string StudentId { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رسوم المواصلات")]
        [DisplayName("رسوم المواصلات")]
        [Range(0, 1000, ErrorMessage = "رسوم المواصلات بين 0 دينار الى 1000 دينار")]
        public int BusFees { get; set; }

        [DisplayName("نسبة الخصم")]
        [Range(0, 100, ErrorMessage = "نسبة الخصم بين 0% الى 100%")]
        public int Discount { get; set; }

    }
}
