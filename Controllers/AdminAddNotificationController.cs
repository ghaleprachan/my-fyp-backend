using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Services.AdminServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Roof_Care.Controllers
{
    public class AdminAddNotificationController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly AdminNotificationServices adminNotification;
        public AdminAddNotificationController()
        {
            _dbContext = new RoofCareEntities();
            adminNotification = new AdminNotificationServices(_dbContext);
        }
        [HttpGet]
        [Route("api/AdminAddNotification/GetNotification")]
        public IHttpActionResult GetNotification(int notificationId)
        {
            return Ok(adminNotification.GetNotification(notificationId));
        }

        [HttpPost]
        [Route("api/AdminAddNotification/AddNotification")]
        public IHttpActionResult AddNotification(Notification notification)
        {
            if (ModelState.IsValid)
            {
                return Ok(adminNotification.AddNotification(notification));
            }
            else
            {
                return Ok(HelperClass.Response(false, GlobalDecleration._invalidModel, "Failed"));
            }
        }
    }
}
