using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolWeb.Models
{
    public class Student
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم الطالب باللغة العربية")]
        [DisplayName("الإسم باللغة العربية")]
        public string ArabicName { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم الطالب باللغة الإنجليزية")]
        [DisplayName("الإسم باللغة الإنجليزية")]
        public string EnglishName { get; set; }

        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني لولي الأمر")]
        [DisplayName("البريد الإلكتروني لولي الأمر")]
        [EmailAddress]
        public string ParentEmail { get; set; }

        [Required(ErrorMessage = "يرجى إدخال العنوان")]
        [DisplayName("عنوان السكن")]
        public string Address { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الصف")]
        [DisplayName("الصف")]
        public string Grade { get; set; }



        // relations
        public int SectionId { get; set; }

        [ForeignKey("SectionId")]
        public Section Section { get; set; }

        public int StudentFeeId { get; set; }

        [ForeignKey("StudentFeeId")]
        public StudentFee StudentFee { get; set; }

        public IEnumerable<Course> Courses { get; set; }
    }
}
