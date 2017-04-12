//----------------------------------------------------------------------------------------------------------------------
// <copyright file="ResultCollectionHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>Results and Result collections</summary>
//----------------------------------------------------------------------------------------------------------------------
using Microsoft.DistributedAutomation;
using Microsoft.DistributedAutomation.ComponentHierarchy;
using Microsoft.DistributedAutomation.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PIEM.ExternalService.WTT
{
    [DataContract]
    public enum FilterOptions
    {

        /// <summary>
        /// Filter based on language
        /// </summary>
        [EnumMember]
        Language,

        /// <summary>
        /// Filter based on Product
        /// </summary>
        [EnumMember]
        Product,

        /// <summary>
        /// Filter based on Priority
        /// </summary>
        [EnumMember]
        Priority,

        /// <summary>
        /// Filter based on User
        /// </summary>
        [EnumMember]
        User,

        /// <summary>
        /// Filter based on Job ids
        /// </summary>
        [EnumMember]
        JobId,

        /// <summary>
        /// Pass true if specified job ids need to be excluded
        /// </summary>
        [EnumMember]
        AreJobIdsExcluded,

        /// <summary>
        /// Pass true if Completed results are required in query
        /// </summary>
        [EnumMember]
        Status,

        /// <summary>
        /// NotRun if NotRun results are required in query
        /// </summary>
        [EnumMember]
        State,

        /// <summary>
        /// Start date
        /// </summary>
        [EnumMember]
        StartDate,


        /// <summary>
        /// End Date
        /// </summary>
        [EnumMember]
        EndDate,


        /// <summary>
        /// If only failed results are required.
        /// </summary>
        [EnumMember]
        IfOnlyFailedOnesAreRequired,

        /// <summary>
        /// If only failed results are required.
        /// </summary>
        [EnumMember]
        Keywords,

        /// <summary>
        /// If Scenario results are required.
        /// </summary>
        [EnumMember]
        Scenarios,


        /// <summary>
        /// If Non Scenario results are required.
        /// </summary>
        [EnumMember]
        NonScenarios,

    }

    public class ResultCollectionHelper : WTTBase
    {
        /// <summary>
        /// Get Result Collection by RCID
        /// </summary>
        /// <param name="RCID">Result Collection ID</param>
        /// <param name="datastore">Result Collection Datastore</param>
        /// <returns>Result Collection</returns>
        public static ResultCollection GetResults(int RCID, string datastore)
        {
            SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTServerName, WTTDBName);
            using (DataStore ds = Enterprise.Connect(datastore, JobsRuntimeDataStore.ServiceName, connectInfo))
            {
                Query query = new Query(typeof(Result));
                QueryStatement qs = new QueryStatement();
                qs.AddExpression("Id", QueryOperator.Equals, RCID);
                query.AddExpression("Result.ResultSummaryCollection", QueryOperator.Has, qs);
                ResultCollection resultCollection = (ResultCollection)ds.Query(query);
                return resultCollection;
            }
        }
        /// <summary>
        /// This method accepts multiple result collection ID values and returns the result collection for same.
        /// </summary> 
        /// <param name="resultCollectionIDs">List of result collection IDs</param>
        ///<param name="filterOptions">Filters used to filter results</param>
        /// <param name="datastore">Result Collection Datastore</param>
        /// <returns>Result collection which contains results for all the result collection IDs specified as input.</returns>
        public static ResultCollection GetResults(string datastore, List<int> resultCollectionIDs, Dictionary<FilterOptions, List<string>> filterOptions)
        {
            SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTServerName, WTTDBName);
            using (DataStore ds = Enterprise.Connect(datastore, JobsRuntimeDataStore.ServiceName, connectInfo))
            {
                if (filterOptions == null)
                {
                    filterOptions = new Dictionary<FilterOptions, List<string>>();
                }
                return (ResultCollection)ds.Query(GenRcIdExp(resultCollectionIDs, filterOptions));
            }
        }

        public static ResultSummaryCollection GetResultSummaries(string datastore, List<int> resultCollectionIDs)
        {
            throw new NotImplementedException();
            //SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTServerName, WTTDBName);
            //using (DataStore ds = Enterprise.Connect(datastore, JobsRuntimeDataStore.ServiceName, connectInfo))
            //{
            //    if (filterOptions == null)
            //    {
            //        filterOptions = new Dictionary<FilterOptions, List<string>>();
            //    }
            //    return (ResultCollection)ds.Query(GenRcIdExp(resultCollectionIDs, filterOptions));
            //}
        }

        /// <summary>
        /// Get the Result collection summary using a WTT query contents
        /// </summary>
        /// <example>
        /// <code>
        /// ResultSummaryCollection resultSummaryCollection = ResultCollectionHelper.GetResultCollectionSummary(File.ReadAllText("C:\\myWTQ.wtq"));
        /// </code>
        /// </example>
        /// /// <param name="datastore">Result Collection Datastore</param>
        /// <param name="wttQuery">WTT query in string format(from WTQ file)</param>
        /// <returns>A collection of Jobs returned from the WTT query.</returns>
        public static ResultSummaryCollection GetResultCollectionSummary(string datastore, string wttQuery)
        {
            SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTServerName, WTTDBName);
            using (DataStore ds = Enterprise.Connect(datastore, JobsRuntimeDataStore.ServiceName, connectInfo))
            {
                if (string.IsNullOrEmpty(wttQuery))
                {
                    throw new ArgumentNullException("wttQuery: Parameter can't be null or empty");
                }

                System.Xml.Linq.XDocument xdoc = XDocument.Load(wttQuery);
                return GetResultCollection(xdoc.CreateReader(), ds);
            }
        }
        /// <summary>
        /// Get result collection list with specified task ID
        /// </summary>
        /// <param name="datastore">Result Collection Datastore</param>
        /// <param name="taskID">Task ID got from VSO</param>
        /// <returns>Result collection list</returns>
        public static ResultSummaryCollection GetResultCollectionWithTaskID(string datastore, int taskID)
        {
            SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTServerName, WTTDBName);
            using (DataStore ds = Enterprise.Connect(datastore, JobsRuntimeDataStore.ServiceName, connectInfo))
            {
                Query query = new Query(typeof(ResultSummary));
                query.AddExpression("Name", QueryOperator.BeginsWith, taskID);

                ResultSummaryCollection resultCollections = ds.Query(query) as ResultSummaryCollection;
                return resultCollections;
            }
        }
        /// <summary>
        /// Get product or product name by the full path
        /// </summary>
        /// <param name="datastore">WTT datastore</param>
        /// <param name="featureFullPath">The full path of the jobs, for example "$\\Threshold\\CORE-OS Core\\PCE-Partner and Customer Engagement\\GSL-Global Services Localization\\Quality Engineering\\Experience Validation\\Windows Server\\SetupandUpgrade\\Upgrade"</param>
        /// <returns>Product or feature name</returns>
        public static Feature GetFeatureFromPath(string datastore, string featureFullPath)
        {
            SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTServerName, WTTDBName);
            using (DataStore ds = Enterprise.Connect(datastore, JobsRuntimeDataStore.ServiceName, connectInfo))
            {
                Query query = new Query(typeof(Feature));
                query.AddExpression("FullPath", QueryOperator.Equals, featureFullPath);
                FeatureCollection fc = ds.Query(query) as FeatureCollection;
                if (fc.Count == 0)
                {
                    return null;
                }
                return fc[0];
            }

        }
        /// <summary>
        /// Get jobs by ID in WTT
        /// </summary>
        /// <param name="datastore">Job datastore in WTT</param>
        /// <param name="jobIDs">Job ID in WTT</param>
        /// <returns>Job collections</returns>
        public static JobCollection GetJobsFromIds(string datastore, List<int> jobIDs)
        {
            SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTServerName, WTTDBName);
            using (DataStore ds = Enterprise.Connect(datastore, JobsRuntimeDataStore.ServiceName, connectInfo))
            {
                Query queryJob = new Query(typeof(Job));
                queryJob.AddExpression("Id", QueryOperator.Within, jobIDs.ToArray());
                return (JobCollection)ds.Query(queryJob, true);
            }
        }
        /// <summary>
        /// Generates the wtt query based on passed criterion
        /// </summary>
        /// <param name="rcIDs">Result collection ids to fetch results from</param>
        /// <param name="filterCriterion">This is a collection having where key is a enum FilterOptions 
        /// (indicates the field you want to filter your results on e.g. Language, Product, Priority, etc.).
        /// </param>
        /// <returns>WTT query object</returns>
        private static Query GenRcIdExp(List<int> rcIDs, Dictionary<FilterOptions, List<string>> filterCriterion)
        {
            bool areJobIdsExcluded = false;

            Query query = new Query(typeof(Result));
            QueryStatement queryCollection = new QueryStatement();

            if (filterCriterion != null && filterCriterion.Count > 0)
            {
                foreach (var key in filterCriterion.Keys)
                {
                    switch (key)
                    {
                        case FilterOptions.Product:
                            AddProductQueryExpression(filterCriterion, query, key, "Job", QueryOperator.Has, "Feature", "FullPath", QueryOperator.BeginsWith);
                            break;
                        case FilterOptions.Priority:
                            AddQueryExpression(filterCriterion, query, key, "Job", "Priority");
                            break;
                        case FilterOptions.Language:
                            AddQueryExpression(filterCriterion, query, key, "ResourceConfigurationValueCollection", "ResourceConfigurationVal");
                            break;
                        case FilterOptions.User:
                            AddQueryExpression(filterCriterion, query, key, "AssignedToAlias");
                            break;
                        case FilterOptions.Status:
                            AddQueryExpression(filterCriterion, query, key, "ResultStatusId");
                            break;
                        case FilterOptions.State:
                            AddQueryExpressionForState(filterCriterion, query, key);
                            break;
                        case FilterOptions.StartDate:
                            AddQueryExpression(filterCriterion, query, key, "LastUpdatedTime", QueryOperator.EqualsGreater);
                            break;
                        case FilterOptions.EndDate:
                            AddQueryExpression(filterCriterion, query, key, "LastUpdatedTime", QueryOperator.EqualsLess);
                            break;
                        case FilterOptions.IfOnlyFailedOnesAreRequired:

                            if (filterCriterion[key].Count > 0 && Convert.ToBoolean(filterCriterion[key][0]))
                            {
                                query.AddExpression("Fail", QueryOperator.Greater, 0);
                                query.AddConjunction(Conjunction.And);
                            }

                            break;
                        case FilterOptions.AreJobIdsExcluded:
                            areJobIdsExcluded = bool.Parse(filterCriterion[FilterOptions.AreJobIdsExcluded].First());
                            break;
                    }
                }

                if (filterCriterion.Keys.Contains(FilterOptions.JobId))
                {
                    if (areJobIdsExcluded)
                    {
                        query.AddExpression("JobId", QueryOperator.NotWithin, filterCriterion[FilterOptions.JobId].ToArray());
                        query.AddConjunction(Conjunction.And);
                    }
                    else
                    {
                        query.AddExpression("JobId", QueryOperator.Within, filterCriterion[FilterOptions.JobId].ToArray());
                        query.AddConjunction(Conjunction.And);
                    }
                }
            }

            queryCollection.AddExpression("Id", QueryOperator.Within, rcIDs.ToArray());
            query.AddExpression("ResultSummaryCollection", QueryOperator.Equals, queryCollection);
            return query;
        }
        /// <summary>
        /// Creates query object for simple fields - the fields which can be queried directly, not the one's having (+) or "has" some sub values.
        /// </summary>
        /// <param name="filterCriterion">List of filter options</param>
        /// <param name="query">Wtt query object</param>
        /// <param name="key">Particular filter value</param>
        /// <param name="fieldValueName">Name of the field to query results on</param>
        /// <param name="queryOperator">WTT Query operator - e.g. Equals, Contains, etc.</param>
        private static void AddQueryExpression(Dictionary<FilterOptions, List<string>> filterCriterion,
            Query query, FilterOptions key, string fieldValueName, QueryOperator queryOperator = QueryOperator.Equals)
        {
            if (filterCriterion[key].Count > 0)
            {
                query.BeginGroup();
                foreach (var value in filterCriterion[key])
                {
                    query.AddExpression(fieldValueName, queryOperator, value);
                    if (filterCriterion[key].IndexOf(value) != filterCriterion[key].Count - 1)
                    {
                        query.AddConjunction(Conjunction.Or);
                    }
                }
                query.EndGroup();
                query.AddConjunction(Conjunction.And);
            }
        }
        /// <summary>
        /// Creates query object for complex fields - the fields which can not be queried directly. The one's having (+) or "has" some sub values.
        /// </summary>
        /// <param name="filterCriterion">List of filter options</param>
        /// <param name="query">Wtt query object</param>
        /// <param name="key">Particular filter value</param>
        /// <param name="fieldCollectionName">Name of the collection field - e.g. Job (+), Machine Configuration Values (+)</param>
        /// <param name="fieldValueName">Name of the sub field to query results on</param>
        /// <param name="queryOperator">WTT Query operator - e.g. Equals, Contains, etc.</param>
        private static void AddQueryExpression(Dictionary<FilterOptions, List<string>> filterCriterion,
           Query query, FilterOptions key, string fieldCollectionName, string fieldValueName, QueryOperator queryOperator = QueryOperator.Equals)
        {
            if (filterCriterion[key].Count > 0)
            {
                QueryStatement statement = new QueryStatement();
                foreach (var value in filterCriterion[key])
                {
                    statement.AddExpression(fieldValueName, queryOperator, value);
                    if (filterCriterion[key].IndexOf(value) != filterCriterion[key].Count - 1)
                    {
                        statement.AddConjunction(Conjunction.Or);
                    }
                }
                if (statement.QueryElementList.Count > 0)
                {
                    query.AddExpression(fieldCollectionName, QueryOperator.Equals, statement);
                    query.AddConjunction(Conjunction.And);
                }
            }
        }
        /// <summary>
        /// Creates query object for Result State.
        /// </summary>
        /// <param name="filterCriterion">List of filter options</param>
        /// <param name="query">Wtt query object</param>
        /// <param name="key">Particular filter value</param>
        private static void AddQueryExpressionForState(Dictionary<FilterOptions, List<string>> filterCriterion,
           Query query, FilterOptions key)
        {
            if (filterCriterion[key].Count > 0)
            {
                query.BeginGroup();
                foreach (var value in filterCriterion[key])
                {
                    switch (value)
                    {
                        case "Pass":
                            query.AddExpression(value, QueryOperator.Greater, 0);
                            query.AddConjunction(Conjunction.And);
                            query.AddExpression("Fail", QueryOperator.Equals, 0);
                            break;
                        case "Fail":
                            query.AddExpression(value, QueryOperator.Greater, 0);
                            break;
                        case "NotRun":
                            query.AddExpression(value, QueryOperator.Greater, 0);
                            query.AddConjunction(Conjunction.And);
                            query.AddExpression("Fail", QueryOperator.Equals, 0);
                            query.AddConjunction(Conjunction.And);
                            query.AddExpression("Pass", QueryOperator.Equals, 0);
                            query.AddConjunction(Conjunction.And);
                            query.AddExpression("NotApplicable", QueryOperator.Equals, 0);
                            break;
                        case "NotApplicable":
                            query.AddExpression(value, QueryOperator.Greater, 0);
                            query.AddConjunction(Conjunction.And);
                            query.AddExpression("Fail", QueryOperator.Equals, 0);
                            query.AddConjunction(Conjunction.And);
                            query.AddExpression("Pass", QueryOperator.Equals, 0);
                            query.AddConjunction(Conjunction.And);
                            query.AddExpression("NotRun", QueryOperator.Equals, 0);
                            break;
                    }

                    if (filterCriterion[key].IndexOf(value) != filterCriterion[key].Count - 1)
                    {
                        query.AddConjunction(Conjunction.Or);
                    }
                }
                query.EndGroup();
                query.AddConjunction(Conjunction.And);
            }
        }
        /// <summary>
        /// Creates query object for simple fields - the fields which can be queried directly, not the one's having (+) or "has" some sub values.
        /// </summary>
        /// <param name="filterCriterion">List of filter options</param>
        /// <param name="query">Wtt query object</param>
        /// <param name="key">Particular filter value</param>
        /// <param name="fieldValueName">Name of the field to query results on</param>
        /// <param name="queryOperator">WTT Query operator - e.g. Equals, Contains, etc.</param>
        private static void AddProductQueryExpression(Dictionary<FilterOptions, List<string>> filterCriterion,
             Query query, FilterOptions key, string outerFieldCollectionName, QueryOperator joiningQueryOperator, string innerFieldCollectionName, string fieldValueName, QueryOperator queryOperator = QueryOperator.Equals)
        {
            foreach (var value in filterCriterion[key])
            {
                QueryStatement jobQuery = new QueryStatement();
                QueryStatement featureQuery = new QueryStatement();
                featureQuery.AddExpression(fieldValueName, QueryOperator.BeginsWith, value);
                jobQuery.AddExpression(innerFieldCollectionName, QueryOperator.Has, featureQuery);
                query.AddExpression(outerFieldCollectionName, QueryOperator.Has, jobQuery);
                if (filterCriterion[key].IndexOf(value) != filterCriterion[key].Count - 1)
                {
                    query.AddConjunction(Conjunction.Or);
                }
                else
                    query.AddConjunction(Conjunction.And);
            }
        }

        /// <summary>
        /// Get the result collections using a WTT query file.
        /// </summary>
        /// <example>
        /// <code>
        /// ResultSummaryCollection rc = ResultCollectionHelper.GetResultCollectionSummary("OSGThreshold", "D:\\myWTQ.wtq");
        /// </code>
        /// </example>
        /// <param name="filePath">Full path and filename of WTT query file.</param>
        /// <param name="ds">Datastore Object</param>
        /// <returns>A collection of result collections returned from the WTT query.</returns>
        private static ResultSummaryCollection GetResultCollection(XmlReader streamReader, DataStore ds)
        {
            try
            {
                Query query = null;

                if (streamReader != null)
                {

                    XPathDocument wtqFile = new XPathDocument(streamReader);
                    XPathNavigator nav = wtqFile.CreateNavigator();

                    // Try to select the node for the OM Query subtree
                    XPathNodeIterator it = nav.Select(@"//Query");

                    if (it.Count > 0)
                    {
                        // Create an OM query object from the XML text
                        it.MoveNext();
                        query = new Query();
                        query.SetXml(it.Current.OuterXml);
                    }
                    else
                    {
                        // Try to select the node for the UI Query subtree
                        it = nav.Select(@"//ObjectQueryBuilder");
                        if (it.Count > 0)
                        {
                            // Create a UI QueryBuilder object from the XML text
                            it.MoveNext();
                            XmlDocument uiQueryDoc = new XmlDocument();
                            uiQueryDoc.Load(it.Current.ReadSubtree());
                            Microsoft.DistributedAutomation.UI.QueryBuilder queryBuilder = new Microsoft.DistributedAutomation.UI.QueryBuilder();

                            queryBuilder.DataStore = ds;
                            queryBuilder.PersistentData = uiQueryDoc;
                            // Clone the UI QueryBuilder's OM Query object
                            query = queryBuilder.Query.Clone();
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

                ResultSummaryCollection resultCollection = (ResultSummaryCollection)ds.Query(query, true);
                if (resultCollection.Count > 0)
                    return resultCollection;
                return null;
            }
            finally
            {
                streamReader.Dispose();
            }

        }



    }
}
