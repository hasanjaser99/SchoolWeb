using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class TeacherRepository : Repository<Teacher>, ITeacherRepository
    {
        private readonly ApplicationDbContext _db;

        public TeacherRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Teacher teacher)
        {
            _db.Update(teacher);

        }
    }
}
