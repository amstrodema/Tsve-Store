using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Model.Hybrid
{
    public class UserHybrid
    {
        public Guid ID { get; set; }
        public Guid RoleID { get; set; }
        public Guid MembershipID { get; set; }
        public Guid CountryID { get; set; }
        public Guid StoreID { get; set; }
        public Guid AppID { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Fname { get; set; }
        public string CV { get; set; }
        public string LName { get; set; }
        public string Gender { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Bio { get; set; }
        public string Level { get; set; }
        public string Country { get; set; }
        public string ProfileImage { get; set; }
        public DateTime SubExpiry { get; set; }

        public Guid InstitutionID { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionAddress { get; set; }
        public string InstitutionState { get; set; }
        public string InstitutionCountry { get; set; }

        public bool IsFollowedByMe { get; set; }
        public bool IsFollowMe { get; set; }
        public bool IsSubscribed { get; set; }
        public bool IsDev { get; set; }
        public bool IsEmailVer { get; set; }
        public bool IsBanned { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsInfluencer { get; set; }

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
    }
}
