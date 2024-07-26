using Store.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Interface
{
    public interface IFeature : IGeneric<Feature>
    {
        Task<IEnumerable<Feature>> GetByItemID(Guid itemID);
    }
}
