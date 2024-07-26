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
    public class FeatureRepository : GenericRepository<Feature>, IFeature
    {
        public FeatureRepository(DbContext db) : base(db)
        {

        }
        public async Task<IEnumerable<Feature>> GetByItemID(Guid itemID)
        {
            return await GetBy(o => o.ItemID == itemID);
        }
    }
}
