namespace Store.Model
{
    public class OrderItem
    {
        public Guid ID { get; set; }
        public Guid StoreID { get; set; }
        public Guid OrderID { get; set; }
        public Guid ItemID { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string CurrenySymbol { get; set; } = string.Empty;

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }

    }
}