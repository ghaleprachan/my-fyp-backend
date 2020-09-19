using Roof_Care.Models;
using Roof_Care.Models.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Helper
{
    public class HelperClass
    {
        public static ResponseModel Response(bool success, string message, dynamic output)
        {
            return new ResponseModel()
            {
                success = success,
                message = message,
                result = output
            };
        }

        public static SearchResponseModel SearchResponse(bool success, string message, dynamic servicesproviders, dynamic services)
        {
            return new SearchResponseModel()
            {
                success = success,
                message = message,
                serviceproviders = servicesproviders,
                services = services
            };
        }
        public static LogInModelResponse LogInValid(bool status, string message, dynamic tokenNumber)
        {
            return new LogInModelResponse()
            {
                success = status,
                message = message,
                tokenNumber = tokenNumber
            };
        }

        public static BookingDetailsResponse BookingResponse(bool status, string message, dynamic customer, dynamic serviceProvider)
        {
            return new BookingDetailsResponse()
            {
                Status = status,
                Message = message,
                Customer = customer,
                ServiceProvider = serviceProvider
            };
        }
    }
}