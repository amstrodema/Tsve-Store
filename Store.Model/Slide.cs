namespace Store.Model
{
    public class Slide
    {
        public Guid ID { get; set; }
        public Guid StoreID { get; set; }
        public string Caption { get; set; } = string.Empty;
        public string Desc { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }

    }
}