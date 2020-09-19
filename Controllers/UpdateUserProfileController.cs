using Roof_Care.AuthenticationDecode;
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
    public class UpdateUserProfileController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private UpdateProfileServices registerServices;
        public UpdateUserProfileController()
        {
            _dbContext = new RoofCareEntities();
            registerServices = new UpdateProfileServices(_dbContext);
        }
        [HttpGet]
        public IHttpActionResult Get(string userId)
        {
            try
            {
                return Ok(registerServices.GetDetailsToUpdate(EncodeDecode.GetUserId(userId)));
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid user model! " + ex);
            }
        }

        [HttpPut]
        public IHttpActionResult Put(string userId, [FromBody]ProfileUpdateModel new_user)
        {
            if (ModelState.IsValid)
            {
                return Ok(registerServices.UpdateProfileDetails(EncodeDecode.GetUserId(userId), new_user));
            }
            else
            {
                return BadRequest("Failed");
            }
        }

        [HttpDelete]
        [Route("api/UpdateUserProfile/DeleteAddress/")]
        public IHttpActionResult DeleteAddress(int id)
        {
            return Ok(registerServices.RemoveAddress(id));
        }

        [HttpDelete]
        [Route("api/UpdateUserProfile/DeleteContact")]
        public IHttpActionResult DeleteContacts(int id)
        {
            return Ok(registerServices.RemoveContacts(id));
        }

        [HttpDelete]
        [Route("api/UpdateUserProfile/DeleteEmail")]
        public IHttpActionResult DeleteEmail(int id)
        {
            return Ok(registerServices.RemoveEmails(id));
        }
    }
}
