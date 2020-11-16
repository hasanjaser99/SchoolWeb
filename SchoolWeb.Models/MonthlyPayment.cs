using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolWeb.Models
{
    public class MonthlyPayment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "يرجى إدخال الشهر")]
        [DisplayName("الشهر")]
        public int Month { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رسوم المدرسة")]
        [DisplayName("رسوم المدرسة")]
        public double SchoolFeesAmount { get; set; }

        [Required(ErrorMessage = "يرجى إدخال رسوم المواصلات")]
        [DisplayName("رسوم المواصلات")]
        public double BusFeesAmount { get; set; }

        [Required(ErrorMessage = "يرجى إدخال حالة الدفع")]
        [DisplayName("حالة الدفع")]
        public Boolean IsPaied { get; set; }


        //// relations
        public int? StudentFeeId { get; set; }

        [ForeignKey("StudentFeeId")]
        public StudentFee StudentFee { get; set; }
    }
}
