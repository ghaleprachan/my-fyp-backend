using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models
{
    public class LastChatUpdateModel
    {
        public string participantOne { get; set; }
        public string participantTwo { get; set; }
        public string lastMessage { get; set; }
        public string senderId { get; set; }
    }
}