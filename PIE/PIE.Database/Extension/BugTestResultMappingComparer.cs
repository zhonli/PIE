using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Extension
{
    public class BugTestResultMappingComparer : IEqualityComparer<BugTestResultMapping>
    {
        public bool Equals(BugTestResultMapping x, BugTestResultMapping y)
        {
            bool b =
                x.WorkItemSourceIDOfBug == y.WorkItemSourceIDOfBug
                && x.BugID == y.BugID
                && x.WorkItemSourceIDOfResult == y.WorkItemSourceIDOfResult
                && x.RCID == y.RCID
                && x.ResultID == y.ResultID
                && x.JobID == y.JobID
                ;
            return b;
        }

        public int GetHashCode(BugTestResultMapping obj)
        {
            int hash = obj.BugID * 13 + obj.WorkItemSourceIDOfBug;
            return Convert.ToInt32(hash);
        }

        private BugTestResultMappingComparer()
        {

        }
        private static BugTestResultMappingComparer _BugTestResultMappingComparer;
        public static BugTestResultMappingComparer Instance
        {
            get
            {
                if (_BugTestResultMappingComparer == null)
                {
                    _BugTestResultMappingComparer = new BugTestResultMappingComparer();
                }
                return _BugTestResultMappingComparer;
            }
        }
    }
}
