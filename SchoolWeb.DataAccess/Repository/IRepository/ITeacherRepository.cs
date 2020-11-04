using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        void Update(Teacher teacher);
    }
}
