namespace PIEM.Database.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Data.Entity.Validation;
    using Common.Logging;

    internal sealed class Configuration : DbMigrationsConfiguration<PIEMContext>
    {
        //private ILogger _logger = new Logger();
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            //AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(PIEMContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate(l=>l.Code,) helper extension method 
            //  to avoid creating duplicate seed data. E.g.


            //try
            //{
            //    context.Languages.AddOrUpdate(l => l.Code, LanguageData.All().ToArray());
            //    context.Projects.AddOrUpdate(ProjectData.Test().ToArray());
            //    context.Sprints.AddOrUpdate(SprintData.Test().ToArray());
            //    context.Features.AddOrUpdate(FeatureData.Test().ToArray());
            //    context.Teams.AddOrUpdate(TeamData.Test().ToArray());
            //    context.Resources.AddOrUpdate(ResourceData.Test().ToArray());
            //    context.Plans.AddOrUpdate(PlanData.Test().ToArray());
            //    context.SaveChanges();
            //    context.TestCollaterals.AddOrUpdate(TestCollateralData.Test().ToArray());
            //    context.TaskLinks.AddOrUpdate(TaskLinkData.Test().ToArray());
            //    context.Assignments.AddOrUpdate(AssignmentData.Test().ToArray());
            //    context.SaveChanges();
            //}
            //catch (DbEntityValidationException ex)
            //{
            //    _logger.Error(ex, "EntityValidationErrors : {0}", ex.EntityValidationErrors);
            //}
            base.Seed(context);

        }


    }
}
