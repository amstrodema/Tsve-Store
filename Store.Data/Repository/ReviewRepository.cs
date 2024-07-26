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
    public class ReviewRepository : GenericRepository<Review>, IReview
    {
        public ReviewRepository(DbContext db) : base(db)
        {

        }
        public async Task<IEnumerable<Review>> GetByItemID(Guid itemID)
        {
            return await GetBy(o => o.ItemID == itemID);
        }
        public async Task<Review> GetByItemIDAndUserID(Guid itemID, Guid userID)
        {
            return await GetOneBy(o => o.ItemID == itemID && o.UserID == userID);
        }
        public async Task<Review> GetByItemIDAndUserEmail(Guid itemID, string email)
        {
            return await GetOneBy(o => o.ItemID == itemID &&  o.Email == email);
        }
    }
}
