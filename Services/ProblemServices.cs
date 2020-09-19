using Roof_Care.AuthenticationDecode;
using Roof_Care.Declarations;
using Roof_Care.ExmxDbSets;
using Roof_Care.Helper;
using Roof_Care.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Roof_Care.Services
{
    public class ProblemServices
    {
        private readonly RoofCareEntities _dbContext;
        public ProblemServices(RoofCareEntities roofCareEntitiesSet)
        {
            this._dbContext = roofCareEntitiesSet;
        }
        public ResponseModel GetAllProblems()
        {
            try
            {
                using (_dbContext)
                {
                    _dbContext.Configuration.ProxyCreationEnabled = false;
                    var problems = (from problem in _dbContext.Problems
                                    select new
                                    {
                                        problem.ProblemId,
                                        problem.ProblemDescription,
                                        problem.PostedDate,
                                        problem.ProblemImage,
                                        problem.User.UserType,
                                        postedById = problem.User.UserId,
                                        postedByUsername = problem.User.Username,
                                        postedBy = problem.User.FullName,
                                        userPhoto = problem.User.UserProfileImage,
                                        Contacts = from contact in problem.User.Contacts
                                                   select new
                                                   {
                                                       contact.ContactType,
                                                       contact.ContactNumber,
                                                   }
                                    }).OrderByDescending(q => q.PostedDate).ToList();
                    if (problems.Count == 0)
                    {
                        return HelperClass.Response(false, GlobalDecleration._noUser, "Error!");
                    }
                    else
                    {
                        return HelperClass.Response(true, GlobalDecleration._successAction, problems);
                    }
                }
            }
            catch (Exception ex)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, ex);
            }
        }

        internal object DeleteProblem(int problemId)
        {
            try
            {
                using (_dbContext)
                {
                    Problem offer = _dbContext.Problems.Find(problemId);
                    if (offer != null)
                    {
                        _dbContext.Problems.Remove(offer);
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
            throw new NotImplementedException();
        }

        internal ResponseModel AddProblem(NewProblemModel problem)
        {
            try
            {
                using (_dbContext)
                {
                    Problem new_problem = new Problem
                    {
                        ProblemBy = EncodeDecode.GetUserId(problem.PostedById),
                        PostedDate = DateTime.Now,
                        ProblemDescription = problem.ProblemDescription,
                        ProblemImage = ParseImage(problem.ProblemImage),
                        Status = "Available",
                    };
                    _dbContext.Problems.Add(new_problem);
                    _dbContext.SaveChanges();
                    return HelperClass.Response(true, GlobalDecleration._uploadedSuccesfully, "Problem Id is= " + new_problem.ProblemId);
                }
            }
            catch (Exception e)
            {
                return HelperClass.Response(false, GlobalDecleration._internalServerError, "Faild to post " + e);
            }
            throw new NotImplementedException();
        }
        private string ParseImage(string base64String)
        {
            try
            {
                string folderLocation = "~/MyUploads/ProblemImages/";
                string imageName = ((RandomString(10) + DateTime.Now) + ".jpg").Replace(":", String.Empty);

                byte[] img = LoadImage(base64String);

                string filePath = HttpContext.Current.Server
                        .MapPath(folderLocation +
                        Path.GetFileName(imageName));

                File.WriteAllBytes(filePath, img);

                string finalLocation = "MyUploads/ProblemImages/";
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
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}