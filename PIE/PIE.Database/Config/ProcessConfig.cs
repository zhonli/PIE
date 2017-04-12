using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Config
{
    public class ProcessConfig : EntityTypeConfiguration<Process>
    {
        public ProcessConfig()
        {
            Property(x => x.ID).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            HasRequired(c => c.Plan).WithOptional(p => p.Process).WillCascadeOnDelete();
        }
    }
}
