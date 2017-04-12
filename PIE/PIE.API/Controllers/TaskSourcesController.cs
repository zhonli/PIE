using System.Data;
using System.Linq;
using System.Web.OData;
using PIEM.Database;
using PIEM.Common.Model;

namespace PIEM.API.Controllers
{
    public class TaskSourcesController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool WorkItemSourceExists(int key)
        {
            return db.WorkItemSources.Any(p => p.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/Sources
        [EnableQuery]
        public IQueryable<TaskSource> Get()
        {
            var sources = db.WorkItemSources.Where(s => string.IsNullOrEmpty(s.TeamFoundationServer) == false).Select(s => new TaskSource()
            {
                ID = s.ID,
                Project = s.TeamProject,
                TFS = s.TeamFoundationServer,
            }).ToList();

            foreach (var item in sources)
            {
                if (string.IsNullOrEmpty(item.TFS))
                    continue;
                string tfs = item.TFS.TrimStart("https://".ToCharArray());
                tfs = tfs.TrimStart("http://".ToCharArray());
                item.FriendlyName = string.Format("{0}@{1}", item.Project, tfs);
            }

            return sources.AsQueryable();
        }

       
    }
}

