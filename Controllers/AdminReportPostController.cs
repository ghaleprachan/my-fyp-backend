using Roof_Care.ExmxDbSets;
using Roof_Care.Services.AdminServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class AdminReportPostController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly AdminReportPostsServices adminReportPostServices;
        public AdminReportPostController()
        {
            _dbContext = new RoofCareEntities();
            adminReportPostServices = new AdminReportPostsServices(_dbContext);
        }

        [HttpGet]
        [Route("api/AdminReportPost/GetReportedOffers")]
        public IHttpActionResult GetReportedOffers(int adminId)
        {
            return Ok(adminReportPostServices.GetReportedOffers(adminId));
        }
        [HttpDelete]
        [Route("api/AdminReportPost/DeleteReportedOffer")]
        public IHttpActionResult DeleteReportedOffer(int offerId)
        {
            return Ok(adminReportPostServices.DeleteReportedOffer(offerId));
        }

        [HttpGet]
        [Route("api/AdminReportPost/GetReportedProblems")]
        public IHttpActionResult GetReportedProblems(int adminId)
        {
            return Ok(adminReportPostServices.GetReportedProblems(adminId));
        }
        [HttpDelete]
        [Route("api/AdminReportPost/DeleteReportedProblem")]
        public IHttpActionResult DeleteReportedProblem(int problemId)
        {
            return Ok(adminReportPostServices.DeleteReportedProblem(problemId));
        }
    }
}
