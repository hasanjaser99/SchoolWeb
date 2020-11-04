using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolWeb.Models
{
    public class ActivityImages
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال صورة النشاط")]
        [DisplayName("صورة النشاط")]
        public string ImageUrl { get; set; }

        //// relations
        public int ActivityId { get; set; }

        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; }

    }
}
