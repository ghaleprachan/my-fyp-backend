using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using Roof_Care.Models.ReplyModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Roof_Care.Services
{
    public class AddFeedbackServices
    {
        private readonly RoofCareEntities _dbContext;
        public AddFeedbackServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal ResponseModel AddNewFeedback(FeedbackModel feedback)
        {
            try
            {
                using (RoofCareEntities dbContext = new RoofCareEntities())
                {
                    int feedbackBy = EncodeDecode.GetUserId(feedback.FeedbackBy);
                    int feedbackTo = EncodeDecode.GetUserId(feedback.FeedbackTo);
                    Feedback new_feedback = new Feedback
                    {
                        FeedbackBy = feedbackBy,
                        FeedbaclTo = feedbackTo,
                        Feedback1 = feedback.Feedback,
                        Rating = feedback.Rating,
                        FeedbackDate = DateTime.Now
                    };
                    dbContext.Feedbacks.Add(new_feedback);
                    dbContext.SaveChanges();
                    User user = dbContext.Users.Find(feedbackTo);
                    if (user != null)
                    {
                        user.UserRating = GetRatings(feedbackTo);
                        dbContext.Entry(user).State = EntityState.Modified;
                    }

                    dbContext.SaveChanges();
                    return HelperClass.Response(true, GlobalDecleration._successAction, "Added");
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal object GetReply(int feedbackId)
        {
            try
            {
                using (_dbContext)
                {
                    var replies = (from reply in _dbContext.FeedbackReplies
                                   select new
                                   {
                                       reply.ReplyId,
                                       reply.User.UserId,
                                       reply.User.Username,
                                       reply.User.FullName,
                                       reply.User.UserProfileImage,
                                       reply.ReplyDate,
                                       reply.Reply,
                                       reply.FeedbackId,
                                       reply.User.UserType,
                                   }).Where(r => r.FeedbackId == feedbackId).ToList();
                    return HelperClass.Response(true, GlobalDecleration._successAction, replies);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal object AddReply(ReplyModel feedbackReply)
        {
            try
            {
                using (_dbContext)
                {
                    int replyGivenBy = EncodeDecode.GetUserId(feedbackReply.FeedbackBy);
                    FeedbackReply new_reply = new FeedbackReply
                    {
                        FeedbackId = feedbackReply.FeedbackId,
                        Reply = feedbackReply.Reply,
                        ReplyBy = replyGivenBy,
                        ReplyDate = DateTime.Now
                    };
                    _dbContext.FeedbackReplies.Add(new_reply);
                    _dbContext.SaveChanges();
                    return HelperClass.Response(true, GlobalDecleration._successAction, feedbackReply.FeedbackId);
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        public double GetRatings(int id)
        {
            try
            {
                using (_dbContext)
                {
                    int fivestar = (from feedback in _dbContext.Feedbacks
                                    select new
                                    {
                                        feedback.Rating,
                                        feedback.FeedbaclTo,
                                    }).Where(user => user.FeedbaclTo.Equals(id) && Math.Round(user.Rating, 1).Equals(5)).ToList().Count();

                    int fourstart = (from feedback in _dbContext.Feedbacks
                                     select new
                                     {
                                         feedback.Rating,
                                         feedback.FeedbaclTo,
                                     }).Where(user => user.FeedbaclTo.Equals(id) && Math.Round(user.Rating, 0).Equals(4)).ToList().Count();

                    int threestar = (from feedback in _dbContext.Feedbacks
                                     select new
                                     {
                                         feedback.Rating,
                                         feedback.FeedbaclTo,
                                     }).Where(user => user.FeedbaclTo.Equals(id) && Math.Round(user.Rating, 0).Equals(3)).ToList().Count();

                    int twostar = (from feedback in _dbContext.Feedbacks
                                   select new
                                   {
                                       feedback.Rating,
                                       feedback.FeedbaclTo,
                                   }).Where(user => user.FeedbaclTo.Equals(id) && Math.Round(user.Rating, 0).Equals(2)).ToList().Count();

                    int onestar = (from feedback in _dbContext.Feedbacks
                                   select new
                                   {
                                       feedback.Rating,
                                       feedback.FeedbaclTo,
                                   }).Where(user => user.FeedbaclTo.Equals(id) && Math.Round(user.Rating, 0).Equals(1)).ToList().Count();

                    double rating = (double)(5 * fivestar + 4 * fourstart + 3 * threestar + 2 * twostar + 1 * onestar) / (onestar + twostar + threestar + fourstart + fivestar);

                    rating = Math.Round(rating, 1);

                    return rating;
                }
            }
            catch (Exception)
            {
                return 0.0;
            }
        }
    }
}