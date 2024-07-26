using Store.Model.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Store.Model.ViewModel
{
    public class ItemVM
    {
        public string Title { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public Guid ID { get; set; }
        public Guid Category { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public IEnumerable<FeaturePicker>? Features { get; set; }
        public string Img1 { get; set; } = string.Empty;
        public string Img2 { get; set; } = string.Empty;
        public string Img3 { get; set; } = string.Empty;
        public string Img4 { get; set; } = string.Empty;
        public string Img5 { get; set; } = string.Empty;
    }
    public class FeaturePicker
    {
        //public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;
    }
}
