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
    public class ItemRepository : GenericRepository<Item>, IItem
    {
        public ItemRepository(DbContext db) : base(db)
        {

        }
        public async Task<IEnumerable<Item>> GetByCategID(Guid catID)
        {
            return await GetBy(o => o.CatID == catID);
        }
        public async Task<Item> GetByTag(string tag)
        {
            return await GetOneBy(o => o.Tag == tag);
        }
        public async Task<IEnumerable<Item>> GetByStoreID(Guid storeID)
        {
            return await GetBy(o =>  o.StoreID == storeID);
        }
        public async Task<IEnumerable<Item>> GetByStoreIDAndCurrency( string currency)
        {
            return await GetBy(o =>  o.Currency == currency);
        }
        public async Task<IEnumerable<Item>> GetFeatured()
        {
            return await GetBy(o =>  o.IsFeatured);
        }
        public async Task<IEnumerable<Item>> GetLatest()
        {
            return await GetBy(o =>   o.IsRecent);
        }
        public async Task<Item> GetByOfferID(Guid offerID)
        {
            return await GetOneBy(p => p.OfferID == offerID);
        }
    }
}
