using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface INewsImagesRepository : IRepository<NewsImages>
    {
        void Update(NewsImages newsImages);
    }
}
