using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Data
{
    class TaskLinkData
    {
        public static TaskLinkData Current { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly TaskLinkData _instance = new TaskLinkData();
        }

        public static IList<TaskLink> Test()
        {
            IList<TaskLink> taskLinks = new List<TaskLink>();
            TaskLink tl1 = new TaskLink() { ID = 1, SourceID = 1, TaskID = 1011 };
            taskLinks.Add(tl1);
            TaskLink tl2 = new TaskLink() { ID = 2, SourceID = 1, TaskID = 1012 };
            taskLinks.Add(tl2);
            TaskLink tl3 = new TaskLink() { ID = 3, SourceID = 1, TaskID = 1013 };
            taskLinks.Add(tl3);
            TaskLink tl4 = new TaskLink() { ID = 4, SourceID = 1, TaskID = 1014 };
            taskLinks.Add(tl4);
            TaskLink tl5 = new TaskLink() { ID = 5, SourceID = 1, TaskID = 1015 };
            taskLinks.Add(tl5);
            TaskLink tl6 = new TaskLink() { ID = 6, SourceID = 1, TaskID = 1016 };
            taskLinks.Add(tl6);
            TaskLink tl7 = new TaskLink() { ID = 7, SourceID = 1, TaskID = 1017 };
            taskLinks.Add(tl7);
            TaskLink tl8 = new TaskLink() { ID = 8, SourceID = 1, TaskID = 1018 };
            taskLinks.Add(tl8);
            return taskLinks;
        }
    }
}
