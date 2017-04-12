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
    public class LanguagesController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool LanguageExists(int key)
        {
            return db.Languages.Any(p => p.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/Languages
        [EnableQuery]
        public IQueryable<Language> Get()
        {
            return db.Languages;
        }

        // GET: odata/Languages(5)
        public SingleResult<Language> Get([FromODataUri] int key)
        {
            IQueryable<Language> result = db.Languages.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        // POST: odata/Languages
        public async Task<IHttpActionResult> Post(Language language)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Languages.Add(language);
            await db.SaveChangesAsync();
            return Created(language);
        }

        // PUT: odata/Languages(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Language update)
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
                if (!LanguageExists(key))
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

        // PATCH: odata/Languages(5)
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Language> language)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Languages.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            language.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LanguageExists(key))
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
        // DELETE: odata/Languages(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var language = await db.Languages.FindAsync(key);
            if (language == null)
            {
                return NotFound();
            }
            db.Languages.Remove(language);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
