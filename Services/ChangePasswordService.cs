using Roof_Care.AuthenticationDecode;
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
    public class ChangePasswordService
    {
        private readonly RoofCareEntities _dbContext;
        public ChangePasswordService(RoofCareEntities roofCareEntitiesModels)
        {
            this._dbContext = roofCareEntitiesModels;
        }

        internal object ValidateUser(UserValidationModel userValidation)
        {
            try
            {
                using (_dbContext)
                {
                    int user_id = EncodeDecode.GetUserId(userValidation.username);
                    User user = _dbContext.Users.Find(user_id);
                    if (user != null)
                    {
                        if (user.Password.Equals(userValidation.password))
                        {
                            return HelperClass.Response(true, GlobalDecleration._successAction, user.UserId);
                        }
                        else
                        {
                            return HelperClass.Response(false, GlobalDecleration._successAction, "Invalid Password");
                        }
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "User Not Found");
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