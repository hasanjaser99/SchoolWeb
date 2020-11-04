using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class MarkRepository : Repository<Mark>, IMarkRepository
    {
        private readonly ApplicationDbContext _db;

        public MarkRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Mark mark)
        {
            _db.Update(mark);

        }
    }
}
