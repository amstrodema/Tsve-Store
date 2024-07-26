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
    public class GroupRepository : GenericRepository<Group>, IGroup
    {
        public GroupRepository(DbContext db) : base(db)
        {

        }
        public async Task<IEnumerable<Group>> GetByStoreID(Guid storeID)
        {
            return await GetBy(p => p.StoreID == storeID);
        }

        public async Task<Group> GetByGroupTag(string tag, Guid storeID)
        {
            return await GetOneBy(p => p.Tag == tag && p.StoreID == storeID);
        }

        public async Task<IEnumerable<Group>> GetByGroupsTag(Guid storeID)
        {
            return await GetBy(p => p.StoreID == storeID);
        }

    }
}
