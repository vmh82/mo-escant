using DataModel.DTO.Administracion;
using DataModel.DTO.Clientes;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Seguridad;
using DataModel.Helpers;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Switch;
using Escant_App.Validations;
using Escant_App.ViewModels.Administracion;
using Escant_App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Escant_App.Common;
using ManijodaServicios.AppSettings;

namespace Escant_App.ViewModels.Seguridad
{
    public class AddUsuarioViewModel:ViewModelBase
    {
        #region 0.DeclaracionVariables
        /// <summary>
        /// 0. Declaración de las variables                 
        /// </summary>
        /// 0.0 Referencias a otras clases
        private SwitchSeguridad _switchSeguridad;
        private SwitchIteraccion _switchIteraccion;
        private SwitchCliente _switchCliente;
        private SwitchAdministracion _switchAdministracion;
        private Generales.Generales generales;        
        

        private readonly IServicioSeguridad_Usuario _usuarioServicio;
        
        private readonly IServicioIteraccion_Galeria _galeriaServicio;

        private readonly IServicioClientes_Persona _personaServicio;
        private readonly IServicioClientes_Cliente _clienteServicio;

        private readonly IServicioAdministracion_Empresa _empresaServicio;

        /// 0.1 Referencias a instancias de objetos a utilizarse en la Clase        
        private Usuario _registro;
        private Galeria _galeria;

        private bool _isEditMode;
        private bool _isDetalleMode;


