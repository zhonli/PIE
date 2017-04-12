using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined the process transaction info
    /// </summary>
    public class Transaction
    {
        /// <summary>
        /// Transaction ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Transaction Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Transaction executed time
        /// </summary>
        public DateTime ExecutedOn { get; set; }
        /// <summary>
        /// Transaction state package
        /// </summary>
        public string State { get; set; }


        /// <summary>
        /// Related Process ID
        /// DB realted
        /// </summary>
        public int ProcessID { get; set; }
        /// <summary>
        /// Related Process info
        /// </summary>
        public Process Process { get; set; }
    }
}
