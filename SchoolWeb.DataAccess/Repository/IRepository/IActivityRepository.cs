using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public interface IActivityRepository : IRepository<Activity>
    {
        void Update(Activity activity);
    }
}
