using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class AdminDistrictsMunicipalityController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly AdminDistrictMunicipalityServices districtMunicipalityServices;
        public AdminDistrictsMunicipalityController()
        {
            _dbContext = new RoofCareEntities();
            districtMunicipalityServices = new AdminDistrictMunicipalityServices(_dbContext);
        }

        [HttpGet]
        [Route("api/AdminDistrictsMunicipality/GetPlaces")]
        public IHttpActionResult GetPlaces(int adminId)
        {
            return Ok(districtMunicipalityServices.GetPlaces(adminId));
        }

        [HttpPost]
        [Route("api/AdminDistrictsMunicipality/AddDistricts")]
        public IHttpActionResult AddDistricts(DistrictModel districtModel)
        {
            if (ModelState.IsValid)
            {
                return Ok(districtMunicipalityServices.AddDistrict(districtModel));
            }
            else
            {
                return Ok(HelperClass.Response(false, GlobalDecleration._invalidModel, "Invalid Model"));
            }
        }
        [HttpPost]
        [Route("api/AdminDistrictsMunicipality/AddMunicipality")]
        public IHttpActionResult AddMunicipality(MunicipalityModel municipalityModel)
        {
            return Ok(districtMunicipalityServices.AddMunicipality(municipalityModel));
        }

        [HttpDelete]
        [Route("api/AdminDistrictsMunicipality/DeleteDistrict")]
        public IHttpActionResult DeleteDistrict(int districtId)
        {
            return Ok(districtMunicipalityServices.DeleteDistrict(districtId));
        }
        [HttpDelete]
        [Route("api/AdminDistrictsMunicipality/DeleteMunicipality")]
        public IHttpActionResult DeleteMunicipality(int muniCipalityId)
        {
            return Ok(districtMunicipalityServices.DeleteMunicipality(muniCipalityId));
        }
        [HttpPut]
        [Route("api/AdminDistrictsMunicipality/UpdateDistricts")]
        public IHttpActionResult UpdateDistricts(District district)
        {
            return Ok(districtMunicipalityServices.UpdateDistricts(district));
        }
        [HttpPut]
        [Route("api/AdminDistrictsMunicipality/UpdateMunicipality")]
        public IHttpActionResult UpdateMunicipality(Municipality municipality)
        {
            return Ok(districtMunicipalityServices.UpdateMunicipality(municipality));
        }
    }
}
