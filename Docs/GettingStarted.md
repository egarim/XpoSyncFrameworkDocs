# Getting Started: creating a XAF App with a Xamarin Forms Prism offline client

Create a new application by using the following command


```<language>
dotnet new XafXamarinOfflineSync --ios --android --name DemoApp
```


#### Adding business classes to your xamarin application

To add a business class to your XAF application just use the normal flow described in this link [implement custom business classes and reference properties xpo](https://docs.devexpress.com/eXpressAppFramework/402163/getting-started/in-depth-tutorial-blazor/business-model-design/business-model-design-with-xpo/implement-custom-business-classes-and-reference-properties-xpo) 

In a XAF application the business classes declared in your module are automatically discovered by the framework as explained [here](https://docs.devexpress.com/eXpressAppFramework/112847/business-model-design-orm/ways-to-add-a-business-class
),in your xamarin application you need to manually register your business objects as you can see here [App.xaml.cs](/DemoApp/DemoApp.Mobile/App.xaml.cs)

For example, take look to the following code, the [Product](/DemoApp/DemoApp.Module/BusinessObjects/Product.cs) type is a custom type defined in the Xaf agnostic module
```csharp

      Type[] GetPersistentTypes()
        {
            Type[] Types = new Type[] {
                typeof(Product),
                typeof(ApplicationUser),
                typeof(PermissionPolicyUser),
                typeof(PermissionPolicyRole),
                typeof(PermissionPolicyTypePermissionObject),
                typeof(PermissionPolicyObjectPermissionsObject),
                typeof(PermissionPolicyNavigationPermissionObject),
            };
            return Types;
        }
```


### Configuring the security system in your Xamarin Application

XAF security system requires a user type and a role type, you can see how that is configure in the following code snippets

In the [Windows Forms Application (WinApplication.Designer.cs) ](/DemoApp/DemoApp.Win/WinApplication.Designer.cs)

 ```csharp
            // securityStrategyComplex1
            // 
            this.securityStrategyComplex1.Authentication = this.authenticationStandard1;
            this.securityStrategyComplex1.RoleType = typeof(DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole);
            // ApplicationUser descends from PermissionPolicyUser and supports OAuth authentication. For more information, refer to the following help topic: https://docs.devexpress.com/eXpressAppFramework/402197
            // If your application uses PermissionPolicyUser or a custom user type, set the UserType property as follows:
            this.securityStrategyComplex1.UserType = typeof(DemoApp.Module.BusinessObjects.ApplicationUser);
            // 
            // securityModule1
            // 

```


In the [Blazor Application (Startup.cs) ](/DemoApp/DemoApp.Blazor.Server/Startup.cs)

 ```csharp
        public void ConfigureServices(IServiceCollection services) {
           
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();
            services.AddSingleton<XpoDataStoreProviderAccessor>();
            services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
            services.AddXaf<DemoAppBlazorApplication>(Configuration);
            services.AddXafReporting();
            services.AddXafSecurity(options => {
                options.RoleType = typeof(PermissionPolicyRole);
                // ApplicationUser descends from PermissionPolicyUser and supports OAuth authentication. For more information, refer to the following help topic: https://docs.devexpress.com/eXpressAppFramework/402197
                // If your application uses PermissionPolicyUser or a custom user type, set the UserType property as follows:
                options.UserType = typeof(DemoApp.Module.BusinessObjects.ApplicationUser);
                // ApplicationUserLoginInfo is only necessary for applications that use the ApplicationUser user type.
                // Comment out the following line if using PermissionPolicyUser or a custom user type.
                options.UserLoginInfoType = typeof(DemoApp.Module.BusinessObjects.ApplicationUserLoginInfo);
                options.Events.OnSecurityStrategyCreated = securityStrategy => ((SecurityStrategy)securityStrategy).RegisterXPOAdapterProviders();
                options.SupportNavigationPermissionsForTypes = false;
            }).AddExternalAuthentication<HttpContextPrincipalProvider>()
            .AddAuthenticationStandard(options => {
                options.IsSupportChangePassword = true;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                options.LoginPath = "/LoginPage";
            });
        }

```

So in your xamarin applcation you can configure your user and role types by changing the return type in the following methods
in the [Xamarin App (App.xaml.cs) ](/DemoApp/DemoApp.Mobile/App.xaml.cs)
 
```csharp
        
        Type GetUserType()
        {
            return typeof(ApplicationUser);
        }
        Type GetRoleType()
        {
            return typeof(PermissionPolicyRoleBase);
        }

```

#### How does all this work?

Data synchronization is possible by creating an instance of the helper class **SyncHelperXafSecured** that implements the following interfaces

```csharp

namespace BIT.Xpo.Providers.OfflineDataSync.NetworkExtensions
{
    public interface ISyncHelperXpo
    {
        void Initialize();
        UnitOfWork CreateUnitOfWork();
        string ConnectionString { get; }
        void Sync();
        void UpdateSchema();
        ISyncDataStore SyncDataStore { get; }
        ISyncDataStoreServerConfiguration SyncDataStoreServerConfiguration { get; }
        string SyncUrl { get; }
    }
}
namespace SyncFrameworkWebClient.Module.Extension
{
    public interface ISyncHelperXaf : ISyncHelperXpo
    {
        IObjectSpace CreateObjectSpace();
        void UpdateDataBase(Type ModuleUpdater,bool TrackModuleUpdater);
    }
}
namespace SyncFrameworkWebClient.Module.Extension
{
    public interface ISyncHelperXafSecured : ISyncHelperXaf
    {
        void LogIn(string UserName, string Password, params string[] AdditionalParameters);
        void Logoff();
        IObjectSpace CreateNonSecuredObjectSpace();
        bool Initialized { get; }
    }
}

```

And then registering the instance with [Prism Dependency Injection](https://prismlibrary.com/docs/dependency-injection/index.html) as shown in the following code snipet

```csharp

        private ISyncHelperConfigurationXafSecured GetHelperConfiguration()
        {
            return new SyncHelperConfigurationXafSecured(
                Preferences.Get(Constants.XpoSyncConnectionString, string.Empty), 
                GetPersistentTypes(),
                GetUserType(),
                GetRoleType(),
                Preferences.Get(Constants.ServerUrlKey, string.Empty),
                null);
            
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterInstance<IContainerRegistry>(containerRegistry);

            RegisterXafSyncHelper(containerRegistry);

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<SyncSettingsPage, SyncSettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<ProductListPage, ProductListPageViewModel>();
            containerRegistry.RegisterForNavigation<ProductFormPage, ProductFormPageViewModel>();
            
        }

        private void RegisterXafSyncHelper(IContainerRegistry containerRegistry)
        {
         
            var HelperConfiguration = GetHelperConfiguration();
            containerRegistry.RegisterInstance<ISyncHelperConfigurationXafSecured>(HelperConfiguration);
            if (!string.IsNullOrEmpty(HelperConfiguration.ConnectionString) && !string.IsNullOrEmpty(HelperConfiguration.ServerUrl))
            {
                var SyncHelperXafSecure = new SyncHelperXafSecured(HelperConfiguration.ConnectionString, HelperConfiguration.ServerUrl, HelperConfiguration.Types, HelperConfiguration.UserType, HelperConfiguration.RoleType);
                containerRegistry.RegisterInstance<ISyncHelperXafSecured>(SyncHelperXafSecure);
            }
        }

```

You can see the actual registration in your App in this file [App.xaml.cs](/DemoApp/DemoApp.Mobile/App.xaml.cs)

#### The infrastructure

This template provides the boilerplate code needed start a new application with synchronization capabilities in little to no time ))

