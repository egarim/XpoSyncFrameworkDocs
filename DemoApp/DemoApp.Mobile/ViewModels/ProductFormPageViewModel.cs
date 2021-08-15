using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using DemoApp.Mobile.Infrastructure;
using DemoApp.Module.BusinessObjects;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoApp.Mobile.ViewModels
{
    public class ProductFormPageViewModel : FormPageViewModel
    {
        public ProductFormPageViewModel(INavigationService navigationService, ISyncHelperXafSecured syncHelperXafSecured, ISyncHelperConfigurationXafSecured configurationXafSecured, IPageDialogService dialogService) : base(navigationService, syncHelperXafSecured, configurationXafSecured, dialogService)
        {

        }
    }
}
