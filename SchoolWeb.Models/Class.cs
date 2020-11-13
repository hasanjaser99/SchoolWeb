using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolWeb.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رقم الحصة")]
        [DisplayName("رقم الحصة")]
        public int ClassNumber { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اليوم")]
        [DisplayName("اليوم")]
        public int Day { get; set; }

        //// relations
        public int? SectionId { get; set; }

        [ForeignKey("SectionId")]
        public Section Section { get; set; }

        public string TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }

        public int? CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}
