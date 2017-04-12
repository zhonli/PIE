namespace PIEM.Common.Model
{
    /// <summary>
    /// Defined the basic link info with VSO task
    /// </summary>
    public class TaskLink
    {
        /// <summary>
        /// Link ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// link related plan info
        /// </summary>
        public virtual Plan Plan { get; set; }
        /// <summary>
        /// VSO Task ID
        /// </summary>
        public int TaskID { get; set; }
        /// <summary>
        /// VSO source ID
        /// </summary>
        public int SourceID { get; set; }
        /// <summary>
        /// VSO source info
        /// </summary>
        public virtual TaskSource Source { get; set; }
    }
}
