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
    public class TransactionConfig : EntityTypeConfiguration<Transaction>
    {
        public TransactionConfig()
        {
            HasRequired(t => t.Process).WithMany(p => p.Transactions).WillCascadeOnDelete();
        }
    }
}
