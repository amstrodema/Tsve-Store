namespace Store.Model
{
    public class Store
    {
        public Guid ID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Logo { get; set; } = string.Empty;
        public string? LogoSuffix { get; set; } = string.Empty;
        public string? LogoImage { get; set; } = string.Empty;
        public string? Tel { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? Url { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? Contact { get; set; } = string.Empty;
        public string? FacebookUrl { get; set; } = string.Empty;
        public string? InstagramUrl { get; set; } = string.Empty;
        public string? XUrl { get; set; } = string.Empty;
        public string? LinkedInUrl { get; set; } = string.Empty;
        public string? Brief { get; set; } = string.Empty;
        public string? FooterTag { get; set; } = "GET IN TOUCH";
        public string? FooterDesc { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public bool IsBanned { get; set; }
        public int Level { get; set; } = 0;

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}