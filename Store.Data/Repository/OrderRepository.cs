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
    public class OrderRepository : GenericRepository<Order>, IOrder
    {
        public OrderRepository(DbContext db) : base(db)
        {

        }
        public async Task<IEnumerable<Order>> GetByStoreID(Guid storeID)
        {
            return await GetBy(o => o.StoreID == storeID);
        }
        public async Task<Order> GetByOrderRef(Guid storeID, string orderRef)
        {
            return await GetOneBy(o => o.StoreID == storeID && o.Ref == orderRef);
        }
    }
}
