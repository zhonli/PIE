using PIEM.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.Database.Data
{
    public class PlanData
    {
        public static PlanData Current { get { return Nested._instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly PlanData _instance = new PlanData();
        }

        public static IList<Plan> Test()
        {
            IList<Plan> plans = new List<Plan>();
            
            var plan1 = new Plan() { Title = "Display Plan", StartDate = DateTime.Parse("2016-07-10"), EndDate = DateTime.Parse("2016-07-20"), CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan1);

            var plan2 = new Plan() { Title = "Specify Plan Parameter (Only input Test case number, language number, Start Date, End Date)", StartDate = DateTime.Parse("2016-07-20"), EndDate = DateTime.Parse("2016-07-25"),  CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01"), ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan2);

            var plan3 = new Plan() { Title = "Specify Plan", StartDate = DateTime.Parse("2016-07-25"), EndDate = DateTime.Parse("2016-07-26"), CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-25"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan3);

            var plan4 = new Plan() { Title = "Create Plan", StartDate = DateTime.Parse("2016-07-01"), EndDate = DateTime.Parse("2016-07-08"), CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan4);

            var plan5 = new Plan() { Title = "Search Plan", StartDate = DateTime.Parse("2016-07-08"), EndDate = DateTime.Parse("2016-07-11"), CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan5);

            var plan6 = new Plan() { Title = "Warning MC resource conflict", StartDate = DateTime.Parse("2016-07-11"), EndDate = DateTime.Parse("2016-07-13"), CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan6);

            var plan7 = new Plan() { Title = "System display warning", StartDate = DateTime.Parse("2016-07-13"), EndDate = DateTime.Parse("2016-07-15"), CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan7);

            var plan8 = new Plan() { Title = "Warning resource conflict", StartDate = DateTime.Parse("2016-07-15"), EndDate = DateTime.Parse("2016-07-21"), CreateBy = "FAREAST\\v-zhongi", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan8);
            
            var plan11 = new Plan() { Title = "Notify create Sanity Plan", StartDate = DateTime.Parse("2016-07-02"), EndDate = DateTime.Parse("2016-07-08"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan11);

            var plan12 = new Plan() { Title = "Notify create QA plan", StartDate = DateTime.Parse("2016-07-08"), EndDate = DateTime.Parse("2016-07-11"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan12);

            var plan13 = new Plan() { Title = "Specify Query condition", StartDate = DateTime.Parse("2016-07-12"), EndDate = DateTime.Parse("2016-07-15"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan13);

            var plan14 = new Plan() { Title = "Display Language details chart", StartDate = DateTime.Parse("2016-07-15"), EndDate = DateTime.Parse("2016-07-21"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan14);

            var plan15 = new Plan() { Title = "Display Test Result details chart", StartDate = DateTime.Parse("2016-07-21"), EndDate = DateTime.Parse("2016-07-25"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan15);

            var plan16 = new Plan() { Title = "Notify create tester rerun test case when bug fix", StartDate = DateTime.Parse("2016-07-25"), EndDate = DateTime.Parse("2016-07-28"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan16);

            var plan21 = new Plan() { Title = "Request a Pactera system account for sending PIEM email", StartDate = DateTime.Parse("2016-07-08"), EndDate = DateTime.Parse("2016-07-10"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan21);

            var plan22 = new Plan() { Title = "Display the plan changes", StartDate = DateTime.Parse("2016-07-11"), EndDate = DateTime.Parse("2016-07-12"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan22);

            var plan23 = new Plan() { Title = "Training: ASP.NET", StartDate = DateTime.Parse("2016-07-14"), EndDate = DateTime.Parse("2016-07-18"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan23);

            var plan24 = new Plan() { Title = "Redesign Report and Execution parts to solve conflict charts issue between Lucy and Zhanfeng", StartDate = DateTime.Parse("2016-07-18"), EndDate = DateTime.Parse("2016-07-20"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan24);

            var plan25 = new Plan() { Title = "Plan manager UI design comments", StartDate = DateTime.Parse("2016-07-20"), EndDate = DateTime.Parse("2016-07-24"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan25);

            var plan26 = new Plan() { Title = "Draw structure diagram and data diagram", StartDate = DateTime.Parse("2016-07-25"), EndDate = DateTime.Parse("2016-07-28"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan26);

            var plan27 = new Plan() { Title = "Training: Examination", StartDate = DateTime.Parse("2016-07-28"), EndDate = DateTime.Parse("2016-07-31"), CreateBy = "FAREAST\\v-yangy", CreateTime = DateTime.Parse("2016-07-01"),  ProjectID = 1, Priority = Priority.middle };
            plans.Add(plan27);

            return plans;
        }

    }
}
