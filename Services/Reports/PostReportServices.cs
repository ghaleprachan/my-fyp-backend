using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Roof_Care.Models.Reports
{
    public class ReportServices
    {
        private readonly RoofCareEntities _dbContext;
        public ReportServices(RoofCareEntities roofCareEntities)
        {
            this._dbContext = roofCareEntities;
        }

        internal ResponseModel AddOfferReport(PostReportModel postReport)
        {
            try
            {
                using (_dbContext)
                {
                    int reported_by = EncodeDecode.GetUserId(postReport.ReportedBy);
                    OfferReport new_report = new OfferReport
                    {
                        ReportedById = reported_by,
                        ReportedOfferId = postReport.PostId,
                        ReportText = postReport.ReportText
                    };
                    _dbContext.OfferReports.Add(new_report);
                    _dbContext.SaveChanges();
                    return HelperClass.Response(true, GlobalDecleration._successAction, "Successfully Reported");
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex.Message);
            }
            throw new NotImplementedException();
        }

        internal ResponseModel AddProblemReport(PostReportModel postReport)
        {
            try
            {
                using (RoofCareEntities dbContext = new RoofCareEntities())
                {
                    int reported_by = EncodeDecode.GetUserId(postReport.ReportedBy);
                    ProblemReport new_report = new ProblemReport
                    {
                        ReportedById = reported_by,
                        ReportedProblemId = postReport.PostId,
                        ReportText = postReport.ReportText
                    };
                    dbContext.ProblemReports.Add(new_report);
                    dbContext.SaveChanges();
                    return HelperClass.Response(true, GlobalDecleration._successAction, "Successfully Reported");
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