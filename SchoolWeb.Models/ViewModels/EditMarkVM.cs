using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SchoolWeb.Models.ViewModels
{
    public class EditMarkVM
    {
        public int Id { get; set; }
        
        [Range(0, Int32.MaxValue, ErrorMessage = "يجب ان تكون العلامة اكبر أو تساوي 0")]
        [DisplayName("الشهر الأول")]
        public int FirstMark { get; set; }

        [Range(0, Int32.MaxValue, ErrorMessage = "يجب ان تكون العلامة اكبر أو تساوي 0")]
        [DisplayName("الشهر الثاني")]
        public int SecondMark { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "يجب ان تكون العلامة اكبر أو تساوي 0")]
        [DisplayName("المشاركة")]
        public int AssignmentsMark { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "يجب ان تكون العلامة اكبر أو تساوي 0")]
        [DisplayName("النهائي")]
        public int FinalMark { get; set; }

        [DisplayName("إسم الطالب")]
        public string StudentName { get; set; }



    }
}
