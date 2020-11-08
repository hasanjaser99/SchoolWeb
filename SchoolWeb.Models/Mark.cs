using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SchoolWeb.Models
{
    public class Mark
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("الشهر الأول")]
        public int FirstMark { get; set; }

        [DisplayName("الشهر الثاني")]
        public int SecondMark { get; set; }

        [DisplayName("المشاركة")]
        public int AssignmentsMark { get; set; }

        [DisplayName("النهائي")]
        public int FinalMark { get; set; }

        // relations
        public int? CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

        public string StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; }
    }
}
