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

        [Required(ErrorMessage = "يرجى إدخال نسبة الخصم")]
        [DisplayName("نسبة الخصم")]
        public double Discount { get; set; }

        //// relations
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        public IEnumerable<MonthlyPayment> MonthlyPayments { get; set; }
    }
}
