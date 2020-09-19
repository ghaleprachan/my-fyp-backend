using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Services.AdminServices
{
    public class AdminLogInServices
    {
        private readonly RoofCareEntities _dbContext;
        public AdminLogInServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal object LogIn(AdminLogIn adminLogIn)
        {
            try
            {
                using (_dbContext)
                {
                    if (_dbContext.Admins.Any(user => user.Username.Equals(
                         adminLogIn.Username, StringComparison.OrdinalIgnoreCase) && user.Password == adminLogIn.Password))
                    {
                        Admin user = _dbContext.Admins.FirstOrDefault(x =>
                        x.Username.Equals(adminLogIn.Username, StringComparison.OrdinalIgnoreCase)
                        && x.Password == adminLogIn.Password);

                        return HelperClass.LogInValid(true, GlobalDecleration._successAction, user);
                    }
                    else
                    {
                        return HelperClass.LogInValid(false, GlobalDecleration._noUser, "Not type found");
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