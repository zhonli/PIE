using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Data
{
    class SprintData
    {
        public static SprintData Current { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly SprintData _instance = new SprintData();
        }

        public Sprint this[string name]
        {
            get
            {
                var sprint = Test().FirstOrDefault(t => t.Path.ToUpper() == name.ToUpper());
                return sprint;
            }
        }
        public static IList<Sprint> Test()
        {
            IList<Sprint> sprints = new List<Sprint>();
            sprints.Add(new Sprint() { ID = 1, Path = "S1", ProjectID = 1, StartDate = DateTime.Parse("2016-07-01"), EndDate = DateTime.Parse("2016-07-31") });
            sprints.Add(new Sprint() { ID = 2, Path = "S2", ProjectID = 1, StartDate = DateTime.Parse("2016-08-01"), EndDate = DateTime.Parse("2016-08-31") });
            sprints.Add(new Sprint() { ID = 3, Path = "S3", ProjectID = 1, StartDate = DateTime.Parse("2016-09-01"), EndDate = DateTime.Parse("2016-09-30") });
            sprints.Add(new Sprint() { ID = 4, Path = "S4", ProjectID = 1, StartDate = DateTime.Parse("2016-10-01"), EndDate = DateTime.Parse("2016-10-31") });

            return sprints;
        }
    }
}
