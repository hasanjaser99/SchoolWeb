using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public class SchoolFeeRepository : Repository<SchoolFee>, ISchoolFeeRepository
    {
        private readonly ApplicationDbContext _db;

        public SchoolFeeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SchoolFee schoolFee)
        {
            _db.Update(schoolFee);

        }
    }
}
