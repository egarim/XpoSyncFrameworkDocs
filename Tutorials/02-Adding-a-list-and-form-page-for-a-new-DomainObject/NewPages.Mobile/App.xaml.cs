using BIT.Xpo.Providers.OfflineDataSync;
using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo.DB;
using NewPages.Mobile.Infrastructure;
using NewPages.Mobile.ViewModels;
using NewPages.Mobile.Views;
using NewPages.Module.BusinessObjects;
using Prism;
using Prism.Ioc;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Diagnostics;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace NewPages.Mobile
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
           
        }
   
        protected override async void OnInitialized()
        {
            InitializeComponent();
            //HACK we need to let xaf know that we are not using .net application configurations
            Tracing.UseConfigurationManager = false;
            await NavigationService.NavigateAsync("NavigationPage/LoginPage");
        }
        ISyncHelperConfigurationXafSecured GetHelperConfiguration()
        {
            return new SyncHelperConfigurationXafSecured(
                Preferences.Get(Constants.XpoSyncConnectionString, string.Empty), 
                GetPersistentTypes(),
                GetUserType(),
                GetRoleType(),
                Preferences.Get(Constants.ServerUrlKey, string.Empty),
                null);
            
        }
        Type GetUserType()
        {
            return typeof(ApplicationUser);
        }
        Type GetRoleType()
        {
            return typeof(PermissionPolicyRoleBase);
        }
        Type[] GetPersistentTypes()
        {
            Type[] Types = new Type[] {
                typeof(Product),
                typeof(Customer),
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

            containerRegistry.RegisterForNavigation<CustomerListPage, CustomerListPageViewModel>();
            containerRegistry.RegisterForNavigation<CustomerFormPage, CustomerFormPageViewModel>();
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

        private static void EnableXpoDebugLog()
        {
            FieldInfo xpoSwitchF = typeof(ConnectionProviderSql).GetField("xpoSwitch", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            TraceSwitch xpoSwitch = (TraceSwitch)xpoSwitchF.GetValue(null);
            xpoSwitch.Level = TraceLevel.Info;
        }
    }
}
