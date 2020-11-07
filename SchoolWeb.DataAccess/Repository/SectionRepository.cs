using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public class SectionRepository : Repository<Section>, ISectionRepository
    {
        private readonly ApplicationDbContext _db;

        public SectionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Section section)
        {
            _db.Update(section);

        }
    }
}
