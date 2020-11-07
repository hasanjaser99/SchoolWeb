using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public interface ITeacherRepository : IRepository<Teacher>
    {
        void Update(Teacher teacher);
    }
}
