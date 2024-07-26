namespace Store.Model
{
    public class Notification
    {
        public Guid ID { get; set; }
        public Guid ReceiverID { get; set; }
        public Guid AssociatedUserID { get; set; }
        public Guid StoreID { get; set; }
        public string Message { get; set; } = string.Empty;
        public string AssociatedUrl { get; set; } = string.Empty;
        public bool IsRead { get; set; }

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}