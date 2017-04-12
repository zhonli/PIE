using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {

        public RoleConfiguration()
        {
            Property(x => x.RoleName).HasMaxLength(50).IsRequired();
        }
    }
}
