using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public interface ICourseTeachersRepository : IRepository<CourseTeachers>
    {
        void Update(CourseTeachers courseTeachers);
    }
}
