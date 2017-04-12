using Microsoft.DistributedAutomation;
using PIEM.ExternalService.WTT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DistributedAutomation.Jobs;

namespace PIEM.ExternalService
{
    public class ResultContext : WTTBase, IDisposable
    {
        private DataStore dataStore;
        public ResultContext(string dataStoreName = "1Windows")
        {
            SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTServerName, WTTDBName);
            this.dataStore = Enterprise.Connect(dataStoreName, JobsRuntimeDataStore.ServiceName, connectInfo);
        }

        public void Dispose()
        {
            this.dataStore.Dispose();
        }
        /// <summary>
        /// Get ResultSummary by ID
        /// </summary>
        /// <param name="rsId">result summary Id</param>
        /// <returns>ResultSummary Entity</returns>
        public Common.Model.WttResultSummary GetResultSummary(int rsId)
        {
            Common.Model.WttResultSummary resultSummary = null;
            Query query = new Query(typeof(ResultSummary));
            query.AddExpression("Id", QueryOperator.Equals, rsId);

            ResultSummaryCollection resultCollections = this.dataStore.Query(query) as ResultSummaryCollection;
            foreach (var item in resultCollections)
            {
                resultSummary = new Common.Model.WttResultSummary();
                resultSummary.ID = item.Id;
                resultSummary.Name = item.Name;
                resultSummary.ActualRuntime = item.ActualRuntime;
                resultSummary.CancelledResults = item.CancelledResults;
                resultSummary.CompletedResults = item.CompletedResults;
                resultSummary.CreateTime = item.CreateTime;
                resultSummary.EstimatedRuntime = item.EstimatedRuntime;
                resultSummary.Guid = item.Guid;
                resultSummary.InProgressResults = item.InProgressResults;
                resultSummary.InvestigateResults = item.InvestigateResults;
                resultSummary.ResolvedResults = item.ResolvedResults;
                resultSummary.ResultSummaryStatusId = (int)item.ResultSummaryStatusId;
                resultSummary.ResultCollectionSetNameId = item.ResultCollectionSetNameId;
                resultSummary.RuntimeLeft = item.RuntimeLeft;
                resultSummary.SignedOffByCount = item.SignedOffByCount;
                resultSummary.TotalResults = item.TotalResults;
                break;
            }
            return resultSummary;
        }
    }
}
