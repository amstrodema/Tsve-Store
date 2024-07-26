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
    public class OfferRepository : GenericRepository<Offer>, IOffer
    {
        public OfferRepository(DbContext db) : base(db)
        {

        }

        public async Task<IEnumerable<Offer>> GetByStoreID(Guid storeID)
        {
            return await GetBy(o => o.StoreID == storeID);
        }
    }
}
