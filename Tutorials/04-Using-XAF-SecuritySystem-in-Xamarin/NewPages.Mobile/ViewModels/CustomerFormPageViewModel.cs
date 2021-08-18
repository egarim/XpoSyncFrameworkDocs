using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using DevExpress.Persistent.Validation;
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
using System.Text;
using DevExpress.ExpressApp.Security;
namespace NewPages.Mobile.ViewModels
{
    public class CustomerFormPageViewModel : FormPageViewModel
    {
        public CustomerFormPageViewModel(INavigationService navigationService, ISyncHelperXafSecured syncHelperXafSecured, ISyncHelperConfigurationXafSecured configurationXafSecured, IPageDialogService dialogService) : base(navigationService, syncHelperXafSecured, configurationXafSecured, dialogService)
        {

        }
        protected override bool CanExecuteDeleteCommand()
        {
            var SecurityStrategy = (this.SyncHelperXaf as SyncHelperXafSecured).GetSecurityStrategy() as SecurityStrategy;
            var CanDeleteCustomer = SecurityStrategy.CanDelete<Customer>();
            return CanDeleteCustomer;

            //return base.CanExecuteDeleteCommand();
        }
        protected async override void ExecuteSaveCommand()
        {

            var ValidationResult = Validator.RuleSet.ValidateTarget(this.ObjectSpace, this.CurrentObject, DefaultContexts.Save);

            StringBuilder stringBuilder = new StringBuilder();
            foreach (RuleSetValidationResultItem ruleSetValidationResultItem in ValidationResult.Results)
            {


                stringBuilder.AppendLine(ruleSetValidationResultItem.ErrorMessage);
            }
           
            if (ValidationResult.ValidationOutcome== ValidationOutcome.Error)
            {

                await this.DialogService.DisplayAlertAsync("Validation Error", stringBuilder.ToString(), "OK");
                return;
            }

            
            base.ExecuteSaveCommand();
        }
    }
}
