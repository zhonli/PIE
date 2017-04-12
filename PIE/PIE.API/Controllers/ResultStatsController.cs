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
using System.Web.OData.Routing;
using Newtonsoft.Json.Linq;
using PIEM.ExternalService;
using System.Collections.Generic;

namespace PIEM.API.Controllers
{
    public class ResultStatsController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool ResultStatsExists(int key)
        {
            return db.ResultStats.Any(rs => rs.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/ResultStats
        [EnableQuery]
        public IQueryable<ResultStats> Get()
        {
            return db.ResultStats;
        }

        // GET: odata/ResultStats(5)
        [EnableQuery]
        public SingleResult<ResultStats> Get([FromODataUri] int key)
        {
            IQueryable<ResultStats> resultStats = db.ResultStats.Where(rs => rs.ID == key);
            return SingleResult.Create(resultStats);
        }
    }
}
