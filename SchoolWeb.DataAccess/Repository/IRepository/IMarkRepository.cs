using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface IMarkRepository : IRepository<Mark>
    {
        void Update(Mark mark);
    }
}
