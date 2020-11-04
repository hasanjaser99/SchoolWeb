using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface IMonthlyPaymentRepository : IRepository<MonthlyPayment>
    {
        void Update(MonthlyPayment monthlyPayment);
    }
}
