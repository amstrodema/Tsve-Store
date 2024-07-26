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
    public class UserRepository : GenericRepository<User>, IUser
    {
        public UserRepository(DbContext db) : base(db)
        {

        }
        public async Task<User> GetActiveUserByUserName(string username)
        {
            return await GetOneBy(u => u.Username == username && u.IsActive);
        }
        public async Task<User> GetActiveUserByPhone(string phone)
        {
            return await GetOneBy(u => u.Tel == phone && u.IsActive);
        }
        public async Task<User> GetActiveUserByUserID(Guid userID)
        {
            return await GetOneBy(u => u.ID == userID && u.IsActive);
        }
        public async Task<User> GetUserByUserNameOrEmail(string usernameOrEmail)
        {
            return await GetOneBy(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);
        }
        public async Task<User> usernameOrPasswordorPhone(string usernameOrEmailorPhone)
        {
            return await GetOneBy(u => u.Username == usernameOrEmailorPhone || u.Email == usernameOrEmailorPhone|| u.Tel == usernameOrEmailorPhone);
        }
        public async Task<User> GetUserByUserNameOrEmail(string username, string email, string tel)
        {
            return await GetOneBy(u => u.Username == username || u.Email == username || u.Tel == tel);
        }
        public async Task<IEnumerable<User>> GetReferrals(string username)
        {
            return await GetBy(u => u.ReferredBy == username);
        }
    }
}
