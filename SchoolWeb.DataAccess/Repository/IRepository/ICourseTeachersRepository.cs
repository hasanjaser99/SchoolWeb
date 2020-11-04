using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface ICourseTeachersRepository : IRepository<CourseTeachers>
    {
        void Update(CourseTeachers courseTeachers);
    }
}
