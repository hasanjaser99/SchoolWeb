using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
 
namespace SchoolWeb.Models.ViewModels
{
    public class FeesOfRegisterationVM
    {
        public IEnumerable<SchoolFee> SchoolFees { get; set; }

        public SchoolFee schoolFeeItem { get; set; }

        public IEnumerable<SelectListItem> Grades { get; set; }

    }
}
