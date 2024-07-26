using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Model.ViewModel
{
    public class GroupVM
    {
        public Group Group { get; set; }
        public IEnumerable <Category> Categories { get; set; }
    }
}
