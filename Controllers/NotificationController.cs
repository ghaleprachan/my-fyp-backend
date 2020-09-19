using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using Roof_Care.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class NotificationController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly NotificationServices notificationServices;
        public NotificationController()
        {
            _dbContext = new RoofCareEntities();
            notificationServices = new NotificationServices(_dbContext);
        }

        [HttpGet]
        [Route("api/Notification/GetAllNotifications/")]
        public IHttpActionResult GetAllNotifications()
        {
            return Ok(notificationServices.GetAllNotifications());
        }
    }
}
