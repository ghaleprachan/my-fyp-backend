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
    public class AdminUserVerifyController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly VerifyUserService verifyUserService;
        public AdminUserVerifyController()
        {
            _dbContext = new RoofCareEntities();
            verifyUserService = new VerifyUserService(_dbContext);
        }

        [HttpGet]
        [Route("api/AdminUserVerify/GetUserToVerify")]
        public IHttpActionResult GetUserToVerify(int adminId)
        {
            return Ok(verifyUserService.GetUserToVerify(adminId));
        }

        [HttpPut]
        [Route("api/AdminUserVerify/VerifyProfile")]
        public IHttpActionResult VerifyProfile(int userId)
        {
            return Ok(verifyUserService.VerifyProfile(userId));
        }
    }
}
