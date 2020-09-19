using Roof_Care.ExmxDbSets;
using Roof_Care.Models.ResetPassword;
using Roof_Care.Services.ResetPassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class ResetPasswordController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly ResetPasswordService resetPassword;

        public ResetPasswordController()
        {
            _dbContext = new RoofCareEntities();
            resetPassword = new ResetPasswordService(_dbContext);
        }

        [HttpPost]
        [Route("api/ResetPassword/VerifyUser")]
        public IHttpActionResult VerifyUser(ResetPasswordModel resetPasswordModel)
        {
            return Ok(resetPassword.VerifyUser(resetPasswordModel));
        }

        [HttpPut]
        [Route("api/ResetPassword/UpdatePassword")]
        public IHttpActionResult UpdatePassword(NewPasswordModel newPassword)
        {
            return Ok(resetPassword.UpdatePassword(newPassword));
        }
    }
}
