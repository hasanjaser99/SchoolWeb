using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class NewsRepository : Repository<News>, INewsRepository
    {
        private readonly ApplicationDbContext _db;

        public NewsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(News news)
        {
            _db.Update(news);

        }
    }
}
