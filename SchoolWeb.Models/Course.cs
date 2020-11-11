using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models
{
    public class Course
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال اسم المادة")]
        [DisplayName("إسم المادة")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الصف")]
        [DisplayName("الصف")]
        public string Grade { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الفصل الدراسي")]
        [DisplayName("الفصل الدراسي")]
        public int Semester { get; set; }

        //// relations
        public IEnumerable<Class> Classes { get; set; }

        [DisplayName("المعلمون")]
        public IEnumerable<CourseTeachers> CourseTeachers { get; set; }
    }
}
