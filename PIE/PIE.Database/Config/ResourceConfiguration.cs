using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class ResourceConfiguration : EntityTypeConfiguration<Resource>
    {

        public ResourceConfiguration()
        {
            Property(x => x.Alias).HasMaxLength(30).IsRequired();
            Property(x => x.Email).HasMaxLength(256).IsRequired();
            
            Property(x => x.FirstName).HasMaxLength(10);
            Property(x => x.LastName).HasMaxLength(10);
            Property(x => x.FullName).HasMaxLength(30);
            Property(x => x.DisplayName).HasMaxLength(200);

            HasMany(r => r.Teams)
                .WithMany(t => t.Members)
                .Map(m =>
                {
                    m.ToTable("Team_Resource");
                    m.MapLeftKey("ResourceID");
                    m.MapRightKey("TeamID");
                });
        }
    }
}
