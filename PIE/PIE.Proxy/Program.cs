using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessProxy processProxy = new ProcessProxy();
            try
            {
                processProxy.Scan();
                processProxy.UpdateProgress();
            }
            finally
            {
                processProxy.Dispose();
            }
        }
    }
}
