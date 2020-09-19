using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Roof_Care.Services.AdminServices
{
    public class AdminReportPostsServices
    {
        private readonly RoofCareEntities _dbContext;
        public AdminReportPostsServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal object GetReportedOffers(int adminId)
        {
            try
            {
                using (_dbContext)
                {
                    Admin admin = _dbContext.Admins.Find(adminId);
                    if (admin != null)
                    {
                        var reportedOffers = (from offerReport in _dbContext.Offers
                                              select new
                                              {
                                                  offerReport.offerId,
                                                  OfferById = offerReport.User.UserId,
                                                  OfferByName = offerReport.User.FullName,
                                                  OfferByImage = offerReport.User.UserProfileImage,
                                                  offerReport.ValidDate,
                                                  offerReport.OfferDescription,
                                                  offerReport.PostedDate,
                                                  offerReport.OfferImage,
                                                  ReportCount = offerReport.OfferReports.Count,
                                                  OfferReports = (from report in offerReport.OfferReports
                                                                  select new
                                                                  {
                                                                      report.Id,
                                                                      report.ReportText,
                                                                      ReportedById = report.User.UserId,
                                                                      ReportedByName = report.User.FullName,
                                                                      ReportedByImage = report.User.UserProfileImage,
                                                                  }).ToList()
                                              })
                                              .Where(reportCount => reportCount.ReportCount > 0)
                                              .OrderByDescending(order => order.ReportCount)
                                              .ToList();
                        return HelperClass.Response(true, GlobalDecleration._successAction, reportedOffers);
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "Invalid Admin Id");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal object DeleteReportedOffer(int offerId)
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
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Offer Deleted Successful");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "Offer not found");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal object GetReportedProblems(int adminId)
        {
            try
            {
                using (_dbContext)
                {
                    Admin admin = _dbContext.Admins.Find(adminId);
                    if (admin != null)
                    {
                        var reportedProblems = (from problemReport in _dbContext.Problems
                                                select new
                                                {
                                                    problemReport.ProblemId,
                                                    ProblemById = problemReport.User.UserId,
                                                    ProblemByName = problemReport.User.FullName,
                                                    ProblemByImage = problemReport.User.UserProfileImage,
                                                    problemReport.ProblemDescription,
                                                    problemReport.PostedDate,
                                                    problemReport.ProblemImage,
                                                    ReportCount = problemReport.ProblemReports.Count,
                                                    ProblemReports = (from report in problemReport.ProblemReports
                                                                      select new
                                                                      {
                                                                          report.Id,
                                                                          report.ReportText,
                                                                          ReportedById = report.User.UserId,
                                                                          ReportedByName = report.User.FullName,
                                                                          ReportedByImage = report.User.UserProfileImage,
                                                                      }).ToList()
                                                })
                                              .Where(reportCount => reportCount.ReportCount > 0)
                                              .OrderByDescending(order => order.ReportCount)
                                              .ToList();
                        return HelperClass.Response(true, GlobalDecleration._successAction, reportedProblems);
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "Invalid Admin Id");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
            throw new NotImplementedException();
        }

        internal object DeleteReportedProblem(int problemId)
        {
            try
            {
                using (_dbContext)
                {
                    Problem problem = _dbContext.Problems.Find(problemId);
                    if (problem != null)
                    {
                        _dbContext.Problems.Remove(problem);
                        _dbContext.SaveChanges();
                        return HelperClass.Response(true, GlobalDecleration._successAction, "Problem Deleted");
                    }
                    else
                    {
                        return HelperClass.Response(false, GlobalDecleration._successAction, "Problem Not Found");
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }
    }
}