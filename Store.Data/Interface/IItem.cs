using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface IItem : IGeneric<Item>
    {
        Task<IEnumerable<Item>> GetByCategID(Guid catID);
        Task<IEnumerable<Item>> GetByStoreID(Guid storeID);
        Task<Item> GetByTag(string tag);
        Task<IEnumerable<Item>> GetFeatured();
        Task<IEnumerable<Item>> GetLatest();
        Task<Item> GetByOfferID(Guid offerID);
        Task<IEnumerable<Item>> GetByStoreIDAndCurrency(string currency);
    }
}
