using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Roof_Care.Services
{
    public class OfferServices
    {
        private readonly RoofCareEntities _dbContext;
        public OfferServices(RoofCareEntities roofCareEntitiesSet)
        {
            this._dbContext = roofCareEntitiesSet;
        }

        internal ResponseModel GetAllOffers()
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;
                    var offers = (from offer in _dbContext.Offers
                                  select new
                                  {
                                      offer.offerId,
                                      offer.OfferDescription,
                                      offer.OfferImage,
                                      offer.ValidDate,
                                      offer.PostedDate,
                                      offer.User.UserType,
                                      postedById = offer.offerBy,
                                      postedByUsername = offer.User.Username,
                                      postedBy = offer.User.FullName,
                                      userPhoto = offer.User.UserProfileImage,
                                      Contacts = from contact in offer.User.Contacts
                                                 select new
                                                 {
                                                     contact.ContactNumber,
                                                     contact.ContactType,
                                                 },
                                  }).OrderByDescending(q => q.PostedDate).ToList();

                    if (offers.Count == 0)
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "Error!");
                    }
                    else
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, offers);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }

        internal ResponseModel DeleteOffer(int offerId)
        {
            try
            {
                using (_dbContext)
                {
                    Offer offer = _dbContext.Offers.Find(offerId);
                    if (offer != null)
                    {
                        _dbContext.Offers.Remove(offer);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Deleted Successfully");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "Post not found");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }

        internal ResponseModel PostNewOffer(NewOfferModel newOffer)
        {
            try
            {
                using (_dbContext)
                {
                    Offer offer = new Offer()
                    {
                        offerBy = EncodeDecode.GetUserId(newOffer.PostedById),
                        PostedDate = DateTime.Now,
                        ValidDate = ParseDate(newOffer.OfferValidDate),
                        OfferImage = ParseImage(newOffer.OfferImage),
                        OfferDescription = newOffer.OfferDescription
                    };
                    _dbContext.Offers.Add(offer);
                    _dbContext.SaveChanges();
                    return HelperClass.Response(true,
                        GlobalDecleration._savedSuccesfully, "Your offer is successfully added");
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false,
                    GlobalDecleration._internalServerError, "Faild to post" + ex);
            }
        }
        private string ParseImage(string bitmapString)
        {
            try
            {
                string folderLocation = "~/MyUploads/OfferImages/";
                string imageName = ((RandomString(20) + DateTime.Now) + ".jpg").Replace(":", String.Empty);

                byte[] img = LoadImage(bitmapString);

                string filePath = HttpContext.Current.Server
                    .MapPath(folderLocation +
                    Path.GetFileName(imageName));

                File.WriteAllBytes(filePath, img);

                string finalLocation = "MyUploads/OfferImages/";
                return (finalLocation + imageName);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public byte[] LoadImage(string bitmapString)
        {
            byte[] imageBytes = Convert.FromBase64String(bitmapString);
            return imageBytes;
        }
        private DateTime ParseDate(string dateInString)
        {
            var format = "M/d/yyyy";
            DateTime dateTime = DateTime.ParseExact(dateInString, format, CultureInfo.InvariantCulture);
            return dateTime;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

}
/*string fileName = RandomString(50);
                    string filePath = "~/UploadImages/OfferImages" + fileName;
                    img.Save(HttpContext.Current.Server.MapPath(filePath));
                    return filePath;*/
/*var img = LoadImage(bitmapString);
img.Save(HttpContext.Current.Server.MapPath(
    "~/UploadImages/OfferImages/") + RandomString(50));
return HttpContext.Current
    .Server.MapPath($"~/UploadImages/OfferImages/" + RandomString(50)+".jpg");*/

/*var ctx = HttpContext.Current;
var root = ctx.Server.MapPath("~/App_Date");
var provider = new MultipartFormDataStreamProvider(root);
try
{
    Request.Content.ReadAsMultipartAsync(provider);
}
catch (Exception e)
{
    return $"Error:{e.Message}";
}*/
