using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using Roof_Care.Models.UserSaved;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Roof_Care.Services.UserSaves
{
    public class SavePostsServices
    {
        private readonly RoofCareEntities _dbContext;
        public SavePostsServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal ResponseModel SaveOffer(SavePostModel userSaved)
        {
            try
            {
                using (_dbContext)
                {
                    int id = EncodeDecode.GetUserId(userSaved.UserId);
                    var old_saved = (from old in _dbContext.UserSaveds
                                     select new
                                     {
                                         old.SavedId,
                                         old.UserId,
                                         old.OfferId,
                                         old.SaveDate
                                     }).Where(od => od.UserId == id && od.OfferId == userSaved.PostId)
                                           .ToList()
                                           .FirstOrDefault();
                    if (old_saved != null)
                    {
                        UserSaved remove_old = _dbContext.UserSaveds.Find(old_saved.SavedId);
                        _dbContext.UserSaveds.Remove(remove_old);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._removedSuccessfully, "Remove Successful");
                    }
                    else
                    {
                        UserSaved new_save = new UserSaved
                        {
                            OfferId = userSaved.PostId,
                            UserId = id,
                            SaveDate = DateTime.Now
                        };
                        _dbContext.UserSaveds.Add(new_save);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._savedSuccesfully, "Saved Successful");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "failed to save " + ex.Message);
            }
            throw new NotImplementedException();
        }

        internal ResponseModel GetSavedOffers(UserIdModel tokenNumber)
        {
            try
            {
                using (_dbContext)
                {
                    int userId = EncodeDecode.GetUserId(tokenNumber.tokenNumber);

                    var savePosts = (from post in _dbContext.UserSaveds
                                     select new
                                     {
                                         post.UserId,
                                         post.SaveDate,
                                         post.OfferId,
                                         post.Offer.ValidDate,
                                         post.Offer.OfferImage,
                                         post.Offer.PostedDate,
                                         post.Offer.OfferDescription,
                                         post.Offer.User.UserType,
                                         PostedById = post.Offer.User.UserId,
                                         PostedByUsername = post.Offer.User.Username,
                                         PostedByImage = post.Offer.User.UserProfileImage,
                                         PostedByName = post.Offer.User.FullName,
                                         Contacts = from contact in post.Offer.User.Contacts
                                                    select new
                                                    {
                                                        contact.ContactNumber,
                                                        contact.ContactType,
                                                    },
                                     }).Where(uId => uId.UserId == userId).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, savePosts);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal ResponseModel GetSavePostsId(UserIdModel userIdModel)
        {
            try
            {
                using (_dbContext)
                {
                    int userId = EncodeDecode.GetUserId(userIdModel.tokenNumber);
                    var savePostId = (from userSaved in _dbContext.UserSaveds
                                      select new
                                      {
                                          userSaved.SavedId,
                                          userSaved.UserId,
                                          userSaved.OfferId,
                                      }).Where(UI => UI.UserId == userId).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, savePostId);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Failed: " + ex.Message);
            }
        }
    }
}