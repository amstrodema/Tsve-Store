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
    public class TransactionRepository : GenericRepository<Transaction>, ITransaction
    {
        public TransactionRepository(DbContext db) : base(db)
        {

        }
    }
}
