using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class CourseTeachersRepository : Repository<CourseTeachers>, ICourseTeachersRepository
    {
        private readonly ApplicationDbContext _db;

        public CourseTeachersRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CourseTeachers courseTeachers)
        {
            _db.Update(courseTeachers);

        }
    }
}
