using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using PIEM.Database;
using System.Web.OData;
using System.Security.Principal;
using PIEM.Common.Model;

namespace WebApplication1.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using PIEM.Database;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<SanityCheck>("SanityChecks");
    builder.EntitySet<WorkItemSource>("WorkItemSources"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class SanityChecksController : ODataController
    {
        private PIEMContext db = new PIEMContext();

        // GET: odata/SanityChecks
        [EnableQuery]
        public IQueryable<SanityCheck> GetSanityChecks()
        {
            List<SanityCheck> SanityChecks = new List<SanityCheck>();

            string userName = null;
            string alias = null;
            Role? userRole = null;
            foreach (KeyValuePair<string, string> keyValue in Request.GetQueryNameValuePairs())
            {
                if (keyValue.Key.ToLower().Trim() == "username")
                {
                    userName = keyValue.Value;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(userName))
            {
                alias = userName.Substring(userName.LastIndexOf('\\') + 1).Trim();
                Resource resource = db.Resources.FirstOrDefault(item => item.Alias == alias);
                if(resource != null)
                {
                    userRole = resource.Role;
                }
            }

            if (string.IsNullOrEmpty(userName))
            {
                return SanityChecks.AsQueryable();
            }
            else if(userRole == Role.Manager)
            {
                return db.SanityChecks;
            }
            else
            {
                List<int> PlanIDs = db.Plans.Where(item => item.CreateBy == userName).Select(item => item.ID).ToList();
                List<int> TaskIDs = db.TaskLinks.Where(item => PlanIDs.Contains(item.ID)).Select(item => item.TaskID).ToList();
                SanityChecks.AddRange(db.SanityChecks.Where(item => TaskIDs.Contains(item.TestedTaskID)));

                return SanityChecks.AsQueryable();
            }

        }

        // GET: odata/SanityChecks(5)
        [EnableQuery]
        public SingleResult<SanityCheck> GetSanityCheck([FromODataUri] int key)
        {
            return SingleResult.Create(db.SanityChecks.Where(sanityCheck => sanityCheck.ID == key));
        }

        // PUT: odata/SanityChecks(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<SanityCheck> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SanityCheck sanityCheck = db.SanityChecks.Find(key);
            if (sanityCheck == null)
            {
                return NotFound();
            }

            patch.Put(sanityCheck);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanityCheckExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(sanityCheck);
        }

        // POST: odata/SanityChecks
        public IHttpActionResult Post(SanityCheck sanityCheck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SanityChecks.Add(sanityCheck);
            db.SaveChanges();

            return Created(sanityCheck);
        }

        // PATCH: odata/SanityChecks(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<SanityCheck> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SanityCheck sanityCheck = db.SanityChecks.Find(key);
            if (sanityCheck == null)
            {
                return NotFound();
            }

            patch.Patch(sanityCheck);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SanityCheckExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(sanityCheck);
        }

        // DELETE: odata/SanityChecks(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            SanityCheck sanityCheck = db.SanityChecks.Find(key);
            if (sanityCheck == null)
            {
                return NotFound();
            }

            db.SanityChecks.Remove(sanityCheck);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SanityCheckExists(int key)
        {
            return db.SanityChecks.Count(e => e.ID == key) > 0;
        }
    }
}
