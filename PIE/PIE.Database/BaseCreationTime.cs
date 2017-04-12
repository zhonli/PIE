using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database
{
    public class BaseCreationTime
    {
        public BaseCreationTime()
        {
            CreationTime = DateTime.Now;
        }

        public DateTime CreationTime { get; set; }
    }
}
