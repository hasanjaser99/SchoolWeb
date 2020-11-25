using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolWeb.Models
{
    public class StudentFee
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رسوم المواصلات")]
        [DisplayName("رسوم المواصلات")]
        public int BusFees { get; set; }

        [DisplayName("نسبة الخصم")]
        public int Discount { get; set; }

        //// relations

        public IEnumerable<MonthlyPayment> MonthlyPayments { get; set; }
    }
}
