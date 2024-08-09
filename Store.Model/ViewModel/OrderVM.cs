using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Model.ViewModel
{
    public class OrderVM
    {
        public Guid ID { get; set; } //item id
        public string ItemName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Ref { get; set; } = string.Empty;
        public string CurrenySymbol { get; set; } = string.Empty;
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public bool IsFinished { get; set; }
        public bool IsPaid { get; set; }
        public ItemFeature[] Features { get; set; }
        public Feature[] ItemFeatures { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
