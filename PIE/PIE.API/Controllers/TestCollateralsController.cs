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
    public class TestCollateralsController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool TestCollateralExists(int key)
        {
            return db.TestCollaterals.Any(p => p.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/TestCollaterals
        [EnableQuery]
        public IQueryable<TestCollateral> Get()
        {
            return db.TestCollaterals;
        }

        // GET: odata/TestCollaterals(5)
        public SingleResult<TestCollateral> Get([FromODataUri] int key)
        {
            IQueryable<TestCollateral> result = db.TestCollaterals.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        // POST: odata/TestCollaterals
        public async Task<IHttpActionResult> Post(TestCollateral testcollateral)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TestCollaterals.Add(testcollateral);
            await db.SaveChangesAsync();
            return Created(testcollateral);
        }

        // PUT: odata/TestCollaterals(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, TestCollateral update)
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
                if (!TestCollateralExists(key))
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

        // PATCH: odata/TestCollaterals(5)
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<TestCollateral> testCollateral)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.TestCollaterals.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            testCollateral.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestCollateralExists(key))
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
        // DELETE: odata/TestCollaterals(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var plan = await db.TestCollaterals.FindAsync(key);
            if (plan == null)
            {
                return NotFound();
            }
            db.TestCollaterals.Remove(plan);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
