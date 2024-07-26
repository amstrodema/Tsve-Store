namespace Store.Model
{
    public class ItemFeature
    {
        public Guid ID { get; set; }
        public Guid StoreID { get; set; }
        public Guid OrderID { get; set; }
        public Guid OrderItemID { get; set; }
        public Guid FeatureID { get; set; }
        public string FeatureOption { get; set; } = string.Empty;

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }

    }
}