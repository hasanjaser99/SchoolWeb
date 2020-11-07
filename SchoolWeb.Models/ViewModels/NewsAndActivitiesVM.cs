using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class NewsAndActivitiesVM
    {
        public IEnumerable<News> ListOfNews { get; set; }

        public IEnumerable<Activity> ListOfActivities { get; set; }
    }
}
