using XafXamarinCustomAuth.Mobile.Infrastructure;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using XafXamarinCustomAuth.Module.BusinessObjects;
using System.Linq;
using System.ComponentModel;

namespace XafXamarinCustomAuth.Mobile.ViewModels
{
    public class LoginPageViewModel : ViewModelBase,IInitialize
    {
        Employee selectedEmployee;
        Company selectedCompany;
        IContainerRegistry containerRegistry;
        IContainerProvider containerProvider;
        IPageDialogService dialogService;
        public LoginPageViewModel(INavigationService navigationService, IContainerRegistry containerRegistry, IContainerProvider containerProvider, IPageDialogService dialogService) : base(navigationService)
        {
            this.containerRegistry = containerRegistry;
            this.containerProvider = containerProvider;
            this.dialogService = dialogService;
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
                SyncHelper.LogIn(this.SelectedEmployee.UserName, this.Password, this.os, this.SelectedEmployee, SelectedCompany);

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
            await this.NavigationService.NavigateAsync("SyncSettingsPage");
        }

        bool CanExecuteSettingsCommand()
        {
            return true;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        ObservableCollection<Company> _Companies;
        public ObservableCollection<Company> Companies { get => _Companies; set => _Companies = value; }

        ObservableCollection<Employee> _Employees;
        public ObservableCollection<Employee> Employees { get => _Employees; set => _Employees = value; }

        public Company SelectedCompany
        {
            get => selectedCompany;
            set
            {
                if (selectedCompany == value)
                    return;
                selectedCompany = value;
                RaisePropertyChanged();
            }
        }
        
        public Employee SelectedEmployee
        {
            get => selectedEmployee;
            set
            {
                if (selectedEmployee == value)
                    return;
                selectedEmployee = value;
                RaisePropertyChanged();
            }
        }
        
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoginCommand.RaiseCanExecuteChanged();
            SettingsCommand.RaiseCanExecuteChanged();
         
            base.OnNavigatedTo(parameters);
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            if (containerRegistry.IsRegistered<ISyncHelperXafSecured>())
            {
                var SyncHelper = containerProvider.Resolve<ISyncHelperXafSecured>();
                if (!SyncHelper.Initialized)
                {
                    SyncHelper.Initialize();
                    SyncHelper.UpdateSchema();
                }
                os = SyncHelper.CreateObjectSpace();
                Employees = new ObservableCollection<Employee>(os.CreateCollection(typeof(Employee)).Cast<Employee>());
                Companies = new ObservableCollection<Company>(os.CreateCollection(typeof(Company)).Cast<Company>());
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Employees)));
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Companies)));
            }
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            if(args?.PropertyName==nameof(SelectedCompany))
            {
                this.Employees = new ObservableCollection<Employee>(this.SelectedCompany.Employees);
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Employees)));
            }
            if (args?.PropertyName == nameof(SelectedCompany) && this.SelectedCompany==null)
            {
                this.Employees =null;
            }
        }
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { SetProperty(ref _UserName, value); }
        }
        private string _Password;
        DevExpress.ExpressApp.IObjectSpace os;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }
    }
}
