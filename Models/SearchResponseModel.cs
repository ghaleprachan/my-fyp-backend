using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models
{
    public class SearchResponseModel
    {
        public bool success { get; set; }
        public string message { get; set; }
        public dynamic serviceproviders { get; set; }
        public dynamic services { get; set; }
    }
}