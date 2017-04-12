using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database
{
    public class TaskHistoryWork : BaseCreationTime, ICloneable
    {
        [Key, Column(Order = 0)]
        public int WorkItemSourceID { get; set; }

        [Key, Column(Order = 1)]
        public int TaskID { get; set; }

        [Key, Column(Order = 2)]
        public int RCID { get; set; }

        [Key, Column(Order = 3)]
        public DateTime Date { get; set; }

        [Key, Column(Order = 4)]
        [MaxLength(20)]
        public string Owner { get; set; }

        [Key, Column(Order = 5)]
        public int ResultStatus { get; set; }

        [Key, Column(Order = 6)]
        public int OutCome { get; set; }

        [Key, Column(Order = 7)]
        [MaxLength(20)]
        public string Language { get; set; }

        public int ResultCount { get; set; }

        public int UpdateNumber { get; set; }

        public DateTime? UpdateTime { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
