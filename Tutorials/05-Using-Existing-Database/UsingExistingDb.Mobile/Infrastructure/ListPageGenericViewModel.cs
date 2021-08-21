using BIT.Xpo.Providers.OfflineDataSync.XafExtensions.Configuration;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using SyncFrameworkWebClient.Module.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace UsingExistingDb.Mobile.Infrastructure
{
    public abstract class  ListPageGenericViewModel<T> : SyncFrameworkViewModel
    {
        public ListPageGenericViewModel(INavigationService navigationService, ISyncHelperXafSecured syncHelperXafSecured, ISyncHelperConfigurationXafSecured configurationXafSecured, IPageDialogService dialogService) : base(navigationService, syncHelperXafSecured, configurationXafSecured, dialogService)
        {

        }
        #region NavigateToItem

        private DelegateCommand<T> _NavigateToItemCommand;
        public DelegateCommand<T> NavigateToItemCommand =>
            _NavigateToItemCommand ?? (_NavigateToItemCommand = new DelegateCommand<T>(ExecuteNavigateToItemCommand, CanNavigateToItemCommand));
        protected async virtual void ExecuteNavigateToItemCommand(T Param)
        {
            FormNavigationParameters Params = new FormNavigationParameters(false, Param.GetType().Name, this.ObjectSpace.GetKeyValue(Param));
            await this.NavigationService.NavigateAsync(GetFormPagePath(), Params);
        }
        protected bool CanNavigateToItemCommand(T Param)
        {
            return true;
        }

        #endregion

        #region AddItem

        private DelegateCommand _AddItemCommand;
        public DelegateCommand AddItemCommand =>
            _AddItemCommand ?? (_AddItemCommand = new DelegateCommand(ExecuteAddItemCommand, CanExecuteAddItemCommand));

        protected virtual async void ExecuteAddItemCommand()
        {

            FormNavigationParameters Params = new FormNavigationParameters(true, typeof(T).Name, null);
            await this.NavigationService.NavigateAsync(GetFormPagePath(), Params);
        }

        protected virtual bool CanExecuteAddItemCommand()
        {
            return true;
        }
        #endregion


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            LoadData();

        }

        protected virtual void LoadData()
        {
           
            Data = new ObservableCollection<T>(this.ObjectSpace.CreateCollection(typeof(T)).Cast<T>());
            this.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Data)));
        }

        ObservableCollection<T> _Data;
        public ObservableCollection<T> Data { get => _Data; set => _Data = value; }

        protected abstract string GetFormPagePath();
    }
}
