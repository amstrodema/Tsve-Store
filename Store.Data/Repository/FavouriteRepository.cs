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
    public class FavouriteRepository : GenericRepository<Favourite>, IFavourite
    {
        public FavouriteRepository(DbContext db) : base(db)
        {

        }
        public async Task<Favourite> GetByUserAndItemID(Guid itemID, Guid userID)
        {
            return await GetOneBy(o => o.ItemID == itemID && o.UserID == userID);
        }
        public async Task<IEnumerable<Favourite>> GetByUserID(Guid userID)
        {
            return await GetBy(o =>o.UserID == userID);
        }
    }
}
