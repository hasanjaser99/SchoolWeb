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
        [DisplayName("إسم الطالب الرباعي (بالعربية)")]
        public string ArabicName { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم الطالب باللغة الإنجليزية")]
        [DisplayName("إسم الطالب الرباعي (بالإنجليزية)")]
        public string EnglishName { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رقم هاتف ولي الأمر")]
        [DisplayName("رقم هاتف ولي الأمر")]
        public string ParentPhoneNumber { get; set; }

        [Required(ErrorMessage = "يرجى إدخال البريد الإلكتروني لولي الأمر")]
        [DisplayName("البريد الإلكتروني لولي الأمر")]
        [EmailAddress]
        public string ParentEmail { get; set; }

        [Required(ErrorMessage = "يرجى إدخال مكان السكن")]
        [DisplayName("مكان السكن")]
        public string Address { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الصف")]
        [DisplayName("الصف")]
        public string Grade { get; set; }

        [DisplayName("صورة عن شهادة الميلاد")]
        public string BornCertificateImage { get; set; }

        [Required(ErrorMessage = "يرجى إدخال طريقة المواصلات")]
        [DisplayName("المواصلات")]
        public string BussState { get; set; }


        // relations

        [DisplayName("الشعبة")]
        public int? SectionId { get; set; }

        [ForeignKey("SectionId")]
        public Section Section { get; set; }

        public int? StudentFeeId { get; set; }

        [ForeignKey("StudentFeeId")]
        public StudentFee StudentFee { get; set; }

    }
}
