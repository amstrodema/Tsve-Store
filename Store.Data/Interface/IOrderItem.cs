using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface IOrderItem : IGeneric<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetByStoreID(Guid storeID);
        Task<IEnumerable<OrderItem>> GetByStoreIDAndOrderID(Guid storeID, Guid orderID);
    }
}
