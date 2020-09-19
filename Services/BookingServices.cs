using Roof_Care.Helper;
using Roof_Care.Models;
using Roof_Care.Declarations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Roof_Care.Models.Booking;
using Roof_Care.AuthenticationDecode;
using Roof_Care.ExmxDbSets;
using System.Data.Entity;
using System.Web.UI.WebControls;

namespace Roof_Care.Services
{
    public class BookingServices
    {
        private readonly RoofCareEntities _dbContext;
        public BookingServices(RoofCareEntities roofCareEntitiesModels)
        {
            _dbContext = roofCareEntitiesModels;
        }

        internal BookingDetailsResponse GetBookingDetails(BookingUserModel userModel)
        {
            try
            {
                using (_dbContext)
                {
                    int customerId = EncodeDecode.GetUserId(userModel.Customer);
                    int serviceProviderId = EncodeDecode.GetUserId(userModel.ServiceProvider);
                    var Customer = (from user in _dbContext.Users
                                    select new
                                    {
                                        user.UserId,
                                        user.FullName,
                                        user.UserProfileImage,
                                        addresses = from address in user.Addresses
                                                    select new
                                                    {
                                                        address.AddressId,
                                                        address.DisctrictId,
                                                        address.District.DistrictName,
                                                        address.MunicipalityId,
                                                        address.Municipality.MunicipalityName,
                                                        address.CurrentLocation
                                                    },
                                    }).Where(uId => uId.UserId == customerId).ToList();

                    var ServiceProvider = (from user in _dbContext.Users
                                           select new
                                           {
                                               user.UserId,
                                               user.FullName,
                                               user.UserProfileImage,
                                               professions = from profession in user.UserProfessions
                                                             select new
                                                             {
                                                                 profession.ProfessionId,
                                                                 profession.Profession.ProfessionName
                                                             },
                                               timeTable = (from userTime in user.Bookings
                                                            select new
                                                            {
                                                                userTime.BookingDate,
                                                                userTime.ServiceDate,
                                                                userTime.ServiceProvider
                                                            }).Where(myId => myId.ServiceProvider == serviceProviderId)

                                           }).Where(id => id.UserId == serviceProviderId).ToList();

                    return HelperClass.BookingResponse(true, GlobalDecleration._successAction, Customer, ServiceProvider);

                }
            }
            catch (Exception ex)
            {
                return HelperClass.BookingResponse(false, GlobalDecleration._internalServerError, ex, ex);
            }
        }

