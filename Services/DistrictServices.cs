using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Roof_Care.Helper;
using Roof_Care.Declarations;
namespace Roof_Care.Services
{
    public class DistrictServices
    {
        private readonly RoofCareEntities _dbContext;
        public DistrictServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal ResponseModel GetCities()
        {
            try
            {
                using (_dbContext)
                {
                    var districts = (from district in _dbContext.Districts
                                     select new
                                     {
                                         district.DistrictId,
                                         district.DistrictName,
                                         Municipalities = from municipality in district.Municipalities
                                                          select new
                                                          {
                                                              municipality.MunicipalityId,
                                                              municipality.MunicipalityName
                                                          }
                                     }).ToList(); ;
                    return HelperClass.Response(true, GlobalDecleration._successAction, districts);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Server Issues " + ex);
            }
            throw new NotImplementedException();
        }
    }
}