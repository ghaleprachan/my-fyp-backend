using Roof_Care.ExmxDbSets;
using Roof_Care.Models.Report;
using Roof_Care.Models.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Roof_Care.Controllers
{
    public class ReportController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly ReportServices reportServices;

        public ReportController()
        {
            _dbContext = new RoofCareEntities();
            reportServices = new ReportServices(_dbContext);
        }
        [HttpPost]
        [Route("api/Report/AddOfferReport/")]
        public IHttpActionResult AddOfferReport([FromBody] PostReportModel postReport)
        {
            if (ModelState.IsValid)
            {
                return Ok(reportServices.AddOfferReport(postReport));
            }
            else
            {
                return BadRequest("Invalid Model");
            }
        }

        [HttpPost]
        [Route("api/Report/AddProblemReport/")]
        public IHttpActionResult AddProblemReport([FromBody] PostReportModel postReport)
        {
            if (ModelState.IsValid)
            {
                return Ok(reportServices.AddProblemReport(postReport));
            }
            else
            {
                return BadRequest("Invalid Model");
            }
        }
    }
}
