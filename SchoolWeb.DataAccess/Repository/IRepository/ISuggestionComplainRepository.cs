using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public interface ISuggestionComplainRepository : IRepository<SuggestionComplain>
    {
        void Update(SuggestionComplain suggestionComplain);
    }
}
