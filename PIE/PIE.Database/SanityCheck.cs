using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database
{
    public class SanityCheck : BaseCreationTime
    {
        [Key]
        public int ID { get; set; }

        public int WorkItemSourceID { get; set; }

        public int TestedTaskID { get; set; }

        public int Type { get; set; }

        public double? Progress { get; set; }

        public int? TestTask { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string AssignedTo { get; set; }

        public int State { get; set; }

        public virtual WorkItemSource WorkItemSource { get; set; }

    }
}
