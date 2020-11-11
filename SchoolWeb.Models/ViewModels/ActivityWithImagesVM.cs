using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
 
namespace SchoolWeb.Models.ViewModels
{
    public class ActivityWithImagesVM
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال عنوان النشاط")]
        [DisplayName("عنوان النشاط")]
        public string Title { get; set; }

        [Required(ErrorMessage = "يرجى إدخال تفاصيل النشاط")]
        [DisplayName("تفاصيل النشاط")]
        public string Description { get; set; }

        [Required(ErrorMessage = "يرجى إدخال تاريخ النشاط")]
        [DisplayName("تاريخ النشاط")]
        public DateTime Date { get; set; }

        [DisplayName("صور النشاط")]
        public IEnumerable<ActivityImages> ActivityImages { get; set; }

        [Required(ErrorMessage = "يرجى إرفاق صور للنشاط")]
        [DataType(DataType.Upload)]
        public IEnumerable<IFormFile> ActivityImagesFiles { get; set; }


    }
}
