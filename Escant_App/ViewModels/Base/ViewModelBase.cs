using Escant_App.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ManijodaServicios.Offline.Interfaz;

namespace Escant_App.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject, INotifyPropertyChanged
    {
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        

        private bool _isBusy;
        private string _pageTitle;
        private string _idAnuncio;

        internal int ItemIndex
        {
            get;
            set;
        }
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ViewModelBase()
        {
            DialogService = ViewModelLocator.Resolve<IDialogService>();
            NavigationService = ViewModelLocator.Resolve<INavigationService>();
            //GlobalSetting.Instance.BaseEndpoint = ViewModelLocator.Resolve<ISettingsService>().UrlBase;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
        public virtual Task InitializeAsync(object navigationData,object navigationData1)
        {
            return Task.FromResult(false);
        }
        public string PageTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                _pageTitle = value;
                RaisePropertyChanged(() => PageTitle);
            }
        }
        public string IdAnuncio
        {
            get => _idAnuncio;
            set
            {
                _idAnuncio = value;
                RaisePropertyChanged(() => IdAnuncio);
            }
        }
    }

}
