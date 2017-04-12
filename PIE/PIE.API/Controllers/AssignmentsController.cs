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
    public class AssignmentsController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool AssignmentExists(int key)
        {
            return db.Assignments.Any(p => p.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/Assignments
        [EnableQuery]
        public IQueryable<Assignment> Get()
        {
            return db.Assignments;
        }

        // GET: odata/Assignments(5)
        public SingleResult<Assignment> Get([FromODataUri] int key)
        {
            IQueryable<Assignment> result = db.Assignments.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        // POST: odata/Assignments
        public async Task<IHttpActionResult> Post(Assignment assignment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            assignment.CreateTime = DateTime.Now;

            db.Assignments.Add(assignment);
            await db.SaveChangesAsync();
            return Created(assignment);
        }

        // PUT: odata/Assignments(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Assignment update)
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
                if (!AssignmentExists(key))
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

        // PATCH: odata/Assignments(5)
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Assignment> assignmentDelta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Assignments.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            assignmentDelta.Patch(entity);

            entity.LastModeifiedTime = DateTime.Now;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(key))
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
        // DELETE: odata/Assignments(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var assignment = await db.Assignments.FindAsync(key);
            if (assignment == null)
            {
                return NotFound();
            }
            db.Assignments.Remove(assignment);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
