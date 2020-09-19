using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using Roof_Care.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class ChangePasswordController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly ChangePasswordService changePasswordService;
        public ChangePasswordController()
        {
            _dbContext = new RoofCareEntities();
            changePasswordService = new ChangePasswordService(_dbContext);
        }
        [HttpPost]
        [Route("api/ChangePassword/ValidateUserPassword")]
        public IHttpActionResult ValidateUserPassword(UserValidationModel userValidation)
        {
            return Ok(changePasswordService.ValidateUser(userValidation));
        }
    }
}
