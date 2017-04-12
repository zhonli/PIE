//----------------------------------------------------------------------------------------------------------------------
// <copyright file="WTTBase.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>Status of the request</summary>
//----------------------------------------------------------------------------------------------------------------------
using Microsoft.DistributedAutomation;
using System.Configuration;

namespace PIEM.ExternalService.WTT
{
    public abstract class WTTBase
    {
        #region Private Members
        private const string DATASTORE_JOBS = "1Windows";
        private const string DATASTORE_MACHINES = "OSGExecutionDS";
        private const string RESOURCE_SERVICE_NAME = "Resource";
        private const string JOBS_SERVICE_NAME = "JobsRuntime";
        private const string CONFIG_FILE_NAME = "AthenaCore.dll.config";
        private const string CONFIG_SETTING_WTTSERVERNAME = "WTTServerName";
        private const string CONFIG_SETTING_WTT_DBNAME = "WTTDBName";

        protected static DataStore jobServiceDataStore = null;
        protected static DataStore resourceServiceDataStore = null;
        private static string wttDBName = string.Empty;
        private static string wttServerName = string.Empty;
        #endregion

        #region Properties
        public static string WTTServerName
        {
            get
            {
                if (!string.IsNullOrEmpty(wttServerName))
                {
                    return wttServerName;
                }

                //Read the config from the app domain first
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[CONFIG_SETTING_WTTSERVERNAME]))
                {
                    wttServerName = ConfigurationManager.AppSettings[CONFIG_SETTING_WTTSERVERNAME];
                    return wttServerName;
                }
                return wttServerName;
            }
        }

        public static string WTTDBName
        {
            get
            {
                if (!string.IsNullOrEmpty(wttDBName))
                {
                    return wttDBName;
                }

                //Read the config from the app domain first
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings[CONFIG_SETTING_WTT_DBNAME]))
                {
                    wttDBName = ConfigurationManager.AppSettings[CONFIG_SETTING_WTT_DBNAME];
                    return wttDBName;
                }
                return wttDBName;
            }
        }

        protected static DataStore ResourceServiceDataStore
        {
            get
            {
                if (resourceServiceDataStore == null)
                {
                    SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTBase.WTTServerName, WTTBase.WTTDBName);
                    resourceServiceDataStore = Enterprise.Connect(DATASTORE_MACHINES, JOBS_SERVICE_NAME, connectInfo);
                }
                return resourceServiceDataStore;
            }
        }

        protected static DataStore JobServiceDataStore
        {
            get
            {
                if (jobServiceDataStore == null)
                {
                    SqlIdentityConnectInfo connectInfo = new SqlIdentityConnectInfo(WTTBase.WTTServerName, WTTBase.WTTDBName);
                    jobServiceDataStore = Enterprise.Connect(DATASTORE_JOBS, JOBS_SERVICE_NAME, connectInfo);
                }
                return jobServiceDataStore;
            }
        }
        #endregion

    }
}
