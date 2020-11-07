using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public class MonthlyPaymentRepository : Repository<MonthlyPayment>, IMonthlyPaymentRepository
    {
        private readonly ApplicationDbContext _db;

        public MonthlyPaymentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(MonthlyPayment monthlyPayment)
        {
            _db.Update(monthlyPayment);

        }
    }
}
