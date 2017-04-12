using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class PlanConfiguration : EntityTypeConfiguration<Plan>
    {

        public PlanConfiguration()
        {
            Property(x => x.Title).HasMaxLength(256).IsRequired();
            Property(x => x.CreateBy).HasMaxLength(30).IsRequired();
            Property(x => x.LastModifiedBy).HasMaxLength(30);

            HasMany(x => x.Assignments).WithRequired(a => a.Plan);
            HasOptional(x => x.TaskLink).WithRequired(t => t.Plan).WillCascadeOnDelete();
        }
    }
}
