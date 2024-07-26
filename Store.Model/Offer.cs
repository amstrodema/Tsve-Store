namespace Store.Model
{
    public class Offer
    {
        public Guid ID { get; set; }
        public Guid StoreID { get; set; }
        public string Caption { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DiscountCaption { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public int No { get; set; }

        public bool IsHomepage { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCoupon { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }

    }
}