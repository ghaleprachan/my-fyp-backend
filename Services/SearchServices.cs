using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Services
{
    public class SearchServices
    {
        private readonly RoofCareEntities _dbContext;
        public SearchServices(RoofCareEntities roofCareEntitiesSet)
        {
            this._dbContext = roofCareEntitiesSet;
        }
        public ResponseModel GetServices()
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;
                    var services = (from service in _dbContext.Professions
                                    select new
                                    {
                                        service.ProfessionId,
                                        service.ProfessionName,
                                        service.ProfessionImage
                                    }).ToList();

                    if (services.Count == 0)
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "Services are not found");
                    }
                    else
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, services);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }
        public ResponseModel GetServiceProviders()
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;
                    var serviceproviders = (from serviceprovider in _dbContext.Users
                                            select new
                                            {
                                                serviceprovider.UserId,
                                                serviceprovider.Username,
                                                serviceprovider.FullName,
                                                serviceprovider.UserProfileImage,
                                                serviceprovider.UserType,
                                                serviceprovider.UserRating,
                                            }).Where(y => y.UserType == "Service Provider").OrderByDescending(z => z.UserRating).ToList();

                    if (serviceproviders.Count == 0)
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "Services are not found");
                    }
                    else
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, serviceproviders);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }
        //SearchResponse
        public ResponseModel Search(string name)
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;

                    var searchResult = (from service in _dbContext.UserProfessions
                                        select new
                                        {
                                            service.User.UserId,
                                            service.User.FullName,
                                            service.User.Username,
                                            service.User.UserProfileImage,
                                            service.User.UserRating,
                                            service.User.UserType,
                                            service.Profession.ProfessionName

                                        }).Where(q => q.FullName.StartsWith(name) || q.ProfessionName.StartsWith(name)).ToList();

                    return HelperClass.Response(false, GlobalDecleration._successAction, searchResult);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }
        public ResponseModel Cities()
        {
            try
            {
                using (_dbContext)
                {
                    var disctricts = (from disctrict in _dbContext.Districts
                                      select new
                                      {
                                          disctrict.DistrictId,
                                          disctrict.DistrictName,
                                          disctrict.DisctrictImage,
                                      }).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, disctricts);
                }
            }
            catch (Exception ex)
            {
                return Helper.HelperClass.Response(false, GlobalDecleration._internalServerError, "Failed to get cities " + ex);
            }
        }
        public ResponseModel GetSPList(string professionType)
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;
                    var serviceProvidersList = (from services in _dbContext.UserProfessions
                                                select new
                                                {
                                                    services.User.UserId,
                                                    services.User.Username,
                                                    services.User.FullName,
                                                    services.User.UserRating,
                                                    services.User.UserType,
                                                    services.User.UserProfileImage,
                                                    services.Profession.ProfessionName,
                                                }).Where(progession
                                                => progession.ProfessionName
                                                == professionType).ToList();

                    return HelperClass.Response(true, GlobalDecleration._successAction, serviceProvidersList);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Error! " + ex);
            }
        }
    }
}

/*var users = (from user in _dbContext.Users
                                        select new
                                        {
                                            user.UserId,
                                            user.FullName,
                                            user.UserProfileImage,
                                            user.UserRating,
                                            user.UserType,
                                            userProfessions = (from userProfession in user.UserProfessions
                                                               select new
                                                               {
                                                                   userProfession.Profession.ProfessionId,
                                                                   userProfession.Profession.ProfessionName,
                                                               }).ToList()
                                        }).Where(q => q.FullName.StartsWith(name)
                                        && q.UserType.Equals("Service Provider")).ToList();*/

/*for (int i = 0; i < searchResult.Count; i++)
                   {
                       if(searchResult[i].userProfessions[i].ProfessionName == name)
                       {

                       }
                   }*/

/*if (searchResult.Count == 0)
{
    return HelperClass.Response(false, GlobalDecleration._successAction, "User not found");
}
else
{*/

//}
/*
 userProfessions = (from userProfession in user.UserProfessions
        select 
        {
                        userProfession.Profession.ProfessionId,
                        userProfession.Profession.ProfessionName,
                        }).Where(y => y.ProfessionName.StartsWith(name))
*/
/*var serviceproviders = _dbContext.Users.Where(q => q.FullName.StartsWith(name)
&& q.UserType.Equals("Service Provider")).ToList();

var services = _dbContext.Professions.Where(p => p.ProfessionName.StartsWith(name)).ToList();

if (serviceproviders.Count == 0 && services.Count == 0)
{
    return HelperClass.SearchResponse(false, GlobalDecleration._noUser, "User are not found", "");
}
else
{
    return HelperClass.SearchResponse(false, GlobalDecleration._successAction, serviceproviders, services);
}*/
