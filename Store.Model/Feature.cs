namespace Store.Model
{
    public class Feature
    {
        public Guid ID { get; set; }
        public Guid StoreID { get; set; }
        public Guid ItemID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Options { get; set; } = string.Empty;

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }

    }
}