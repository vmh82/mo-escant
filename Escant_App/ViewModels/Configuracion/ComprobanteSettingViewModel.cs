using DataModel.DTO;
using DataModel.DTO.Configuracion;
using DataModel.Enums;
using DataModel.Helpers;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using Escant_App.Validations;
using Escant_App.ViewModels.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Configuracion
{
    public class ComprobanteSettingViewModel: ViewModelBase
    {
        /// <summary>
        /// 0. Declaración de las variables         
        /// </summary>
        private readonly ISettingsService _settingsService;
        private SwitchConfiguracion _switchConfiguracion;
        private Generales.Generales generales = new Generales.Generales();
        private bool _isSettingExists;
        private string _facturacionSettingId;

        /// <summary>
        /// 0.1 Declaración de las variables de lectura y escritura para los controles
        /// </summary>
        private ValidatableObject<string> _secuencialFactura;
        private ValidatableObject<string> _secuencialRetencion;
        private ValidatableObject<string> _secuencialNotaDebito;
        private ValidatableObject<string> _secuencialNotaCredito;
        private ValidatableObject<string> _secuencialGuiaRemision;

        //private ValidatableObject<string> _codigoEstablecimientoValidacion;
        /// <summary>
        /// 0.2 Declaración de las variables de control de errores
        /// </summary>
        private bool _isSecuencialFacturaValid;
        private bool _isSecuencialRetencionValid;
        private bool _isSecuencialNotaDebitoValid;
        private bool _isSecuencialNotaCreditoValid;
        private bool _isSecuencialGuiaRemisionValid;

        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="settingService"></param>
        public ComprobanteSettingViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , ISettingsService settingService)
        {
            BindingContext = new ConectividadModelo(seguridadServicio);
            _settingsService = settingService;
            _switchConfiguracion = new SwitchConfiguracion(_settingsService);            
            inicioControlesValidacion();
            AddValidations();
        }

        public void inicioControlesValidacion()
        {
            _secuencialFactura=new ValidatableObject<string>();
            _secuencialRetencion = new ValidatableObject<string>();
            _secuencialNotaDebito = new ValidatableObject<string>();
            _secuencialNotaCredito = new ValidatableObject<string>();
            _secuencialGuiaRemision = new ValidatableObject<string>();        
        }

        public ValidatableObject<string> SecuencialFactura
        {
            get => _secuencialFactura;
            set
            {
                _secuencialFactura = value;
                RaisePropertyChanged(() => SecuencialFactura);
            }
        }
        public ValidatableObject<string> SecuencialRetencion
        {
            get => _secuencialRetencion;
            set
            {
                _secuencialRetencion = value;
                RaisePropertyChanged(() => SecuencialRetencion);
            }
        }
        public ValidatableObject<string> SecuencialNotaDebito
        {
            get => _secuencialNotaDebito;
            set
            {
                _secuencialNotaDebito = value;
                RaisePropertyChanged(() => SecuencialNotaDebito);
            }
        }
        public ValidatableObject<string> SecuencialNotaCredito
        {
            get => _secuencialNotaCredito;
            set
            {
                _secuencialNotaCredito = value;
                RaisePropertyChanged(() => SecuencialNotaCredito);
            }
        }
        public ValidatableObject<string> SecuencialGuiaRemision
        {
            get => _secuencialGuiaRemision;
            set
            {
                _secuencialGuiaRemision = value;
                RaisePropertyChanged(() => SecuencialGuiaRemision);
            }
        }
        public bool IsSecuencialFacturaValid
        {
            get => _isSecuencialFacturaValid;
            set
            {
                _isSecuencialFacturaValid = value;
                RaisePropertyChanged(() => IsSecuencialFacturaValid);
            }
        }
        public bool IsSecuencialRetencionValid
        {
            get => _isSecuencialRetencionValid;
            set
            {
                _isSecuencialRetencionValid = value;
                RaisePropertyChanged(() => IsSecuencialRetencionValid);
            }
        }
        public bool IsSecuencialNotaDebitoValid
        {
            get => _isSecuencialNotaDebitoValid;
            set
            {
                _isSecuencialNotaDebitoValid = value;
                RaisePropertyChanged(() => IsSecuencialNotaDebitoValid);
            }
        }
        public bool IsSecuencialNotaCreditoValid
        {
            get => _isSecuencialNotaCreditoValid;
            set
            {
                _isSecuencialNotaCreditoValid = value;
                RaisePropertyChanged(() => IsSecuencialNotaCreditoValid);
            }
        }
        public bool IsSecuencialGuiaRemisionValid
        {
            get => _isSecuencialGuiaRemisionValid;
            set
            {
                _isSecuencialGuiaRemisionValid = value;
                RaisePropertyChanged(() => IsSecuencialGuiaRemisionValid);
            }
        }
        /// <summary>
        /// Método para definir las validaciones en pantalla
        /// </summary>
        private void AddValidations()
        {
            _secuencialFactura.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _secuencialRetencion.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _secuencialNotaDebito.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _secuencialNotaCredito.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _secuencialGuiaRemision.Validations.Add(new IsNotNullOrEmptyRule<string>(""));

        }
        private bool Validate()
        {
            IsSecuencialFacturaValid = !SecuencialFactura.Validate();
            IsSecuencialRetencionValid = !SecuencialRetencion.Validate();
            IsSecuencialNotaDebitoValid = !SecuencialNotaDebito.Validate();
            IsSecuencialNotaCreditoValid = !SecuencialNotaCredito.Validate();
            IsSecuencialGuiaRemisionValid = !SecuencialGuiaRemision.Validate();

            return !IsSecuencialFacturaValid
                && !IsSecuencialRetencionValid
                && !IsSecuencialNotaDebitoValid
                && !IsSecuencialNotaCreditoValid
                && !IsSecuencialGuiaRemisionValid;

        }

        /// <summary>
        /// 3. Definición de métodos de la pantalla
        /// </summary>
        /// <summary>
        #region DefinicionMetodos
        /// 3.1 Guardar los datos de pantalla
        /// </summary>
        public ICommand SaveSettingCommand => new Command(async () => await SaveSettingAsync());
        /// <summary>
        /// 3.2 Descartar el almacenamiento de datos
        /// </summary>
        public ICommand DiscardCommand => new Command(async () => await DiscardAsync());
        #endregion DefinicionMetodos
        /// <summary>
        /// 4. Métodos de administración de datos
        /// </summary>
        /// <returns></returns>
        #region MétodosAdministraciónDatos        
        /// <summary>
        /// 4.1. Método para Cargar información de pantalla, la llamada realizada desde el frontend al inicializar
        /// </summary>
        /// <returns></returns>
        public async Task LoadSetting()
        {
            IsBusy = true;
            //var setting = await _settingsService.GetSettingAsync(x => x.SettingType == (int)SettingType.FacturacionSetting);
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var setting = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.ComprobantesSetting }, estaConectado);
            if (setting != null)
            {
                _isSettingExists = true;
                _facturacionSettingId = setting.Id;


                ComprobantesSetttings comprobantesSettings = JsonConvert.DeserializeObject<ComprobantesSetttings>(setting.Data);
                if (comprobantesSettings != null)
                {
                    SecuencialFactura.Value = comprobantesSettings.SecuencialFactura;
                    SecuencialGuiaRemision.Value = comprobantesSettings.SecuencialGuiaRemision;
                    SecuencialNotaCredito.Value = comprobantesSettings.SecuencialNotaCredito;
                    SecuencialNotaDebito.Value = comprobantesSettings.SecuencialNotaDebito;
                    SecuencialRetencion.Value = comprobantesSettings.SecuencialRetencion;
                }
            }
            else
            {
                _isSettingExists = false;
            }
            IsBusy = false;
        }
        /// <summary>
        /// 4.2 Guardar datos de pantalla
        /// </summary>
        /// <returns></returns>
        private async Task SaveSettingAsync()
        {
            IsBusy = true;
            bool result = Validate();
            if (!result)
            {
                IsBusy = false;
                return;
            }
            var comprobantesSettings = new ComprobantesSetttings
            {
                SecuencialFactura = SecuencialFactura.Value,
                SecuencialNotaDebito = SecuencialNotaDebito.Value,
                SecuencialNotaCredito = SecuencialNotaDebito.Value,
                SecuencialGuiaRemision = SecuencialGuiaRemision.Value,
                SecuencialRetencion = SecuencialRetencion.Value
            };
            var setting = new Setting
            {
                Id = Generator.GenerateKey(),
                SettingType = (int)SettingType.ComprobantesSetting,
                Data = JsonConvert.SerializeObject(comprobantesSettings)                
            };            
            string informedMessage = "";
            /*if (_isSettingExists)
            {
                setting.Id = _facturacionSettingId;
                await _settingsService.UpdateSettingAsync(setting);
                informedMessage = TextsTranslateManager.Translate("StoreUpdateSuccess");
            }
            else
            {
                await _settingsService.InsertSettingAsync(setting);
                informedMessage = TextsTranslateManager.Translate("StoreInfoSaved");
            }*/
            if (_isSettingExists)
            {
                setting.Id = _facturacionSettingId;
            }
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var resultado = await _switchConfiguracion.GuardaConfiguracion(setting, estaConectado, _isSettingExists);
            IsBusy = false;
            await NavigationService.RemoveLastFromBackStackAsync();
            DialogService.ShowToast(informedMessage);
        }
        private async Task DiscardAsync()
        {
            await NavigationService.RemoveLastFromBackStackAsync();
        }
        #endregion
    }
}
