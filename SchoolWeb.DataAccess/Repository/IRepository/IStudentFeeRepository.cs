using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface IStudentFeeRepository : IRepository<StudentFee>
    {
        void Update(StudentFee studentFee);
    }
}
