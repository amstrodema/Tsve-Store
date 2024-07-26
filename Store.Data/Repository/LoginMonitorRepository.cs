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
    public class LoginMonitorRepository : GenericRepository<LoginMonitor>, ILoginMonitor
    {
        public LoginMonitorRepository(DbContext db) : base(db)
        {

        }
        public async Task<LoginMonitor> GetMonitorByUserID(Guid userID)
        {
            return await GetOneBy(p => p.UserID == userID);
        }
        public async Task<LoginMonitor> GetMonitorByUserID(Guid userID, Guid appCode)
        {
            return await GetOneBy(p => p.UserID == userID && p.ClientCode == appCode);
        }
        public async Task<LoginMonitor> GetMonitorByUserIDOnly(Guid userID)
        {
            return await GetOneBy(u => u.UserID == userID);
        }
    }
}
