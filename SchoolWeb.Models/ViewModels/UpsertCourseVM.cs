using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class UpsertCourseVM
    {
        public Course Course { get; set; }

        public IEnumerable<SelectListItem> Grades { get; set; }

        public IEnumerable<SelectListItem> Semesters { get; set; }

        public IEnumerable<SelectListItem> Teachers { get; set; }

        public IEnumerable<string> SelectedTeachers { get; set; }

        public IEnumerable<SelectListItem> StaticCoursesName { get; set; }
    }
}
