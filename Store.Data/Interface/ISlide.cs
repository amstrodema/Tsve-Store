using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface ISlide : IGeneric<Slide>
    {
        Task<IEnumerable<Slide>> GetByStoreID(Guid storeID);
    }
}
