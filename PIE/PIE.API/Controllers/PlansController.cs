using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.OData;
using PIEM.Common.Model;
using PIEM.Database;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.OData.Routing;
using PIEM.ExternalService;

namespace PIEM.API.Controllers
{
    public class PlansController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool PlanExists(int key)
        {
            return db.Plans.Any(p => p.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/Plans
        [EnableQuery(MaxExpansionDepth = 3)]
        public IQueryable<Plan> Get()
        {
            return db.Plans;
        }

        // GET: odata/Plans(5)
        [EnableQuery]
        public SingleResult<Plan> Get([FromODataUri] int key)
        {
            IQueryable<Plan> result = db.Plans.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        // POST: odata/Plans
        public async Task<IHttpActionResult> Post(Plan plan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            plan.CreateTime = DateTime.Now;

            db.Plans.Add(plan);
            await db.SaveChangesAsync();
            return Created(plan);
        }

        // PUT: odata/Plans(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Plan update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.ID)
            {
                return BadRequest();
            }
            update.LastModeifiedTime = DateTime.Now;
            db.Entry(update).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }

        // PATCH: odata/Plans(5)
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Plan> deltaPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Plans.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            entity.LastModeifiedTime = DateTime.Now;
            deltaPlan.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }
        // DELETE: odata/Plans(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var plan = await db.Plans.FindAsync(key);
            if (plan == null)
            {
                return NotFound();
            }
            db.Plans.Remove(plan);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET /Plans(5)/Project
        [EnableQuery]
        public SingleResult<Project> GetProject([FromODataUri] int key)
        {
            var result = db.Plans.Where(p => p.ID == key).Select(p => p.Project);
            return SingleResult.Create(result);
        }
      
        // GET /Plans(5)/TaskLink
        [EnableQuery]
        public SingleResult<TaskLink> GetTaskLink([FromODataUri] int key)
        {
            var result = db.Plans.Where(p => p.ID == key).Select(p => p.TaskLink);
            return SingleResult.Create(result);
        }


        // PUT /Plans(1)/Project/$ref
        [AcceptVerbs("POST", "PUT")]
        public async Task<IHttpActionResult> CreateRef([FromODataUri] int key, string navigationProperty, [FromBody] Uri link)
        {
            var plan = await db.Plans.SingleOrDefaultAsync(p => p.ID == key);
            if (plan == null)
            {
                return NotFound();
            }
            var relatedKey = Helpers.GetKeyFromUri<int>(Request, link);
            switch (navigationProperty)
            {
                case "Project":
                    var project = await db.Projects.SingleOrDefaultAsync(f => f.ID == relatedKey);
                    if (project == null)
                    {
                        return NotFound();
                    }

                    plan.Project = project;
                    break;   

                case "TaskLink":
                    var taskLink = await db.TaskLinks.SingleOrDefaultAsync(f => f.ID == key);
                    if (taskLink == null)
                    {
                        return NotFound();
                    }

                    plan.TaskLink = taskLink;
                    break;

                case "TestCollateral":
                    var testCollateral = await db.TestCollaterals.SingleOrDefaultAsync(f => f.ID == relatedKey);
                    if (testCollateral == null)
                    {
                        return NotFound();
                    }

                    plan.TestCollateral = testCollateral;
                    break;
                default:
                    return StatusCode(HttpStatusCode.NotImplemented);
            }
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE /Plans(1)/Project/$ref
        public async Task<IHttpActionResult> DeleteRef([FromODataUri] int key, string navigationProperty, [FromBody] Uri link)
        {
            var plan = db.Plans.SingleOrDefault(p => p.ID == key);
            if (plan == null)
            {
                return NotFound();
            }

            switch (navigationProperty)
            {
                case "Project":
                    plan.Project = null;
                    break;
                case "TaskLink":
                    plan.TaskLink = null;
                    break;
                case "Assignments":
                    plan.Assignments = new List<Assignment>();
                    break;
                default:
                    return StatusCode(HttpStatusCode.NotImplemented);
            }
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // Other controller methods not shown.
        [HttpPost]
        public async Task<IHttpActionResult> Execute([FromODataUri]int key, ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            int sourceId = (int)parameters["sourceId"];

            string resultSummaries = (string)parameters["resultSummaries"];
            if (string.IsNullOrEmpty(resultSummaries))
            {
                return BadRequest("resultSummaries not defined");
            }

            JArray rsArray = (JArray)JsonConvert.DeserializeObject(resultSummaries);
            if (rsArray == null || rsArray.Count == 0)
            {
                return BadRequest("resultSummaries is empty");
            }

            var entity = await db.Plans.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Status = PlanStatus.Executing;
            entity.ExecutedOn = DateTime.Now;

            Process process = new Process();
            process.ID = entity.ID;
            process.Name = entity.Title;
            process.StartOn = entity.ExecutedOn.Value;
            process.Status = ProcessStatus.Running;
            process.Owner = entity.CreateBy; //TODO: used context user
            process.ActualWorkhours = entity.Workhours;
            process.Progress = 0f;
            process.PassRate = 0f;
            db.Processes.Add(process);

            foreach (dynamic rs in rsArray)
            {
                ResultSummary resultSummary = new ResultSummary();
                resultSummary.Name = rs.Name;
                resultSummary.ProcessID = process.ID;
                resultSummary.SourceID = sourceId;
                resultSummary.RSID = rs.ID;


                db.ResultSummaries.Add(resultSummary);
            }

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (!PlanExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public async Task<IHttpActionResult> ExecuteVoid([FromODataUri]int key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           
            var entity = await db.Plans.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            entity.Status = PlanStatus.Executing;
            entity.ExecutedOn = DateTime.Now;

            Process process = new Process();
            process.ID = entity.ID;
            process.Name = entity.Title;
            process.StartOn = entity.ExecutedOn.Value;
            process.Status = ProcessStatus.Running;
            process.Owner = entity.CreateBy; //TODO: used context user
            process.ActualWorkhours = entity.Workhours;
            process.Progress = 0f;
            process.PassRate = 0f;
            db.Processes.Add(process);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (!PlanExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
