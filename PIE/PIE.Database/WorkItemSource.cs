using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database
{
    public class WorkItemSource
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int Type { get; set; }

        [MaxLength(100)]
        public string ServerUri { get; set; }

        [MaxLength(100)]
        public string TeamFoundationServer { get; set; }

        [MaxLength(50)]
        public string TeamProject { get; set; }

        [MaxLength(50)]
        public string SQLSERVER { get; set; }

        [MaxLength(50)]
        public string IDENTITYDATABASE { get; set; }

        [MaxLength(50)]
        public string ENTERPRISENAME { get; set; }

        [MaxLength(50)]
        public string DATASTORE { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
