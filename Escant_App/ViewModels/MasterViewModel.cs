using System;
using System.IO;
using System.Threading.Tasks;
using DataModel;
using DataModel.DTO;
using DataModel.DTO.Configuracion;
using DataModel.Enums;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Switch;
using Escant_App.Services;
using Escant_App.ViewModels.Base;
using Escant_App.ViewModels.Configuracion;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Escant_App.ViewModels
{
    public class MasterViewModel : ViewModelBase
    {
        private Generales.Generales generales = new Generales.Generales();
        private SwitchConfiguracion _switchConfiguracion;
        private ImageSource _storeImageSource = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });
        private readonly ManijodaServicios.Offline.Interfaz.ISettingsService _settingsService;

        public MasterViewModel(ManijodaServicios.Offline.Interfaz.ISettingsService settingService)
        {
            _settingsService = settingService;
            _switchConfiguracion = new SwitchConfiguracion(_settingsService);
        }
        public async Task LoadSetting()
        {
            IsBusy = true;                                        
            if (SettingsOnline.oAplicacion != null)
            {
                StoreName = SettingsOnline.oAplicacion.Nombre;
                StorePhoneNumer= SettingsOnline.oAplicacion.Telefono;                
                if (SettingsOnline.oAplicacion.Logo != null)
                    StoreImageSource = ImageSource.FromStream(() => new MemoryStream(SettingsOnline.oAplicacion.Logo));
            }
            IsBusy = false;
        }
        public async Task Logout()
        {
            GlobalSettings.User = null;
            await NavigationService.NavigateToModalAsync<LoginViewModel>(null);
            MessagingCenter.Send<string>("", Escant_App.AppSettings.Settings.TimerRequestStop);
        }
        public ImageSource StoreImageSource
        {
            get => _storeImageSource;
            set
            {
                _storeImageSource = value;
                RaisePropertyChanged(() => StoreImageSource);
            }
        }

        private string _storeName;
        public string StoreName
        {
            get => _storeName;
            set
            {
                _storeName = value;
                RaisePropertyChanged(() => StoreName);
            }
        }

        private string _storePhoneNumer;
        public string StorePhoneNumer
        {
            get => _storePhoneNumer;
            set
            {
                _storePhoneNumer = value;
                RaisePropertyChanged(() => StorePhoneNumer);
            }
        }

        public async Task NavigateToStoreSetting()
        {
            await NavigationService.NavigateToAsync<StoreSettingViewModel>(false);
        }
    }
}
