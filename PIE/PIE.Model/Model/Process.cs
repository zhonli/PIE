using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Builder;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined all executing info
    /// </summary>
    public class Process
    {
        /// <summary>
        /// Process ID, equal with Plan ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Planning info
        /// </summary>
        public Plan Plan { get; set; }
        /// <summary>
        /// Process Name, equal with plan title
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// related result summary info
        /// </summary>
        [Contained]
        public IList<ResultSummary> ResultSummaries { get; set; }
        /// <summary>
        /// Progress of the process
        /// </summary>
        public float Progress { get; set; }
        /// <summary>
        /// Completed workhours
        /// </summary>
        public float CompletedWorkhours { get; set; }
        /// <summary>
        /// Actual workhours
        /// </summary>
        public float ActualWorkhours { get; set; }
        /// <summary>
        /// Pass rate
        /// </summary>
        public float? PassRate { get; set; }
        /// <summary>
        /// Process status
        /// </summary>
        public ProcessStatus Status { get; set; }
        /// <summary>
        /// Actual start date
        /// </summary>
        public DateTime StartOn { get; set; }
        /// <summary>
        /// Actual end date
        /// </summary>
        public DateTime? EndOn { get; set; }
        /// <summary>
        /// task owner, equal with plan creater
        /// </summary>
        public string Owner { get; set; }
        /// <summary>
        /// [new feature]
        /// </summary>
        public IList<ProcessTask> Tasks { get; set; }
        /// <summary>
        /// The last transaction
        /// </summary>
        public Transaction Transaction { get; set; }
        /// <summary>
        /// The last transaction ID
        /// </summary>
        public int? TransactionID { get; set; }
        /// <summary>
        /// All realted transactions
        /// </summary>
        [Contained]
        public IList<Transaction> Transactions { get; set; }


    }
}
