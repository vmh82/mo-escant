using Escant_App.ViewModels.Base;
using DataModel.DTO.Seguridad;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Escant_App.Validations;
using Escant_App.Models;
using Escant_App.Services;
using ManijodaServicios.Resources.Texts;
using System.Linq;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using Escant_App.ViewModels.Seguridad;
using ManijodaServicios.AppSettings;
using Escant_App.Common;
using ManijodaServicios.Interfaces;
using DataModel.DTO.Configuracion;
using Newtonsoft.Json;
using DataModel.DTO;
using DataModel.Enums;

namespace Escant_App.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private Generales.Generales generales;
        private readonly IServicioSeguridad_Usuario _servicioSeguridad;
        private IServicioFirestore _servicioFirestore;
        private IServicioProductos_Precio _servicioPrecio;
        private readonly ISettingsService _settingsService;
        private ValidatableObject<string> _userName;
        private ValidatableObject<string> _password;
        private SwitchSeguridad _switchSeguridad;
        private SwitchConfiguracion _switchConfiguracion;
        private SwitchProductos _switchProductos;
        private Usuario _usuarioSeleccionado;
        private List<PerfilMenu> _perfilMenuSeleccionado;


        //public LoginViewModel(IAuthenticationService authenticationService)
        public LoginViewModel(ISettingsService settingService, IServicioSeguridad_Usuario servicioSeguridad,IServicioFirestore servicioFirestore
            ,IServicioAdministracion_Catalogo catalogoServicio, IServicioProductos_Precio servicioPrecio)
        {
            try
            {
                //_authenticationService = authenticationService;
                _servicioSeguridad = servicioSeguridad;
                _servicioFirestore = servicioFirestore;
                _settingsService = settingService;
                _servicioPrecio = servicioPrecio;
                generales = new Generales.Generales(catalogoServicio);
                BindingContext = new ConectividadModelo(_servicioSeguridad);
                _switchSeguridad = new SwitchSeguridad(_servicioSeguridad,_servicioFirestore);
                _switchConfiguracion = new SwitchConfiguracion(_settingsService);
                _switchProductos = new SwitchProductos(_servicioPrecio);
                _userName = new ValidatableObject<string>();
                _password = new ValidatableObject<string>();
                AddValidations();
                IsUserNameValid = true;
                IsPasswordValid = true;
                UserName.Value = "admin";
                Password.Value = "tempP@ss123";

                UsernameHint = TextsTranslateManager.Translate("Username");
                PasswordHint = TextsTranslateManager.Translate("Password");
            }
            catch(Exception ex)
            {

            }
        }

        private string _usernameHint;
        public string UsernameHint
        {
            get => _usernameHint;
            set
            {
                _usernameHint = value;
                RaisePropertyChanged(() => UsernameHint);
            }
        }
        private string _passwordHint;
        public string PasswordHint
        {
            get => _passwordHint;
            set
            {
                _passwordHint = value;
                RaisePropertyChanged(() => PasswordHint);
            }
        }
        private bool _isUserNameValid;
        public bool IsUserNameValid
        {
            get => _isUserNameValid;
            set
            {
                _isUserNameValid = value;
                RaisePropertyChanged(() => IsUserNameValid);
            }
        }

        private bool _isPasswordValid;
        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set
            {
                _isPasswordValid = value;
                RaisePropertyChanged(() => IsPasswordValid);
            }
        }
        private bool _userNameHasError;
        public bool UserNameHasError
        {
            get { return _userNameHasError; }
            set
            {
                _userNameHasError = value;
                RaisePropertyChanged(() => UserNameHasError);
            }
        }

        private string _userNameErrorMessage;
        public string UserNameErrorMessage
        {
            get => _userNameErrorMessage;
            set
            {
                _userNameErrorMessage = value;
                RaisePropertyChanged(() => UserNameErrorMessage);
            }
        }
        private bool _passwordHasError;
        public bool PasswordHasError
        {
            get { return _passwordHasError; }
            set
            {
                _passwordHasError = value;
                RaisePropertyChanged(() => PasswordHasError);
            }
        }

        private string _passwordErrorMessage;
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set
            {
                _passwordErrorMessage = value;
                RaisePropertyChanged(() => PasswordErrorMessage);
            }
        }

        public ValidatableObject<string> UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public ValidatableObject<string> Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        
        public Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set
            {
                _usuarioSeleccionado = value;
                RaisePropertyChanged(() => UsuarioSeleccionado);
            }
        }
        public List<PerfilMenu> PerfilMenuSeleccionado
        {
            get => _perfilMenuSeleccionado;
            set
            {
                _perfilMenuSeleccionado = value;
                RaisePropertyChanged(() => PerfilMenuSeleccionado);
            }
        }
        public ICommand SignInCommand => new Command(async () => await SignInAsync());
        public ICommand ForgotCommand => new Command(async () => await ForgotAsync());
        public ICommand RegisterCommand => new Command(async () => await RegisterAsync());
        public ICommand ValidateUserNameCommand => new Command(() => ValidateUserName());
        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());
        

        private async Task SignInAsync()
        {
            IsBusy = true;
            bool isValid = Validate();
            if (isValid)
            {
                UserAuthentication oUsuario = new UserAuthentication()
                {
                    email = UserName.Value,
                    password = Encriptar.encriptarRJC(Password.Value), //Encriptar.encriptar(Password.Value),
                    passwordSinEnc = Password.Value,
                    returnSecureToken = true
                };
                bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));

                bool isAuth = await _switchSeguridad.Login(oUsuario, estaConectado);

                //bool isAuth = await _authenticationService.LoginAsync(UserName.Value, Password.Value);
                if (isAuth)
                {
                    IsBusy = false;
                    UsuarioSeleccionado = await _switchSeguridad.ConsultaUsuario(new Usuario()
                    {
                        Email=GlobalSettings.User.Email
                        ,Clave= GlobalSettings.User.Clave
                    },estaConectado);                    
                    if(UsuarioSeleccionado != null)
                    {
                        PerfilMenuSeleccionado = await _switchSeguridad.ConsultaPerfilesMenu(new PerfilMenu() { IdPerfil = UsuarioSeleccionado.IdPerfil }, estaConectado);
                        SettingsOnline.oPerfilMenu = PerfilMenuSeleccionado.Where(x => x.EstaActivo == 1).ToList();
                        SettingsOnline.oLCatalogo = await generales.LoadListaCatalogo(new DataModel.DTO.Administracion.Catalogo() {Deleted=0 });
                        SettingsOnline.oLPrecio = await _switchProductos.ConsultaPrecios(new DataModel.DTO.Productos.Precio() { Deleted=0}, estaConectado);
                        if (SettingsOnline.oPerfilMenu != null)
                            SettingsOnline.EsMenuVacio = false;
                        await LoadSetting();
                    }
                    MessagingCenter.Send<string>(string.Empty, "OnDataSourceChanged");
                    await NavigationService.RemoveModalAsync();
                    if (Escant_App.AppSettings.Settings.IsFirstTimeLoggedIn)
                    {
                        Escant_App.AppSettings.Settings.IsFirstTimeLoggedIn = false;
                        //MessagingCenter.Send<string>("", Escant_App.AppSettings.Settings.SyncImmediately);
                    }
                    else
                    {
                        //MessagingCenter.Send<string>("", Escant_App.AppSettings.Settings.TimerRequestStart);
                        var y = 0;
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Importante", "El usuario y password no se encuentran registrados", "OK");
                }
            }
            IsBusy = false;
        }

        public async Task LoadSetting()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var setting = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.StoreSetting }, estaConectado);
            if (setting != null)
            {
                StoreSettings storeSettings = JsonConvert.DeserializeObject<StoreSettings>(setting.Data);
                if (storeSettings != null)
                {
                    var settingFacturacion = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.FacturacionSetting }, estaConectado);
                    var settingImpresion = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.PrinterSetting }, estaConectado);

                    SettingsOnline.oAplicacion = new DataModel.DTO.Iteraccion.Aplicacion()
                    { Id = setting.Id
                       ,
                        Nombre = storeSettings.StoreName
                        ,
                        Administrador = storeSettings.StoreOwner
                       ,
                        Direccion = storeSettings.Address
                       ,
                        Telefono = storeSettings.Mobile
                       ,
                        Logo = setting.Logo
                        ,
                        IdClienteConectado = "" //Por completar ciclo
                       ,
                        NombreConectado = UsuarioSeleccionado.Nombre
                       ,
                        IdentificacionConectado = UsuarioSeleccionado.Identificacion
                        ,
                        PerfilConectado = (PerfilMenuSeleccionado.FirstOrDefault()).CodigoPerfil
                        ,
                        ImagenUsuario = UsuarioSeleccionado.Imagen
                        ,
                        ParametrosFacturacion = settingFacturacion.Data
                        ,
                        ParametrosImpresion=settingImpresion.Data
                        ,
                        IdentificacionAdministrador=storeSettings.Ruc
                    };
                }
            }
            IsBusy = false;
        }

        private async Task RegisterAsync()
        {
            var usuario = new Usuario()
            {
                Origen = "LoginView"
            };
            await NavigationService.NavigateToModalAsync<AddUsuarioViewModel>(usuario);            
        }
        private async Task ForgotAsync()
        {
            await NavigationService.NavigateToModalAsync<PasswordOlvidadoViewModel>(null);
        }

        private void AddValidations()
        {
            _userName.Validations.Add(new EmailRule<string>("User"));
            //_userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = TextsTranslateManager.Translate("UserNameRequired") });            
            _password.Validations.Add(new PasswordRule<string>(""));
            //_password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = TextsTranslateManager.Translate("PasswordRequired") });
        }

        private bool Validate()
        {
            IsUserNameValid = _userName.Validate();
            IsPasswordValid = _password.Validate();
            return IsUserNameValid && IsPasswordValid;
        }

        private bool ValidateUserName()
        {
            return _userName.Validate();
        }
        private bool ValidatePassword()
        {
            return _password.Validate();
        }
    }

}
