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
    public class RegisterController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private RegisterServices registerServices;
        public RegisterController()
        {
            _dbContext = new RoofCareEntities();
            this.registerServices = new RegisterServices(_dbContext);
        }
        [HttpPost]
        public IHttpActionResult Get(NewRegisterModel new_user)
        {
            if (ModelState.IsValid)
            {
                return Ok(registerServices.RegisterNewUser(new_user));
            }
            else
            {
                return Ok("Invalid Model");
            }
        }
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(registerServices.GetProfessions());
        }
    }
}
