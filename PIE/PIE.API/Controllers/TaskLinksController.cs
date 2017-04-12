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
using PIEM.ExternalService;

namespace PIEM.API.Controllers
{
    public class TaskLinksController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool TaskLinkExists(int key)
        {
            return db.TaskLinks.Any(p => p.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/TaskLinks
        [EnableQuery]
        public IQueryable<TaskLink> Get()
        {
            return db.TaskLinks;
        }

        // GET: odata/TaskLinks(5)
        [EnableQuery]
        public SingleResult<TaskLink> Get([FromODataUri] int key)
        {
            IQueryable<TaskLink> result = db.TaskLinks.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        // POST: odata/TaskLinks
        public async Task<IHttpActionResult> Post(TaskLink taskLink)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaskLinks.Add(taskLink);
            await db.SaveChangesAsync();
            return Created(taskLink);
        }

        // PUT: odata/TaskLinks(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, TaskLink update)
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
                if (!TaskLinkExists(key))
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

        // PATCH: odata/TaskLinks(5)
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<TaskLink> taskLinkDelta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.TaskLinks.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            taskLinkDelta.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskLinkExists(key))
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
        // DELETE: odata/TaskLinks(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var plan = await db.TaskLinks.FindAsync(key);
            if (plan == null)
            {
                return NotFound();
            }
            db.TaskLinks.Remove(plan);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
