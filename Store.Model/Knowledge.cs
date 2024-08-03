namespace Store.Model
{
    public class Knowledge
    {
        public Guid ID { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public string Tag { get; set; }

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }

    }
}