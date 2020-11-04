using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال عنوان الخبر")]
        [DisplayName("عنوان الخبر")]
        public string Title { get; set; }

        [Required(ErrorMessage = "يرجى إدخال تفاصيل الخبر")]
        [DisplayName("تفاصيل الخبر")]
        public string Description { get; set; }

        [Required(ErrorMessage = "يرجى إدخال تاريخ الخبر")]
        [DisplayName("تاريخ الخبر")]
        public DateTime Date { get; set; }

        //// relations
        public IEnumerable<NewsImages> NewsImages { get; set; }
    }
}
