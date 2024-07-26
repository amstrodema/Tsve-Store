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
    public class CategoryRepository : GenericRepository<Category>, ICategory
    {
        public CategoryRepository(DbContext db) : base(db)
        {

        }
        public async Task<Category> GetByCategoryTag(string tag)
        {
            return await GetOneBy(p => p.Tag == tag);
        }
    }
}
