using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Roof_Care.Services.AdminServices
{
    public class AdminNotificationServices
    {
        private readonly RoofCareEntities _dbContext;
        public AdminNotificationServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal object AddNotification(Notification notification)
        {
            try
            {
                using (_dbContext)
                {
                    notification.NotificationDate = DateTime.Now;
                    _dbContext.Notifications.Add(notification);
                    _dbContext.SaveChanges();

                    return HelperClass.Response(true, GlobalDecleration._successAction, notification.NotificationId.ToString());
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal object GetNotification(int notificationId)
        {
            try
            {
                using (_dbContext)
                {
                    var notification = _dbContext.Notifications.Find(notificationId);
                    return HelperClass.Response(true, GlobalDecleration._successAction, notification);
                }
            }catch(Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }
    }
}