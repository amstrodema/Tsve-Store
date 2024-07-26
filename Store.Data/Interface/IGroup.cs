using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface IGroup : IGeneric<Group>
    {
        Task<IEnumerable<Group>> GetByStoreID(Guid storeID);
        Task<Group> GetByGroupTag(string tag, Guid storeID);
        Task<IEnumerable<Group>> GetByGroupsTag(Guid storeID);
    }
}
