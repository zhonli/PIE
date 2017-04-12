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

namespace PIEM.API.Controllers
{
    public class ResourcesController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool ResourceExists(int key)
        {
            return db.Resources.Any(p => p.ResourceID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/Resources
        [EnableQuery]
        public IQueryable<Resource> Get()
        {
            return db.Resources;
        }

        // GET: odata/Resources(5)
        public SingleResult<Resource> Get([FromODataUri] int key)
        {
            IQueryable<Resource> result = db.Resources.Where(p => p.ResourceID == key);
            return SingleResult.Create(result);
        }

        // POST: odata/Resources
        public async Task<IHttpActionResult> Post(Resource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Resources.Add(resource);
            await db.SaveChangesAsync();
            return Created(resource);
        }

        // PUT: odata/Resources(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Resource update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.ResourceID)
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
                if (!ResourceExists(key))
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

        // PATCH: odata/Resources(5)
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Resource> resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Resources.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            resource.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourceExists(key))
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
        // DELETE: odata/Resources(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var plan = await db.Resources.FindAsync(key);
            if (plan == null)
            {
                return NotFound();
            }
            db.Resources.Remove(plan);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
