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
    public class KeyRepository : GenericRepository<Key>, IKey
    {
        public KeyRepository(DbContext db) : base(db)
        {

        }
    }
}
