using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface ISchoolFeeRepository : IRepository<SchoolFee>
    {
        void Update(SchoolFee schoolFee);
    }
}
