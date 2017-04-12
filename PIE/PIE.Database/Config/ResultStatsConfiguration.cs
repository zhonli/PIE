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
    public class ResultStatsConfiguration : EntityTypeConfiguration<ResultStats>
    {

        public ResultStatsConfiguration()
        {
            HasRequired(s => s.ResultSummary).WithMany(s => s.ResultStats).HasForeignKey(s=>s.ResultSummaryID);
        }
    }
}
