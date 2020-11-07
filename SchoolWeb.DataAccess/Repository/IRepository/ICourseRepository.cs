using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public interface ICourseRepository : IRepository<Course>
    {
        void Update(Course course);
    }
}
