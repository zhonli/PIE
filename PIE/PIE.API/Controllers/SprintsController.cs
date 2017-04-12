﻿using System.Data;
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
using System.Collections.Generic;


namespace PIEM.API.Controllers
{
    public class SprintsController: ODataController
    {
        PIEMContext db = new PIEMContext();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/Sprints
        [EnableQuery]
        public IQueryable<Sprint> Get()
        {
            return db.Sprints;
        }
    }
}