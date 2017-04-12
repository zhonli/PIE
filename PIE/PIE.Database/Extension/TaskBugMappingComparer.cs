using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Extension
{
    public class TaskBugMappingComparer : IEqualityComparer<TaskBugMapping>
    {
        public bool Equals(TaskBugMapping x, TaskBugMapping y)
        {
            bool b =
                x.WorkItemSourceID == y.WorkItemSourceID
                && x.TaskID == y.TaskID
                && x.BugID == y.BugID
                ;
            return b;
        }

        public int GetHashCode(TaskBugMapping obj)
        {
            int hash = obj.TaskID * 13 + obj.WorkItemSourceID;
            return Convert.ToInt32(hash);
        }

        private TaskBugMappingComparer()
        {

        }
        private static TaskBugMappingComparer _TaskBugMappingComparer;
        public static TaskBugMappingComparer Instance
        {
            get
            {
                if (_TaskBugMappingComparer == null)
                {
                    _TaskBugMappingComparer = new TaskBugMappingComparer();
                }
                return _TaskBugMappingComparer;
            }
        }
    }
}
