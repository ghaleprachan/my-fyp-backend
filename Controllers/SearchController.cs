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
    public class SearchController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly SearchServices searchServices;
        public SearchController()
        {
            _dbContext = new RoofCareEntities();
            searchServices = new SearchServices(_dbContext);
        }
        [HttpGet]
        [Route("api/search/GetCities/")]
        public IHttpActionResult GetCities()
        {
            return Ok(searchServices.Cities());
        }
        [HttpGet]
        [Route("api/search/GetServices/")]
        public IHttpActionResult GetServices()
        {
            return Ok(searchServices.GetServices());
        }
        [HttpGet]
        [Route("api/search/GetSPList/")]
        public IHttpActionResult GetSPList(string professionType)
        {
            return Ok(searchServices.GetSPList(professionType));
        }

        [HttpGet]
        [Route("api/search/GetServiceProviders/")]
        public IHttpActionResult GetServiceProviders()
        {
            return Ok(searchServices.GetServiceProviders());
        }
        [HttpGet]
        [Route("api/search/Search/{name}")]
        public IHttpActionResult Search(string name)
        {
            return Ok(searchServices.Search(name));
        }
    }
}
