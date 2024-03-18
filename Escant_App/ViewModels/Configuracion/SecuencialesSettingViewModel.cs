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
    public class SecuencialesSettingViewModel:ViewModelBase
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
        private ValidatableObject<string> _secuencialOrdenCompra;
        private ValidatableObject<string> _secuencialOrdenCompraPeriodo;
        private ValidatableObject<string> _secuencialOrdenVenta;
        private ValidatableObject<string> _secuencialOrdenVentaPeriodo;
        private ValidatableObject<string> _secuencialReciboPago;
        private ValidatableObject<string> _secuencialReciboPagoPeriodo;

        //private ValidatableObject<string> _codigoEstablecimientoValidacion;
        /// <summary>
        /// 0.2 Declaración de las variables de control de errores
        /// </summary>
        private bool _isSecuencialOrdenCompraValid;
        private bool _isSecuencialOrdenCompraPeriodoValid;
        private bool _isSecuencialOrdenVentaValid;
        private bool _isSecuencialOrdenVentaPeriodoValid;
        private bool _isSecuencialReciboPagoValid;
        private bool _isSecuencialReciboPagoPeriodoValid;

        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="settingService"></param>
        public SecuencialesSettingViewModel(IServicioSeguridad_Usuario seguridadServicio
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
            _secuencialOrdenCompra = new ValidatableObject<string>();
            _secuencialOrdenCompraPeriodo = new ValidatableObject<string>();
            _secuencialOrdenVenta = new ValidatableObject<string>();
            _secuencialOrdenVentaPeriodo = new ValidatableObject<string>();
            _secuencialReciboPago = new ValidatableObject<string>();
            _secuencialReciboPagoPeriodo = new ValidatableObject<string>();
        }

        public ValidatableObject<string> SecuencialOrdenCompra
        {
            get => _secuencialOrdenCompra;
            set
            {
                _secuencialOrdenCompra = value;
                RaisePropertyChanged(() => SecuencialOrdenCompra);
            }
        }
        public ValidatableObject<string> SecuencialOrdenCompraPeriodo
        {
            get => _secuencialOrdenCompraPeriodo;
            set
            {
                _secuencialOrdenCompraPeriodo = value;
                RaisePropertyChanged(() => SecuencialOrdenCompraPeriodo);
            }
        }
        public ValidatableObject<string> SecuencialOrdenVenta
        {
            get => _secuencialOrdenVenta;
            set
            {
                _secuencialOrdenVenta = value;
                RaisePropertyChanged(() => SecuencialOrdenVenta);
            }
        }
        public ValidatableObject<string> SecuencialOrdenVentaPeriodo
        {
            get => _secuencialOrdenVentaPeriodo;
            set
            {
                _secuencialOrdenVentaPeriodo = value;
                RaisePropertyChanged(() => SecuencialOrdenVentaPeriodo);
            }
        }
        public ValidatableObject<string> SecuencialReciboPago
        {
            get => _secuencialReciboPago;
            set
            {
                _secuencialReciboPago = value;
                RaisePropertyChanged(() => SecuencialReciboPago);
            }
        }
        public ValidatableObject<string> SecuencialReciboPagoPeriodo
        {
            get => _secuencialReciboPagoPeriodo;
            set
            {
                _secuencialReciboPagoPeriodo = value;
                RaisePropertyChanged(() => SecuencialReciboPagoPeriodo);
            }
        }
        public bool IsSecuencialOrdenCompraValid
        {
            get => _isSecuencialOrdenCompraValid;
            set
            {
                _isSecuencialOrdenCompraValid = value;
                RaisePropertyChanged(() => IsSecuencialOrdenCompraValid);
            }
        }
        public bool IsSecuencialOrdenCompraPeriodoValid
        {
            get => _isSecuencialOrdenCompraPeriodoValid;
            set
            {
                _isSecuencialOrdenCompraPeriodoValid = value;
                RaisePropertyChanged(() => IsSecuencialOrdenCompraPeriodoValid);
            }
        }
        public bool IsSecuencialOrdenVentaValid
        {
            get => _isSecuencialOrdenVentaValid;
            set
            {
                _isSecuencialOrdenVentaValid = value;
                RaisePropertyChanged(() => IsSecuencialOrdenVentaValid);
            }
        }
        public bool IsSecuencialOrdenVentaPeriodoValid
        {
            get => _isSecuencialOrdenVentaPeriodoValid;
            set
            {
                _isSecuencialOrdenVentaPeriodoValid = value;
                RaisePropertyChanged(() => IsSecuencialOrdenVentaPeriodoValid);
            }
        }
        public bool IsSecuencialReciboPagoValid
        {
            get => _isSecuencialReciboPagoValid;
            set
            {
                _isSecuencialReciboPagoValid = value;
                RaisePropertyChanged(() => IsSecuencialReciboPagoValid);
            }
        }
        public bool IsSecuencialReciboPagoPeriodoValid
        {
            get => _isSecuencialReciboPagoPeriodoValid;
            set
            {
                _isSecuencialReciboPagoPeriodoValid = value;
                RaisePropertyChanged(() => IsSecuencialReciboPagoPeriodoValid);
            }
        }
        /// <summary>
        /// Método para definir las validaciones en pantalla
        /// </summary>
        private void AddValidations()
        {
            _secuencialOrdenCompra.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _secuencialOrdenCompraPeriodo.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _secuencialOrdenVenta.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _secuencialOrdenVentaPeriodo.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _secuencialReciboPago.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _secuencialReciboPagoPeriodo.Validations.Add(new IsNotNullOrEmptyRule<string>(""));

        }
        private bool Validate()
        {
            IsSecuencialOrdenCompraValid = !SecuencialOrdenCompra.Validate();
            IsSecuencialOrdenCompraPeriodoValid = !SecuencialOrdenCompraPeriodo.Validate();
            IsSecuencialOrdenVentaValid = !SecuencialOrdenVenta.Validate();
            IsSecuencialOrdenVentaPeriodoValid = !SecuencialOrdenVentaPeriodo.Validate();
            IsSecuencialReciboPagoValid = !SecuencialReciboPago.Validate();
            IsSecuencialReciboPagoPeriodoValid = !SecuencialReciboPagoPeriodo.Validate();

            return !IsSecuencialOrdenCompraValid
                && !IsSecuencialOrdenCompraPeriodoValid
                && !IsSecuencialOrdenVentaValid
                && !IsSecuencialOrdenVentaPeriodoValid
                && !IsSecuencialReciboPagoValid
                && !IsSecuencialReciboPagoPeriodoValid;

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
            var setting = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.SecuencialesSetting }, estaConectado);
            if (setting != null)
            {
                _isSettingExists = true;
                _facturacionSettingId = setting.Id;


                SecuencialSetttings comprobantesSettings = JsonConvert.DeserializeObject<SecuencialSetttings>(setting.Data);
                if (comprobantesSettings != null)
                {
                    SecuencialOrdenCompra.Value = comprobantesSettings.SecuencialOrdenCompra;
                    SecuencialOrdenCompraPeriodo.Value = comprobantesSettings.SecuencialOrdenCompraPeriodo;
                    SecuencialOrdenVenta.Value = comprobantesSettings.SecuencialOrdenVenta;
                    SecuencialOrdenVentaPeriodo.Value = comprobantesSettings.SecuencialOrdenVentaPeriodo;
                    SecuencialReciboPago.Value = comprobantesSettings.SecuencialOrdenCompraPeriodo;
                    SecuencialReciboPagoPeriodo.Value = comprobantesSettings.SecuencialReciboPagoPeriodo;
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
            var comprobantesSettings = new SecuencialSetttings
            {
                SecuencialOrdenCompra = SecuencialOrdenCompra.Value,
                SecuencialOrdenCompraPeriodo = SecuencialOrdenCompraPeriodo.Value,
                SecuencialOrdenVenta = SecuencialOrdenVenta.Value,
                SecuencialOrdenVentaPeriodo = SecuencialOrdenVentaPeriodo.Value,
                SecuencialReciboPago = SecuencialReciboPago.Value,
                SecuencialReciboPagoPeriodo = SecuencialReciboPagoPeriodo.Value

            };
            var setting = new Setting
            {
                Id = Generator.GenerateKey(),
                SettingType = (int)SettingType.SecuencialesSetting,
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
