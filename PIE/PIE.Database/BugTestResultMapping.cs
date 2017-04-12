using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database
{
    public class BugTestResultMapping
    {
        [Key, Column(Order = 0)]
        public int WorkItemSourceIDOfBug { get; set; }

        [Key, Column(Order = 1)]
        public int BugID { get; set; }

        [Key, Column(Order = 2)]
        public int WorkItemSourceIDOfResult { get; set; }

        [Key, Column(Order =3)]
        public int RCID { get; set; }

        [Key, Column(Order = 4)]
        public int ResultID { get; set; }

        [Key, Column(Order = 5)]
        public int JobID { get; set; }
    }
}
