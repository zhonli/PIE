using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.OData.Builder;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined the result collection detail info per statistics
    /// </summary>
    public class ResultStats
    {
        /// <summary>
        /// Statistics ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Result Summary ID
        /// </summary>
        public int ResultSummaryID { get; set; }
        /// <summary>
        /// Result Summary
        /// </summary>
        public virtual ResultSummary ResultSummary { get; set; }
        /// <summary>
        /// Actual running time
        /// </summary>
        public decimal ActualRuntime { get; set; }
        /// <summary>
        /// The count of cancelled results
        /// </summary>
        public int CancelledResults { get; set; }
        /// <summary>
        /// The count of completed results
        /// </summary>
        public int CompletedResults { get; set; }
        /// <summary>
        /// Created time of result collection 
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Estimated running time
        /// </summary>
        public decimal EstimatedRuntime { get; set; }

        public int InProgressResults { get; set; }

        public int InvestigateResults { get; set; }

        public int ResolvedResults { get; set; }

        public int ResultCollectionSetNameId { get; set; }

        public decimal RuntimeLeft { get; set; }

        public int SignedOffByCount { get; set; }
        /// <summary>
        /// The total count of results
        /// </summary>
        public int TotalResults { get; set; }

        public int ResultSummaryStatusId { get; set; }
        /// <summary>
        /// The time of statistics
        /// </summary>
        public DateTime StatsOn { get; set; }
    }
}