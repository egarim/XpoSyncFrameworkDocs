using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using DevExpress.ExpressApp;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoApp.Mobile.Infrastructure
{
    public abstract class SyncFrameworkViewModel : ViewModelBase
    {
        private IObjectSpace objectSpace;
        private ISyncHelperXafSecured syncHelperXaf;
        private ISyncHelperConfigurationXafSecured syncHelperConfiguration;
        private IPageDialogService dialogService;
        public IObjectSpace ObjectSpace { get => objectSpace; set => objectSpace = value; }
        public ISyncHelperConfigurationXafSecured SyncHelperConfiguration { get => syncHelperConfiguration; set => syncHelperConfiguration = value; }
        public ISyncHelperXafSecured SyncHelperXaf { get => syncHelperXaf; set => syncHelperXaf = value; }
        public IPageDialogService DialogService { get => dialogService; set => dialogService = value; }

        public SyncFrameworkViewModel(INavigationService navigationService, ISyncHelperXafSecured syncHelperXafSecured, ISyncHelperConfigurationXafSecured configurationXafSecured, IPageDialogService dialogService)
            : base(navigationService)
        {
            this.SyncHelperConfiguration = configurationXafSecured;
            this.SyncHelperXaf = syncHelperXafSecured;
            this.DialogService = dialogService;
            ObjectSpace = SyncHelperXaf.CreateObjectSpace();
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}
