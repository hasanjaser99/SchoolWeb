using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public class StudentFeeRepository : Repository<StudentFee>, IStudentFeeRepository
    {
        private readonly ApplicationDbContext _db;

        public StudentFeeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(StudentFee studentFee)
        {
            _db.Update(studentFee);

        }
    }
}
