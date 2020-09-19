using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using Roof_Care.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class LogInController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly UserValidationService userValidationService;
        public LogInController()
        {
            _dbContext = new RoofCareEntities();
            userValidationService = new UserValidationService(_dbContext);
        }
        /*[HttpGet]
        public IHttpActionResult Get(string encoded)
        {
            return Ok(EncodeDecode.GetUserId(encoded));
        }*/
        [HttpPost]
        public LogInModelResponse Post([FromBody]UserValidationModel userModel)
        {
            return userValidationService.VerifyUser(userModel);
        }
    }
}
