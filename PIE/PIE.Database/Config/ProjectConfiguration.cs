using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {

        public ProjectConfiguration()
        {
            Property(x => x.Name).HasMaxLength(256).IsRequired();
            Property(x => x.CreateBy).HasMaxLength(30).IsRequired();
            Property(x => x.LastModifiedBy).HasMaxLength(30);

            HasMany(p => p.Teams)
                .WithMany(t => t.Projects)
                .Map(m =>
                {
                    m.ToTable("Project_Team");
                    m.MapLeftKey("ProjectID");
                    m.MapRightKey("TeamID");
                });
        }
    }
}
