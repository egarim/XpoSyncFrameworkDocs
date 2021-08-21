using UsingExistingDb.Mobile.Infrastructure;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using UsingExistingDb.Module.BusinessObjects;
using System.Linq;
using System.Diagnostics;

namespace UsingExistingDb.Mobile.ViewModels
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
              

                //TODO This code is just to make sure the database is readable by XPO/XAF, you can remove it in production
                try
                {
                    var Users = SyncHelper.CreateNonSecuredObjectSpace().CreateCollection(typeof(ApplicationUser)).Cast<ApplicationUser>().ToList();
                    var Admin = Users.FirstOrDefault(u => u.UserName == "Admin");
                    var RolesCount = Admin.Roles.Count;
                    foreach (DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole permissionPolicyRole in Admin.Roles)
                    {
                        Debug.WriteLine(permissionPolicyRole.Name);
                        Debug.WriteLine(permissionPolicyRole.IsAdministrative);
                    }
                    var LoginValid = Admin.ComparePassword(this.Password);
                }
                catch (Exception ex)
                {
                    var message = $"There is a problem with the initial data{ex.Message}";
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
