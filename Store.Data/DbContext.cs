using Microsoft.EntityFrameworkCore;
using Store.Model;

namespace Store.Data
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbContext(DbContextOptions<DbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(o => o.Amount).HasPrecision(14, 2);
            modelBuilder.Entity<Item>().Property(o => o.Price).HasPrecision(14, 2);
            modelBuilder.Entity<Item>().Property(o => o.OldPrice).HasPrecision(14, 2);
            modelBuilder.Entity<OrderItem>().Property(o => o.Qty).HasPrecision(14, 2);
            modelBuilder.Entity<OrderItem>().Property(o => o.Price).HasPrecision(14, 2);
            modelBuilder.Entity<Offer>().Property(o => o.Discount).HasPrecision(14, 2);
        }
        public virtual DbSet<BillingDetail> Tsve_BillingDetails { get; set; }
        public virtual DbSet<Category> Tsve_Categories { get; set; }
        public virtual DbSet<Group> Tsve_Groups { get; set; }
        public virtual DbSet<Favourite> Tsve_Favourites { get; set; }
        public virtual DbSet<Customer> Tsve_Customers { get; set; }
        public virtual DbSet<Feature> Tsve_Features { get; set; }
        public virtual DbSet<FeatureOption> Tsve_FeatureOptions { get; set; }
        public virtual DbSet<Model.File> Tsve_Files { get; set; }
        public virtual DbSet<Item> Tsve_Items { get; set; }
        public virtual DbSet<ItemFeature> Tsve_ItemFeatures { get; set; }
        public virtual DbSet<Key> Tsve_Keys { get; set; }
        public virtual DbSet<Notification> Tsve_Notifications { get; set; }
        public virtual DbSet<Order> Tsve_Orders { get; set; }
        public virtual DbSet<OrderItem> Tsve_OrderItems { get; set; }
        public virtual DbSet<Review> Tsve_Reviews { get; set; }
        public virtual DbSet<Tracking> Tsve_Trackings { get; set; }
        public virtual DbSet<ShippingDetail> Tsve_ShippingDetails { get; set; }
        public virtual DbSet<Model.Store> Tsve_Stores { get; set; }
        public virtual DbSet<User> Tsve_Users { get; set; }
        public virtual DbSet<Payment> Tsve_Payments { get; set; }
        public virtual DbSet<Transaction> Tsve_Transactions { get; set; }
        public virtual DbSet<LoginMonitor> Tsve_LoginMonitors { get; set; }
        public virtual DbSet<Slide> Tsve_Slides { get; set; }
        public virtual DbSet<Offer> Tsve_Offers { get; set; }
        public virtual DbSet<Brand> Tsve_Brands { get; set; }
    }
}