using BIT.Data.Functions.RestClientNet;
using BIT.Xpo.Providers.OfflineDataSync;
using BIT.Xpo.Providers.OfflineDataSync.NetworkExtensions;
using BIT.Xpo.Providers.OfflineDataSync.NetworkExtensions.Configuration;
using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using XafXamarinCustomAuth.Module.BusinessObjects;

namespace XafXamarinCustomAuth.Mobile
{
    public class LocalSyncHelperXafSecured : SyncHelperXafSecured
    {
        public LocalSyncHelperXafSecured(string connectionString, string SyncUrl, Type[] Types, Type UserType, Type RoleType) : base(connectionString, SyncUrl, Types, UserType, RoleType)
        {

        }

        public LocalSyncHelperXafSecured(string connectionString, string SyncUrl, Func<Exception, ISyncDataStore, ApiFunction, List<BIT.Data.Sync.Delta>, bool> ExceptionFunction, Type[] Types, Type UserType, Type RoleType) : base(connectionString, SyncUrl, ExceptionFunction, Types, UserType, RoleType)
        {

        }

        public LocalSyncHelperXafSecured(ISyncDataStore dataStore, ISyncDataStoreServerConfiguration syncDataStoreServerConfiguration, Func<Exception, ISyncDataStore, ApiFunction, List<BIT.Data.Sync.Delta>, bool> ExceptionFunction, Type[] Types, Type UserType, Type RoleType) : base(dataStore, syncDataStoreServerConfiguration, ExceptionFunction, Types, UserType, RoleType)
        {

        }

        public LocalSyncHelperXafSecured(ISyncDataStore dataStore, ISyncDataStoreServerConfiguration syncDataStoreServerConfiguration, Type[] Types, Type UserType, Type RoleType) : base(dataStore, syncDataStoreServerConfiguration, Types, UserType, RoleType)
        {

        }

        public LocalSyncHelperXafSecured(ISyncHelperConfiguration syncHelperConfiguration, Type UserType, Type RoleType) : base(syncHelperConfiguration, UserType, RoleType)
        {

        }

        public LocalSyncHelperXafSecured(ISyncHelperConfigurationXafSecured syncHelperConfiguration) : base(syncHelperConfiguration)
        {

        }

        public LocalSyncHelperXafSecured(string connectionString, string SyncUrl, Type[] Types) : base(connectionString, SyncUrl, Types)
        {

        }

        public LocalSyncHelperXafSecured(string connectionString, string SyncUrl, Func<Exception, ISyncDataStore, ApiFunction, List<BIT.Data.Sync.Delta>, bool> ExceptionFunction, Type[] Types) : base(connectionString, SyncUrl, ExceptionFunction, Types)
        {

        }

        public LocalSyncHelperXafSecured(ISyncDataStore dataStore, ISyncDataStoreServerConfiguration syncDataStoreServerConfiguration, Func<Exception, ISyncDataStore, ApiFunction, List<BIT.Data.Sync.Delta>, bool> ExceptionFunction, Type[] Types) : base(dataStore, syncDataStoreServerConfiguration, ExceptionFunction, Types)
        {

        }

        public LocalSyncHelperXafSecured(ISyncDataStore dataStore, ISyncDataStoreServerConfiguration syncDataStoreServerConfiguration, Type[] Types) : base(dataStore, syncDataStoreServerConfiguration, Types)
        {

        }
        protected override AuthenticationBase GetAuthentication()
        {
            return new CustomAuthentication();
        }
        public override void Initialize()
        {
            base.Initialize();

            ISecurityStrategyBase securityStrategyBase = this.GetSecurityStrategy();
            ((SecurityStrategy)securityStrategyBase).AnonymousAllowedTypes.Add(typeof(Company));
            ((SecurityStrategy)securityStrategyBase).AnonymousAllowedTypes.Add(typeof(Employee));

        }
        public override void LogIn(string UserName, string Password, params object[] AdditionalParameters)
        {
            if (!Initialized)
            {
                throw new Exception("Please execute the initialize method before login");
            }


            IObjectSpace loginObjectSpace = AdditionalParameters[0] as IObjectSpace;
            Authentication.SetLogonParameters(GetLogonParameters(UserName, Password, AdditionalParameters));

            this.GetSecurityStrategy().Logon(loginObjectSpace);

        }
        protected override object GetLogonParameters(string login, string password, params object[] AdditionalParameters)
        {
            CustomLogonParameters customLogonParameters = new CustomLogonParameters();
            customLogonParameters.UserName = login;
            customLogonParameters.Password = password;
            customLogonParameters.Company = AdditionalParameters[1] as Company;
            customLogonParameters.Employee = AdditionalParameters[2] as Employee;
            return customLogonParameters;
        }






    }
}
