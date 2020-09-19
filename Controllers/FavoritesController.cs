using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using Roof_Care.Models.Favorites;
using Roof_Care.Services;
using Roof_Care.Services.UserSaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class FavoritesController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly FavoritesServices favoritesService;

        public FavoritesController()
        {
            _dbContext = new RoofCareEntities();
            favoritesService = new FavoritesServices(_dbContext);
        }

        [HttpPost]
        [Route("api/Favorites/AddToFavorites")]
        public IHttpActionResult AddToFavorites(FavoritesModel favorites)
        {
            if (ModelState.IsValid)
            {
                return Ok(favoritesService.AddToFavorites(favorites));
            }
            else
            {
                return BadRequest("Invalid model");
            }
        }

        [HttpPost]
        [Route("api/Favorites/GetUserFavorites")]
        public IHttpActionResult GetUserFavorites(UserIdModel tokenNumber)
        {
            return Ok(favoritesService.GetUserFavorites(tokenNumber));
        }

        [HttpPost]
        [Route("api/Favorites/GetFavUserId")]
        public IHttpActionResult GetFavUserId(FavoritesModel tokenNumber)
        {
            return Ok(favoritesService.GetFavUserId(tokenNumber));
        }
    }
}
