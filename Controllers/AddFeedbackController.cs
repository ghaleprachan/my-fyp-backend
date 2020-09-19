using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using Roof_Care.Models.ReplyModel;
using Roof_Care.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class AddFeedbackController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly AddFeedbackServices chatsServices;
        private AddFeedbackController()
        {
            _dbContext = new RoofCareEntities();
            chatsServices = new AddFeedbackServices(_dbContext);
        }

        [HttpPost]
        [Route("api/AddFeedback/PostFeedback")]
        public IHttpActionResult PostFeedback(FeedbackModel feedback)
        {
            if (ModelState.IsValid)
            {
                return Ok(chatsServices.AddNewFeedback(feedback));
            }
            else
            {
                return BadRequest("Invalid Model");
            }
        }

        [HttpPost]
        [Route("api/AddFeedback/PostReply")]
        public IHttpActionResult PostReply(ReplyModel feedbackReply)
        {
            return Ok(chatsServices.AddReply(feedbackReply));
        }

        [HttpGet]
        [Route("api/AddFeedback/GetReply")]
        public IHttpActionResult GetReply(int feedbackId)
        {
            return Ok(chatsServices.GetReply(feedbackId));
        }
    }
}
