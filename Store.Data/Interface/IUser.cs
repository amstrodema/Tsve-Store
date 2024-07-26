using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface IUser : IGeneric<User>
    {
        Task<User> GetActiveUserByUserName(string username);
        Task<User> GetUserByUserNameOrEmail(string usernameOrEmail);
        Task<User> GetUserByUserNameOrEmail(string username, string email, string tel);
        Task<User> GetActiveUserByUserID(Guid userID);
        Task<IEnumerable<User>> GetReferrals(string username);
        Task<User> GetActiveUserByPhone(string phone);
        Task<User> usernameOrPasswordorPhone(string usernameOrEmailorPhone);
    }
}
