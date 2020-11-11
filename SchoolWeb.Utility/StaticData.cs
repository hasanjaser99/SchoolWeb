using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Utility
{
    public static class StaticData
    {

        // users roles 

        public const string Role_Waiting = "Waiting";

        public const string Role_Student = "Student";

        public const string Role_Teacher = "Teacher";

        public const string Role_Admin = "Admin";


        // dropDown data 
        public static List<SelectListItem> GradesList = new List<SelectListItem>() {
                     new SelectListItem{ Text="تمهيدي", Value="KG1"},
                     new SelectListItem{ Text="روضة", Value="KG2"},
        };

        public static List<SelectListItem> SelectedGradesList = new List<SelectListItem>() {
                     new SelectListItem{ Text = "جميع المستويات الدراسية", Value="All"
                         , Selected = true},
                     new SelectListItem{ Text="تمهيدي", Value="KG1"},
                     new SelectListItem{ Text="روضة", Value="KG2"},
        };


        public static List<SelectListItem> BussStatesList = new List<SelectListItem>() {
                    new SelectListItem { Text = "ذهاب", Value="Go"},
                    new SelectListItem { Text = "عودة", Value="Back"},
                    new SelectListItem { Text = "ذهاب و عودة", Value="GoAndBack"},
                    new SelectListItem { Text = "بدون مواصلات", Value="without"},
        };



        public static List<SelectListItem> SemestersList = new List<SelectListItem>() {
                    new SelectListItem { Text = "الفصل الأول", Value="1"},
                    new SelectListItem { Text = "الفصل الثاني", Value="2"},
        };



        // static info 

        public const string aboutUsText = " يسعدنا دعوتكم لتسجيل أطفالكم الأحبة في الروضة ،حيث التعليم المميز والأمثل. بادارة ومعلمات قديرات ومتخصصات نهتم بالتربية الاسلامية وتعليم الانجليزية والكمبيوتر. نتميز بالموقع والمساحات والغرف الصفية الواسعة وبالحدائق.  ألعاب داخلية و خارجية ممتعة. نهتم بالنشاطات المختلفة والتواصل مع أولياء الأمور.";
        public const string regestirationDetails = " يسعدنا دعوتكم لتسجيل أطفالكم الأحبة في الروضة ،حيث التعليم المميز والأمثل. بادارة ومعلمات قديرات ومتخصصات نهتم بالتربية الاسلامية وتعليم الانجليزية والكمبيوتر. نتميز بالموقع والمساحات والغرف الصفية الواسعة وبالحدائق.  ألعاب داخلية و خارجية ممتعة. نهتم بالنشاطات المختلفة والتواصل مع أولياء الأمور.";


    }
}
