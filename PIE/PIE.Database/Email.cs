using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database
{
    public class Email : BaseCreationTime
    {
        [Key]
        public int ID { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string To { get; set; }

        [MaxLength(250)]
        public string CC { get; set; }

        public int Type { get; set; }

        [MaxLength(20000)]
        public string Content { get; set; }
    }
}
