using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using Roof_Care.Declarations;
using System.Data.Entity;
using Roof_Care.Models.Booking;

namespace Roof_Care.Services
{
    public class BillServices
    {
        private readonly RoofCareEntities _dbContext;
        public BillServices(RoofCareEntities roofCareEntities)
        {
            _dbContext = roofCareEntities;
        }
        internal ResponseModel AddBill(BillModel bill)
        {
            try
            {
                using (_dbContext)
                {
                    if (bill != null)
                    {
                        Bill new_bill = new Bill
                        {
                            ServiceCharge = bill.ServiceCharge,
                            TravellingCost = bill.TravellingCost,
                            Discount = bill.Discount,
                            TotalCharge = bill.TotalCharge,
                            PaidStatus = "false",
                            CustomerAcceptance = "false",
                            IssuedDate = DateTime.Now
                        };
                        _dbContext.Bills.Add(new_bill);

                        Booking booking = _dbContext.Bookings.Find(bill.BookingId);
                        if (booking != null)
                        {
                            booking.BillId = new_bill.BillId;
                            booking.SpecialistAcceptance = "true";
                            _dbContext.Entry(booking).State = EntityState.Modified;
                        }
                        _dbContext.SaveChanges();

                        return HelperClass.Response(true, GlobalDecleration._successAction, new_bill.BillId);
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._invalidModel, "Null Bill Detected");
                    }
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