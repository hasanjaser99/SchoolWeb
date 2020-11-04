using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface INewsRepository : IRepository<News>
    {
        void Update(News news);
    }
}
