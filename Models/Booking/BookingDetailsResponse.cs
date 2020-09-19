using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models.Booking
{
    public class BookingDetailsResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public dynamic Customer { get; set; }
        public dynamic ServiceProvider { get; set; }
    }
}