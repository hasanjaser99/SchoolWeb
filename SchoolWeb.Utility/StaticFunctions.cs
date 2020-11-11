using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.Utility
{
    public static class StaticFunctions
    {
        public static string GetGrade(string grade)
        {
            if (grade.ToUpper() == "KG1")
            {
                return "تمهيدي";
            }

            return "بستان";
        }

        public static string GetSemester(int semester)
        {
            if(semester == 1)
            {
                return "الأول";
            }

            return "الثاني";
        }
    }
}
