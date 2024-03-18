using DataModel.DTO.Administracion;
using DataModel.DTO.Clientes;
using DataModel.DTO.Iteraccion;
using DataModel.Helpers;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Switch;
using Escant_App.Validations;
using Escant_App.ViewModels.Administracion;
using Escant_App.ViewModels.Base;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Clientes
{
    class AddClienteViewModel:ViewModelBase
    {

        #region 0.DeclaracionVariables
        private SwitchIteraccion _switchIteraccion;
        private SwitchCliente _switchCliente;
        private Generales.Generales generales;
        
        private readonly IServicioClientes_Cliente _clienteServicio;
        private readonly IServicioClientes_Persona _personaServicio;
        private readonly IServicioIteraccion_Galeria _galeriaServicio;

        /// 0.1 Referencias a instancias de objetos a utilizarse en la Clase        
        private Cliente _registro;
        private Galeria _galeria;


        private bool _isEditMode;

        /// <summary>
        /// 0.2 Declaración de las variables de lectura y escritura para los controles
        /// </summary>
        private ImageSource _fuenteImagenRegistro = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });
        private bool _esPersonaNatural;
        private Empresa _empresaSeleccionada;
        private string _id;
        private string _idEmpresa;
        private string _empresa;
        private string _idPersona;
        private ValidatableObject<string> _identificacion;
        private ValidatableObject<string> _nombre;
        private string _idUnidadOrganizacion;
        private string _unidadOrganizacion;
        private string _idGenero;
        private string _genero;
        private Catalogo _generoSeleccionado;
        private DateTime _fechaNacimiento;
        private ValidatableObject<string> _direccion;
        private ValidatableObject<string> _celular;
        private string _telefonoFijo;
        private ValidatableObject<string> _email;
        private float _porcentajeDescuento;
        private Catalogo _unidadOrganizacionSeleccionado;
        private Catalogo _porcentajeDescuentoSeleccionado;
        private string _descripcion;
        private bool _esContacto;
        private bool _esPorDefecto;
        private string _idGaleria;
        private string _imagen;


        /// <summary>
        /// 0.3 Declaración variables control de datos
        /// </summary>

        private bool _esEmpresa;
        
        private bool _isRegistroExiste;
        private bool _isRegistro1Existe;
        
        private bool _isVisibleEmpresaSeleccionada;
        private bool _isVisibleGeneroSeleccionado;


        /// <summary>
        /// 0.4 Declaración de las variables de control de errores
        /// </summary>
        private bool _isIdentificacionValid;
        private bool _isNombreValid;
        private bool _isUnidadOrganizacionValid;
        private bool _isGeneroSeleccionadoValid;
        private bool _isFechaNacimientoValid;
        private bool _isDireccionValid;
        private bool _isCelularValid;
        private bool _isTelefonoFijoValid;
        private bool _isEmailValid;
        private bool _isEsPorDefectoValid;

        public CultureInfo Cultura => Escant_App.AppSettings.Settings.CustomCurrencySymbol == "$" ? new System.Globalization.CultureInfo("en-US") : new System.Globalization.CultureInfo("en-US");
        #endregion DeclaracionVariables

        #region 1.Constructor
        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public AddClienteViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioAdministracion_Catalogo catalogoServicio
                                  , IServicioClientes_Cliente clienteServicio
                                  , IServicioClientes_Persona personaServicio
                                  , IServicioIteraccion_Galeria galeriaServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);
            //Instanciación Acorde al model
            _clienteServicio = clienteServicio;
            _personaServicio = personaServicio;
            _galeriaServicio = galeriaServicio;
            _switchCliente = new SwitchCliente(_clienteServicio,_personaServicio);
            _switchIteraccion = new SwitchIteraccion(_galeriaServicio);
            generales = new Generales.Generales(catalogoServicio, galeriaServicio);
            inicioControlesValidacion();
            AddValidations();
            MessagingCenter.Unsubscribe<Empresa>(this, Escant_App.AppSettings.Settings.EmpresaSeleccionadaAddClienteView);
            MessagingCenter.Subscribe<Empresa>(this, Escant_App.AppSettings.Settings.EmpresaSeleccionadaAddClienteView, (args) =>
            {
                EmpresaSeleccionada = args;
                cargaDatosExternos();
            });
            MessagingCenter.Unsubscribe<Catalogo>(this, Escant_App.AppSettings.Settings.CatalogoSeleccionadoGeneroAddClienteView);
            MessagingCenter.Subscribe<Catalogo>(this, Escant_App.AppSettings.Settings.CatalogoSeleccionadoGeneroAddClienteView, (args) =>
            {                
                GeneroSeleccionado = args.CodigoCatalogo=="genero"?args:GeneroSeleccionado;
                Genero = GeneroSeleccionado!=null && GeneroSeleccionado.Nombre!=null?GeneroSeleccionado.Nombre:Genero;
                PorcentajeDescuentoSeleccionado = args.CodigoCatalogo == "tipodescuento" ? args : PorcentajeDescuentoSeleccionado;
                UnidadOrganizacionSeleccionado = args.CodigoCatalogo == "tipopersona" ? args : UnidadOrganizacionSeleccionado;
                UnidadOrganizacion = UnidadOrganizacionSeleccionado.Nombre;
                UnidadOrganizacion = UnidadOrganizacionSeleccionado != null && UnidadOrganizacionSeleccionado.Nombre != null ? UnidadOrganizacionSeleccionado.Nombre : UnidadOrganizacion;
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
        public bool EsEmpresa
        {
            get => _esEmpresa;
            set
            {
                _esEmpresa = value;
                RaisePropertyChanged(() => EsEmpresa);
                EsPersonaNatural = !EsEmpresa;
                eventoEsEmpresa();
            }
        }

        public bool IsVisibleEmpresaSeleccionada
        {
            get => _isVisibleEmpresaSeleccionada;
            set
            {
                _isVisibleEmpresaSeleccionada = value;
                RaisePropertyChanged(() => IsVisibleEmpresaSeleccionada);                
            }
        }

        public bool IsVisibleGeneroSeleccionado
        {
            get => _isVisibleGeneroSeleccionado;
            set
            {
                _isVisibleGeneroSeleccionado = value;
                RaisePropertyChanged(() => IsVisibleGeneroSeleccionado);
            }
        }


        public bool EsPersonaNatural
        {
            get => _esPersonaNatural;
            set
            {
                _esPersonaNatural = value;
                RaisePropertyChanged(() => EsPersonaNatural);
            }
        }

        public Empresa EmpresaSeleccionada
        {
            get => _empresaSeleccionada;
            set
            {
                _empresaSeleccionada = value;
                RaisePropertyChanged(() => EmpresaSeleccionada);
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
        public string IdEmpresa
        {
            get => _idEmpresa;
            set
            {
                _idEmpresa = value;
                RaisePropertyChanged(() => IdEmpresa);
            }
        }
        public string Empresa
        {
            get => _empresa;
            set
            {
                _empresa = value;
                RaisePropertyChanged(() => Empresa);
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
        public ValidatableObject<string> Identificacion
        {
            get => _identificacion;
            set
            {
                _identificacion = value;
                RaisePropertyChanged(() => Identificacion);
            }
        }
        public ValidatableObject<string> Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                RaisePropertyChanged(() => Nombre);
            }
        }
        public string IdUnidadOrganizacion
        {
            get => _idUnidadOrganizacion;
            set
            {
                _idUnidadOrganizacion = value;
                RaisePropertyChanged(() => IdUnidadOrganizacion);
            }
        }
        public string UnidadOrganizacion
        {
            get => _unidadOrganizacion;
            set
            {
                _unidadOrganizacion = value;
                RaisePropertyChanged(() => UnidadOrganizacion);
                eventoUnidadOrganizacion();
            }
        }
        public string IdGenero
        {
            get => _idGenero;
            set
            {
                _idGenero = value;
                RaisePropertyChanged(() => IdGenero);
            }
        }
        public string Genero
        {
            get => _genero;
            set
            {
                _genero = value;
                RaisePropertyChanged(() => Genero);
            }
        }
        public Catalogo GeneroSeleccionado
        {
            get => _generoSeleccionado;
            set
            {
                _generoSeleccionado = value;
                RaisePropertyChanged(() => GeneroSeleccionado);
            }
        }
        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set
            {
                _fechaNacimiento = value;
                RaisePropertyChanged(() => FechaNacimiento);
            }
        }
        public ValidatableObject<string> Direccion
        {
            get => _direccion;
            set
            {
                _direccion = value;
                RaisePropertyChanged(() => Direccion);
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
        public string TelefonoFijo
        {
            get => _telefonoFijo;
            set
            {
                _telefonoFijo = value;
                RaisePropertyChanged(() => TelefonoFijo);
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
        public float PorcentajeDescuento
        {
            get =>  _porcentajeDescuento;
            set
            {
                 _porcentajeDescuento = value;
                RaisePropertyChanged(() => PorcentajeDescuento);
            }
        }

        public Catalogo UnidadOrganizacionSeleccionado
        {
            get => _unidadOrganizacionSeleccionado;
            set
            {
                _unidadOrganizacionSeleccionado = value;
                RaisePropertyChanged(() => UnidadOrganizacionSeleccionado);
            }
        }

        public Catalogo PorcentajeDescuentoSeleccionado
        {
            get => _porcentajeDescuentoSeleccionado;
            set
            {
                _porcentajeDescuentoSeleccionado = value;
                RaisePropertyChanged(() => PorcentajeDescuentoSeleccionado);
            }
        }

        public string Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                RaisePropertyChanged(() => Descripcion);
            }
        }
        public bool EsContacto
        {
            get => _esContacto;
            set
            {
                _esContacto = value;
                RaisePropertyChanged(() => EsContacto);
            }
        }
        public bool EsPorDefecto
        {
            get => _esPorDefecto;
            set
            {
                _esPorDefecto = value;
                RaisePropertyChanged(() => EsPorDefecto);
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

        public bool IsIdentificacionValid
        {
            get => _isIdentificacionValid;
            set
            {
                _isIdentificacionValid = value;
                RaisePropertyChanged(() => IsIdentificacionValid);
            }
        }
        public bool IsNombreValid
        {
            get => _isNombreValid;
            set
            {
                _isNombreValid = value;
                RaisePropertyChanged(() => IsNombreValid);
            }
        }

        public bool IsUnidadOrganizacionValid
        {
            get => _isUnidadOrganizacionValid;
            set
            {
                _isUnidadOrganizacionValid = value;
                RaisePropertyChanged(() => IsUnidadOrganizacionValid);
            }
        }

        public bool IsGeneroSeleccionadoValid
        {
            get => _isGeneroSeleccionadoValid;
            set
            {
                _isGeneroSeleccionadoValid = value;
                RaisePropertyChanged(() => IsGeneroSeleccionadoValid);
            }
        }
        public bool IsFechaNacimientoValid
        {
            get => _isFechaNacimientoValid;
            set
            {
                _isFechaNacimientoValid = value;
                RaisePropertyChanged(() => IsFechaNacimientoValid);
            }
        }
        public bool IsDireccionValid
        {
            get => _isDireccionValid;
            set
            {
                _isDireccionValid = value;
                RaisePropertyChanged(() => IsDireccionValid);
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
        public bool IsTelefonoFijoValid
        {
            get => _isTelefonoFijoValid;
            set
            {
                _isTelefonoFijoValid = value;
                RaisePropertyChanged(() => IsTelefonoFijoValid);
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



        public bool IsEsPorDefectoValid
        {
            get => _isEsPorDefectoValid;
            set
            {
                _isEsPorDefectoValid = value;
                RaisePropertyChanged(() => IsEsPorDefectoValid);
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
        public ICommand AddRegistro_ContactCommand => new Command(async () => await AddRegistro_ContactAsync());
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());
        public ICommand OpenUnidadOrganizacionCommand => new Command(async () => await OpenUnidadOrganizacionAsync());
        public ICommand OpenEmpresaCommand => new Command(async () => await OpenEmpresaAsync());
        public ICommand OpenGeneroCommand => new Command(async () => await OpenGeneroAsync());
        public ICommand OpenPorcentajeDescuentoCommand => new Command(async () => await OpenPorcentajeDescuentoAsync());
        

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
            PageTitle = TextsTranslateManager.Translate("AddClienteClientesTitle");
            //Bind data for edit mode
            await Task.Run(() =>
            {
                if (navigationData is Cliente registro && registro != null)
                {
                    _isEditMode = registro.EsEdicion == 1 ? true : false;
                    _registro = registro;
                    PageTitle = TextsTranslateManager.Translate("EditClienteClientesTitle");
                    BindExistingData(registro);
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
            var iregistro = new Cliente
            {
                Id = _isRegistroExiste ? Id : Generator.GenerateKey(),
                IdEmpresa=IdEmpresa,
                Empresa=Empresa,
                Identificacion = Identificacion.Value,
                Nombre = Nombre.Value,
                IdUnidadOrganizacion = UnidadOrganizacionSeleccionado!=null?UnidadOrganizacionSeleccionado.Id:IdUnidadOrganizacion,
                UnidadOrganizacion = UnidadOrganizacionSeleccionado != null?UnidadOrganizacionSeleccionado.Nombre:UnidadOrganizacion,
                IdGenero = EsEmpresa?"":(GeneroSeleccionado!=null?GeneroSeleccionado.Id:IdGenero),
                Genero = EsEmpresa ? "" : (GeneroSeleccionado != null ? GeneroSeleccionado.Nombre:Genero),
                FechaNacimiento = DateTimeHelpers.GetDate(FechaNacimiento),
                Direccion = Direccion.Value,
                Celular = Celular.Value,
                TelefonoFijo = TelefonoFijo,
                Email = Email.Value,                
                PorcentajeDescuento = PorcentajeDescuento,
                Descripcion = Descripcion,
                EsContacto = EsContacto ? 1 : 0,
                EsPorDefecto = EsPorDefecto ? 1 : 0,
                IdGaleria = _galeria != null ? _galeria.Id : IdGaleria,
                Imagen = _galeria != null ? _galeria.Image : Imagen,

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
            //1. Añadir el nuevo registro de persona
            var iregistro1 = new Persona
            {
                Id = _isRegistro1Existe ? Id : Generator.GenerateKey(),
                Identificacion = Identificacion.Value,
                EsNatural = EsPersonaNatural?1:0
            };
            var resultado1 = await _switchCliente.GuardaRegistro_Persona(iregistro1, estaConectado, _isRegistro1Existe);
            //2. Añadir los datos del Cliente
            iregistro.IdPersona = iregistro1.Id;
            var resultado = await _switchCliente.GuardaRegistro_Cliente(iregistro, estaConectado, _isRegistroExiste);
            if (_isRegistroExiste)
            {
                await NavigationService.RemoveLastFromBackStackAsync();
            }
            else
            {
                ResetForm();
            }
            IsBusy = false;
            informedMessage = resultado.mensaje;
            DialogService.ShowToast(informedMessage);
        }
        /// <summary>
        /// Método de Asignación de valores recibidos por parámetros
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        private async Task BindExistingData(Cliente iregistro)
        {
            Id = iregistro.Id;
            IdEmpresa = iregistro.IdEmpresa;
            Empresa = iregistro.Empresa;
            IdPersona = iregistro.IdPersona;
            Identificacion.Value = iregistro.Identificacion;
            Nombre.Value = iregistro.Nombre;
            IdUnidadOrganizacion = iregistro.IdUnidadOrganizacion;
            UnidadOrganizacion = iregistro.UnidadOrganizacion;
            IdGenero= iregistro.IdGenero;
            Genero = iregistro.Genero;
            FechaNacimiento = DateTimeHelpers.GetDateLong(iregistro.FechaNacimiento);
            Direccion.Value = iregistro.Direccion;
            Celular.Value = iregistro.Celular;
            TelefonoFijo = iregistro.TelefonoFijo;
            Email.Value = iregistro.Email;
            PorcentajeDescuento = iregistro.PorcentajeDescuento.HasValue?iregistro.PorcentajeDescuento.Value:0;
            Descripcion = iregistro.Descripcion;
            EsContacto = iregistro.EsContacto==1?true:false;
            EsPorDefecto = iregistro.EsPorDefecto==1?true:false;
            IdGaleria = iregistro.IdGaleria;
            Imagen = iregistro.Imagen;            
            if (Imagen != null)
            {
                CargaImagen(4);
            }
        }
        private void ConvertBackModelToEntity()
        {
            
            _registro.Descripcion = Descripcion;
            _registro.Identificacion = Identificacion.Value;
            _registro.Nombre = Nombre.Value;
            _registro.UnidadOrganizacion = UnidadOrganizacion;
            _registro.Genero = Genero;
            _registro.Direccion = Direccion.Value;
            _registro.Celular = Celular.Value;
            _registro.Email = Email.Value;
            _registro.TelefonoFijo = TelefonoFijo;
            _registro.EsContacto = EsContacto ? 1 : 0;
            _registro.EsPorDefecto = EsPorDefecto ? 1 : 0;
            _registro.IdGaleria = IdGaleria;
            _registro.Imagen = Imagen;
        }

        private void cargaDatosExternos()
        {
            IdEmpresa = EmpresaSeleccionada.Id;
            Empresa = EmpresaSeleccionada.Descripcion;
            Identificacion.Value = EmpresaSeleccionada.Identificacion;
            Nombre.Value = EmpresaSeleccionada.Descripcion;
            //IdUnidadOrganizacion = "";
            //UnidadOrganizacion.Value = "";
            //IdGenero = "";
            //Genero.Value = "";
            //FechaNacimiento = DateTimeHelpers.GetDateActual();
            Direccion.Value = EmpresaSeleccionada.Direccion;
            Celular.Value = EmpresaSeleccionada.Celular;
            TelefonoFijo = EmpresaSeleccionada.TelefonoFijo;
            Email.Value = EmpresaSeleccionada.Email;
            PorcentajeDescuento = 0;
            Descripcion = "";
            EsContacto = false;
            EsPorDefecto = EmpresaSeleccionada.PorDefecto==1?true:false;
            IdGaleria = EmpresaSeleccionada.IdGaleria;
            Imagen = EmpresaSeleccionada.Imagen;
            if (Imagen != null)
            {
                CargaImagen(4);
            }

        }

        #endregion 4.MétodosAdministraciónDatos

        #region 5.MétodosGenerales
        /// <summary>
        /// Reiniciar valores de controles
        /// </summary>
        private void ResetForm()
        {
            IdEmpresa = "";
            Empresa = TextsTranslateManager.Translate("SelectEmpresa");
            Identificacion.Value = "";
            Nombre.Value = "";
            IdUnidadOrganizacion = "";
            UnidadOrganizacion= TextsTranslateManager.Translate("SelectUnidadOrganizacion");
            UnidadOrganizacionSeleccionado = null;
            IdGenero = "";
            Genero = TextsTranslateManager.Translate("SelectGenero");
            GeneroSeleccionado = null;
            FechaNacimiento = DateTimeHelpers.GetDateActual();
            Direccion.Value = "";
            Celular.Value = "";
            TelefonoFijo = "";
            Email.Value = "";
            PorcentajeDescuento = 0;
            Descripcion = "";
            EsContacto = false;
            EsPorDefecto = false;
            IdGaleria = "";
            Imagen = "";
            EsEmpresa = false;
            IsVisibleEmpresaSeleccionada = false;
            IsVisibleGeneroSeleccionado = true;

            FuenteImagenRegistro = ImageSource.FromStream(() =>
            {
                var stream = new MemoryStream();
                return stream;
            });

            _isEditMode = false;
            _isRegistroExiste = false;
            _isRegistro1Existe = false;
        }
        /// <summary>
        /// Método para iniciar instancia de Controles de Validación
        /// </summary>
        public void inicioControlesValidacion()
        {
            
            _identificacion = new ValidatableObject<string>();
            _nombre = new ValidatableObject<string>();            
//            _genero = new ValidatableObject<string>();
            _direccion = new ValidatableObject<string>();
            _celular = new ValidatableObject<string>();
            _email = new ValidatableObject<string>();
            //_telefonoFijo = new ValidatableObject<string>();
            
        }
        /// <summary>
        /// Método para definir las validaciones en pantalla
        /// </summary>
        private void AddValidations()
        {
            _identificacion.Validations.Add(new NumerosRule<string>(""));
            _nombre.Validations.Add(new IsNotNullOrEmptyRule<string>(""));            
  //          _genero.Validations.Add(new IsNotNullOrEmptyRule<string>(""));            
            _direccion.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _celular.Validations.Add(new NumerosRule<string>(""));
            _email.Validations.Add(new EmailRule<string>(""));
            //_telefonoFijo.Validations.Add(new NumerosRule<string>(""));            
        }
        /// <summary>
        /// Método para ejecutar la validación de los controles de pantalla
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {                        
            IsIdentificacionValid = !Identificacion.Validate();
            IsNombreValid = !Nombre.Validate();            
    //        IsGeneroSeleccionadoValid = !Genero.Validate();
            IsDireccionValid = !Direccion.Validate();
            IsCelularValid = !Celular.Validate();
            IsEmailValid = !Email.Validate();
            //IsTelefonoFijoValid = !TelefonoFijo.Validate();
            

            return !IsIdentificacionValid
                && !IsNombreValid
                && !IsUnidadOrganizacionValid
                && !IsDireccionValid
                && !IsCelularValid
                && !IsEmailValid
                //&& !IsTelefonoFijoValid
                ;
        }
        /// <summary>
        /// Método para salir de la pantalla
        /// </summary>
        /// <returns></returns>
        private async Task Cancel_CommandAsync()
        {
            await NavigationService.RemoveLastFromBackStackAsync();
        }
        /// <summary>
        /// Evento cuando se escoge la opción EsProveedor
        /// </summary>
        /// <returns></returns>
        private async Task eventoEsEmpresa()
        {
            if (EsEmpresa)
            {
                IsVisibleEmpresaSeleccionada = true;
                IsVisibleGeneroSeleccionado = false;
            }
            else
            {
                IsVisibleEmpresaSeleccionada = false;
                IsVisibleGeneroSeleccionado = true;
            }
        }
        private async Task eventoUnidadOrganizacion()
        {
            if (UnidadOrganizacion.Contains("ridica"))            
                EsEmpresa = true;            
            else            
                EsEmpresa = false;            
        }
        public async Task OpenUnidadOrganizacionAsync()
        {
            UnidadOrganizacionSeleccionado = new Catalogo()
            {
                CodigoCatalogo = "tipopersona"
            ,
                EsJerarquico = 0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(UnidadOrganizacionSeleccionado);
        }
        /// <summary>
        /// Abrir pantalla para selección de tipo de proveedor
        /// </summary>
        /// <returns></returns>
        public async Task OpenEmpresaAsync()
        {            
            await NavigationService.NavigateToAsync<EmpresaViewModel>(EmpresaSeleccionada);
        }
        /// <summary>
        /// Abrir pantalla para selección de género
        /// </summary>
        /// <returns></returns>
        public async Task OpenGeneroAsync()
        {
            GeneroSeleccionado = new Catalogo()
            {
                CodigoCatalogo = "genero"
            , EsJerarquico=0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(GeneroSeleccionado);
        }
        public async Task OpenPorcentajeDescuentoAsync()
        {
            PorcentajeDescuentoSeleccionado = new Catalogo()
            {
                CodigoCatalogo = "tipodescuento"
            ,
                EsJerarquico = 0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(PorcentajeDescuentoSeleccionado);
        }

        private async Task BrowseGalleryAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "Cliente",
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
                Directorio = "Cliente",
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
        public async Task AddRegistro_ContactAsync()
        {
            try
            {
                ResetForm();
                var contact = await Contacts.PickContactAsync();

                if (contact == null)
                    return;

                var id = contact.Id;
                var namePrefix = contact.NamePrefix;
                var givenName = contact.GivenName;
                var middleName = contact.MiddleName;
                var familyName = contact.FamilyName;
                var nameSuffix = contact.NameSuffix;
                var displayName = contact.DisplayName;
                var phones = contact.Phones; // List of phone numbers
                var emails = contact.Emails; // List of email addresses
                Nombre.Value = displayName;//givenName + " " + middleName + " " + familyName;                
                Identificacion.Value = id;
                Celular.Value = phones.FirstOrDefault().PhoneNumber.ToString().Replace(" ", "");
                Email.Value = emails.FirstOrDefault().EmailAddress;
                Direccion.Value = string.Empty;
            }
            catch (Exception ex)
            {
                // Handle exception here.
            }
        }

        #endregion 5.MétodosGenerales

    }
}
