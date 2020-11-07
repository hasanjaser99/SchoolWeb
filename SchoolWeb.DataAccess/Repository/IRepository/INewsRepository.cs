using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public interface INewsRepository : IRepository<News>
    {
        void Update(News news);
    }
}
