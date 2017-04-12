using PIEM.Common.Model;
using System.Data.Entity.ModelConfiguration;

namespace PIEM.Database.Config
{
    public class LanguageConfiguration : EntityTypeConfiguration<Language>
    {

        public LanguageConfiguration()
        {
            Property(x => x.Code).HasMaxLength(20).IsRequired();
            Property(x => x.Name).HasMaxLength(50).IsRequired();
        }
    }
}