Here is a list of the files that are important in the synchronization flow 

1. [LoginPageViewModel](/DemoApp/DemoApp.Mobile/ViewModels/LoginPageViewModel.cs): Provides the basic functionality to login using XAF security system
2. [SyncSettingsPageViewModel](/DemoApp/DemoApp.Mobile/ViewModels/SyncSettingsPageViewModel.cs): Allows you to store the configuration values needed to create an instance of SyncHelperXafSecured
3. [ViewModelBase](/DemoApp/DemoApp.Mobile/Infrastructure/ViewModelBase.cs): Prism basic view model functionality for navigation and data binding
4. [SyncFrameworkViewModel](/DemoApp/DemoApp.Mobile/Infrastructure/SyncFrameworkViewModel.cs): A view model that include all the services needed for a business appliation includes access to ISyncHelperXafSecured service
5. [ListPageGenericViewModel](/DemoApp/DemoApp.Mobile/Infrastructure/ListPageGenericViewModel.cs): A view model with the functionality to load a list of records and navigate to a different view passing data about the selected record
6. [FormPageViewModel](/DemoApp/DemoApp.Mobile/Infrastructure/FormPageViewModel.cs): This view model contains the boiler plate code for CRUD operations

**NOTE**: The use of the files described above is **NOT MANDATORY** you can provide your own implementations



