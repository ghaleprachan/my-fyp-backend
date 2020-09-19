using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace Roof_Care.Models.AdminModels
{
    public class AdminDistrictMunicipalityServices
    {
        private readonly RoofCareEntities _dbContext;
        public AdminDistrictMunicipalityServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal object GetPlaces(int adminId)
        {
            try
            {
                using (_dbContext)
                {
                    Admin admin = _dbContext.Admins.Find(adminId);
                    if (admin != null)
                    {
                        var available_places = (from places in _dbContext.Districts
                                                select new
                                                {
                                                    places.DistrictId,
                                                    places.DistrictName,
                                                    places.DisctrictImage,
                                                    Municipalities = (from municipalities in places.Municipalities
                                                                      select new
                                                                      {
                                                                          municipalities.MunicipalityId,
                                                                          municipalities.MunicipalityName
                                                                      }).ToList()
                                                }).ToList();
                        return HelperClass.Response(true, GlobalDecleration._successAction, available_places);
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "Invalid Admin");
                    }

                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal object UpdateDistricts(District new_district)
        {
            try
            {
                using (_dbContext)
                {
                    District old_district = _dbContext.Districts.Find(new_district.DistrictId);
                    if (old_district != null)
                    {
                        old_district.DistrictName = new_district.DistrictName;
                        _dbContext.Entry(old_district).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Updated");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "District not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal object UpdateMunicipality(Municipality municipality)
        {
            try
            {
                using (_dbContext)
                {
                    Municipality old_mun = _dbContext.Municipalities.Find(municipality.MunicipalityId);
                    if (old_mun != null)
                    {
                        old_mun.MunicipalityName = municipality.MunicipalityName;
                        old_mun.DisctrictId = municipality.DisctrictId;
                        _dbContext.Entry(old_mun).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Updated");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "Municipality not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal object DeleteMunicipality(int muniCipalityId)
        {
            try
            {
                using (_dbContext)
                {
                    Municipality municipality = _dbContext.Municipalities.Find(muniCipalityId);
                    if (municipality != null)
                    {
                        _dbContext.Municipalities.Remove(municipality);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Delete Success");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "No Municipality Found");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal object DeleteDistrict(int districtId)
        {
            try
            {
                using (_dbContext)
                {
                    District district = _dbContext.Districts.Find(districtId);
                    if (district != null)
                    {
                        _dbContext.Districts.Remove(district);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Delete Success");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "No District Found");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal object AddMunicipality(MunicipalityModel municipalityModel)
        {
            try
            {
                using (_dbContext)
                {
                    Municipality municipality = new Municipality
                    {
                        MunicipalityName = municipalityModel.MunicipalityName,
                        DisctrictId = municipalityModel.DistrictId
                    };
                    _dbContext.Municipalities.Add(municipality);
                    _dbContext.SaveChanges();
                    return HelperClass.Response(true, GlobalDecleration._successAction, "Municipality Added");
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal object AddDistrict(DistrictModel districtModel)
        {
            try
            {
                using (_dbContext)
                {
                    District district = new District
                    {
                        DistrictName = districtModel.DistrictName,
                        DisctrictImage = ParseImage(districtModel.DistrictImage)
                    };
                    _dbContext.Districts.Add(district);
                    _dbContext.SaveChanges();
                    return HelperClass.Response(true, GlobalDecleration._successAction, "District Added");
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }
        private string ParseImage(string bitmapString)
        {
            try
            {
                string folderLocation = "~/MyUploads/CityImages/";
                string imageName = ((RandomString(10) + DateTime.Now) + ".jpg").Replace(":", String.Empty);

                byte[] img = LoadImage(bitmapString);

                string filePath = HttpContext.Current.Server
                    .MapPath(folderLocation +
                    Path.GetFileName(imageName));

                File.WriteAllBytes(filePath, img);

                string finalLocation = "MyUploads/CityImages/";
                return (finalLocation + imageName);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public byte[] LoadImage(string bitmapString)
        {
            byte[] imageBytes = Convert.FromBase64String(bitmapString);
            return imageBytes;
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}