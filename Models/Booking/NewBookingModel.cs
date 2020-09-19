using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models.Booking
{
    public class NewBookingModel
    {
        public int ServiceProvider { get; set; }
        public int Customer { get; set; }
        public string ServiceType { get; set; }
        public System.DateTime ServiceDate { get; set; }
        public string CustomerAddress { get; set; }
        public string ProblemDescription { get; set; }
        public string ProblemImage { get; set; }
    }
}