using Roof_Care.ExmxDbSets;
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
    public class BillController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly BillServices billServices;
        public BillController()
        {
            _dbContext = new RoofCareEntities();
            billServices = new BillServices(_dbContext);
        }

        [HttpPost]
        [Route("api/Bill/PostBill/")]
        public IHttpActionResult PostBill(BillModel bill)
        {
            if (ModelState.IsValid)
            {
                return Ok(billServices.AddBill(bill));
            }
            else
            {
                return BadRequest("Invalid Model");
            }
        }
    }
}
