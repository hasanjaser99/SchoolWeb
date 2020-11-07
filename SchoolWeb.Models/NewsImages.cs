using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolWeb.Models
{
    public class NewsImages
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال صورة الخبر")]
        [DisplayName("صورة الخبر")]
        public string ImageUrl { get; set; }

        //// relations
        public int? NewsId { get; set; }

        [ForeignKey("NewsId")]
        public News News { get; set; }
    }
}
