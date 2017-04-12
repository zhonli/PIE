using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class TestCollateralConfiguration : EntityTypeConfiguration<TestCollateral>
    {

        public TestCollateralConfiguration()
        {
            Property(c => c.Workhours).IsOptional();
            Property(c => c.LanguageCount).IsOptional();
            Property(c => c.TestCaseCount).IsOptional();
            HasRequired(c => c.Plan).WithOptional(p => p.TestCollateral).WillCascadeOnDelete();
        }

    }
}
