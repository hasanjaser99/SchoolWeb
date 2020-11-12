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
            if (semester == 1)
            {
                return "الأول";
            }

            return "الثاني";
        }

        public static string GetClassName(int classNumber)
        {
            switch (classNumber)
            {
                case 1:
                    return "الأولى";
                case 2:
                    return "الثانية";
                case 3:
                    return "الثالثة";
                case 4:
                    return "الرابعة";
                case 5:
                    return "الخامسة";

                default: return "";
            }
        }

        public static string GetDayName(int dayNumber)
        {
            switch (dayNumber)
            {
                case 1:
                    return "الأحد";
                case 2:
                    return "الإثنين";
                case 3:
                    return "الثلاثاء";
                case 4:
                    return "الأربعاء";
                case 5:
                    return "الخميس";

                default: return "";
            }
        }
    }
}
