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

namespace PIEM.API.Controllers
{
    public class ProjectsController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool ProjectExists(int key)
        {
            return db.Projects.Any(p => p.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/Projects
        [EnableQuery]
        public IQueryable<Project> Get()
        {
            return db.Projects;
        }

        // GET: odata/Projects(5)
        public SingleResult<Project> Get([FromODataUri] int key)
        {
            IQueryable<Project> result = db.Projects.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        // POST: odata/Projects
        public async Task<IHttpActionResult> Post(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Projects.Add(project);
            await db.SaveChangesAsync();
            return Created(project);
        }

        // PUT: odata/Projects(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Project update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.ID)
            {
                return BadRequest();
            }
            db.Entry(update).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(key))
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

        // PATCH: odata/Projects(5)
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Project> project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Projects.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            project.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(key))
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
        // DELETE: odata/Projects(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var plan = await db.Projects.FindAsync(key);
            if (plan == null)
            {
                return NotFound();
            }
            db.Projects.Remove(plan);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }


        // DELETE http://host/Projects(1)/Plans/$ref?$id=http://host/Plans(1)
        public async Task<IHttpActionResult> DeleteRef([FromODataUri] int key, [FromODataUri] string relatedKey, string navigationProperty)
        {
            var project = await db.Projects.SingleOrDefaultAsync(p => p.ID == key);
            if (project == null)
            {
                return StatusCode(HttpStatusCode.NotFound);
            }

            switch (navigationProperty)
            {
                case "Plans":
                    var planId = Convert.ToInt32(relatedKey);
                    var plan = await db.Plans.SingleOrDefaultAsync(p => p.ID == planId);

                    if (plan == null)
                    {
                        return NotFound();
                    }
                    plan.Project = null;
                    break;
                default:
                    return StatusCode(HttpStatusCode.NotImplemented);

            }
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // Other controller methods not shown.
    }
}
