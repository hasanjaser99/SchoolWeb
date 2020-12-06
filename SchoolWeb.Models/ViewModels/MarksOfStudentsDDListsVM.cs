using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class MarksOfStudentsDDListsVM
    {
        public IEnumerable<SelectListItem> CoursesList { get; set; }
        public IEnumerable<SelectListItem> GradesList { get; set; }
        public IEnumerable<SelectListItem> SectionsList { get; set; }
        public IEnumerable<SelectListItem> SemestersList { get; set; }


    }
}
