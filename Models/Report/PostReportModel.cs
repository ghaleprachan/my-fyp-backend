using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models.Report
{
    public class PostReportModel
    {
        public string ReportedBy { get; set; }
        public int PostId { get; set; }
        public string ReportText { get; set; }
    }
}