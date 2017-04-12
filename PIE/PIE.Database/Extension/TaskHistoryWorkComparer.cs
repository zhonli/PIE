using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Extension
{
    public class TaskHistoryWorkComparer : IEqualityComparer<TaskHistoryWork>
    {
        public bool Equals(TaskHistoryWork x, TaskHistoryWork y)
        {
            bool b = 
                x.WorkItemSourceID == y.WorkItemSourceID 
                && x.TaskID == y.TaskID 
                && x.RCID == y.RCID
                && x.Date == y.Date
                && x.Owner == y.Owner
                && x.ResultStatus == y.ResultStatus
                && x.OutCome == y.OutCome
                && x.Language == y.Language
                ;
            return b;
        }

        public int GetHashCode(TaskHistoryWork obj)
        {
            int hash = obj.TaskID * 13 + obj.RCID;
            return Convert.ToInt32(hash);
        }

        private TaskHistoryWorkComparer()
        {

        }
        private static TaskHistoryWorkComparer _TaskHistoryWorkComparer;
        public static TaskHistoryWorkComparer Instance
        {
            get
            {
                if (_TaskHistoryWorkComparer == null)
                {
                    _TaskHistoryWorkComparer = new TaskHistoryWorkComparer();
                }
                return _TaskHistoryWorkComparer;
            }
        }
    }
}
