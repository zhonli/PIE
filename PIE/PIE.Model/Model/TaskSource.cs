using System;

namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined the VSO source info
    /// </summary>
    public class TaskSource
    {
        /// <summary>
        /// VSO source ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Team foundation server
        /// </summary>
        public string TFS { get; set; }
        /// <summary>
        /// Project Name
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// Customized friendly name
        /// </summary>
        public string FriendlyName { get; set; }

    }
}
