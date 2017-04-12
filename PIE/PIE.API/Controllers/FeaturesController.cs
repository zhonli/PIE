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
    public class FeaturesController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool FeatureExists(int key)
        {
            return db.Features.Any(p => p.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/Features
        [EnableQuery]
        public IQueryable<Feature> Get()
        {
            return db.Features;
        }

        // GET: odata/Features(5)
        public SingleResult<Feature> Get([FromODataUri] int key)
        {
            IQueryable<Feature> result = db.Features.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        // POST: odata/Features
        public async Task<IHttpActionResult> Post(Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Features.Add(feature);
            await db.SaveChangesAsync();
            return Created(feature);
        }

        // PUT: odata/Features(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Feature update)
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
                if (!FeatureExists(key))
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

        // PATCH: odata/Features(5)
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Feature> feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Features.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            feature.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureExists(key))
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
        // DELETE: odata/Features(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var plan = await db.Features.FindAsync(key);
            if (plan == null)
            {
                return NotFound();
            }
            db.Features.Remove(plan);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
