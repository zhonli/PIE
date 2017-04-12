using PIEM.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace PIEM.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            System.Data.Entity.Database.SetInitializer(new PIEMInitializer());
            DbInterception.Add(new PIEMInterceptorTransientErrors());
            DbInterception.Add(new PIEMInterceptorLogging());
        }

        
    }
}
