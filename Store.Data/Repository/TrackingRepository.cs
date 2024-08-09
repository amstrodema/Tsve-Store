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
    public class TrackingRepository : GenericRepository<Tracking>, ITracking
    {
        public TrackingRepository(DbContext db) : base(db)
        {

        }

        public async Task<IEnumerable<Tracking>> GetByOrderID(Guid orderID)
        {
            return await GetBy(o => o.OrderID == orderID);
        }
    }
}
