using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        private readonly ApplicationDbContext _db;

        public ActivityRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Activity activity)
        {
            _db.Update(activity);

        }
    }
}
