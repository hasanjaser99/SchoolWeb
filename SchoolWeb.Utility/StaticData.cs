using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Utility
{
    public static class StaticData
    {
        public const string Role_Waiting = "Waiting";

        public const string Role_Student = "Student";

        public const string Role_Teacher = "Teacher";

        public const string Role_Admin = "Admin";

        public static List<SelectListItem> GradesList = new List<SelectListItem>() {
                     new SelectListItem{ Text="تمهيدي", Value="KG1"},
                     new SelectListItem{ Text="روضة", Value="KG2"},
        };

        public static List<SelectListItem> BussStatesList = new List<SelectListItem>() {
                    new SelectListItem { Text = "ذهاب", Value="Go"},
                    new SelectListItem { Text = "عودة", Value="Back"},
                    new SelectListItem { Text = "ذهاب و عودة", Value="GoAndBack"},
                    new SelectListItem { Text = "بدون مواصلات", Value="without"},
        };
    }
}
