using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class ClassRepository : Repository<Class>, IClassRepository
    {
        private readonly ApplicationDbContext _db;

        public ClassRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Class classs)
        {
            _db.Update(classs);

        }
    }
}
