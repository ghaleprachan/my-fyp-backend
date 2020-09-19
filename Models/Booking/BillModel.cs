using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Models.Booking
{
    public class BillModel
    {
        public int BookingId { get; set; }
        public double ServiceCharge { get; set; }
        public double TravellingCost { get; set; }
        public double Discount { get; set; }
        public double TotalCharge { get; set; }
    }
}