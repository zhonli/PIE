using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class TeamConfiguration : EntityTypeConfiguration<Team>
    {

        public TeamConfiguration()
        {
            Property(x => x.TeamName).HasMaxLength(50).IsRequired();
        }
    }
}
