using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models.StatesMode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Services
{
    public class UserStatesServices
    {
        private readonly RoofCareEntities _dbContext;
        public UserStatesServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal object GetUserStates(string username)
        {
            try
            {
                using (_dbContext)
                {
                    int userId = EncodeDecode.GetUserId(username);
                    var user_states = (from states in _dbContext.UserStates
                                       select new
                                       {
                                           states.OnBudget,
                                           states.OnTime,
                                           states.ServiceProviderId,
                                           states.Completed
                                       })
                                      .Where(u => u.ServiceProviderId == userId)
                                      .ToList();

                    if (user_states.Count > 0)
                    {
                        double total_states = user_states.Count;
                        double on_budget_positive = (from budget in user_states
                                                     select new
                                                     {
                                                         budget.OnBudget
                                                     })
                                          .Where(o => o.OnBudget == "true")
                                          .ToList()
                                          .Count;
                        double completed_positive = (from completed in user_states
                                                     select new
                                                     {
                                                         completed.Completed
                                                     })
                                              .Where(o => o.Completed == "true")
                                              .ToList()
                                              .Count;
                        double on_time_positive = (from on_time in user_states
                                                   select new
                                                   {
                                                       on_time.OnTime
                                                   })
                                              .Where(o => o.OnTime == "true")
                                              .ToList()
                                              .Count;

                        double on_budget_per = (on_budget_positive / total_states) * 100d;
                        double on_time_per = (on_time_positive / total_states) * 100d;
                        double completed_per = (completed_positive / total_states) * 100d;

                        States final_states = new States
                        {
                            OnTime = on_time_per,
                            OnBudget = on_budget_per,
                            Completed = completed_per
                        };
                        return HelperClass.Response(true, GlobalDecleration._successAction, final_states);
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "No states given for user");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }
        /* private float TotalStates()
         {
             using (_dbContext)
             {

             }
         }*/
    }
}