using Roof_Care.ExmxDbSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models.ReplyModel
{
    public class ReplyModel
    {
        public int FeedbackId { get; set; }
        public string FeedbackBy { get; set; }
        public string Reply { get; set; }
    }
}