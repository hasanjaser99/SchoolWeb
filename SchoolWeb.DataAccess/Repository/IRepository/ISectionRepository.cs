using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface ISectionRepository : IRepository<Section>
    {
        void Update(Section section);
    }
}
