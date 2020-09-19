using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models.Booking
{
    public class UpdateBookingModel
    {
        public int BookingId { get; set; }
        public string ServiceType { get; set; }
        public string ProblemDescription { get; set; }
        public DateTime ServiceDate { get; set; }
        public string ServiceAddress { get; set; }
    }
}