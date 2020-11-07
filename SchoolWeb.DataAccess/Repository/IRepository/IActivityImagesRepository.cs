using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public interface IActivityImagesRepository : IRepository<ActivityImages>
    {
        void Update(ActivityImages activityImages);
    }
}
