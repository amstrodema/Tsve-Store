﻿namespace Store.Model
{
    public class File
    {
        public Guid ID { get; set; }
        public Guid StoreID { get; set; }
        public string Resource { get; set; }

        public bool IsVideo { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }

    }
}