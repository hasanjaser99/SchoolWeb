using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public interface ISectionRepository : IRepository<Section>
    {
        void Update(Section section);
    }
}
