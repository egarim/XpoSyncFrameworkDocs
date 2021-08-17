using BIT.Xpo.Providers.OfflineDataSync;
using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using GettingStarted.Mobile.Infrastructure;
using GettingStarted.Module.BusinessObjects;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Essentials;

namespace GettingStarted.Mobile.ViewModels
{
    public class SyncSettingsPageViewModel : ViewModelBase
    {
        IContainerRegistry containerRegistry;
        IContainerProvider containerProvider;
        IPageDialogService dialogService;
        public SyncSettingsPageViewModel(INavigationService navigationService, IContainerRegistry containerRegistry, IContainerProvider containerProvider,IPageDialogService dialogService) : base(navigationService)
        {
            this.containerRegistry = containerRegistry;
            this.containerProvider = containerProvider;
            this.dialogService = dialogService;
           
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            this.SaveSettingsCommand.RaiseCanExecuteChanged();
        }
     

 
        string _ServerUrl;
        public string ServerUrl
        {
            get { return _ServerUrl; }
            set { SetProperty(ref _ServerUrl, value); }
        }
        string _Identity;
        public string Identity
        {
            get { return _Identity; }
            set { SetProperty(ref _Identity, value); }
        }
        string _LastSynchronization;
        public string LastSynchronization
        {
            get { return _LastSynchronization; }
            set { SetProperty(ref _LastSynchronization, value); }
        }
        #region SaveCommand

        private DelegateCommand _SaveSettingsCommand;
        public DelegateCommand SaveSettingsCommand =>
            _SaveSettingsCommand ?? (_SaveSettingsCommand = new DelegateCommand(ExecuteSaveSettingsCommand, CanExecuteSaveSettingsCommand));

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

         
            var SyncHelperXafSecure = new SyncHelperXafSecured(Configuration);
            SyncHelperXafSecure.Initialize();
            SyncHelperXafSecure.UpdateSchema(false, false);
           

            containerRegistry.RegisterInstance<ISyncHelperXafSecured>(SyncHelperXafSecure);
            containerRegistry.Register<IObjectSpace>(() =>
            {
                return SyncHelperXafSecure.CreateObjectSpace();
            });
            this.SyncCommand.RaiseCanExecuteChanged();
        }

        bool CanExecuteSaveSettingsCommand()
        {
            return !string.IsNullOrEmpty(this.ServerUrl) && !string.IsNullOrEmpty(this.Identity);
        }

        #endregion

        #region SyncCommand
        private DelegateCommand _SyncCommand;
        public DelegateCommand SyncCommand =>
            _SyncCommand ?? (_SyncCommand = new DelegateCommand(ExecuteSyncCommand, CanExecuteSyncCommand));

        async void ExecuteSyncCommand()
        {
            try
            {
                var SyncHelper = containerProvider.Resolve<ISyncHelperXafSecured>();
                SyncHelper.Sync();
                string SyncDate = DateTime.Now.ToString(Constants.DateFormat);
                Preferences.Set(Constants.SyncDate, SyncDate);
                this.LastSynchronization = SyncDate;
            }
            catch (Exception ex)
            {
                await dialogService.DisplayAlertAsync("Synchronization error", ex.Message, "OK");
            }
          
        }

        bool CanExecuteSyncCommand()
        {
            bool SyncHelperRegister = containerRegistry.IsRegistered<ISyncHelperXafSecured>();
            return SyncHelperRegister;
        }


        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {

            this.ServerUrl=  Preferences.Get(Constants.ServerUrlKey, string.Empty);
            this.Identity=Preferences.Get(Constants.IdentityKey, $"{DeviceInfo.Name}-{DeviceInfo.Model}-{DeviceInfo.Manufacturer}".Replace(" ","-"));
            this.LastSynchronization = Preferences.Get(Constants.SyncDate, DateTime.MinValue.ToString(Constants.DateFormat));
            base.OnNavigatedTo(parameters);
        }

    }
}
