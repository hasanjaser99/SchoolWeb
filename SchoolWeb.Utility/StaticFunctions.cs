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

            return "روضة";
        }
    }
}
