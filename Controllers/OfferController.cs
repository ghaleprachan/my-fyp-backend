using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using Roof_Care.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class OfferController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly OfferServices offerService;
        private OfferController()
        {
            _dbContext = new RoofCareEntities();
            offerService = new OfferServices(_dbContext);
        }
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(offerService.GetAllOffers());
        }
        /*[HttpPost]
        public IHttpActionResult Post(Offer offer)
        {
            if (ModelState.IsValid)
            {
                return Ok(offerService.AddOffer(offer));
            }
            else
            {
                return Ok(Declarations.GlobalDecleration._invalidModel);
            }
        }*/

        [HttpPost]
        public IHttpActionResult Post([FromBody] NewOfferModel offer)
        {
            if (ModelState.IsValid)
            {
                return Ok(offerService.PostNewOffer(offer));
            }
            else
            {
                return Ok(Declarations.GlobalDecleration._invalidModel);
            }
        }

        [HttpDelete]
        [Route("api/Offer/DeleteOffer")]
        public IHttpActionResult DeleteOffer(int offerId)
        {
            return Ok(offerService.DeleteOffer(offerId));
        }
    }
}