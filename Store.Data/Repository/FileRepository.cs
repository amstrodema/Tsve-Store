using Microsoft.EntityFrameworkCore;
using Store.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.Repository
{
    public class FileRepository : GenericRepository<Model.File>, IFile
    {
        public FileRepository(DbContext db) : base(db)
        {

        }
    }
}
