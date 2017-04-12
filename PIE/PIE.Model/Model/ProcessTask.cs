using System;

namespace PIEM.Common.Model
{
    /// <summary>
    /// [New Feature] Defined all activities in process
    /// </summary>
    public class ProcessTask
    {
        public int ID { get; set; }

        public int ProcessID { get; set; }

        public Process Process { get; set; }

        public string Name { get; set; }

        public ProcessTaskStatus Status { get; set; }

        public DateTime ExecutedOn { get; set; }

    }
}