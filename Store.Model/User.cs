namespace Store.Model
{
    public class User
    {
        public Guid ID { get; set; }
        public Guid RoleID { get; set; }
        public Guid MembershipID { get; set; }
        public Guid AppID { get; set; }
        public Guid CountryID { get; set; }
        public Guid StoreID { get; set; }
        public string ReferredBy { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Fname { get; set; } = string.Empty;
        public string LName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Tel { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordVer { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string InstitutionLevel { get; set; } = string.Empty;
        public string CV { get; set; } = string.Empty;
        public string ProfileImage { get; set; } = string.Empty;
        public string EmailVerCode { get; set; } = string.Empty;
        public DateTime EmailVerExp { get; set; }
        public DateTime PasswordVerExp { get; set; }
        public DateTime SubExpiry { get; set; }

        public bool IsSubscribed { get; set; }
        public bool IsDev { get; set; }
        public bool IsEmailVer { get; set; }
        public bool IsBanned { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsOwner { get; set; }
        public bool IsInfluencer { get; set; }
        public bool IsLearningCenter { get; set; }

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}