using System.Data;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.OData;
using PIEM.Common.Model;
using PIEM.Database;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System;
using System.Web.OData.Routing;
using Newtonsoft.Json.Linq;
using PIEM.ExternalService;
using System.Collections.Generic;
using PIEM.API.TFSProxy;
using System.Globalization;
using System.Configuration;

namespace PIEM.API.Controllers
{
    public class ProcessesController : ODataController
    {
        PIEMContext db = new PIEMContext();

        private bool ProcessExists(int key)
        {
            return db.Processes.Any(p => p.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // GET: odata/Processes
        [EnableQuery]
        public IQueryable<Process> Get()
        {
            return db.Processes;
        }

        // GET: odata/Processes(5)
        [EnableQuery]
        public SingleResult<Process> Get([FromODataUri] int key)
        {
            IQueryable<Process> result = db.Processes.Where(p => p.ID == key);
            return SingleResult.Create(result);
        }

        // GET ~/Processes(5)/Transactions         
        [EnableQuery]
        public IHttpActionResult GetTransactions(int key)
        {
            var transactions = db.Processes.Include(p => p.Transactions).Single(a => a.ID == key).Transactions;
            return Ok(transactions);
        }

        // GET ~/Processes(5)/ResultSummaries         
        [EnableQuery]
        public IHttpActionResult GetResultSummaries(int key)
        {
            var resultSummaries = db.Processes.Include(p => p.ResultSummaries).Single(a => a.ID == key).ResultSummaries;

            for (int i = 0; i < resultSummaries.Count; i++)
            {
                var resultStats = GetLastResultStats(resultSummaries[i]);
                if (resultStats != null)
                    resultSummaries[i].LastStats = resultStats;

            }
            return Ok(resultSummaries);
        }

        private ResultStats GetLastResultStats(ResultSummary rs)
        {
            var stats = db.ResultStats.Where(s => s.ResultSummaryID == rs.ID).OrderByDescending(s => s.StatsOn).ToList();
            if (stats == null || stats.Count() == 0)
                return null;
            else
                return stats[0];
        }

        [EnableQuery]
        [ODataRoute("Processes({processId})/Transactions({transactionId})")]
        public IHttpActionResult GetTransaction(int processId, int transactionId)
        {
            var transactions = db.Processes.Include(p => p.Transactions).Single(a => a.ID == processId).Transactions;
            var tran = transactions.Single(t => t.ID == transactionId);
            return Ok(tran);
        }

        [EnableQuery]
        [ODataRoute("Processes({processId})/ResultSummaries({resultSummaryId})")]
        public IHttpActionResult GetResultSummary(int processId, int resultSummaryId)
        {
            var resultSummaries = db.Processes.Include(p => p.ResultSummaries).Single(a => a.ID == processId).ResultSummaries;

            var rs = resultSummaries.Single(t => t.ID == resultSummaryId);

            var resultStats = GetLastResultStats(rs);
            if (resultStats != null)
                rs.LastStats = resultStats;

            return Ok(rs);
        }

        // PUT ~/Processes(5)/Transactions(101)    
        [ODataRoute("Processes({processId})/Transactions({transactionId})")]
        public async Task<IHttpActionResult> PutToTransaction(int processId, int transactionId, [FromBody]Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var process = db.Processes.Include(p => p.Transactions).Single(a => a.ID == processId);
            var transactions = process.Transactions;

            var originTran = transactions.Single(t => t.ID == transactionId);
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    originTran.ExecutedOn = DateTime.Now;
                    originTran.State = transaction.State;

                    if (transaction.Name == "UpdateProgress")
                    {
                        dynamic state = JObject.Parse(transaction.State);
                        process.Progress = state.Progress;
                        process.CompletedWorkhours = state.CompletedWorkhours;
                        process.ActualWorkhours = state.ActualWorkhours;
                    }

                    if (transaction.Name == "BlockProcess")
                    {
                        process.Status = ProcessStatus.Blocked;
                    }

                    if (transaction.Name == "RunProcess")
                    {
                        process.Status = ProcessStatus.Running;
                    }

                    if (transaction.Name == "CutProcess" || transaction.Name == "AbortProcess")
                    {
                        process.Status = ProcessStatus.Cutted;

                        var plan = db.Plans.Find(processId);
                        plan.Status = PlanStatus.Closed;
                        plan.ClosedOn = DateTime.Now;
                    }
                    if (transaction.Name == "CloseProcess")
                    {
                        process.Status = ProcessStatus.Closed;
                        dynamic state = JObject.Parse(transaction.State);
                        //process.Progress = state.Progress;
                        //process.CompletedWorkhours = state.CompletedWorkhours;
                        process.ActualWorkhours = state.ActualWorkhours;
                        process.EndOn = state.EndOn;

                        var plan = db.Plans.Find(processId);
                        plan.Status = PlanStatus.Closed;
                        plan.ClosedOn = state.EndOn;

                        CloseVSOTaskAsync(process);

                    }

                    await db.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch
                {
                    dbContextTransaction.Rollback();
                }
            }
            return Updated(originTran);

        }

        // POST ~/Processes(5)/Transactions   
        [ODataRoute("Processes({processId})/Transactions")]
        public async Task<IHttpActionResult> PostToTransaction(int processId, [FromBody]Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    transaction.ExecutedOn = DateTime.Now;
                    db.Transactions.Add(transaction);

                    var process = db.Processes.Single(a => a.ID == processId);
                    process.Transaction = transaction;
                    if (transaction.Name == "UpdateProgress")
                    {
                        dynamic state = JObject.Parse(transaction.State);
                        process.Progress = state.Progress;
                        process.CompletedWorkhours = state.CompletedWorkhours;
                        process.ActualWorkhours = state.ActualWorkhours;
                    }

                    if (transaction.Name == "BlockProcess")
                    {
                        process.Status = ProcessStatus.Blocked;
                    }

                    if (transaction.Name == "RunProcess")
                    {
                        process.Status = ProcessStatus.Running;
                    }

                    if (transaction.Name == "CutProcess" || transaction.Name == "AbortProcess")
                    {
                        process.Status = ProcessStatus.Cutted;

                        var plan = db.Plans.Find(processId);
                        plan.Status = PlanStatus.Closed;
                        plan.ClosedOn = DateTime.Now;
                    }

                    if (transaction.Name == "CloseProcess")
                    {
                        process.Status = ProcessStatus.Closed;
                        dynamic state = JObject.Parse(transaction.State);
                        //process.Progress = state.Progress;
                        //process.CompletedWorkhours = state.CompletedWorkhours;
                        process.ActualWorkhours = state.ActualWorkhours;
                        process.EndOn = state.EndOn;


                        var plan = db.Plans.Find(processId);
                        plan.Status = PlanStatus.Closed;
                        plan.ClosedOn = state.EndOn;

                        CloseVSOTaskAsync(process);
                    }

                    await db.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch
                {
                    dbContextTransaction.Rollback();
                }
            }
            return Created(transaction);
        }

