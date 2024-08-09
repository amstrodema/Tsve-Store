namespace Store.Model
{
    public class Order
    {
        public Guid ID { get; set; }
        public Guid StoreID { get; set; }
        public Guid BillingID { get; set; }
        public Guid ShippingID { get; set; }
        public Guid TransactionID { get; set; }
        public decimal Amount { get; set; }
        public string Ref { get; set; } = string.Empty;
        public string PaymentLink { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public bool IsFinished { get; set; }
        public bool IsPaid { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}