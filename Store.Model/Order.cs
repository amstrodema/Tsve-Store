namespace Store.Model
{
    public class Order
    {
        public Guid ID { get; set; }
        public Guid StoreID { get; set; }
        public Guid BillingID { get; set; }
        public Guid ShippingID { get; set; }
        public decimal Amount { get; set; }
        public string Ref { get; set; } = string.Empty;

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}