using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database
{
    public class TaskBugMapping
    {
        [Key, Column(Order = 0)]
        public int WorkItemSourceID { get; set; }

        [Key, Column(Order = 1)]
        public int TaskID { get; set; }

        [Key, Column(Order = 2)]
        public int BugID { get; set; }

        [MaxLength(20)]
        public string BugStatus { get; set; }

        public virtual WorkItemSource WorkItemSource { get; set; }
    }
}
