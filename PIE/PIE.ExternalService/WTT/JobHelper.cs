//----------------------------------------------------------------------------------------------------------------------
// <copyright file="JobHelper.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>A wrapper of WTT object model to manage WTT jobs.</summary>
//----------------------------------------------------------------------------------------------------------------------

using Microsoft.DistributedAutomation;
using Microsoft.DistributedAutomation.Jobs;
using System;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PIEM.ExternalService.WTT
{
    public class JobHelper : WTTBase
    {
        private const string QUERY_ID = "Id";
        protected static JobsDefinitionDataStore m_JobsDataStore = null;
        protected static JobsRuntimeDataStore m_JobsRuntimeDataStore = null;

        #region Public Methods
        /// <summary>
        /// Check Whether the WTT Job Exists
        /// </summary>
        /// <param name="JobId">WTT Job Id</param>
        /// <param name="datastore">WTT Job Datastore</param>
        /// <returns>True or False</returns>
        public static bool DoesJobExists(string JobId, string datastore)
        {
            JobCollection result = getJobById(JobId, datastore);
            if (result == null)
                 return false;
            return (result.Count > 0);
        }
        /// <summary>
        /// Get WTT Job By ID
        /// </summary>
        /// <param name="JobId">WTT job Id</param>
        /// <param name="datastore">WTT Job Datastore</param>
        /// <returns>WTT Job Collection</returns>
        public static JobCollection getJobById(string JobId, string datastore)
        {
            SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTServerName, WTTDBName);
            using (DataStore ds = Enterprise.Connect(datastore, JobsRuntimeDataStore.ServiceName, connectInfo))
            {
                int retryCount = 3;
                JobCollection result = null;
                do
                {
                    try
                    {
                        retryCount--;
                        Query jobQuery = new Query(typeof(Microsoft.DistributedAutomation.Jobs.Job));
                        jobQuery.AddExpression("Id", QueryOperator.Equals, JobId);
                        result = (JobCollection)ds.Query(jobQuery);
                    }
                    catch (ConfigurationErrorsException confEx)
                    {
                        throw confEx;
                    }
                    catch (Exception ex)
                    {
                        if(retryCount==0)
                            throw ex;
                    }
                }

                while ((result == null) && (retryCount > 0));
                return result;
            }

        }

        /// <summary>
        /// Get Job Id by WTQ File
        /// </summary>
        /// <param name="filePath">WTQ File</param>
        /// <param name="datastore">Job Datastore</param>
        /// <returns>WTT Job Collection</returns>
        public static JobCollection GetJobsFromQueryFile(string filePath, string datastore)
        {
            System.Xml.Linq.XDocument xdoc = XDocument.Load(filePath);
            JobCollection jobCollection = GetJobCollection(xdoc.CreateReader(), datastore);
            return jobCollection;
        }
        /// <summary>
        /// Get Datastore from WTQ file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>WTT Datastore</returns>
        public static string getDatastoreFromQueryFile(string filePath)
        {
            string datastore = null;
            System.Xml.Linq.XDocument xdoc = XDocument.Load(filePath);
            datastore = getDatastore(xdoc.CreateReader());
            return datastore;
        }
        #endregion

        #region Private Methods
        private static bool checkDatastore(XmlReader xr, string datastore)
        {
            string definitionController = null;
            while (xr.Read())
            {
                if (xr.Name == "Data")
                {
                    definitionController = xr.GetAttribute("DefinitionController");
                }
            }
            if (datastore != definitionController)
            {
                return false;
            }
            return true;
        }
        private static string getDatastore(XmlReader xr)
        {
            string definitionController = null;
            while (xr.Read())
            {
                if (xr.Name == "Data")
                {
                    definitionController = xr.GetAttribute("DefinitionController");
                }
            }
            return definitionController;
        }
        private static JobCollection GetJobCollection(XmlReader streamReader, string datastore)
        {
            SqlIdentityConnectInfo sqlIdentityConnectInfo = new SqlIdentityConnectInfo(WTTServerName, WTTDBName);
            JobsDefinitionDataStore jobDS = (JobsRuntimeDataStore)(Enterprise.Connect(datastore, typeof(JobsRuntimeDataStore), sqlIdentityConnectInfo));
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
                            queryBuilder.DataStore = jobDS;
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

                JobCollection jobCollection = (JobCollection)jobDS.Query(query, true);
                if (jobCollection.Count > 0)
                    return jobCollection;
                return null;
            }
            finally
            {
                streamReader.Dispose();
            }

        }
        #endregion
    }

}
