using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace PIEM.ExternalService
{
    /// <summary>
    /// 1. Use VSO query file (*.wiq) to get workitems:
    ///     TFSQuery tfsQuery = new TFSQuery(@"F:\Task\VSO PIE\New Query Chen Li.wiq");
    ///     Get workitem (task and bug) list:
    ///     WorkItemCollection wic = tfsQuery.ExecQuery();
    ///     foreach (WorkItem wi in wic)
    ///     {
    ///         DateTime? createDate = wi[CoreField.CreatedDate] as DateTime?;
    ///         string assignedTo = wi[CoreField.AssignedTo] as string;
    ///     }
    ///     Get workitem from specific workitem ID:
    ///     WorkItem workItem = tfsQuery.QueryWorkItem(8147392);
    /// 
    /// 2. Use shared query link, you only need to new this TFSQuery via Uri instead of *.wiq file, other steps are the same:
    ///     Ensure the Uri contains:
    ///     1. https://microsoft.visualstudio.com
    ///     2. ProjectCollection: defaultcollection
    ///     3. TeamProject: OS
    ///     4. _workitems?path=xxxxx&_a=query
    ///     Uri uri = new Uri("https://microsoft.visualstudio.com/defaultcollection/OS/_workitems?path=Shared%20Queries%2FGSL-GS%20Loc%2FWDGLocEngSup%20Triage%20-%20Assigned&_a=query");
    ///     TFSQuery tfsQuery = new TFSQuery(uri);
    /// </summary>
    public class TFSQuery : IDisposable
    {
        private Uri _tfsUri = null;
        private Uri _queryUri = null;
        private NetworkCredential _credential = null;
        private TfsTeamProjectCollection _tfsTeamProjectCollection = null;
        private WorkItemStore _workItemStore = null;
        private string _teamProject = null;
        private string _wiql = null;
        private QueryDefinition _queryDefinition = null;
        private string queryFolderPath = null;
        //private WorkItemCollection _workItemCollection = null;
        private ITestManagementService _ITestManagementService;
        private ITestManagementTeamProject _ITestManagementTeamProject = null;
        public TFSQuery(Uri uri)
        {
            this.InitUriQuery(uri);
        }

        public TFSQuery(string wiqPath)
        {
            this.InitWiqQuery(wiqPath);
        }
        public TFSQuery(string tfsUri, string teamProject, string wiql)
        {
            this._tfsUri = new Uri(tfsUri);
            this._wiql = wiql;
            this._teamProject = teamProject;
            this.ConnectToFoundationServer();
            _ITestManagementService = (ITestManagementService)this._tfsTeamProjectCollection.GetService(typeof(ITestManagementService));
            _ITestManagementTeamProject = _ITestManagementService.GetTeamProject(this._teamProject);
        }

        public NetworkCredential Credential
        {
            get
            {
                return this._credential;
            }
            set
            {
                this._credential = value;
            }
        }
        public string TFSCollectionURL
        {
            get
            {
                return this._tfsUri.ToString();
            }
        }
        public string TeamProject
        {
            get
            {
                return this._teamProject;
            }
        }

        #region public methods
        public WorkItemCollection ExecQuery()
        {
            WorkItemCollection itemCollection;
            if (string.IsNullOrEmpty(this._wiql))
            {
                if (this._queryDefinition != null)
                {
                    itemCollection = this._workItemStore.Query(this._queryDefinition.QueryText.Replace("@project", string.Format("'{0}'", this._teamProject)));
                }
                else
                {
                    throw new Exception("Query is null or empty.");
                }
            }
            else
            {
                itemCollection = this._workItemStore.Query(this._wiql.Replace("@project", string.Format("'{0}'", this._teamProject)));
            }
            return itemCollection;
        }
        public WorkItem QueryWorkItem(int ID)
        {
            WorkItem wi = this._workItemStore.GetWorkItem(ID);
            return wi;
        }
        public void ResetWiql(string wiql)
        {
            this._wiql = wiql;
        }

        public IEnumerable<ITestRun> GetAllTestRuns(List<int> planIds)
        {
            string queryStr = "select * from TestResult where TestPlanId in (" + string.Join(",", planIds.ToArray()) + ")";
            IEnumerable<ITestRun> testRuns = _ITestManagementService.QueryTestRuns(queryStr);
            return testRuns;
        }

        public List<ITestPoint> GetTestPoints(List<int> planIds)
        {
            ITestPlanCollection plans = _ITestManagementTeamProject.TestPlans.Query("select * from TestPlan where PlanId in (" + string.Join(",", planIds.ToArray()) + ")");
            List<ITestPoint> testPoints = new List<ITestPoint>();
            plans.Cast<ITestPlan>().AsParallel().ForAll((p) =>
            {
                ITestPointCollection points = p.QueryTestPoints("Select * from TestPoint");
                lock (testPoints)
                {
                    foreach (var point in points)
                    {
                        testPoints.Add(point);
                    }
                }
            });
            return testPoints;
        }

        public ITestPlanCollection GetTestPlan(List<int> planIds)
        {
            if (planIds.Count > 0)
            {
                ITestPlanCollection plans = _ITestManagementTeamProject.TestPlans.Query("select * from TestPlan where PlanId in (" + string.Join(",", planIds.ToArray()) + ")");
                return plans;
            }
            else
            {
                return null;
            }
        }
        public ITestPlan GetTestPlan(int planId)
        {
            ITestPlan plan = _ITestManagementTeamProject.TestPlans.Find(planId);
            return plan;
        }

        public ITestPointCollection GetTestPoints(ITestPlan testPlan)
        {
            if (testPlan != null)
            {
                return testPlan.QueryTestPoints("Select * from TestPoint");
            }
            else
            {
                return null;
            }
        }

        public ITestCase GetTestCase(int id)
        {
            ITestCase testCase = _ITestManagementTeamProject.TestCases.Find(id);
            return testCase;
        }

        public ITestConfiguration GetTestconfiguration(int id)
        {
            return this._ITestManagementTeamProject.TestConfigurations.Find(id);
        }

        public void Dispose()
        {
            //this._workItemCollection = null;
            this._workItemStore = null;
        }
        #endregion
        #region private methods
        private void ConnectToFoundationServer()
        {
            if (this._credential == null)
            {
                if (this._tfsUri.ToString().Contains(".visualstudio.com"))
                {
                    TfsClientCredentials tfsc = new TfsClientCredentials(new AadCredential());
                    _tfsTeamProjectCollection = new TfsTeamProjectCollection(_tfsUri, tfsc);
                }
                else
                {
                    this._credential = System.Net.CredentialCache.DefaultCredentials.GetCredential(_tfsUri, "Windows");
                    _tfsTeamProjectCollection = new TfsTeamProjectCollection(_tfsUri, _credential);
                }
            }
            else
            {
                if (_tfsUri.ToString().Contains(".visualstudio.com"))
                {
                    SimpleWebTokenCredential basicAu = new SimpleWebTokenCredential(_credential.UserName, _credential.Password);
                    TfsClientCredentials tfsc = new TfsClientCredentials(basicAu);
                    tfsc.AllowInteractive = false;
                    _tfsTeamProjectCollection = new TfsTeamProjectCollection(_tfsUri, tfsc);
                }
                else
                {
                    _tfsTeamProjectCollection = new TfsTeamProjectCollection(_tfsUri, _credential);
                }

            }

            _tfsTeamProjectCollection.Authenticate();
            _workItemStore = (WorkItemStore)_tfsTeamProjectCollection.GetService(typeof(WorkItemStore));
        }
        private void InitUriQuery(Uri queryUri)
        {
            this._queryUri = queryUri;
            Regex TFSQueryFragmentRegex = new Regex("[#|?]path=(?<QueryType>[^/]+)(?<QueryPath>.+)&_a=query");
            Regex TFSQueryLocalPathRegex = new Regex("/(?<ProjectCollection>[^/]+)/(?<TeamProject>\\S+)/_workitems");
            string fragment = string.IsNullOrEmpty(queryUri.Fragment) ? queryUri.Query : queryUri.Fragment;
            Match fragmentMatch = TFSQueryFragmentRegex.Match(HttpUtility.UrlDecode(fragment));
            Match localPathMatch = TFSQueryLocalPathRegex.Match(HttpUtility.UrlDecode(queryUri.LocalPath));
            if (fragmentMatch.Success && localPathMatch.Success)
            {
                if (queryUri.ToString().Contains(".visualstudio.com"))
                {
                    this._tfsUri = new Uri(string.Format("{0}://{1}/{2}", queryUri.Scheme, queryUri.Authority, localPathMatch.Groups["ProjectCollection"].ToString()));
                }
                else
                {
                    this._tfsUri = new Uri(string.Format("{0}://{1}/tfs/{2}", queryUri.Scheme, queryUri.Authority, localPathMatch.Groups["ProjectCollection"].ToString()));
                }

                this._teamProject = localPathMatch.Groups["TeamProject"].ToString();
                this.queryFolderPath = fragmentMatch.Groups["QueryType"].ToString() + '/' + fragmentMatch.Groups["QueryPath"].ToString();
            }

            this.ConnectToFoundationServer();
            _ITestManagementService = (ITestManagementService)this._tfsTeamProjectCollection.GetService(typeof(ITestManagementService));
            _ITestManagementTeamProject = _ITestManagementService.GetTeamProject(this._teamProject);
            this._queryDefinition = this.GetQueryQueryDefinition();
        }
        private void InitWiqQuery(string wiqPath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(wiqPath);
            this._tfsUri = new Uri(doc.SelectSingleNode("/WorkItemQuery/TeamFoundationServer").InnerText);
            this._wiql = doc.SelectSingleNode("/WorkItemQuery/Wiql").InnerText;
            this._teamProject = doc.SelectSingleNode("/WorkItemQuery/TeamProject").InnerText;
            this.ConnectToFoundationServer();
            _ITestManagementService = (ITestManagementService)this._tfsTeamProjectCollection.GetService(typeof(ITestManagementService));
            _ITestManagementTeamProject = _ITestManagementService.GetTeamProject(this._teamProject);
        }
        private QueryDefinition GetQueryQueryDefinition()
        {
            QueryHierarchy queryHierarchy = _workItemStore.Projects[this._teamProject].QueryHierarchy;
            List<string> queryPaths = this.queryFolderPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            QueryItem QueryItem = queryHierarchy;
            for (int i = 0; i < queryPaths.Count(); i++)
            {
                if (i != queryPaths.Count() - 1)
                {
                    bool Found = false;
                    if (QueryItem.GetType() == typeof(QueryHierarchy))
                    {
                        var RootHierarchy = QueryItem as QueryHierarchy;

                        foreach (QueryFolder folder in queryHierarchy)
                        {
                            if (folder.Name.ToLowerInvariant() == queryPaths[i].ToLowerInvariant())
                            {
                                QueryItem = folder;
                                Found = true;
                                break;
                            }
                        }
                    }
                    else if (QueryItem.GetType() == typeof(QueryFolder))
                    {
                        QueryFolder QueryFolder = QueryItem as QueryFolder;
                        foreach (QueryFolder QI in QueryFolder)
                        {
                            if (QI.Name.ToLowerInvariant() == queryPaths[i].ToLowerInvariant())
                            {
                                QueryItem = QI;
                                Found = true;
                                break;
                            }
                        }
                    }
                    if (!Found)
                    {
                        return null;
                    }
                }
                else
                {
                    var QueryDeepFolder = QueryItem as QueryFolder;
                    foreach (QueryItem QI in QueryDeepFolder)
                    {
                        if (QI.GetType() == typeof(QueryDefinition))
                        {
                            var queryDefinition = QI as QueryDefinition;
                            if (queryDefinition.Name.ToLowerInvariant() == queryPaths[i].ToLowerInvariant())
                            {
                                return queryDefinition;
                            }
                        }

                    }

                }
            }
            return null;
        }
        #endregion
    }
}
