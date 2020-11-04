using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models
{
    public class SchoolFee
    {
        [Key]
        [DisplayName("المستوى الدراسي")]
        public string Grade { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الرسوم")]
        [DisplayName("إجمالي الرسوم")]
        public int SchoolFees { get; set; }


    }
}
