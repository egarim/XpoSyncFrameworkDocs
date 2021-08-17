using NewPages.Mobile.Infrastructure;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewPages.Mobile.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        IContainerRegistry containerRegistry;
        IContainerProvider containerProvider;
        IPageDialogService dialogService;
        public LoginPageViewModel(INavigationService navigationService, IContainerRegistry containerRegistry, IContainerProvider containerProvider, IPageDialogService dialogService) : base(navigationService)
        {
            this.containerRegistry = containerRegistry;
            this.containerProvider = containerProvider;
            this.dialogService= dialogService; 
        }

        private DelegateCommand _LoginCommand;
        public DelegateCommand LoginCommand =>
            _LoginCommand ?? (_LoginCommand = new DelegateCommand(ExecuteLoginCommand, CanExecuteLoginCommand));

        async void ExecuteLoginCommand()
        {
            try
            {
                var SyncHelper = containerProvider.Resolve<ISyncHelperXafSecured>();
                if (!SyncHelper.Initialized)
                {
                    SyncHelper.Initialize();
                    SyncHelper.UpdateSchema();
                }
                SyncHelper.LogIn(this.UserName, this.Password);

                await this.NavigationService.NavigateAsync("/MainPage/NavigationPage/HomePage");
            }
            catch (Exception ex)
            {

                await dialogService.DisplayAlertAsync("Invalid login", ex.Message, "OK");
            }
          
        }

        bool CanExecuteLoginCommand()
        {
                                                                   
            bool SyncHelperRegister = containerRegistry.IsRegistered<ISyncHelperXafSecured>();
            return SyncHelperRegister;
        }

        private DelegateCommand _SettingsCommand;
        public DelegateCommand SettingsCommand =>
            _SettingsCommand ?? (_SettingsCommand = new DelegateCommand(ExecuteSettingsCommand, CanExecuteSettingsCommand));

        async void ExecuteSettingsCommand()
        {
            await  this.NavigationService.NavigateAsync("SyncSettingsPage");
        }

        bool CanExecuteSettingsCommand()
        {
            return true;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoginCommand.RaiseCanExecuteChanged();
            SettingsCommand.RaiseCanExecuteChanged();
            base.OnNavigatedTo(parameters);
        }


        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { SetProperty(ref _UserName, value); }
        }
        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }
    }
}
