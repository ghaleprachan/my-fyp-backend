using Roof_Care.AuthenticationDecode;
using Roof_Care.ExmxDbSets;
using Roof_Care.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class UserStatesController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private UserStatesServices statesServices;
        public UserStatesController()
        {
            _dbContext = new RoofCareEntities();
            this.statesServices = new UserStatesServices(_dbContext);
        }

        [HttpGet]
        [Route("api/UserStates/GetUserStates")]
        public IHttpActionResult GetUserStates(string username)
        {
            return Ok(statesServices.GetUserStates(username));
        }
    }
}
