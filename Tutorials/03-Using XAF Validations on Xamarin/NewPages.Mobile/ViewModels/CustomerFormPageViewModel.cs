using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using DevExpress.Persistent.Validation;
using NewPages.Mobile.Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewPages.Mobile.ViewModels
{
    public class CustomerFormPageViewModel : FormPageViewModel
    {
        public CustomerFormPageViewModel(INavigationService navigationService, ISyncHelperXafSecured syncHelperXafSecured, ISyncHelperConfigurationXafSecured configurationXafSecured, IPageDialogService dialogService) : base(navigationService, syncHelperXafSecured, configurationXafSecured, dialogService)
        {

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
