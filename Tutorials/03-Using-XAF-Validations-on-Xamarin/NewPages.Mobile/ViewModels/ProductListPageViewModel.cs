using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using NewPages.Mobile.Infrastructure;
using NewPages.Module.BusinessObjects;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NewPages.Mobile.ViewModels
{
    public class ProductListPageViewModel : ListPageGenericViewModel<Product>
    {
        public ProductListPageViewModel(INavigationService navigationService, ISyncHelperXafSecured syncHelperXafSecured, ISyncHelperConfigurationXafSecured configurationXafSecured, IPageDialogService dialogService) : base(navigationService, syncHelperXafSecured, configurationXafSecured, dialogService)
        {

        }

        protected override string GetFormPagePath()
        {
            return "ProductFormPage";
        }
    }
}
