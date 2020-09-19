using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using Roof_Care.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class RecentBookingController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly RecentBookingServices recentBookings;

        public RecentBookingController()
        {
            _dbContext = new RoofCareEntities();
            recentBookings = new RecentBookingServices(_dbContext);
        }

        [HttpPost]
        [Route("api/RecentBooking/GetRecentBookings")]
        public IHttpActionResult GetRecentBookings(UserIdModel userId)
        {
            return Ok(recentBookings.GetRecentBookings(userId));
        }

        [HttpDelete]
        [Route("api/RecentBooking/DeleteRecentBooking")]
        public IHttpActionResult DeleteRecentBooking(int bookingId)
        {
            return Ok(recentBookings.DeleteRecentBooking(bookingId));
        }
    }
}
