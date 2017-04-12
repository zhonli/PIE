using PIEM.Common.Model;

using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using PIEM.Database;

namespace PIEM.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors(PlatformCorsPolicy.Instance);
                        
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "PieService";
            builder.EntitySet<Project>("Projects");
            builder.EntitySet<Plan>("Plans");
            builder.EntitySet<TestCollateral>("TestCollaterals");
            builder.EntitySet<TaskLink>("TaskLinks");
            builder.EntitySet<Process>("Processes");
            builder.EntitySet<ResultStats>("ResultStats");
            
            //builder.EntitySet<Feature>("Features");
            //builder.EntitySet<Language>("Languages");
            builder.EntitySet<Team>("Teams");
            builder.EntitySet<Resource>("Resources");
            builder.EntitySet<TaskSource>("TaskSources");
            builder.EntitySet<Assignment>("Assignments");
            
            var planType = builder.EntityType<Plan>();
            planType.Action("ExecuteVoid");

            var actionConfig = planType.Action("Execute");
            actionConfig.Parameter<int>("sourceId");
            actionConfig.Parameter<string>("resultSummaries");

            builder.EntityType<Process>().Action("Block").Parameter<string>("transaction");
            builder.EntityType<Process>().Action("Run");
            builder.EntityType<Process>().Action("Abort").Parameter<string>("transaction");

            builder.Function("GetWttRCs")
                   .ReturnsCollection<WttResultSummary>()
                   .Parameter<int>("TaskID");

            builder.Function("GetWttRC")
                   .Returns<WttResultSummary>()
                   .Parameter<int>("RCID");

            builder.EntitySet<VSOTask>("VSOTasks");

            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());

        }
    }
}
