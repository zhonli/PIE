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
    public class BillingCycleController : ODataController
    {
        PIEMContext db = new PIEMContext();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/BillingCycles
        [EnableQuery]
        public IQueryable<BillingCycle> Get()
        {
            return null;
        }

        
    }
}
