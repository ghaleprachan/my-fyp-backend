using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using Roof_Care.Models.Booking;
using Roof_Care.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class BookingController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly BookingServices bookingServices;
        public BookingController()
        {
            _dbContext = new RoofCareEntities();
            bookingServices = new BookingServices(_dbContext);
        }
        [HttpPost]
        [Route("api/Booking/GetAllBookings/")] // This is to get the final booking
        public IHttpActionResult GetAllBookings(UserIdModel userToken)
        {
            try
            {
                return Ok(bookingServices.GetAllBookings(EncodeDecode.GetUserId(userToken.tokenNumber)));

            }
            catch (Exception ex)
            {
                return Ok(HelperClass.Response(true, GlobalDecleration._successAction, ex));
            }
        }

        [HttpPost]
        [Route("api/Booking/GetBookingDetails/")] // These is to the needed contents for booking 
        public IHttpActionResult GetBookingDetails([FromBody]BookingUserModel bookingUser)
        {
            if (ModelState.IsValid)
            {
                /*
                 This is used to get all the initial details of booking*/
                return Ok(bookingServices.GetBookingDetails(bookingUser));
            }
            else
            {
                return BadRequest("Bad Request");
            }
        }

        [HttpPost]
        [Route("api/Booking/PostBooking/")]
        public IHttpActionResult PostBooking(NewBookingModel new_booking)
        {
            if (ModelState.IsValid)
            {
                return Ok(bookingServices.AddBooking(new_booking));
            }
            else
            {
                return BadRequest("Invalid Model");
            }
        }

        [HttpPost]
        [Route("api/Booking/UpdateGetBooking")]
        public IHttpActionResult UpdateGetBookings(UpdateBookingModel updateModel)
        {
            return Ok(bookingServices.UpdateGetBooking(updateModel));
        }
        [HttpPost]
        [Route("api/Booking/GetAllBookingRequests")]
        public IHttpActionResult GetAllBookingRequests(UserIdModel userIdModel)
        {
            return Ok(bookingServices.GetAllBookingsRequests(userIdModel));
        }
        [HttpPut]
        [Route("api/Booking/BookingCompleted")]
        public IHttpActionResult BookingCompleted(int bookingId)
        {
            return Ok(bookingServices.BookingCompleted(bookingId));
        }
        [HttpDelete]
        [Route("api/Booking/DeleteBooking")]
        public IHttpActionResult DeleteBooking(int bookingId)
        {
            return Ok(bookingServices.DeleteBooking(bookingId));
        }
        [HttpPut]
        [Route("api/Booking/CompleteWithQrCode")]
        public IHttpActionResult CompleteWithQrCode(string customerToken, string specialistToken)
        {
            return Ok(bookingServices.CompleteWithQrCode(customerToken, specialistToken));
        }
    }
}
