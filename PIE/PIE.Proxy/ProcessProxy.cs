using PIEM.Common.Model;
using PIEM.Database;
using PIEM.ExternalService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PIEM.Proxy
{
    public class ProcessProxy : IDisposable
    {
        private ResultContext wtt = new ResultContext();
        private PIEMContext db = new PIEMContext();

        private IList<Process> runningProcesses = new List<Process>();

        private string intervalUnit = "d";
        private int interval = 1;
        public ProcessProxy()
        {
            Init();
        }

        public void Init()
        {
            this.intervalUnit = ConfigurationManager.AppSettings["intervalUnit"];
            int.TryParse(ConfigurationManager.AppSettings["interval"], out interval);
        }

        /// <summary>
        /// Scan all the executing process, and loop all the result summaries.
        /// If the last result statistics is expired, then create new result statistics.
        /// </summary>
        public void Scan()
        {
            this.runningProcesses = db.Processes.Include(p => p.ResultSummaries).Where(p => p.Plan.Type == PlanType.Execution && p.Status == ProcessStatus.Running).ToList();

            foreach (var ps in this.runningProcesses)
            {
                foreach (var rs in ps.ResultSummaries)
                {
                    WttResultSummary wttrs = wtt.GetResultSummary(rs.RSID);
                    if (wttrs == null)
                        continue;
                    // Get last statistics
                    var lastStats = GetLastResultStats(rs);
                    
                    if (lastStats == null || Expired(lastStats))
                    {
                        var newStats = new ResultStats();
                        
                        #region Init new result statistics

                        newStats.ResultSummaryID = rs.ID;
                        newStats.ActualRuntime = wttrs.ActualRuntime;
                        newStats.CancelledResults = wttrs.CancelledResults;
                        newStats.CompletedResults = wttrs.CompletedResults;
                        newStats.CreateTime = wttrs.CreateTime;
                        newStats.EstimatedRuntime = wttrs.EstimatedRuntime;
                        newStats.InProgressResults = wttrs.InProgressResults;
                        newStats.InvestigateResults = wttrs.InvestigateResults;
                        newStats.ResolvedResults = wttrs.ResolvedResults;
                        newStats.ResultSummaryStatusId = (int)wttrs.ResultSummaryStatusId;
                        newStats.ResultCollectionSetNameId = wttrs.ResultCollectionSetNameId;
                        newStats.RuntimeLeft = wttrs.RuntimeLeft;
                        newStats.SignedOffByCount = wttrs.SignedOffByCount;
                        newStats.TotalResults = wttrs.TotalResults;
                        newStats.StatsOn = DateTime.Now; 
                        #endregion

                        rs.LastStats = newStats;

                        db.ResultStats.Add(newStats);
                    }
                }

                #region Calc progress & pass rate
                int completedResults = 0;
                int cancelledResults = 0;
                int totalResults = 0;
                foreach (var rs in ps.ResultSummaries)
                {
                    completedResults += rs.LastStats.CompletedResults;
                    cancelledResults += rs.LastStats.CancelledResults;
                    totalResults += rs.LastStats.TotalResults;
                }

                if (totalResults > 0)
                {
                    ps.Progress = (float)Math.Round((double)(completedResults + cancelledResults) / totalResults * 100, 2, MidpointRounding.AwayFromZero);
                    ps.PassRate = (float)Math.Round((double)completedResults / totalResults * 100, 2, MidpointRounding.AwayFromZero);
                } 
                #endregion

            }
        }

        /// <summary>
        ///  Estimate whether the result statistics is expired.
        /// </summary>
        /// <param name="resultStats"></param>
        /// <returns></returns>
        private bool Expired(ResultStats resultStats)
        {
            TimeSpan expiredTime = DateTime.Now - resultStats.StatsOn;
            switch (intervalUnit)
            {
                case "n":
                    if (expiredTime.TotalMinutes > this.interval)
                    {
                        return true;
                    }

                    break;

                case "h":
                    if (expiredTime.TotalHours > this.interval)
                    {
                        return true;
                    }

                    break;
                default:
                case "d":
                    if (expiredTime.TotalDays > this.interval)
                    {
                        return true;
                    }

                    break;
            };
            return false;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void UpdateProgress()
        {
            db.SaveChanges();
        }

        private ResultStats GetLastResultStats(ResultSummary rs)
        {
            var stats = db.ResultStats.Where(s => s.ResultSummaryID == rs.ID).OrderByDescending(s => s.StatsOn).ToList();
            if (stats == null || stats.Count() == 0)
                return null;
            else
                return stats[0];
        }

        public void Dispose()
        {
            wtt.Dispose();
            db.Dispose();
        }
    }
}
