using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using Roof_Care.Models.Reports;
using Roof_Care.Models.UserSaved;
using Roof_Care.Services.UserSaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class SavePostsController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly SavePostsServices savePosts;

        public SavePostsController()
        {
            _dbContext = new RoofCareEntities();
            savePosts = new SavePostsServices(_dbContext);
        }
        [HttpPost]
        [Route("api/SavedPosts/GetSavedOffers/")]
        public IHttpActionResult GetSavedOffers(UserIdModel tokenNumber)
        {
            if (ModelState.IsValid)
            {
                return Ok(savePosts.GetSavedOffers(tokenNumber));
            }
            else
            {
                return BadRequest("Invalid Model");
            }
        }

        [HttpPost]
        [Route("api/SavedPosts/SavedPostOfferId")]
        public IHttpActionResult SavedPostOfferId(UserIdModel userIdModel)
        {
            if (ModelState.IsValid)
            {
                return Ok(savePosts.GetSavePostsId(userIdModel));
            }
            else
            {
                return BadRequest("Invalid Model");
            }
        }

        [HttpPost]
        [Route("api/SavePosts/SaveOffer/")]
        public IHttpActionResult SaveOffer(SavePostModel userSaved)
        {
            if (ModelState.IsValid)
            {
                return Ok(savePosts.SaveOffer(userSaved));
            }
            else
            {
                return BadRequest("Bad request");
            }
        }
    }
}
