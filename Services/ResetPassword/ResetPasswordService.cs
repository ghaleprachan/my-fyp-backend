using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models.ResetPassword;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Roof_Care.Services.ResetPassword
{
    public class ResetPasswordService
    {
        private readonly RoofCareEntities _dbContext;
        public ResetPasswordService(RoofCareEntities roofCareEntities)
        {
            _dbContext = roofCareEntities;
        }

        internal object VerifyUser(ResetPasswordModel resetPassword)
        {
            try
            {
                using (_dbContext)
                {
                    var user_details = (from user in _dbContext.Users
                                        select new
                                        {
                                            user.UserId,
                                            user.Username,
                                            Contacts = (from contact in user.Contacts
                                                        select new
                                                        {
                                                            contact.ContactNumber
                                                        }).Where(n => n.ContactNumber
                                                        == resetPassword.phoneNumber).ToList().FirstOrDefault()
                                        }).Where(u => u.Username == resetPassword.username).ToList().FirstOrDefault();

                    if (user_details != null)
                    {
                        if (user_details.Contacts != null)
                        {
                            return HelperClass.Response(true, GlobalDecleration._successAction, user_details);
                        }
                        else
                        {
                            return HelperClass.Response(false, GlobalDecleration._successAction, "Number don't match");
                        }
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "User Don't exists");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal object UpdatePassword(NewPasswordModel newPassword)
        {
            try
            {
                using (RoofCareEntities dbContext = new RoofCareEntities())
                {
                    User old_user = dbContext.Users.Find(newPassword.userid);
                    if (old_user != null)
                    {
                        old_user.Password = newPassword.password;
                        dbContext.Entry(old_user).State = EntityState.Modified;
                        dbContext.SaveChanges();

                        var new_details = (from user in dbContext.Users
                                           select new
                                           {
                                               user.UserId,
                                               user.Username,
                                           }).Where(u => u.UserId == newPassword.userid).ToList().FirstOrDefault();
                        string token = EncodeDecode.EncodeUserId(new_details.UserId, new_details.Username);
                        return HelperClass.Response(true, GlobalDecleration._successAction, token);
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "failed to update user");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }
    }
}