        internal object CompleteWithQrCode(string customerToken, string specialistToken)
        {
            try
            {
                using (_dbContext)
                {
                    int customerId = EncodeDecode.GetUserId(customerToken);
                    int specialistId = EncodeDecode.GetUserId(specialistToken);

                    var old_booking = (from booking in _dbContext.Bookings
                                       select new
                                       {
                                           booking.BookingId,
                                           booking.Customer,
                                           booking.ServiceProvider
                                       })
                                           .Where(c => c.Customer == customerId && specialistId == c.ServiceProvider)
                                           .ToList()
                                           .FirstOrDefault();
                    return BookingCompleted(old_booking.BookingId);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._successAction, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal object BookingCompleted(int bookingId)
        {
            try
            {
                using (_dbContext)
                {
                    Booking booking = _dbContext.Bookings.Find(bookingId);
                    if (booking != null)
                    {
                        booking.CompletedStatus = "true";
                        _dbContext.Entry(booking).State = EntityState.Modified;
                        Bill bill = _dbContext.Bills.Find(booking.BillId);
                        if (bill != null)
                        {
                            bill.PaidStatus = "true";
                            _dbContext.Entry(bill).State = EntityState.Modified;
                        }
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Success");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal ResponseModel GetAllBookings(int userId)
        {
            try
            {
                using (_dbContext)
                {
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
                                    (user.CustomerId == userId || user.SpecialistId == userId)).ToList()
                                    .OrderByDescending(d => d.BookingDate);
                    return HelperClass.Response(true, GlobalDecleration._successAction, bookings);
                }
            }
            catch (Exception ex)
            {
                HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal ResponseModel UpdateGetBooking(UpdateBookingModel updateModel)
        {
            try
            {
                using (_dbContext)
                {
                    Booking old_booking = _dbContext.Bookings.Find(updateModel.BookingId);
                    if (old_booking != null)
                    {
                        old_booking.ServiceType = updateModel.ServiceType;
                        old_booking.ProblemDescription = updateModel.ProblemDescription;
                        old_booking.ServiceDate = updateModel.ServiceDate;
                        old_booking.CustomerAddress = updateModel.ServiceAddress;
                        _dbContext.Entry(old_booking).State = EntityState.Modified;
                    }
                    _dbContext.SaveChanges();

                    var new_booking = (from booking in _dbContext.Bookings
                                       select new
                                       {
                                           booking.BookingId,
                                           CustomerId = booking.Customer,
                                           CustomerUsername = booking.User1.Username,
                                           SpecialistId = booking.ServiceProvider,
                                           SpecialistUsername = booking.User.Username,
                                           CustomerName = booking.User1.FullName,
                                           SpecialistName = booking.User.FullName,
                                           CustomerImage = booking.User1.UserProfileImage,
                                           SpecialistImage = booking.User.UserProfileImage,
                                           booking.ServiceDate,
                                           booking.ServiceType,
                                           booking.ProblemDescription,
                                           booking.ProblemImage,
                                           booking.SpecialistAcceptance,
                                           booking.BookingDate,
                                           ServiceAddress = booking.CustomerAddress,
                                       }).Where(bookId => bookId.BookingId == old_booking.BookingId).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, new_booking);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal ResponseModel GetAllBookingsRequests(UserIdModel userIdModel)
        {
            try
            {
                using (_dbContext)
                {
                    int userId = EncodeDecode.GetUserId(userIdModel.tokenNumber);
                    var booking_requests = (from booking in _dbContext.Bookings
                                            select new
                                            {
                                                booking.BookingId,
                                                CustomerId = booking.Customer,
                                                CustomerUsername = booking.User1.Username,
                                                SpecialistId = booking.ServiceProvider,
                                                SpecialistUsername = booking.User.Username,
                                                CustomerName = booking.User1.FullName,
                                                SpecialistName = booking.User.FullName,
                                                CustomerImage = booking.User1.UserProfileImage,
                                                SpecialistImage = booking.User.UserProfileImage,
                                                booking.ServiceDate,
                                                booking.ServiceType,
                                                booking.ProblemDescription,
                                                booking.ProblemImage,
                                                booking.SpecialistAcceptance,
                                                booking.BookingDate,
                                                ServiceAddress = booking.CustomerAddress,
                                            }).Where(u => u.SpecialistAcceptance == "false" && (
                                            u.CustomerId == userId ||
                                            u.SpecialistId == userId)).OrderByDescending(d => d.BookingDate).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, booking_requests);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }

        internal ResponseModel AddBooking(NewBookingModel new_booking)
        {
            try
            {
                using (_dbContext)
                {
                    Booking booking = new Booking
                    {
                        ServiceProvider = new_booking.ServiceProvider,
                        Customer = new_booking.Customer,
                        CustomerAddress = new_booking.CustomerAddress,
                        ServiceDate = new_booking.ServiceDate,
                        ServiceType = new_booking.ServiceType,
                        ProblemDescription = new_booking.ProblemDescription,
                        ProblemImage = ParseImage(new_booking.ProblemImage),
                        BookingDate = DateTime.Now,
                        SpecialistAcceptance = "false",
                        CompletedStatus = "false"
                    };
                    _dbContext.Bookings.Add(booking);
                    _dbContext.SaveChanges();

                    var notify_details = (from bookings in _dbContext.Bookings
                                          select new
                                          {
                                              bookings.BookingId,
                                              SpecialistName = bookings.User.FullName,
                                              SpecialistId = bookings.ServiceProvider,
                                              SpecialistUsername = bookings.User.Username,
                                              CustomerName = bookings.User1.FullName,
                                              CustomerId = bookings.Customer,
                                              CustomerUsername = bookings.User1.Username,
                                          }).Where(book => book.BookingId == booking.BookingId).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, notify_details);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        private string ParseImage(string bitmapString)
        {
            try
            {
                string folderLocation = "~/MyUploads/BookingImage/";
                string imageName = ((RandomString(10) + DateTime.Now) + ".jpg").Replace(":", String.Empty);

                byte[] img = LoadImage(bitmapString);

                string filePath = HttpContext.Current.Server
                    .MapPath(folderLocation +
                    Path.GetFileName(imageName));

                File.WriteAllBytes(filePath, img);

                string finalLocation = "MyUploads/BookingImage/";
                return (finalLocation + imageName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public byte[] LoadImage(string bitmapString)
        {
            byte[] imageBytes = Convert.FromBase64String(bitmapString);
            return imageBytes;
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal ResponseModel DeleteBooking(int bookingId)
        {
            try
            {
                using (_dbContext)
                {
                    Booking booking = _dbContext.Bookings.Find(bookingId);
                    if (booking != null)
                    {
                        if (booking.Bill != null)
                        {
                            Bill bill = _dbContext.Bills.Find(booking.BillId);
                            if (bill != null)
                            {
                                _dbContext.Bills.Remove(bill);
                            }
                        }
                        _dbContext.Bookings.Remove(booking);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Remove Success");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "Not Booking of type Id");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }
    }
}