using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined the Wtt result summary object
    /// </summary>
    public class WttResultSummary
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public Guid Guid { get; set; }

        public decimal ActualRuntime { get; set; }

        public int CancelledResults { get; set; }

        public int CompletedResults { get; set; }

        public DateTime CreateTime { get; set; }

        public decimal EstimatedRuntime { get; set; }

        public int InProgressResults { get; set; }

        public int InvestigateResults { get; set; }

        public int ResolvedResults { get; set; }

        public int ResultCollectionSetNameId { get; set; }

        public decimal RuntimeLeft { get; set; }

        public int SignedOffByCount { get; set; }

        public int TotalResults { get; set; }

        public int ResultSummaryStatusId { get; set; }
    }
}
