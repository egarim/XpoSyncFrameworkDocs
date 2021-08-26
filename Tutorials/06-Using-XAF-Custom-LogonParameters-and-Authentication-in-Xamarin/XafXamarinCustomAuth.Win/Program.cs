using System;
using System.Configuration;
using System.Windows.Forms;
using BIT.Xpo.Providers.OfflineDataSync;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraEditors;
using XafXamarinCustomAuth.Module.BusinessObjects;

namespace XafXamarinCustomAuth.Win {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            //HACK register SyncDataStore
            SyncDataStore.Register();
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.Latest;
            WindowsFormsSettings.LoadApplicationSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
			DevExpress.Utils.ToolTipController.DefaultController.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            if(Tracing.GetFileLocationFromSettings() == DevExpress.Persistent.Base.FileLocation.CurrentUserApplicationDataFolder) {
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            }
            Tracing.Initialize();
            XafXamarinCustomAuthWindowsFormsApplication winApplication = new XafXamarinCustomAuthWindowsFormsApplication();
            winApplication.GetSecurityStrategy().RegisterXPOAdapterProviders();
            if(ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
            try {
                winApplication.CreateCustomLogonWindowObjectSpace +=
                application_CreateCustomLogonWindowObjectSpace;
                winApplication.Setup();
                winApplication.Start();
            }
            catch(Exception e) {
                winApplication.StopSplash();
                winApplication.HandleException(e);
            }
        }
        private static void application_CreateCustomLogonWindowObjectSpace(object sender,
    CreateCustomLogonWindowObjectSpaceEventArgs e)
        {
            e.ObjectSpace = ((XafApplication)sender).CreateObjectSpace(typeof(CustomLogonParameters));
            NonPersistentObjectSpace nonPersistentObjectSpace = e.ObjectSpace as NonPersistentObjectSpace;
            if (nonPersistentObjectSpace != null)
            {
                if (!nonPersistentObjectSpace.IsKnownType(typeof(Company), true))
                {
                    IObjectSpace additionalObjectSpace = ((XafApplication)sender).CreateObjectSpace(typeof(Company));
                    nonPersistentObjectSpace.AdditionalObjectSpaces.Add(additionalObjectSpace);
                    nonPersistentObjectSpace.Disposed += (s2, e2) => {
                        additionalObjectSpace.Dispose();
                    };
                }
            }
        ((CustomLogonParameters)e.LogonParameters).RefreshPersistentObjects(e.ObjectSpace);
        }
    }
}
