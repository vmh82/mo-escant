using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Escant_App.ViewModels.Base;
using System.IO;
using Escant_App.Services;
using DataModel.Enums;
using DataModel;
using DataModel.DTO.Configuracion;
using Newtonsoft.Json;
using ManijodaServicios.Switch;
using DataModel.DTO.Seguridad;
using ManijodaServicios.Offline.Interfaz;
using Escant_App.ViewModels.Configuracion;
using ManijodaServicios.AppSettings;
using DataModel.DTO;

namespace Escant_App.ViewModels
{
    public class NavigationViewModel : ViewModelBase
    {
        private Generales.Generales generales = new Generales.Generales();
        private SwitchConfiguracion _switchConfiguracion;
        public Action<bool> AbreInicio { get; set; }
        private ImageSource _storeImageSource = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });
        
        private readonly ISettingsService _settingsService;
        private readonly IAuthenticationService _authenticationService;
        private SwitchSeguridad _switchSeguridad;
        

        public NavigationViewModel(ISettingsService settingService, IAuthenticationService authenticationService)
        {
            _settingsService = settingService;
            _authenticationService = authenticationService;
            _switchConfiguracion = new SwitchConfiguracion(_settingsService);
            MessagingCenter.Unsubscribe<string>(this, "OnDataSourceChanged");
            MessagingCenter.Subscribe<string>(this, "OnDataSourceChanged", async (args) =>
            {
                var x = args;
                await abreInicioAsync();
            });
        }
        
        public async Task LoadSetting()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var setting = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.StoreSetting }, estaConectado);
            //var setting = await _settingsService.GetSettingAsync(x => x.SettingType == (int)SettingType.StoreSetting);
            if (setting != null)
            {
                StoreSettings storeSettings = JsonConvert.DeserializeObject<StoreSettings>(setting.Data);
                if (storeSettings != null)
                {
                    SettingsOnline.oAplicacion = new DataModel.DTO.Iteraccion.Aplicacion()
                    {
                        Nombre= storeSettings.StoreName
                       ,Telefono= storeSettings.Mobile
                       ,Logo= setting.Logo
                    };
                    StoreName = storeSettings.StoreName;
                    StorePhoneNumer = storeSettings.Mobile;
                }
                if (setting.Logo != null)
                    StoreImageSource = ImageSource.FromStream(() => new MemoryStream(setting.Logo));
            }
            IsBusy = false;
        }
        public async Task CheckLogin()
        {
            try
            {
                if (!_authenticationService.IsAuthenticated || SettingsOnline.EsMenuVacio)
                    await NavigationService.NavigateToModalAsync<LoginViewModel>(null);
                else
                {
                    _switchSeguridad = new SwitchSeguridad();
                    Generales.Generales generales = new Generales.Generales();
                    bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                    await _switchSeguridad.RefrescaCredencialesOnline(estaConectado);
                    MessagingCenter.Send<string>("", Escant_App.AppSettings.Settings.TimerRequestStart);
                }
            }
            catch(Exception ex)
            {

            }
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
            await NavigationService.NavigateToAsync<StoreSettingViewModel>();
        }

        private async Task abreInicioAsync()
        {
            AbreInicio?.Invoke(true);

        }


    }
}
