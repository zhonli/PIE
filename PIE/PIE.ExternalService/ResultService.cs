using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PIEM.Common.Model;
using PIEM.ExternalService.WTT;
using Microsoft.DistributedAutomation.Jobs;

namespace PIEM.ExternalService
{
    public class ResultService
    {
        private string dataStore = "1Windows";
        public ResultService()
        {

        }
        /// <summary>
        /// Get Result summaries by task id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public IList<WttResultSummary> GetResultSummaries(int taskId)
        {
            IList<WttResultSummary> retList = new List<WttResultSummary>();
            ResultSummaryCollection resultSummaries = ResultCollectionHelper.GetResultCollectionWithTaskID(this.dataStore, taskId);

            foreach (var resultSummary in resultSummaries)
            {
                WttResultSummary rsEntity = new WttResultSummary();
                rsEntity.ID = resultSummary.Id;
                rsEntity.Name = resultSummary.Name;
                rsEntity.ActualRuntime = resultSummary.ActualRuntime;
                rsEntity.CancelledResults = resultSummary.CancelledResults;
                rsEntity.CompletedResults = resultSummary.CompletedResults;
                rsEntity.CreateTime = resultSummary.CreateTime;
                rsEntity.EstimatedRuntime = resultSummary.EstimatedRuntime;
                rsEntity.Guid = resultSummary.Guid;
                rsEntity.InProgressResults = resultSummary.InProgressResults;
                rsEntity.InvestigateResults = resultSummary.InvestigateResults;
                rsEntity.ResolvedResults = resultSummary.ResolvedResults;
                rsEntity.ResultSummaryStatusId = (int)resultSummary.ResultSummaryStatusId;
                rsEntity.ResultCollectionSetNameId = resultSummary.ResultCollectionSetNameId;
                rsEntity.RuntimeLeft = resultSummary.RuntimeLeft;
                rsEntity.SignedOffByCount = resultSummary.SignedOffByCount;
                rsEntity.TotalResults = resultSummary.TotalResults;

                retList.Add(rsEntity);
            }

            return retList;
        }
        /// <summary>
        /// Get result summary by result collection Id
        /// </summary>
        /// <param name="rcId"></param>
        /// <returns></returns>
        public WttResultSummary GetResultSummary(int rcId)
        {
            using (ResultContext context = new ResultContext())
            {
                var rs = context.GetResultSummary(rcId);
                return rs;
            }
        }
    }
}
