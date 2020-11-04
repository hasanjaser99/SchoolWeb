using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface IActivityImagesRepository : IRepository<ActivityImages>
    {
        void Update(ActivityImages activityImages);
    }
}
