using Microsoft.AspNet.SignalR;
using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using Roof_Care.Services;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class ChatsController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly ChatsServices chatsServices;
        private ChatsController()
        {
            _dbContext = new RoofCareEntities();
            chatsServices = new ChatsServices(_dbContext);
        }
        [HttpGet]
        [Route("api/chats/GetLastChats/")]
        public IHttpActionResult GetLastChats(string userId)
        {
            return Ok(chatsServices.GetChatHeading(userId));
        }

        /*[HttpPut]
        [Route("api/chats/UpdateLastChat/")]
        public IHttpActionResult UpdateLastChat(LastChatUpdateModel lastChatUpdateModel)
        {
            return Ok(chatsServices.UpDateLastChat(lastChatUpdateModel));
        }*/

        [HttpPut]
        [Route("api/chats/UpdateSeen/")]
        public IHttpActionResult UpdateSeen(LastChatUpdateModel lastChatUpdateModel)
        {
            return Ok(chatsServices.UpdateSeen(lastChatUpdateModel));
        }

        [HttpGet]
        public IHttpActionResult Get(string senderId, string receiverId)
        {
            return Ok(chatsServices.GetChats(senderId, receiverId));
        }

        [HttpPost]
        public IHttpActionResult Post(NewChatModel chat)
        {
            return Ok(chatsServices.AddChats(chat));
        }
    }
}
