using Roof_Care.ExmxDbSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models
{
    public class ProfileUpdateModel
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string UserProfileImage { get; set; }
        public string AboutUser { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Email> Emails { get; set; }
        public List<Address> Addresses { get; set; }
    }
}