using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models
{
    public class FeedbackModel
    {
        public string FeedbackBy { get; set; }
        public string FeedbackTo { get; set; }
        public string Feedback { get; set; }
        public float Rating { get; set; }
    }
}