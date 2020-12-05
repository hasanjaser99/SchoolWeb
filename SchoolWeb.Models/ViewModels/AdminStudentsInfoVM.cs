using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class AdminStudentsInfoVM
    {
        public IEnumerable<SelectListItem> Grades { get; set; }

        public IEnumerable<SelectListItem> Sections { get; set; }

    }
}