        /// <summary>
        /// 0.2 Declaración de las variables de lectura y escritura para los controles
        /// </summary>
        private ImageSource _fuenteImagenRegistro = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });
        private string _id;
        private string _idPersona;
        private string _nombre;
        private ValidatableObject<string> _nombres;
        private ValidatableObject<string> _apellidos;
        private ValidatableObject<string> _identificacion;
        private ValidatableObject<string> _celular;
        private ValidatableObject<string> _email;
        private ValidatableObject<string> _clave;
        private ValidatableObject<string> _pinDigitos;
        private string _idToken;
        private string _idGaleria;
        private string _imagen;
        private string _idPerfil;
        private string _Perfil;
        private bool _estaActivo;


        /// <summary>
        /// 0.3 Declaración variables control de datos
        /// </summary>       

        private bool _isRegistroExiste;

        private string _emailAnterior;
        private string _claveAnterior;


        /// <summary>
        /// 0.4 Declaración de las variables de control de errores
        /// </summary>
        private bool _isNombresValid;
        private bool _isApellidosValid;
        private bool _isIdentificacionValid;
        private bool _isCelularValid;
        private bool _isEmailValid;
        private bool _isClaveValid;
        private bool _isPinDigitosValid;

        private bool _isEnabledPerfil;
        private bool _isVisibleBotones;
        /// <summary>
        /// 0.5 Declaración de las variables de control externo
        /// </summary>

        private Catalogo _perfilSeleccionado;

        #endregion DeclaracionVariables

        #region 1.Constructor
        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public AddUsuarioViewModel(IServicioSeguridad_Usuario usuarioServicio                                  
                                  , IServicioAdministracion_Catalogo catalogoServicio
                                  , IServicioIteraccion_Galeria galeriaServicio
                                  , IServicioClientes_Cliente clienteServicio
                                  , IServicioClientes_Persona personaServicio
                                  , IServicioAdministracion_Empresa empresaServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(usuarioServicio);
            //Instanciación Acorde al model
            _usuarioServicio = usuarioServicio;
            _galeriaServicio = galeriaServicio;
            _clienteServicio = clienteServicio;
            _personaServicio = personaServicio;
            _empresaServicio = empresaServicio;
            _switchSeguridad = new SwitchSeguridad(_usuarioServicio);
            _switchCliente = new SwitchCliente(_clienteServicio, _personaServicio);
            _switchIteraccion = new SwitchIteraccion(_galeriaServicio);
            _switchAdministracion = new SwitchAdministracion(_empresaServicio);
            generales = new Generales.Generales(catalogoServicio, galeriaServicio);
            inicioControlesValidacion();
            AddValidations();
            MessagingCenter.Unsubscribe<Catalogo>(this, Escant_App.AppSettings.Settings.CatalogoSeleccionadoPerfilAddUsuarioView);
            MessagingCenter.Subscribe<Catalogo>(this, Escant_App.AppSettings.Settings.CatalogoSeleccionadoPerfilAddUsuarioView, (args) =>
            {
                PerfilSeleccionado = args.CodigoCatalogo == "perfil" ? args : PerfilSeleccionado;
                IdPerfil = PerfilSeleccionado.Id;
                Perfil = PerfilSeleccionado.Nombre;
            });
            
        }
        #endregion 1.Constructor    

        #region 2.InstanciaAsignaVariablesControles
        /// 2. Asignacion y Lectura de variables vinculadas a los controles
        /// <summary>
        /// Método Asignación o lectura de CodigoEstablecimiento
        /// </summary>

        public ImageSource FuenteImagenRegistro
        {
            get
            {
                return _fuenteImagenRegistro;
            }
            set
            {
                _fuenteImagenRegistro = value;
                RaisePropertyChanged(() => FuenteImagenRegistro);
            }
        }
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }
        public string IdPersona
        {
            get => _idPersona;
            set
            {
                _idPersona = value;
                RaisePropertyChanged(() => IdPersona);
            }
        }
        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                RaisePropertyChanged(() => Nombre);
            }
        }
        public ValidatableObject<string> Nombres
        {
            get => _nombres;
            set
            {
                _nombres = value;
                RaisePropertyChanged(() => Nombres);
            }
        }
        public ValidatableObject<string> Apellidos
        {
            get => _apellidos;
            set
            {
                _apellidos = value;
                RaisePropertyChanged(() => Apellidos);
            }
        }
        public ValidatableObject<string> Identificacion
        {
            get => _identificacion;
            set
            {
                _identificacion = value;
                RaisePropertyChanged(() => Identificacion);
            }
        }
        public ValidatableObject<string> Celular
        {
            get => _celular;
            set
            {
                _celular = value;
                RaisePropertyChanged(() => Celular);
            }
        }
        public ValidatableObject<string> Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }
        public ValidatableObject<string> Clave
        {
            get => _clave;
            set
            {
                _clave = value;
                RaisePropertyChanged(() => Clave);
            }
        }
        public ValidatableObject<string> PinDigitos
        {
            get => _pinDigitos;
            set
            {
                _pinDigitos = value;
                RaisePropertyChanged(() => PinDigitos);
            }
        }
        public string IdToken
        {
            get => _idToken;
            set
            {
                _idToken = value;
                RaisePropertyChanged(() => IdToken);
            }
        }
        public string IdGaleria
        {
            get => _idGaleria;
            set
            {
                _idGaleria = value;
                RaisePropertyChanged(() => IdGaleria);
            }
        }
        public string Imagen
        {
            get => _imagen;
            set
            {
                _imagen = value;
                RaisePropertyChanged(() => Imagen);
            }
        }
        public string IdPerfil
        {
            get => _idPerfil;
            set
            {
                _idPerfil = value;
                RaisePropertyChanged(() => IdPerfil);
            }
        }
        public string Perfil
        {
            get => _Perfil;
            set
            {
                _Perfil = value;
                RaisePropertyChanged(() => Perfil);
            }
        }
        public bool EstaActivo
        {
            get => _estaActivo;
            set
            {
                _estaActivo = value;
                RaisePropertyChanged(() => EstaActivo);
            }
        }

        public bool IsNombresValid
        {
            get => _isNombresValid;
            set
            {
                _isNombresValid = value;
                RaisePropertyChanged(() => IsNombresValid);
            }
        }
        public bool IsApellidosValid
        {
            get => _isApellidosValid;
            set
            {
                _isApellidosValid = value;
                RaisePropertyChanged(() => IsApellidosValid);
            }
        }
        public bool IsIdentificacionValid
        {
            get => _isIdentificacionValid;
            set
            {
                _isIdentificacionValid = value;
                RaisePropertyChanged(() => IsIdentificacionValid);
            }
        }
        public bool IsCelularValid
        {
            get => _isCelularValid;
            set
            {
                _isCelularValid = value;
                RaisePropertyChanged(() => IsCelularValid);
            }
        }
        public bool IsEmailValid
        {
            get => _isEmailValid;
            set
            {
                _isEmailValid = value;
                RaisePropertyChanged(() => IsEmailValid);
            }
        }
        public bool IsClaveValid
        {
            get => _isClaveValid;
            set
            {
                _isClaveValid = value;
                RaisePropertyChanged(() => IsClaveValid);
            }
        }
        public bool IsPinDigitosValid
        {
            get => _isPinDigitosValid;
            set
            {
                _isPinDigitosValid = value;
                RaisePropertyChanged(() => IsPinDigitosValid);
            }
        }

        public bool IsEnabledPerfil
        {
            get => _isEnabledPerfil;
            set
            {
                _isEnabledPerfil = value;
                RaisePropertyChanged(() => IsEnabledPerfil);
            }
        }

        public bool IsVisibleBotones
        {
            get => _isVisibleBotones;
            set
            {
                _isVisibleBotones = value;
                RaisePropertyChanged(() => IsVisibleBotones);
            }
        }

        public Catalogo PerfilSeleccionado
        {
            get => _perfilSeleccionado;
            set
            {
                _perfilSeleccionado = value;
                RaisePropertyChanged(() => PerfilSeleccionado);
            }
        }

        public string EmailAnterior
        {
            get => _emailAnterior;
            set
            {
                _emailAnterior = value;
                RaisePropertyChanged(() => EmailAnterior);
            }
        }

        public string ClaveAnterior
        {
            get => _claveAnterior;
            set
            {
                _claveAnterior = value;
                RaisePropertyChanged(() => ClaveAnterior);
            }
        }

        #endregion 2.InstanciaAsignaVariablesControles        

        #region 3.DefinicionMetodos
        /// <summary>
        /// 3. Definición de métodos de la pantalla
        /// </summary>
        /// <summary>
        public ICommand BrowseGalleryCommand => new Command(async () => await BrowseGalleryAsync());
        public ICommand OpenCameraCommand => new Command(async () => await OpenCameraAsync());
        public ICommand RemoveImageCommand => new Command(async () => await RemoveImageAsync());
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());
        public ICommand OpenPerfilCommand => new Command(async () => await OpenPerfilAsync());
        public ICommand NombresUnfocused => new Command(async () => await NombresUnfocused_CommandAsync());
        public ICommand ApellidosUnfocused => new Command(async () => await ApellidosUnfocused_CommandAsync());

        #endregion 3.DefinicionMetodos

        #region 4.MétodosAdministraciónDatos
        /// <summary>
        /// 4. Métodos de administración de datos
        /// </summary>
        /// <returns></returns>
        /// 4.1. Método para Cargar información de pantalla, la llamada realizada desde el frontend al inicializar
        /// </summary>
        /// <returns></returns>
        public async Task Load()
        {
            IsBusy = true;
            await LoadInicio();
            IsBusy = false;
        }
        /// <summary>
        /// 4.2 Método iniciar valores
        /// </summary>
        /// <returns></returns>
        public async Task LoadInicio()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            ResetForm();
        }
        /// <summary>
        /// 4.3 Método iniciar Pantalla con 1 parámetro
        /// </summary>
        /// <param name="navigationData"></param>
        /// <returns></returns>
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            PageTitle = TextsTranslateManager.Translate("AddUsuarioSeguridadTitle");
            //Bind data for edit mode
            await Task.Run(() =>
            {
                if (navigationData is Usuario registro && registro != null)
                {
                    _registro = registro;
                    if (registro.Origen == "LoginView")
                    {
                        ResetForm();
                    }
                    else
                    {
                        _isEditMode = registro.EsEdicion == 1 ? true : false;                        
                        PageTitle = TextsTranslateManager.Translate("EditUsuarioSeguridadTitle");
                        BindExistingData(registro);
                    }
                }
                else
                {
                    Load();
                }
            });
            IsBusy = false;
        }

        /// <summary>
        /// Método para almacenar información
        /// </summary>
        /// <returns></returns>
        private async Task Ok_CommandAsync()
        {
            IsBusy = true;
            //1. Validar información
            bool result = Validate();
            if (!result)
            {
                IsBusy = false;
                return;
            }
            _isRegistroExiste = _isEditMode;
            var iregistro = new Usuario
            {
                Id = _isRegistroExiste ? Id : Generator.GenerateKey(),
                IdPersona = IdPersona,
                Nombre = Nombre,
                Nombres = Nombres.Value,
                Apellidos = Apellidos.Value,
                Identificacion = Identificacion.Value,
                Celular = Celular.Value,
                Email = Email.Value,
                EmailAnterior=EmailAnterior,
                Clave = Encriptar.encriptarRJC(Clave.Value),
                ClaveAnterior= Encriptar.encriptarRJC(ClaveAnterior),
                PinDigitos = Encriptar.encriptarRJC(PinDigitos.Value),
                IdToken = IdToken,
                Imagen = _galeria != null ? _galeria.Image : Imagen,
                IdPerfil = IdPerfil,
                Perfil = Perfil,
                EstaActivo = EstaActivo ? 1 : 0,           
                Origen=_registro!=null?_registro.Origen:""
            };
            iregistro.EsEdicion = _isRegistroExiste ? 1 : 0;
            string informedMessage = "";

            if (_isEditMode)
            {
                ConvertBackModelToEntity();
            }
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            //0. Añadir imagen a Galeria
            //2. Definición de información de Galeria
            if (_galeria != null)
            {
                _galeria.Nombre = Identificacion.Value;  //Nombre de imagen
                _galeria.GuardarStorage = 1;     //Almacenar Imagen en Storage            
                var resultadoimagen = await _switchIteraccion.GuardaRegistro_Galeria(_galeria, estaConectado, _isRegistroExiste);
            }
            //1. Añadir el nuevo usuario
            var resultado = await _switchSeguridad.GuardaRegistro_Usuario(iregistro, estaConectado, _isRegistroExiste);

            //2. Añadir el nuevo registro de persona
            var consulta = (await _switchCliente.ConsultaPersonas(new Persona() { Identificacion = Identificacion.Value },estaConectado));
            var existePersona = consulta!=null && consulta.Count() > 0 ? true : false;
            var iregistro1 = new Persona
            {
                Id = consulta!=null && existePersona? consulta.FirstOrDefault().Id:""
            };
            if (!existePersona)
            {
                iregistro1 = new Persona
                {
                    Id = Generator.GenerateKey(),
                    Identificacion = Identificacion.Value,
                    EsNatural = Identificacion.Value.Length > 10 ? 0 : 1
                };
                var resultado1 = await _switchCliente.GuardaRegistro_Persona(iregistro1, estaConectado, existePersona);
            }
            //3. Añadir el nuevo registro de Empresa en caso de serlo
            var consultaEmpresa = (await _switchAdministracion.ConsultaEmpresas(new Empresa() { Identificacion = Identificacion.Value }, estaConectado));
            var existeEmpresa = consultaEmpresa!=null && consultaEmpresa.Count() > 0 ? true : false;
            var iregistroEmpresa = new Empresa
            {
                Id = consultaEmpresa!=null && existeEmpresa ? consultaEmpresa.FirstOrDefault().Id:""
            };
            if (!existeEmpresa && Identificacion.Value.Length > 10)
            {            
                iregistroEmpresa = new Empresa
                {
                    Id = Generator.GenerateKey(),
                    Codigo = Nombre.Replace(" ", ""),
                    Descripcion = Nombre,
                    Identificacion = Identificacion.Value,
                    Direccion = "",
                    Celular = Celular.Value,
                    Email = Email.Value,
                    TelefonoFijo = "",
                    Representante = "",
                    EsProveedor = 0,
                    PorDefecto = 0,
                    IdGaleria = _galeria != null ? _galeria.Id : IdGaleria,
                    Imagen = _galeria != null ? _galeria.Image : Imagen,
                    NoEsConsulta = 1
                };
                iregistroEmpresa.EsEdicion = existePersona ? 1 : 0;
                var resultadoEmpresa = await _switchAdministracion.GuardaRegistro_Empresa(iregistroEmpresa, estaConectado, existePersona);
            }
            //4. Añadir los datos del Cliente
            var consultaCliente = (await _switchCliente.ConsultaCliente(new Cliente() { Identificacion = Identificacion.Value }, estaConectado));
            var existeCliente = consultaCliente!=null ? true : false;
            var iregistroCliente = new Cliente
            {
                Id = consultaCliente!=null? consultaCliente.Id:""
            };
            if (!existeCliente)
            {
                iregistroCliente = new Cliente
                {
                    Id = Generator.GenerateKey(),
                    IdEmpresa = iregistroEmpresa.Id,
                    Empresa = Nombre,
                    Identificacion = Identificacion.Value,
                    Nombre = Nombre,
                    IdUnidadOrganizacion = "",
                    UnidadOrganizacion = "",
                    IdGenero = "",
                    Genero = "",
                    FechaNacimiento = 0,
                    Direccion = "",
                    Celular = Celular.Value,
                    TelefonoFijo = "",
                    Email = Email.Value,
                    PorcentajeDescuento = 0,
                    Descripcion = "",
                    EsContacto = 0,
                    EsPorDefecto = 0,
                    IdGaleria = _galeria != null ? _galeria.Id : IdGaleria,
                    Imagen = _galeria != null ? _galeria.Image : Imagen,

                };
                iregistroCliente.EsEdicion = existePersona ? 1 : 0;
                iregistroCliente.IdPersona = iregistro1.Id;
                var resultado2 = await _switchCliente.GuardaRegistro_Cliente(iregistroCliente, estaConectado, existePersona);
            }
            if (_isRegistroExiste)
            {
                await NavigationService.RemoveLastFromBackStackAsync();
            }
            else
            {
                ResetForm();
            }
            IsBusy = false;
            if(SettingsOnline.oAplicacion!=null)
                SettingsOnline.oAplicacion.ImagenUsuario = iregistro.Imagen;
            informedMessage = resultado.mensaje;
            DialogService.ShowToast(informedMessage);
            if (IsVisibleBotones)
            {
                await NavigationService.RemoveModalAsync();
            }
        }
        /// <summary>
        /// Método de Asignación de valores recibidos por parámetros
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        private async Task BindExistingData(Usuario iregistro)
        {
            Id = iregistro.Id;
            IdPersona = iregistro.IdPersona;
            Nombre = iregistro.Nombre;
            Nombres.Value = iregistro.Nombres;
            Apellidos.Value = iregistro.Apellidos;
            Identificacion.Value = iregistro.Identificacion;                        
            Celular.Value = iregistro.Celular;
            Email.Value = iregistro.Email;
            EmailAnterior= iregistro.Email;
            Clave.Value = Encriptar.Desencriptar(iregistro.Clave);
            ClaveAnterior = Encriptar.Desencriptar(iregistro.Clave);
            PinDigitos.Value = Encriptar.Desencriptar(iregistro.PinDigitos);
            IdPerfil = iregistro.IdPerfil;
            Perfil = iregistro.Perfil;            
            EstaActivo = iregistro.EstaActivo == 1 ? true : false;
            IdGaleria = iregistro.IdGaleria;
            Imagen = iregistro.Imagen;
            if (Imagen != null)
            {
                CargaImagen(4);
            }
        }
        private void ConvertBackModelToEntity()
        {

            _registro.Id = Id;
            _registro.IdPersona = IdPersona;
            _registro.Nombre = Nombre;
            _registro.Nombres = Nombres.Value;
            _registro.Apellidos = Apellidos.Value;
            _registro.Identificacion = Identificacion.Value;
            _registro.Celular = Celular.Value;
            _registro.Email = Email.Value;
            _registro.Clave = Clave.Value;
            _registro.PinDigitos = PinDigitos.Value;
            _registro.IdToken = IdToken;
            _registro.IdGaleria = IdGaleria;
            _registro.Imagen = Imagen;
            _registro.IdPerfil = IdPerfil;
            _registro.Perfil = Perfil;
            _registro.EstaActivo = EstaActivo?1:0;            
        }
       

        #endregion 4.MétodosAdministraciónDatos

        #region 5.MétodosGenerales
        /// <summary>
        /// Reiniciar valores de controles
        /// </summary>
        private void ResetForm()
        {
            Id="";
            IdPersona="";
            Nombre="";
            Nombres.Value="";
            Apellidos.Value="";
            Identificacion.Value="";
            Celular.Value="";
            Email.Value="";
            EmailAnterior = "";
            Clave.Value = "";
            ClaveAnterior = "";
            PinDigitos.Value = "";
            IdToken = "";
            Imagen = "";
            IdPerfil = _registro.Origen == "LoginView" ? "fe132fa3-8691-40c4-b573-0c803100bfc7" : ""; 
            Perfil = _registro.Origen=="LoginView"?"Visitante":TextsTranslateManager.Translate("SelectPerfil");
            IsEnabledPerfil = _registro.Origen == "LoginView" ? false : true;
            IsVisibleBotones = _registro.Origen == "LoginView" ? true : false;
            EstaActivo =true;

        FuenteImagenRegistro = ImageSource.FromStream(() =>
            {
                var stream = new MemoryStream();
                return stream;
            });

            _isEditMode = false;
            _isRegistroExiste = false;            
        }
        /// <summary>
        /// Método para iniciar instancia de Controles de Validación
        /// </summary>
        public void inicioControlesValidacion()
        {

            _nombres = new ValidatableObject<string>();
            _apellidos = new ValidatableObject<string>();
            _identificacion = new ValidatableObject<string>();
            _celular = new ValidatableObject<string>();
            _email = new ValidatableObject<string>();
            _clave = new ValidatableObject<string>();
            _pinDigitos = new ValidatableObject<string>();

        }
        /// <summary>
        /// Método para definir las validaciones en pantalla
        /// </summary>
        private void AddValidations()
        {            
            _nombres.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _apellidos.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _identificacion.Validations.Add(new NumerosRule<string>(""));            
            _celular.Validations.Add(new NumerosRule<string>(""));
            _email.Validations.Add(new EmailRule<string>(""));
            _clave.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _pinDigitos.Validations.Add(new NumerosRule<string>(""));            

        }
        /// <summary>
        /// Método para ejecutar la validación de los controles de pantalla
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {                        

            IsNombresValid = !Nombres.Validate();
            IsApellidosValid = !Apellidos.Validate();
            IsIdentificacionValid = !Identificacion.Validate();
            IsCelularValid = !Celular.Validate();
            IsEmailValid = !Email.Validate();            
            IsClaveValid = !Clave.Validate();            
            IsPinDigitosValid = !PinDigitos.Validate();


            return !IsNombresValid
                && !IsApellidosValid
                && !IsIdentificacionValid
                && !IsCelularValid
                && !IsCelularValid
                && !IsEmailValid
                && !IsClaveValid
                && !IsPinDigitosValid;
        }
        /// <summary>
        /// Método para salir de la pantalla
        /// </summary>
        /// <returns></returns>
        private async Task Cancel_CommandAsync()
        {
            await NavigationService.RemoveLastFromBackStackAsync();
        }
        
        
        public async Task OpenPerfilAsync()
        {
            PerfilSeleccionado = new Catalogo()
            {
                CodigoCatalogo = "perfil"
            ,
                EsJerarquico = 0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(PerfilSeleccionado);
        }        

        private async Task BrowseGalleryAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "Usuario",
                GuardarStorage = 0
            };
            galeria.Nombre = Identificacion.Value == "" ? galeria.Id : Identificacion.Value;
            _galeria = await generales.PickAndShowFile(galeria);
            await CargaImagen(2);
        }

        private async Task OpenCameraAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "Usuario",
                GuardarStorage = 0
            };
            galeria.Nombre = Identificacion.Value == "" ? galeria.Id : Identificacion.Value;
            _galeria = await generales.TakePhoto(galeria);
            await CargaImagen(1);
        }
        private async Task RemoveImageAsync()
        {
            await CargaImagen(3);

        }
        private async Task CargaImagen(int Origen)
        {
            switch (Origen)
            {
                case 1:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(_galeria.Image));
                        return stream;
                    });
                    break;
                case 2:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(_galeria.Image));
                        return stream;
                    });
                    break;
                case 3:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = new MemoryStream();
                        return stream;
                    });
                    break;
                case 4:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(Imagen));
                        return stream;
                    });
                    break;
            }
        }

        private async Task NombresUnfocused_CommandAsync()
        {
            await eventoNombre();

        }
        private async Task ApellidosUnfocused_CommandAsync()
        {
            await eventoNombre();

        }
        private async Task eventoNombre()
        {
            Nombre = Nombres.Value + " " + Apellidos.Value;

        }
        #endregion 5.MétodosGenerales
    }
}
