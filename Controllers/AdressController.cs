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
    public class AdressController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly DistrictServices chatsServices;

        public AdressController()
        {
            _dbContext = new RoofCareEntities();
            chatsServices = new DistrictServices(_dbContext);
        }
        public IHttpActionResult Get()
        {
            return Ok(chatsServices.GetCities());
        }
    }
}
