using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Services
{
    public class RecentBookingServices
    {
        private readonly RoofCareEntities _dbContext;
        public RecentBookingServices(RoofCareEntities roofCareEntities)
        {
            _dbContext = roofCareEntities;
        }

        internal ResponseModel GetRecentBookings(UserIdModel userId)
        {
            try
            {
                using (_dbContext)
                {
                    int id = EncodeDecode.GetUserId(userId.tokenNumber);
                    var bookings = (from booking in _dbContext.Bookings
                                    select new
                                    {
                                        booking.BookingId,
                                        SpecialistId = booking.ServiceProvider,
                                        SpecialistUserName = booking.User.Username,
                                        SpecialistName = booking.User.FullName,
                                        SpecialistImage = booking.User.UserProfileImage,
                                        SpecialistRating = booking.User.UserRating,

                                        CustomerId = booking.Customer,
                                        CustomerUserName = booking.User1.Username,
                                        CustomerName = booking.User1.FullName,
                                        CustomerImage = booking.User1.UserProfileImage,

                                        booking.ServiceType,
                                        booking.ServiceDate,
                                        booking.BookingDate,
                                        booking.CustomerAddress,
                                        booking.ProblemDescription,
                                        booking.ProblemImage,
                                        booking.SpecialistAcceptance,
                                        booking.CompletedStatus,

                                        booking.BillId,
                                        booking.Bill.ServiceCharge,
                                        booking.Bill.TravellingCost,
                                        booking.Bill.Discount,
                                        booking.Bill.TotalCharge,
                                        booking.Bill.CustomerAcceptance,
                                        booking.Bill.PaidStatus,
                                        booking.Bill.IssuedDate
                                    }).Where(user => user.SpecialistAcceptance.Equals("true")
                                    &&
                                    user.BillId != null
                                    &&
                                    user.CompletedStatus.Equals("true")
                                    &&
                                    (user.CustomerId == id || user.SpecialistId == id)).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, bookings);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal ResponseModel DeleteRecentBooking(int bookingId)
        {
            try
            {
                using (_dbContext)
                {
                    string msg;
                    bool result = false;

                    Booking old_booking = _dbContext.Bookings.Find(bookingId);
                    if (old_booking != null)
                    {
                        _dbContext.Bookings.Remove(old_booking);
                        _dbContext.SaveChanges();
                        msg = "Deleted Successfully";
                        result = true;
                    }
                    else
                    {
                        msg = "Not History of booking found";
                        result = false;
                    }

                    return HelperClass.Response(result, GlobalDecleration._successAction, msg);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }
    }
}