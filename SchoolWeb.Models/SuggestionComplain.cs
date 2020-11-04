using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models
{
    public class SuggestionComplain
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال عنوان الإقتراح/الشكوى")]
        [DisplayName("عنوان الإقتراح/الشكوى")]
        public string Title { get; set; }

        [Required(ErrorMessage = "يرجى إدخال تفاصيل الإقتراح/الشكوى")]
        [DisplayName("تفاصيل الإقتراح/الشكوى")]
        public string Description { get; set; }

        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني")]
        [DisplayName("البريد الإلكتروني")]
        [EmailAddress]
        public string ParentEmail { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رقم الهاتف")]
        [DisplayName("رقم الهاتف")]
        [EmailAddress]
        public string PhoneNumber { get; set; }

    }
}
