using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using GettingStarted.Mobile.Infrastructure;
using GettingStarted.Module.BusinessObjects;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GettingStarted.Mobile.ViewModels
{
    public class ProductFormPageViewModel : FormPageViewModel
    {
        public ProductFormPageViewModel(INavigationService navigationService, ISyncHelperXafSecured syncHelperXafSecured, ISyncHelperConfigurationXafSecured configurationXafSecured, IPageDialogService dialogService) : base(navigationService, syncHelperXafSecured, configurationXafSecured, dialogService)
        {

        }
    }
}
