using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Model.ViewModel
{
    public class CheckOutVM
    {
        public IEnumerable<OrderVM> Orders { get; set; }
        public BillingDetail BillingDetail { get; set; }
        public ShippingDetail ShippingDetail { get; set; }
        public bool IsNewsletter { get; set; }
        public string? Cart { get; set; }
        public bool IsCreateAccount { get; set; }
        public bool IsDifferentShipping { get; set; }
    }
}
