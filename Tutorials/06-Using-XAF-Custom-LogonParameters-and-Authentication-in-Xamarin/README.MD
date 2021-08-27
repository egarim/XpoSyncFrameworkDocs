# 06 Using XAF Custom LogonParameters and Authentication in Xamarin

This example is based on the official DevExpress example on the following link
https://docs.devexpress.com/eXpressAppFramework/112982/data-security-and-safety/security-system/task-based-help/how-to-use-custom-logon-parameters-and-authentication

This example uses a modify version of SyncHelperXafSecured you can take a look to the implementation [here](XafXamarinCustomAuth.Mobile/LocalSyncHelperXafSecured.cs)

```csharp
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


```

The types Employee and Company should be registered in the mobile app [App](XafXamarinCustomAuth.Mobile/App.xaml.cs)

```csharp
        Type[] GetPersistentTypes()
        {
            Type[] Types = new Type[] {
                typeof(Employee),
                typeof(Company),
                typeof(Product),
                typeof(ApplicationUser),
                typeof(PermissionPolicyUser),
                typeof(PermissionPolicyRole),
                typeof(PermissionPolicyTypePermissionObject),
                typeof(PermissionPolicyObjectPermissionsObject),
                typeof(PermissionPolicyNavigationPermissionObject),
                typeof(ModelDifference),
                typeof(ModelDifferenceAspect)
            };
            return Types;
        }
```

the registration of the interface ISyncHelperXafSecured now uses the custom LocalSyncHelperXafSecured class
you can check the registration [here](XafXamarinCustomAuth.Mobile/App.xaml.cs)
```csharp

        private void RegisterXafSyncHelper(IContainerRegistry containerRegistry)
        {
         
            var HelperConfiguration = GetHelperConfiguration();
            containerRegistry.RegisterInstance<ISyncHelperConfigurationXafSecured>(HelperConfiguration);
            if (!string.IsNullOrEmpty(HelperConfiguration.ConnectionString) && !string.IsNullOrEmpty(HelperConfiguration.ServerUrl))
            {
                var SyncHelperXafSecure = new LocalSyncHelperXafSecured(HelperConfiguration.ConnectionString, HelperConfiguration.ServerUrl, HelperConfiguration.Types, HelperConfiguration.UserType, HelperConfiguration.RoleType);
                containerRegistry.RegisterInstance<ISyncHelperXafSecured>(SyncHelperXafSecure);
            }
           
        }

```

the registration of the interface ISyncHelperXafSecured in the [SyncSettingsPageViewModel](XafXamarinCustomAuth.Mobile/ViewModels/SyncSettingsPageViewModel.cs)

```csharp

     void ExecuteSaveSettingsCommand()
        {


            var Configuration=   containerProvider.Resolve<ISyncHelperConfigurationXafSecured>();
            string dataConnectionString = SyncHelperXafSecured.GetSqliteConnectionString("Data");
            string deltaConnectionString = SyncHelperXafSecured.GetSqliteConnectionString("Delta");
            var cnx = SyncDataStore.GetConnectionString(dataConnectionString, deltaConnectionString, this.Identity, "");

            Preferences.Set(Constants.XpoSyncConnectionString, cnx);
            Preferences.Set(Constants.ServerUrlKey, ServerUrl);
            Preferences.Set(Constants.IdentityKey, Identity);

            Configuration.ConnectionString = cnx;
            Configuration.ServerUrl = ServerUrl;

         
            var SyncHelperXafSecure = new LocalSyncHelperXafSecured(Configuration);
            SyncHelperXafSecure.Initialize();
            SyncHelperXafSecure.UpdateSchema(false, false);
           

            containerRegistry.RegisterInstance<ISyncHelperXafSecured>(SyncHelperXafSecure);
            this.SyncCommand.RaiseCanExecuteChanged();
        }
```

There are some changes in  [LoginPageViewModel](XafXamarinCustomAuth.Mobile/ViewModels/LoginPageViewModel.cs) to present a list of
companies and employees, same kind of changes are implemented in the view [LoginPage](XafXamarinCustomAuth.Mobile/Views/LoginPage.xaml)
