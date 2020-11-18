using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class AdminStudentDetailsVM
    {
        public Student Student { get; set; }

        public IEnumerable<Mark> Marks { get; set; }

        public IEnumerable<MonthlyPayment> MonthlyPayments { get; set; }

        public IEnumerable<SelectListItem> Semesters { get; set; }

        public string SelectedSemester { get; set; }

    }
}
