using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public class ActivitiyImagesRepository : Repository<ActivityImages>, IActivityImagesRepository
    {
        private readonly ApplicationDbContext _db;

        public ActivitiyImagesRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ActivityImages activityImages)
        {
            _db.Update(activityImages);

        }
    }
}
