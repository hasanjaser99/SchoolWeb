using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class ClassScheduleVM
    {
        public IEnumerable<Teacher> Teachers { get; set; }

        public IEnumerable<Class> Classes { get; set; }

    }
}
