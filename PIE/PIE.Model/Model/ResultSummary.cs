using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.OData.Builder;
using System.Linq;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined the result collection summary info
    /// </summary>
    public class ResultSummary
    {
        /// <summary>
        /// Unique ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Result summary name, equal with result collection name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Result summary ID
        /// </summary>
        public int RSID { get; set; }
        /// <summary>
        /// Source ID
        /// </summary>
        public int SourceID { get; set; }
        /// <summary>
        /// The Last statistics info
        /// </summary>
        public virtual ResultStats LastStats { get; set; }

        /// <summary>
        /// Related process ID
        /// DB
        /// </summary>
        public int ProcessID { get; set; }
        /// <summary>
        /// Related process info
        /// </summary>
        [ForeignKey("ProcessID")]
        private Process Process { get; set; }
        /// <summary>
        /// All the statistics info
        /// </summary>
        public virtual IList<ResultStats> ResultStats { get; set; }

    }

}