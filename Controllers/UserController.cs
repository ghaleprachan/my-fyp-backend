using Roof_Care.AuthenticationDecode;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
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
    public class UserController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private UserServices userServices;
        public UserController()
        {
            _dbContext = new RoofCareEntities();
            this.userServices = new UserServices(_dbContext);
        }

        // GET api/<controller>
        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            return Ok(userServices.GetAllUsers());
        }
        [HttpPost]
        [Route("api/user/GetUserBasicDetails/")]
        public IHttpActionResult GetUserBasicDetails(UserIdModel userIdModel)
        {
            try
            {
                return Ok(userServices.GetUserBasicDetails(EncodeDecode.GetUserId(userIdModel.tokenNumber)));
            }
            catch (Exception ex)
            {
                return BadRequest("Error" + ex);
            }
        }

        [HttpPost]
        [Route("api/user/GetUserHeader/")]
        public IHttpActionResult GetUserHeader(UserIdModel userIdModel)
        {
            try
            {
                return Ok(userServices.GetUserHeader(EncodeDecode.GetUserId(userIdModel.tokenNumber)));
            }
            catch (Exception ex)
            {
                return BadRequest("Error" + ex);
            }
        }

        // GET api/<controller>/5
        [HttpPost]
        [Route("api/user/GetUserDetails/")]
        public IHttpActionResult GetUserDetails(UserIdModel userModel)
        {
            try
            {
                return Ok(userServices.GetUserDetails(EncodeDecode.GetUserId(userModel.tokenNumber)));
            }
            catch (Exception ex)
            {
                return BadRequest("Error " + ex);
            }
        }
        [HttpPost]
        [Route("api/user/GetUserAbout/")]
        public IHttpActionResult GetUserAbout(UserIdModel userModel)
        {
            try
            {
                return Ok(userServices.GetUserAbout(EncodeDecode.GetUserId(userModel.tokenNumber)));
            }
            catch (Exception ex)
            {
                return BadRequest("Error " + ex);
            }
        }

        [HttpPost]
        [Route("api/user/GetUserPosts/")]
        public IHttpActionResult GetUserPosts(UserIdModel userModel)
        {
            try
            {
                return Ok(userServices.GetUserPosts(EncodeDecode.GetUserId(userModel.tokenNumber)));
            }
            catch (Exception ex)
            {
                return BadRequest("Error " + ex);
            }
        }

        [HttpPost]
        [Route("api/user/GetUserFeedbacks/")]
        public IHttpActionResult GetUserFeedbacks(UserIdModel userModel)
        {
            try
            {
                return Ok(userServices.GetUserFeedbacks(EncodeDecode.GetUserId(userModel.tokenNumber)));
            }
            catch (Exception ex)
            {
                return BadRequest("Error " + ex);
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                return Ok(userServices.AddNewUser(user));
            }
            else
            {
                return Ok(HelperClass.Response(false, Declarations.GlobalDecleration._invalidModel, "Error!"));
            }
        }

        //?myId=1&myFavId=8
        [HttpPost]
        [Route("api/user/AddToFavorites/")]
        public IHttpActionResult AddToFavorites(string myId, string myFavId)
        {
            return Ok(userServices.AddToFavotites(EncodeDecode.GetUserId(myId), EncodeDecode.GetUserId(myFavId)));
        }
    }
}
