using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class CoursesIndexVM
    {
        public IEnumerable<Course> Courses { get; set; }

        public IEnumerable<SelectListItem> Grades { get; set; }

    }
}
