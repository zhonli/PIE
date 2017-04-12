using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class AssignmentConfiguration : EntityTypeConfiguration<Assignment>
    {

        public AssignmentConfiguration()
        {
            HasRequired(a => a.Plan).WithMany(p => p.Assignments);
            HasRequired(a => a.Resource).WithMany(r => r.Assignments);
        }
    }
}
