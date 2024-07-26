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
    public class ItemFeatureRepository : GenericRepository<ItemFeature>, IItemFeature
    {
        public ItemFeatureRepository(DbContext db) : base(db)
        {

        }
    }
}
