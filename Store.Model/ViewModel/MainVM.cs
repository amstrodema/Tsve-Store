using Store.Model.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Model.ViewModel
{
    public class MainVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<CategoryHybrid> CategoryHybrids { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
        public IEnumerable<OrderVM> OrderVMs { get; set; }
        public IEnumerable<File> Files { get; set; }
        public IEnumerable<Feature> Features { get; set; }
        public IEnumerable<Item> Stocks { get; set; }
        public IEnumerable<Item> Featured { get; set; }
        public IEnumerable<Item> Favourite { get; set; }
        public string Faves { get; set; }
        public IEnumerable<Item> Latest { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<Group> Groups { get; set; }
        public IEnumerable<Slide> Slides { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public IEnumerable<dynamic> dynamics { get; set; }
        public List<Offer> Offers { get; set; }
        public Order Order { get; set; }
        public Group Group { get; set; }
        public Item Stock { get; set; }
        public Category Category { get; set; }
        public Offer? Offer { get; set; }
        //public Offer? Offer3 { get; set; }
        //public Offer? Offer4 { get; set; }
        public int FaveCount { get; set; }
        public int Ratings { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow.AddHours(1).AddDays(1);
        public BillingDetail BillingDetail { get; set; }
        public ShippingDetail ShippingDetail { get; set; }
    }
}
