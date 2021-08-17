using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using GettingStarted.Mobile.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;

namespace GettingStarted.Mobile.ViewModels
{
    public class HomePageViewModel : SyncFrameworkViewModel
    {
        public HomePageViewModel(INavigationService navigationService, ISyncHelperXafSecured syncHelperXafSecured, ISyncHelperConfigurationXafSecured configurationXafSecured, IPageDialogService dialogService) : base(navigationService, syncHelperXafSecured, configurationXafSecured, dialogService)
        {

        }


        string _LastSynchronization;
        public string LastSynchronization
        {
            get { return _LastSynchronization; }
            set { SetProperty(ref _LastSynchronization, value); }
        }
        string _Transactions;
        public string Transactions
        {
            get { return _Transactions; }
            set { SetProperty(ref _Transactions, value); }
        }
        string _Identity;
        public string Identity
        {
            get { return _Identity; }
            set { SetProperty(ref _Identity, value); }
        }
        string _ServerUrl;
        public string ServerUrl
        {
            get { return _ServerUrl; }
            set { SetProperty(ref _ServerUrl, value); }
        }
        #region SyncCommand
        private DelegateCommand _SyncCommand;

   
        public DelegateCommand SyncCommand =>
            _SyncCommand ?? (_SyncCommand = new DelegateCommand(ExecuteSyncCommand, CanExecuteSyncCommand));

        async void ExecuteSyncCommand()
        {

            try
            {
                this.SyncHelperXaf.Sync();
                string SyncDate = DateTime.Now.ToString(Constants.DateFormat);
                Preferences.Set(Constants.SyncDate, SyncDate);
                this.LastSynchronization = SyncDate;
                this.Transactions = this.SyncHelperXaf.SyncDataStore.GetDeltasToProcess().Count().ToString();
            }
            catch (Exception ex)
            {
                await DialogService.DisplayAlertAsync("Synchronization error", ex.Message, "OK");
            }

           
        }

        bool CanExecuteSyncCommand()
        {
            return true;
        }


        #endregion
        public override void OnNavigatedTo(INavigationParameters parameters)
        {

            base.OnNavigatedTo(parameters);
            this.Identity = Preferences.Get(Constants.IdentityKey, $"{DeviceInfo.Name}-{DeviceInfo.Model}-{DeviceInfo.Manufacturer}".Replace(" ", "-"));
            this.LastSynchronization = Preferences.Get(Constants.SyncDate, Constants.DateFormat);
            this.ServerUrl = Preferences.Get(Constants.ServerUrlKey, string.Empty);
            this.Transactions = this.SyncHelperXaf.SyncDataStore.GetDeltasToProcess().Count().ToString();


        }

    }
}
