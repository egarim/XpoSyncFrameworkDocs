using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using UsingExistingDb.Mobile.Infrastructure;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Navigation;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsingExistingDb.Mobile.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        IContainerProvider containerProvider;

        #region LogoutCommand
        private DelegateCommand _LogoutCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="!:AppMapViewModelBase"/> class.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <exception cref="System.ArgumentNullException">navigationService</exception>
        protected MainPageViewModel(INavigationService navigationService, IContainerProvider containerProvider) : base(navigationService)
        {
            this.containerProvider = containerProvider;
        }

        public DelegateCommand LogoutCommand =>
            _LogoutCommand ?? (_LogoutCommand = new DelegateCommand(ExecuteLogoutCommand, CanExecuteLogoutCommand));

        async void ExecuteLogoutCommand()
        {
            var SyncHelper= this.containerProvider.Resolve<ISyncHelperXafSecured>();
            SyncHelper.Logoff();
            await this.NavigationService.NavigateAsync("/LoginPage");
        }

        bool CanExecuteLogoutCommand()
        {
            return true;
        }
        #endregion
        




    }
}
