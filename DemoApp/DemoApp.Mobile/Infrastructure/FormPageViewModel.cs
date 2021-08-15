using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Prism.Commands;
using DemoApp.Module.BusinessObjects;
using System.Diagnostics;

namespace DemoApp.Mobile.Infrastructure
{
    public abstract class FormPageViewModel:SyncFrameworkViewModel
    {
      
        bool IsNewObject;
        object currentObjectKey;
        object currentObject;
        Type currentObjectType;

        public FormPageViewModel(INavigationService navigationService, ISyncHelperXafSecured syncHelperXafSecured, ISyncHelperConfigurationXafSecured configurationXafSecured, IPageDialogService dialogService) : base(navigationService, syncHelperXafSecured, configurationXafSecured, dialogService)
        {

        }

        public bool IsNew { get => IsNewObject; set => IsNewObject = value; }
        public object CurrentObjectKey { get => currentObjectKey; set => currentObjectKey = value; }
        public object CurrentObject { get => currentObject; set => currentObject = value; }
        public Type ObjectTypeName { get => currentObjectType; set => currentObjectType = value; }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            //HACK be careful when you debug this method, if the page loose focus this event might execute twice without you noticing and it will create duplicate records
            base.OnNavigatedTo(parameters);
            IsNewObject = parameters.GetValue<bool>(nameof(FormNavigationParameters.IsNewObject));
            CurrentObjectKey = parameters.GetValue<object>(nameof(FormNavigationParameters.CurrentObjectKey));
            ObjectTypeName = SyncHelperConfiguration.Types.FirstOrDefault(t => t.Name == parameters.GetValue<string>(nameof(FormNavigationParameters.ObjectTypeName)));
            if (!IsNew && currentObjectKey != null && ObjectTypeName != null)
            {
                this.LoadObject();
            }
            else if (IsNewObject && ObjectTypeName != null)
            {

                this.CurrentObject = this.ObjectSpace.CreateObject(ObjectTypeName);
            }
            WireChangeNotification();
            EvaluateCommandsState();

        }
        public override void Destroy()
        {
            UnwireChangeNotification();
            base.Destroy();
          
        }
        protected void UnwireChangeNotification()
        {
            if (CurrentObject != null)
            {
                ((System.ComponentModel.INotifyPropertyChanged)this.CurrentObject).PropertyChanged -= FormPageViewModel_PropertyChanged;
            }
        }
        protected void WireChangeNotification()
        {
            if (CurrentObject != null)
            {
                ((System.ComponentModel.INotifyPropertyChanged)this.CurrentObject).PropertyChanged += FormPageViewModel_PropertyChanged;
            }
        }

        protected virtual void FormPageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            EvaluateCommandsState();
        }

        protected virtual  void EvaluateCommandsState()
        {
            this.SaveCommand.RaiseCanExecuteChanged();
            this.DeleteCommand.RaiseCanExecuteChanged();
            this.CancelCommand.RaiseCanExecuteChanged();
        }

        protected virtual void LoadObject()
        {
            this.CurrentObject=   this.ObjectSpace.GetObjectByKey(ObjectTypeName, this.CurrentObjectKey);
            this.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(CurrentObject)));
        }
        #region SaveCommand
        private DelegateCommand _SaveCommand;
        public DelegateCommand SaveCommand =>
            _SaveCommand ?? (_SaveCommand = new DelegateCommand(ExecuteSaveCommand, CanExecuteSaveCommand));

        protected async virtual void ExecuteSaveCommand()
        {
            this.ObjectSpace.CommitChanges();
            await this.NavigationService.GoBackAsync();
        }

        protected virtual bool CanExecuteSaveCommand()
        {
            return this.ObjectSpace.IsModified;
        }
        #endregion
        #region DeleteCommand
        private DelegateCommand _DeleteCommand;
        public DelegateCommand DeleteCommand =>
            _DeleteCommand ?? (_DeleteCommand = new DelegateCommand(ExecuteDeleteCommand, CanExecuteDeleteCommand));

        protected async virtual void ExecuteDeleteCommand()
        {
            this.ObjectSpace.Delete(this.CurrentObject);
            this.ObjectSpace.CommitChanges();
            await this.NavigationService.GoBackAsync();
        }

        protected virtual bool CanExecuteDeleteCommand()
        {
            return this.CurrentObject != null && !this.ObjectSpace.IsNewObject(this.CurrentObject);
        }
        #endregion
        #region CancelCommand
        private DelegateCommand _CancelCommand;

   

        public DelegateCommand CancelCommand =>
            _CancelCommand ?? (_CancelCommand = new DelegateCommand(ExecuteCancelCommand, CanExecuteCancelCommand));

        protected async virtual void ExecuteCancelCommand()
        {
            this.ObjectSpace.Rollback();
            await this.NavigationService.GoBackAsync();
        }

        protected virtual bool CanExecuteCancelCommand()
        {
            return this.CurrentObject != null && this.ObjectSpace.IsModified;
        }
        #endregion
        

    }
}
