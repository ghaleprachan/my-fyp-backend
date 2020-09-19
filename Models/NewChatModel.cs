using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models
{
    public class NewChatModel
    {
        public string senderId { get; set; }
        public string receiverid { get; set; }
        public string message { get; set; }
    }
}