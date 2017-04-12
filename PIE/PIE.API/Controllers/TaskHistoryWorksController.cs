//----------------------------------------------------------------------------------------------------------------------
// <copyright file="RequestSetController.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary></summary>
//----------------------------------------------------------------------------------------------------------------------
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
    public class TaskHistoryWorksController : ODataController
    {
        PIEMContext db = new PIEMContext();

        [EnableQuery]
        public IQueryable<TaskHistoryWork> Get()
        {
            return db.TaskHistoryWorks;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}