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
    public class TeamsController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool TeamExists(int key)
        {
            return db.Teams.Any(p => p.TeamID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/Teams
        [EnableQuery]
        public IQueryable<Team> Get()
        {
            return db.Teams;
        }

        // GET: odata/Teams(5)
        public SingleResult<Team> Get([FromODataUri] int key)
        {
            IQueryable<Team> result = db.Teams.Where(p => p.TeamID == key);
            return SingleResult.Create(result);
        }

        // POST: odata/Teams
        public async Task<IHttpActionResult> Post(Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Teams.Add(team);
            await db.SaveChangesAsync();
            return Created(team);
        }

        // PUT: odata/Teams(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Team update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.TeamID)
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
                if (!TeamExists(key))
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

        // PATCH: odata/Teams(5)
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Team> team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Teams.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            team.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(key))
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
        // DELETE: odata/Teams(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var plan = await db.Teams.FindAsync(key);
            if (plan == null)
            {
                return NotFound();
            }
            db.Teams.Remove(plan);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
