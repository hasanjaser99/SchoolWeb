using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public interface INewsImagesRepository : IRepository<NewsImages>
    {
        void Update(NewsImages newsImages);
    }
}
