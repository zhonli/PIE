using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class TaskLinkConfiguration : EntityTypeConfiguration<TaskLink>
    {

        public TaskLinkConfiguration()
        {
            Ignore(tl => tl.Source);
        }
    }
}
