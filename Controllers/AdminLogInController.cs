using Roof_Care.ExmxDbSets;
using Roof_Care.Models.AdminModels;
using Roof_Care.Services.AdminServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class AdminLogInController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly AdminLogInServices adminLogInServices;
        private AdminLogInController()
        {
            _dbContext = new RoofCareEntities();
            adminLogInServices = new AdminLogInServices(_dbContext);
        }

        [HttpPost]
        [Route("api/AdminLogIn/LogIn")]
        public IHttpActionResult LogIn(AdminLogIn admin)
        {
            return Ok(adminLogInServices.LogIn(admin));
        }
    }
}
