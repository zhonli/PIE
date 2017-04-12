using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Data
{
    public class AssignmentData
    {
        public static AssignmentData Current { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly AssignmentData _instance = new AssignmentData();
        }

        public static IList<Assignment> Test()
        {
            IList<Assignment> assignments = new List<Assignment>();

            Assignment asign1 = new Assignment() { PlanID = 1, ResourceID = 1, Units =50f, CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01 15:50:54.283") };
            assignments.Add(asign1);
            Assignment asign2 = new Assignment() { PlanID = 1, ResourceID = 2, Units =50f, CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01 15:50:54.283") };
            assignments.Add(asign2);
            Assignment asign3 = new Assignment() { PlanID = 2, ResourceID = 1, Units =50f, CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01 15:50:54.283") };
            assignments.Add(asign3);
            Assignment asign4 = new Assignment() { PlanID = 2, ResourceID = 2, Units =50f, CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01 15:50:54.283") };
            assignments.Add(asign4);
            Assignment asign5 = new Assignment() { PlanID = 3, ResourceID = 1, Units =50f, CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01 15:50:54.283") };
            assignments.Add(asign5);
            Assignment asign6 = new Assignment() { PlanID = 3, ResourceID = 2, Units =50f, CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01 15:50:54.283") };
            assignments.Add(asign6);
            Assignment asign7 = new Assignment() { PlanID = 4, ResourceID = 1, Units =50f, CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01 15:50:54.283") };
            assignments.Add(asign7);
            Assignment asign8 = new Assignment() { PlanID = 4, ResourceID = 2, Units =50f, CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01 15:50:54.283") };
            assignments.Add(asign8);
            Assignment asign9 = new Assignment() { PlanID = 5, ResourceID = 1, Units =50f, CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01 15:50:54.283") };
            assignments.Add(asign9);
            Assignment asign10 = new Assignment() { PlanID = 5, ResourceID = 2, Units =50f, CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01 15:50:54.283") };
            assignments.Add(asign10);

            return assignments;
        }

    }
}
