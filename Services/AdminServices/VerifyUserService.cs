using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Roof_Care.Services.AdminServices
{
    public class VerifyUserService
    {
        private readonly RoofCareEntities _dbContext;
        public VerifyUserService(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal object GetUserToVerify(int adminId)
        {
            try
            {
                using (_dbContext)
                {
                    Admin admin = _dbContext.Admins.Find(adminId);
                    if (admin != null)
                    {
                        var users = (from user in _dbContext.Users
                                     select new
                                     {
                                         user.UserId,
                                         user.FullName,
                                         user.UserProfileImage,
                                         user.Gender,
                                         user.UserType,
                                         user.AboutUser,
                                         user.Verified,
                                         Contacts = (from contact in user.Contacts
                                                     select new
                                                     {
                                                         contact.ContactNumber
                                                     }).ToList(),
                                         Emails = (from email in user.Emails
                                                   select new
                                                   {
                                                       email.Email1
                                                   }),
                                         Professions = (from profession in user.UserProfessions
                                                        select new
                                                        {
                                                            profession.Profession.ProfessionId,
                                                            profession.Profession.ProfessionName
                                                        }).ToList()
                                     })
                                     .Where(u => u.Verified == null || u.Verified == "false")
                                     .Where(p => p.UserType == "Service Provider")
                                     .ToList()
                                     .OrderBy(o => o.UserId);
                        return HelperClass.Response(true, GlobalDecleration._successAction, users);
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._invalidModel, "Invalid User");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal object VerifyProfile(int userId)
        {
            try
            {
                using (_dbContext)
                {
                    User user = _dbContext.Users.Find(userId);
                    if (user != null)
                    {
                        user.Verified = "true";
                        _dbContext.Entry(user).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "User Verified");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "User not found");
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