        // DELETE ~/Processes(5)/Transactions(101)      
        [ODataRoute("Processes({processId})/Transactions({transactionId})")]
        public IHttpActionResult DeleteTransaction(int processId, int transactionId)
        {
            var process = db.Processes.Include(p => p.Transactions).Single(a => a.ID == processId);
            var originTran = process.Transactions.Single(t => t.ID == transactionId);
            if (process.Transactions.Remove(originTran))
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        // PUT: odata/Processes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Process update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.ID)
            {
                return BadRequest();
            }

            db.Entry(update).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcessExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }

        // PATCH: odata/Processes(5)
        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Process> processDelta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Processes.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }

            processDelta.Patch(entity);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcessExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }
        // DELETE: odata/Processes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var process = await db.Processes.FindAsync(key);
            if (process == null)
            {
                return NotFound();
            }
            db.Processes.Remove(process);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // Other controller methods not shown.
        [HttpPost]
        public async Task<IHttpActionResult> Run([FromODataUri]int key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var entity = await db.Processes.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Status = ProcessStatus.Running;

            Transaction tran = new Transaction();
            tran.Name = "RunProcess";
            tran.ProcessID = key;
            tran.ExecutedOn = DateTime.Now;

            db.Transactions.Add(tran);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (!ProcessExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Block([FromODataUri]int key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var entity = await db.Processes.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Status = ProcessStatus.Blocked;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (!ProcessExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        private async Task CloseVSOTaskAsync(Process process)
        {
            await Task.Run(() =>
            {
                try
                {
                    CloseVSOTask(process);
                }
                catch
                {

                }
            });
        }
        /// <summary>
        /// Close VSO task
        /// </summary>
        /// <param name="process"></param>
        private void CloseVSOTask(Process process)
        {
            var plan = db.Plans.Find(process.ID);
            TaskLink taskLink = plan.TaskLink;
            WorkItemSource source = db.WorkItemSources.First(s => s.ID == taskLink.SourceID);
            string TFSDataUri = ConfigurationManager.AppSettings["TFSDataUri"].TrimEnd('/');
            string teamProjectCollection = GetTeamProjectCollection(source.TeamFoundationServer);
            string serverBaseUri = GetServerBaseUri(source.TeamFoundationServer);
            
            //  Get workitems by invoking ODataTFS api 
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            var proxy = new TFSData(new Uri(TFSDataUri));
            proxy.SendingRequest += (s, e) =>
            {
                e.RequestHeaders.Add("TfsServer", serverBaseUri);
            };

            var workItem = proxy.Execute<WorkItem>(new Uri(string.Format(CultureInfo.InvariantCulture,
                                                                        "{0}/{1}/Projects('{2}')/WorkItems({3})",
                                                                        TFSDataUri,
                                                                        teamProjectCollection,
                                                                        source.TeamProject,
                                                                        taskLink.TaskID))).FirstOrDefault();

            if (workItem != null && workItem.State != "Completed")
            {
                workItem.CompletedWork = process.CompletedWorkhours;
                workItem.Cost = process.CompletedWorkhours;
                workItem.RemainingWork = 0;
                workItem.RemainingDays = 0;
                workItem.State = "Completed";
                workItem.ClosedDate = process.EndOn.Value;
                proxy.UpdateObject(workItem);
                proxy.SaveChanges(System.Data.Services.Client.SaveChangesOptions.ReplaceOnUpdate);
            }

        }
        /// <summary>
        /// Get team project name by team fundation server uri.
        /// such as DefaultCollection
        /// </summary>
        /// <param name="teamFoundationServer"></param>
        /// <returns></returns>
        private string GetTeamProjectCollection(string teamFoundationServer)
        {
            int lastIndex = teamFoundationServer.LastIndexOf('/');
            return teamFoundationServer.Substring(lastIndex + 1);
        }
        /// <summary>
        /// get server base uri by team fundation server uri
        /// such as https://microsoft.visualstudio.com/
        /// </summary>
        /// <param name="teamFoundationServer"></param>
        /// <returns></returns>
        private string GetServerBaseUri(string teamFoundationServer)
        {
            int lastIndex = teamFoundationServer.LastIndexOf('/');
            return teamFoundationServer.Substring(0, lastIndex);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Abort([FromODataUri]int key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var entity = await db.Processes.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Status = ProcessStatus.Cutted;
            entity.EndOn = DateTime.Now;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (!ProcessExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Get result summaries by task Id
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpGet]
        [ODataRoute("GetWttRCs(TaskID={taskId})")]
        public IHttpActionResult GetWttRCs([FromODataUri] int taskId)
        {
            ResultService service = new ResultService();
            var rsList = service.GetResultSummaries(taskId);
            return Ok(rsList);
        }
        /// <summary>
        /// Get result collection by result collection Id
        /// </summary>
        /// <param name="rcId"></param>
        /// <returns></returns>
        [HttpGet]
        [ODataRoute("GetWttRC(RCID={rcId})")]
        public IHttpActionResult GetWttRC([FromODataUri] int rcId)
        {
            ResultService service = new ResultService();
            WttResultSummary rs = service.GetResultSummary(rcId);
            return Ok(rs);
        }
    }
}
