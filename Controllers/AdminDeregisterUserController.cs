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
    public class AdminDeregisterUserController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly AdminDeregisterUserServices adminDeregisterUser;
        public AdminDeregisterUserController()
        {
            _dbContext = new RoofCareEntities();
            adminDeregisterUser = new AdminDeregisterUserServices(_dbContext);
        }
        [HttpGet]
        [Route("api/AdminDeregisterUser/GetReportedUsers")]
        public IHttpActionResult GetReportedUsers(int adminId)
        {
            return Ok(adminDeregisterUser.GetReportedUsers(adminId));
        }

        [HttpDelete]
        [Route("api/AdminDeregisterUser/DeregisterUser")]
        public IHttpActionResult DeregisterUser(int userId)
        {
            return Ok(adminDeregisterUser.DeregisterUser(userId));
        }
    }
}
