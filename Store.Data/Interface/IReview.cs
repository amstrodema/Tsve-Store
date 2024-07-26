using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface IReview : IGeneric<Review>
    {
        Task<IEnumerable<Review>> GetByItemID(Guid itemID);
        Task<Review> GetByItemIDAndUserID(Guid itemID, Guid userID);
        Task<Review> GetByItemIDAndUserEmail(Guid itemID, string email);

    }
}
