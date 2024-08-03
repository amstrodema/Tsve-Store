using Microsoft.EntityFrameworkCore;
using Store.Data.Interface;
using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Repository
{
    public class KnowledgeRepository : GenericRepository<Knowledge>, IKnowledge
    {
        public KnowledgeRepository(DbContext db) : base(db)
        {

        }

        public async Task<Knowledge> GetByTag(string tag)
        {
            return await GetOneBy(o => o.Tag == tag);
        }
    }
}
