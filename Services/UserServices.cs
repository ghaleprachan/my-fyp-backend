using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roof_Care.Services
{
    public class UserServices
    {
        private readonly RoofCareEntities _dbContext;
        public UserServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }
        public ResponseModel GetAllUsers()
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;

                    //var offers = _dbContext.Offers.Include("User").ToList();
                    var Users = (from users in _dbContext.Users
                                 select new
                                 {
                                     userid = users.UserId,
                                     username = users.Username,
                                     passoword = users.Password,
                                     fullname = users.FullName,
                                     gender = users.Gender,
                                     image = users.UserProfileImage,
                                     ratings = users.UserRating,
                                     usertype = users.UserType,
                                     contacts = from contact in users.Contacts
                                                select new
                                                {
                                                    contactnumber = contact.ContactNumber,
                                                    contacttype = contact.ContactType,
                                                }
                                 }).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, Users);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }

        internal ResponseModel GetUserHeader(int userId)
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;
                    var user = (from users in _dbContext.Users
                                select new
                                {
                                    users.UserId,
                                    users.Username,
                                    users.Password,
                                    users.FullName,
                                    users.Gender,
                                    users.UserProfileImage,
                                    users.UserRating,
                                    users.UserType
                                }).
                                ToList().Where(x => x.UserId == userId).FirstOrDefault();

                    if (user == null)
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "");
                    }
                    else
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, user);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        public ResponseModel GetUserDetails(int userId)
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;
                    //var user = _dbContext.Users.Find(userId);
                    /* User user = _dbContext.Users.
                         Include("Contacts").
                         Include("Emails").
                         Include("Addresses").
                         ToList().Where(x => x.UserId == userId).FirstOrDefault();*/

                    var user = (from users in _dbContext.Users
                                select new
                                {
                                    users.UserId,
                                    users.Username,
                                    users.Gender,
                                    users.UserType,
                                    Contacts = from contact in users.Contacts
                                               select new
                                               {
                                                   contact.ContactNumber,
                                                   contact.ContactType,
                                               },
                                    Emails = from emails in users.Emails
                                             select new
                                             {
                                                 emails.Email1,
                                                 emails.EmailType
                                             },
                                    Addresses = from addresses in users.Addresses
                                                select new
                                                {
                                                    addresses.District.DistrictName,
                                                    addresses.Municipality.MunicipalityName,
                                                    addresses.CurrentLocation,
                                                    addresses.AddressType
                                                },
                                    Favorites = from favorites in users.Favorites
                                                select new
                                                {
                                                    favorites.SPId,
                                                    favorites.User1.FullName

                                                },
                                    Recommendations = (from fv in _dbContext.Favorites
                                                       select new
                                                       {
                                                           fvone = fv.SPId,
                                                           fByid = fv.CustomerId
                                                       }).Where(x => x.fvone == userId).Count(),
                                }).
                                ToList().Where(x => x.UserId == userId).FirstOrDefault();

                    if (user == null)
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "");
                    }
                    else
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, user);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }

        }

        // Have to add work ratio and about the user includes his her description for long paragraph
        internal object GetUserAbout(int userId)
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;
                    var user = (from users in _dbContext.Users
                                select new
                                {
                                    users.UserId,
                                    users.Username,
                                    users.Gender,
                                    users.UserType,
                                    users.AboutUser,
                                    UserProfessions = from service in users.UserProfessions
                                                      select new
                                                      {
                                                          service.ProfessionId,
                                                          service.UserProfessionId,
                                                          service.Profession.ProfessionName
                                                      },
                                }).
                                ToList().Where(x => x.UserId == userId).FirstOrDefault();

                    if (user == null)
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "");
                    }
                    else
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, user);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        public ResponseModel GetUserPosts(int userId)
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;

                    var user = (from users in _dbContext.Users
                                select new
                                {
                                    userid = users.UserId,
                                    username = users.Username,
                                    fullname = users.FullName,
                                    image = users.UserProfileImage,
                                    offers = from offer in users.Offers
                                             select new
                                             {
                                                 offer.offerId,
                                                 offer.OfferDescription,
                                                 offer.OfferImage,
                                                 offer.PostedDate,
                                                 offer.ValidDate
                                             },
                                    problems = from problem in users.Problems
                                               select new
                                               {
                                                   problem.ProblemId,
                                                   problem.ProblemDescription,
                                                   problem.ProblemImage,
                                                   problem.PostedDate
                                               }
                                }).
                                ToList().Where(x => x.userid == userId).FirstOrDefault();

                    if (user == null)
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "");
                    }
                    else
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, user);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }

        }

        public ResponseModel GetUserFeedbacks(int userId)
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;
                    var user = (from users in _dbContext.Users
                                select new
                                {
                                    userid = users.UserId,
                                    username = users.Username,
                                    fullname = users.FullName,
                                    feedbacks = (from feedback in _dbContext.Feedbacks
                                                 select new
                                                 {
                                                     feedback.FeedbackBy,
                                                     feedback.FeedbaclTo,
                                                     feedback.Rating,
                                                     feedback.Feedback1, //This is a feedback description
                                                     feedback.FeedbackId,
                                                     feedback.User.FullName,
                                                     feedback.User.Username,
                                                     feedback.User.UserType,
                                                     feedback.User.UserProfileImage,
                                                     feedback.FeedbackDate,
                                                     ReplayCount = (from reply in feedback.FeedbackReplies
                                                                    select new
                                                                    {
                                                                        reply.ReplyId,
                                                                        reply.FeedbackId
                                                                    })
                                                                    .Where(f => f.FeedbackId == feedback.FeedbackId)
                                                                    .ToList()
                                                                    .Count
                                                 }).Where(x => x.FeedbaclTo == userId)
                                                 .OrderByDescending(d => d.FeedbackDate)
                                }).ToList()
                                .Where(x => x.userid == userId)
                                .FirstOrDefault();

                    if (user == null)
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "");
                    }
                    else
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, user);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }

        }

        internal ResponseModel AddNewUser(User user)
        {
            try
            {
                using (_dbContext)
                {
                    if (UserValidation(user.Username) == false)
                    {
                        return HelperClass.Response(false, GlobalDecleration._alreadyExits, "Failed");
                    }
                    else
                    {
                        User newUser = new User
                        {
                            Username = user.Username,
                            Password = user.Password,
                            FullName = user.FullName,
                            Gender = user.Gender,
                            UserType = user.UserType,
                            UserRating = user.UserRating,
                            UserProfileImage = user.UserProfileImage,
                            UserProfessions = user.UserProfessions,
                            Addresses = user.Addresses,
                            Contacts = user.Contacts,
                            Emails = user.Emails
                        };
                        _dbContext.Users.Add(newUser);
                        _dbContext.SaveChanges();

                        int id = newUser.UserId;
                        return HelperClass.Response(true, GlobalDecleration._savedSuccesfully, "Success: " + id);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }

        public bool UserValidation(string username)
        {
            using (_dbContext)
            {
                User existing_user = _dbContext.Users.FirstOrDefault(x => x.Username.Equals(username));
                if (existing_user == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        internal ResponseModel AddToFavotites(int myId, int myFavId)
        {
            try
            {
                using (_dbContext)
                {
                    /*var checkFavorites = (from favorite in _dbContext.Favorites
                                         select new
                                         {
                                             favId = favorite.FavoriteId,
                                             myInTable = favorite.CustomerId,
                                             myFavTable = favorite.SPId
                                         }).Where(x=>x.myInTable == myId && x.myFavTable == myFavId).ToList();*/
                    if (myId == myFavId)
                    {
                        return HelperClass.Response(false,
                            GlobalDecleration._noUser, "Abe sale apne aap to favorites me dalega kya");
                    }
                    Favorite checkInFav = _dbContext.Favorites.FirstOrDefault(x =>
                    x.CustomerId == myId &&
                    x.SPId == myFavId);
                    if (checkInFav == null)
                    {
                        Favorite new_favorite = new Favorite
                        {
                            CustomerId = myId,
                            SPId = myFavId
                        };
                        _dbContext.Favorites.Add(new_favorite);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Added Successfully");
                    }
                    else
                    {
                        _dbContext.Favorites.Remove(checkInFav);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Removed From Favorites");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Error is: " + ex);
            }
            throw new NotImplementedException();
        }

        public ResponseModel GetUserBasicDetails(int id)
        {
            try
            {
                using (_dbContext)
                {
                    var userBasicDetails = (from user in _dbContext.Users
                                            select new
                                            {
                                                user.UserId,
                                                user.Username,
                                                user.FullName,
                                                user.UserProfileImage,
                                                user.UserType
                                            }).Where(u => u.UserId == id).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, userBasicDetails);
                }
            }
            catch (Exception ex)
            {
                return Helper.HelperClass.Response(false, GlobalDecleration._internalServerError, "Server Issue!" + ex);
            }
        }
    }
}