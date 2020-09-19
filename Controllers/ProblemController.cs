using Roof_Care.ExmxDbSets;
using Roof_Care.Models;
using Roof_Care.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Roof_Care.Controllers
{
    public class ProblemController : ApiController
    {
        private readonly RoofCareEntities _dbContext;
        private readonly ProblemServices problemServices;
        public ProblemController()
        {
            _dbContext = new RoofCareEntities();
            problemServices = new ProblemServices(_dbContext);
        }
        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(problemServices.GetAllProblems());
        }
        [HttpPost]
        public IHttpActionResult Post(NewProblemModel problem)
        {
            if (ModelState.IsValid)
            {
                return Ok(problemServices.AddProblem(problem));
            }
            else
            {
                return Ok(Declarations.GlobalDecleration._invalidModel);
            }
        }

        [HttpDelete]
        [Route("api/Problem/DeleteProblem")]
        public IHttpActionResult DeleteProblem(int problemId)
        {
            return Ok(problemServices.DeleteProblem(problemId));
        }
    }
}
