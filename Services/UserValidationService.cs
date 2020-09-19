using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Roof_Care.Services
{
    public class UserValidationService
    {
        private readonly RoofCareEntities _dbContext;
        public UserValidationService(RoofCareEntities roofCareEntitiesModels)
        {
            this._dbContext = roofCareEntitiesModels;
        }

        internal LogInModelResponse VerifyUser(UserValidationModel userModel)
        {
            try
            {
                using (_dbContext)
                {
                    if (_dbContext.Users.Any(user => user.Username.Equals(
                         userModel.username, StringComparison.OrdinalIgnoreCase) && user.Password == userModel.password))
                    {
                        User user = _dbContext.Users.FirstOrDefault(x => x.Username.Equals(userModel.username, StringComparison.OrdinalIgnoreCase)
                        && x.Password == userModel.password);

                        string encryptionToken = EncodeDecode.EncodeUserId(user.UserId, user.Username);
                        AfterLogInModel afterLogInModel = new AfterLogInModel
                        {
                            username = user.Username,
                            fullName = user.FullName,
                            userImage = user.UserProfileImage
                        };
                        return HelperClass.LogInValid(true, GlobalDecleration._successAction, encryptionToken);
                    }
                    else
                    {
                        return HelperClass.LogInValid(false, GlobalDecleration._noUser, "Not type found");
                    }
                }
            }
            catch
            {
                return HelperClass.LogInValid(false, GlobalDecleration._internalServerError, "Sorry for inconvenient!");
            }

        }
    }
}