using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Services.AdminServices
{
    public class AdminDeregisterUserServices
    {
        private readonly RoofCareEntities _dbContext;
        public AdminDeregisterUserServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal object GetReportedUsers(int adminId)
        {
            try
            {
                using (_dbContext)
                {
                    Admin admin = _dbContext.Admins.Find(adminId);
                    if (admin != null)
                    {
                        var reportedUsers = (from user in _dbContext.Users
                                             select new
                                             {
                                                 user.UserId,
                                                 user.Username,
                                                 user.FullName,
                                                 user.Gender,
                                                 user.UserType,
                                                 user.UserProfileImage,
                                                 user.UserRating,
                                                 user.AboutUser,
                                                 user.Verified,
                                                 ReportCount = user.Reports1.Count,
                                                 Reports = (from report in user.Reports1
                                                            select new
                                                            {
                                                                report.ReportedTo,
                                                                report.ReportId,
                                                                report.ReportDescription,
                                                                ReportedById = report.User.UserId,
                                                                ReportedByName = report.User.FullName,
                                                                ReportedByImage = report.User.UserProfileImage
                                                            })
                                                            .Where(u => u.ReportedTo == user.UserId)
                                                           .ToList()
                                             })
                                             .Where(c => c.ReportCount > 0 && c.UserType.Equals("Service Provider"))
                                             .OrderByDescending(r => r.ReportCount)
                                             .ToList();
                        return HelperClass.Response(true, GlobalDecleration._successAction, reportedUsers);
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "Invalid admin id");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal object DeregisterUser(int userId)
        {
            try
            {
                using (_dbContext)
                {
                    User user = _dbContext.Users.Find(userId);
                    if (user != null)
                    {
                        _dbContext.Users.Remove(user);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "User removed");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "Not user found");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }
    }
}