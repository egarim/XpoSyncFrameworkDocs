using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using NewPages.Mobile.Infrastructure;
using NewPages.Mobile.Views;
using NewPages.Module.BusinessObjects;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp.Security;

namespace NewPages.Mobile.ViewModels
{
    public class CustomerListPageViewModel : ListPageGenericViewModel<Customer>
    {
        public CustomerListPageViewModel(INavigationService navigationService, ISyncHelperXafSecured syncHelperXafSecured, ISyncHelperConfigurationXafSecured configurationXafSecured, IPageDialogService dialogService) : base(navigationService, syncHelperXafSecured, configurationXafSecured, dialogService)
        {

        }

        protected override string GetFormPagePath()
        {
            return nameof(CustomerFormPage);
        }
        protected override bool CanExecuteAddItemCommand()
        {
            SecurityStrategy SecurityStrategy = (this.SyncHelperXaf as SyncHelperXafSecured).GetSecurityStrategy() as SecurityStrategy;
            var CanCreateCustomer= SecurityStrategy.CanCreate<Customer>();
            return CanCreateCustomer;
        }
    }
}
