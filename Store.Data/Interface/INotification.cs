using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface INotification : IGeneric<Notification>
    {
        Task<int> CheckForNotification(Guid userID);
    }
}
