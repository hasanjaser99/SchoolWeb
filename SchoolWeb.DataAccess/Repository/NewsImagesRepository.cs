using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class NewsImagesRepository : Repository<NewsImages>, INewsImagesRepository
    {
        private readonly ApplicationDbContext _db;

        public NewsImagesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(NewsImages newsImages)
        {
            _db.Update(newsImages);

        }
    }
}
