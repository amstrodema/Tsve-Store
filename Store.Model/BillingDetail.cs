namespace Store.Model
{
    public class BillingDetail
    {
        public Guid ID { get; set; }
        public Guid StoreID { get; set; }
        public string? FName { get; set; } = string.Empty;
        public string? LName { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Tel { get; set; } = string.Empty;
        public string? Addr1 { get; set; } = string.Empty;
        public string? Addr2 { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Zip { get; set; } = string.Empty;

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}