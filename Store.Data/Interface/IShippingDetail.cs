using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface IShippingDetail : IGeneric<ShippingDetail>
    {
        Task<ShippingDetail> GetByOrder(Guid orderID);
    }
}
