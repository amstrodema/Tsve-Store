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
    public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItem
    {
        public OrderItemRepository(DbContext db) : base(db)
        {

        }
        public async Task<IEnumerable<OrderItem>> GetByStoreID(Guid storeID)
        {
            return await GetBy(o => o.StoreID == storeID);
        }
        public async Task<IEnumerable<OrderItem>> GetByStoreIDAndOrderID(Guid storeID, Guid orderID)
        {
            return await GetBy(o => o.StoreID == storeID && o.OrderID  == orderID);
        }
    }
}
