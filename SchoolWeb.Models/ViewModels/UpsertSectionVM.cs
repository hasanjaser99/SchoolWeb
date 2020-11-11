using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class UpsertSectionVM
    {
        public Section Section { get; set; }

        public IEnumerable<SelectListItem> Grades { get; set; }

        public IEnumerable<SelectListItem> Teachers { get; set; }


    }
}
