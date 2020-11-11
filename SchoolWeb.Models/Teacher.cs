using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models
{
    public class Teacher
    {

        public string Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال إسم المعلم")]
        [DisplayName("إسم المعلم")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رقم الهاتف")]
        [DisplayName("رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني")]
        [DisplayName("البريد الإلكتروني")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "يرجى إدخال المجال")]
        [DisplayName("المجال")]
        public string Field { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الخبرة")]
        [DisplayName("الخبرة")]
        public string Experience { get; set; }

        [Required(ErrorMessage = "يرجى إدخال العنوان")]
        [DisplayName("عنوان السكن")]
        public string Address { get; set; }

        [DisplayName("الصورة")]
        public string ImageUrl { get; set; }


        // relations
        public IEnumerable<Section> Sections { get; set; }

        public IEnumerable<Class> Classes { get; set; }

        public IEnumerable<CourseTeachers> CourseTeachers { get; set; }
    }
}
