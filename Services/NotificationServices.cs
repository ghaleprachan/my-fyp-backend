using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Roof_Care.Helper;
using Roof_Care.Declarations;
using Roof_Care.AuthenticationDecode;

namespace Roof_Care.Services
{
    public class NotificationServices
    {
        private readonly RoofCareEntities _dbContext;
        public NotificationServices(RoofCareEntities roofCareEntitiesModels)
        {
            _dbContext = roofCareEntitiesModels;
        }

        internal ResponseModel GetAllNotifications()
        {
            try
            {
                using (_dbContext)
                {
                    var notifications = (from notification in _dbContext.Notifications
                                         select new
                                         {
                                             notification.NotificationId,
                                             notification.NotificationDate,
                                             notification.NotificationText,
                                             notification.NotificationType,
                                         }).ToList()
                                         .OrderByDescending(date => date.NotificationDate);
                    return HelperClass.Response(true, GlobalDecleration._successAction, notifications);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }
    }
}