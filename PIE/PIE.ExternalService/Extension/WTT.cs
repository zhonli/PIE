using Microsoft.DistributedAutomation.Asset;
using Microsoft.DistributedAutomation.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIEM.ExternalService.Extension
{
    public static class WTT
    {
        public static int GetOutcome(this Result result)
        {
            int Outcome = 0;

            if (result.ResultStatusId == Microsoft.DistributedAutomation.Jobs.ResultStatus.Completed)
            {
                if (result.Pass == 1)
                {
                    Outcome = 1;
                }
                else if (result.Fail == 1)
                {
                    Outcome = 2;
                }
                else if (result.NotApplicable == 1)
                {
                    Outcome = 3;
                }
                else if (result.NotRun == 1)
                {
                    Outcome = 4;
                }
                else if (result.InfrastructureFail == 1)
                {
                    Outcome = 5;
                }
            }
            return Outcome;
        }

        public static string GetLanguage(this Result result)
        {
            string Lang = "";
            foreach (ResourceConfigurationValue config in result.ResourceConfig.ResourceConfigurationValueCollection)
            {
                if (config.DimensionId == 24)
                {
                    Lang = config.ResourceConfigurationVal;
                    break;
                }
            }
            return Lang;
        }

        public static List<int> GetBugIDs(this Result result)
        {
            List<int> lt = new List<int>();

            ResultBugCollection resultBugCollection = result.ResultBugCollection;
            if (resultBugCollection != null && resultBugCollection.Count > 0)
            {
                foreach (ResultBug resultBug in resultBugCollection)
                {
                    lt.Add(resultBug.BugId);
                }
            }

            return lt.Distinct().ToList();
        }

    }
}
