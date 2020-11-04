using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال عنوان النشاء")]
        [DisplayName("عنوان النشاط")]
        public string Title { get; set; }

        [Required(ErrorMessage = "يرجى إدخال تفاصيل النشاط")]
        [DisplayName("تفاصيل النشاط")]
        public string Description { get; set; }

        [Required(ErrorMessage = "يرجى إدخال تاريخ النشاط")]
        [DisplayName("تاريخ النشاط")]
        public DateTime Date { get; set; }

        //// relations
        public IEnumerable<ActivityImages> ActivityImages { get; set; }
    }
}
