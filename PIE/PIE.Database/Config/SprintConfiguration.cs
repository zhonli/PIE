using PIEM.Common.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class SprintConfiguration : EntityTypeConfiguration<Sprint>
    {

        public SprintConfiguration()
        {
            Property(x => x.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Path).HasMaxLength(50).IsRequired();
            Property(x => x.StartDate).IsOptional();
            Property(x => x.EndDate).IsOptional();
            Property(x => x.ProjectID).IsOptional();
            HasOptional(x => x.Project).WithMany(p => p.Sprints).WillCascadeOnDelete();
        }
    }
}
