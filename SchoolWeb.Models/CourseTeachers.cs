using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Models
{
    public class CourseTeachers
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
