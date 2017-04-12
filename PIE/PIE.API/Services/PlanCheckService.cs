using PIEM.Common.Model;
using PIEM.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;

namespace PIEM.API.Services
{
    public class PlanCheckService
    {
        private Plan checking;
        private float maxWorkloadPerDay;
        private DateTime startDate;
        private DateTime endDate;
        private float workloadPerDay;
        private int workduration;

        public PlanCheckService()
        {
            InitMaxWorkloadPerDay();
        }

        public PlanWarning Check(TestCollateral testCollateral)
        {
            try
            {
                InitCheck(testCollateral);
                return GetWarning();
            }
            catch(Exception ex)
            {
                //TODO:log exception
            }
            return null;
        }

        private PlanWarning GetWarning()
        {
            PlanWarning warning = new PlanWarning(this.checking);

            for (DateTime dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {

                IList<Plan> others = GetPlan(dt);

                float workloadOthersInCurDay = others.Sum(p => p.TestCollateral.Workhours / (p.EndDate.Value - p.StartDate.Value).Days);

                if (workloadPerDay + workloadOthersInCurDay >= maxWorkloadPerDay)
                {

                    warning.AddConflicts(others, dt);
                    return warning;
                }
            }

            return null;
            
        }

        private void InitCheck(TestCollateral testCollateral)
        {
            using (PIEMContext context = new PIEMContext())
            {
                Plan plan = context.Plans.First(p => p.ID == testCollateral.ID);
                this.checking = plan;
                this.startDate = plan.StartDate.Value.Date;
                this.endDate = plan.EndDate.Value.Date;
                this.workduration = (this.endDate - this.startDate).Days;
                this.workloadPerDay = testCollateral.Workhours / this.workduration;
            }
        }

        public void InitMaxWorkloadPerDay()
        {
            string maxWorkloadPerDay = ConfigurationManager.AppSettings["maxWorkloadPerDay"];
            if (!string.IsNullOrEmpty(maxWorkloadPerDay))
            {
                this.maxWorkloadPerDay = int.Parse(maxWorkloadPerDay);
                return;
            }

            string standardWorkHour = ConfigurationManager.AppSettings["standardWorkHour"];
            if (string.IsNullOrEmpty(standardWorkHour))
                standardWorkHour = "8";
            using (PIEMContext context = new PIEMContext())
            {
                int resourceCount = context.Resources.Count();
                this.maxWorkloadPerDay = int.Parse(standardWorkHour) * resourceCount;
            }
        }

        public IList<Plan> GetPlan(DateTime date)
        {
            IList<Plan> plans = new List<Plan>();
            using (PIEMContext context = new PIEMContext())
            {
                var items = context.Plans.Where(p => p.Status == PlanStatus.Created || p.Status == PlanStatus.Executing);
                foreach (var item in items)
                {
                    if (item.StartDate > date)
                        continue;
                    if (item.EndDate < date)
                        continue;
                    if (item.ID == this.checking.ID)
                        continue;
                    if (plans.Contains(item))
                        continue;
                    context.Entry(item).Reference(p => p.TestCollateral).Load();
                    plans.Add(item);
                }
            }
            return plans;
        }

    }


}