using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class TaskSourceConfiguration : EntityTypeConfiguration<TaskSource>
    {

        public TaskSourceConfiguration()
        {
            Property(x => x.ServerUri).IsRequired();
            Property(x => x.ProjectCollection).HasMaxLength(50).IsRequired();
            Property(x => x.Project).HasMaxLength(50).IsRequired();
        }
    }
}
