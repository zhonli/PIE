using PIEM.Common.Logging;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace PIEM.Database
{
    public class PIEMInitializer : CreateDatabaseIfNotExists<PIEMContext>
    {
        private ILogger _logger = new Logger();
        protected override void Seed(PIEMContext context)
        {
            //context.Languages.AddRange(LanguageData.All());
            //context.Projects.AddRange(ProjectData.Test());
            //context.Sprints.AddRange(SprintData.Test());
            //context.Features.AddRange(FeatureData.Test());
            //context.Plans.AddRange(PlanData.Test());
            //context.Assignments.AddRange(AssignmentData.Test());
            //context.Teams.AddRange(TeamData.Test());
            //context.Resources.AddRange(ResourceData.Test());

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                _logger.Error(ex, "EntityValidationErrors : {0}", ex.EntityValidationErrors);
            }
            base.Seed(context);
        }
    }
}