using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolWeb.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال إسم الشعبة")]
        [DisplayName("إسم الشعبة")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الصف")]
        [DisplayName("الصف")]
        public string Grade { get; set; }

        // relation
        public string TeacherId { get; set; }

        [ForeignKey("TeacherId")]
        public Teacher Teacher { get; set; }

        public IEnumerable<Class> Classes { get; set; }

        public IEnumerable<Student> Students { get; set; }
    }
}
