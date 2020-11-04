using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface IStudentRepository : IRepository<Student>
    {
        void Update(Student student);
    }
}
