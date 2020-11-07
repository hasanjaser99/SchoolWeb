using SchoolWeb.Data;
using SchoolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchoolWeb.DataAccess.Repository
{
    public class SuggestionComplainRepository : Repository<SuggestionComplain>, ISuggestionComplainRepository
    {
        private readonly ApplicationDbContext _db;

        public SuggestionComplainRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SuggestionComplain suggestionComplain)
        {
            _db.Update(suggestionComplain);

        }
    }
}
