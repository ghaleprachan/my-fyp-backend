using Roof_Care.ExmxDbSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models
{
    public class NewRegisterModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string UserType { get; set; }
        public string UserProfileImage { get; set; }
        public Contact Contacts { get; set; }
        public Email Emails { get; set; }
        public Address Addresses { get; set; }
        public List<UserProfession> UserProfession { get; set; }
    }
}