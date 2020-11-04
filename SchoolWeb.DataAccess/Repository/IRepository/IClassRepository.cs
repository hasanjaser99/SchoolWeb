using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface IClassRepository : IRepository<Class>
    {
        void Update(Class classs);
    }
}
