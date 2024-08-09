using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface IOrder : IGeneric<Order>
    {
        Task<Order> GetByOrderRef(string orderRef);
    }
}
