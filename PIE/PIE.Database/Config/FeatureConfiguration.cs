using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class FeatureConfiguration : EntityTypeConfiguration<Feature>
    {

        public FeatureConfiguration()
        {
            Property(x => x.Name).HasMaxLength(256).IsRequired();

            HasOptional(x => x.ParentFeature)
            .WithMany(t => t.SubFeatures)
            .HasForeignKey(x => x.ParentFeatureID);
        }
    }
}
