using DataModel.DTO.Administracion;
using DataModel.DTO.Iteraccion;
using DataModel.Helpers;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Switch;
using Escant_App.Validations;
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

namespace Escant_App.ViewModels.Administracion
{
    class AddEmpresaViewModel : ViewModelBase
    {
        #region 0.DeclaracionVariables
        /// <summary>
        /// 0. Declaración de las variables                 
        /// </summary>
        /// 0.0 Referencias a otras clases
        private SwitchAdministracion _switchAdministracion;
        private SwitchIteraccion _switchIteraccion;
        private Generales.Generales generales;

        private readonly IServicioAdministracion_Empresa _empresaServicio;
        private readonly IServicioAdministracion_Proveedor _proveedorServicio;
        private readonly IServicioIteraccion_Galeria _galeriaServicio;

        /// 0.1 Referencias a instancias de objetos a utilizarse en la Clase        
        private Empresa _registro;
        private Proveedor _proveedor;
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
        private ValidatableObject<string> _codigo;
        private ValidatableObject<string> _descripcion;
        private ValidatableObject<string> _identificacion;
        private ValidatableObject<string> _direccion;
        private ValidatableObject<string> _celular;
        private ValidatableObject<string> _email;
        private ValidatableObject<string> _telefonoFijo;
        private ValidatableObject<string> _representante;
        private bool _esProveedor;
        private Catalogo _tipoProveedorSeleccionado;
        private bool _esContribuyenteEspecial;
        private bool _poseeFacturacionElectronica;
        private bool _porDefecto;
        private string _idGaleria;
        private string _imagen;


        /// <summary>
        /// 0.3 Declaración variables control de datos
        /// </summary>

        private bool _isRegistroExiste;
        private bool _isRegistro1Existe;

        private bool _isVisibleTipoProveedor;
        private bool _isVisibleEsContribuyenteEspecial;
        private bool _isVisiblePoseeFacturacionElectronica;

        /// <summary>
        /// 0.4 Declaración de las variables de control de errores
        /// </summary>
        private bool _isCodigoValid;
        private bool _isDescripcionValid;
        private bool _isDireccionValid;
        private bool _isIdentificacionValid;
        private bool _isCelularValid;
        private bool _isEmailValid;
        private bool _isTelefonoFijoValid;
        private bool _isRepresentanteValid;


        #endregion DeclaracionVariables

        #region 1.Constructor
        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public AddEmpresaViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  ,IServicioAdministracion_Empresa empresaServicio
                                  ,IServicioAdministracion_Catalogo catalogoServicio
                                  ,IServicioAdministracion_Proveedor proveedorServicio
                                  ,IServicioIteraccion_Galeria galeriaServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);
            //Instanciación Acorde al model
            _empresaServicio = empresaServicio;
            _proveedorServicio = proveedorServicio;
            _galeriaServicio = galeriaServicio;
            _switchAdministracion = new SwitchAdministracion(_empresaServicio, _proveedorServicio);
            _switchIteraccion = new SwitchIteraccion(_galeriaServicio);
            generales = new Generales.Generales(catalogoServicio,galeriaServicio);
            inicioControlesValidacion();
            AddValidations();
            MessagingCenter.Unsubscribe<Catalogo>(this, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddEmpresaView);
            MessagingCenter.Subscribe<Catalogo>(this, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddEmpresaView, (args) =>
            {
                TipoProveedorSeleccionado = args;                    
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
        public ValidatableObject<string> Codigo
        {
            get => _codigo;
            set
            {
                _codigo = value;
                RaisePropertyChanged(() => Codigo);
            }
        }
        public ValidatableObject<string> Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                RaisePropertyChanged(() => Descripcion);
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
        public ValidatableObject<string> Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }
        public ValidatableObject<string> TelefonoFijo
        {
            get => _telefonoFijo;
            set
            {
                _telefonoFijo = value;
                RaisePropertyChanged(() => TelefonoFijo);
            }
        }
        public ValidatableObject<string> Representante
        {
            get => _representante;
            set
            {
                _representante = value;
                RaisePropertyChanged(() => Representante);
            }
        }
        public bool EsProveedor
        {
            get => _esProveedor;
            set
            {
                _esProveedor = value;
                RaisePropertyChanged(() => EsProveedor);
                eventoProveedor();
            }
        }
        public Catalogo TipoProveedorSeleccionado
        {
            get => _tipoProveedorSeleccionado;
            set
            {
                _tipoProveedorSeleccionado = value;
                RaisePropertyChanged(() => TipoProveedorSeleccionado);
            }
        }
        public bool EsContribuyenteEspecial
        {
            get => _esContribuyenteEspecial;
            set
            {
                _esContribuyenteEspecial = value;
                RaisePropertyChanged(() => EsContribuyenteEspecial);                
            }
        }
        public bool PoseeFacturacionElectronica
        {
            get => _poseeFacturacionElectronica;
            set
            {
                _poseeFacturacionElectronica = value;
                RaisePropertyChanged(() => PoseeFacturacionElectronica);
            }
        }
        public bool PorDefecto
        {
            get => _porDefecto;
            set
            {
                _porDefecto = value;
                RaisePropertyChanged(() => PorDefecto);
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

        public bool IsCodigoValid
        {
            get => _isCodigoValid;
            set
            {
                _isCodigoValid = value;
                RaisePropertyChanged(() => IsCodigoValid);
            }
        }
        public bool IsDescripcionValid
        {
            get => _isDescripcionValid;
            set
            {
                _isDescripcionValid = value;
                RaisePropertyChanged(() => IsDescripcionValid);
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
        public bool IsEmailValid
        {
            get => _isEmailValid;
            set
            {
                _isEmailValid = value;
                RaisePropertyChanged(() => IsEmailValid);
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
        public bool IsRepresentanteValid
        {
            get => _isRepresentanteValid;
            set
            {
                _isRepresentanteValid = value;
                RaisePropertyChanged(() => IsRepresentanteValid);
            }
        }


        public bool IsVisibleTipoProveedor
        {
            get => _isVisibleTipoProveedor;
            set
            {
                _isVisibleTipoProveedor = value;
                RaisePropertyChanged(() => IsVisibleTipoProveedor);
            }
        }
        public bool IsVisibleEsContribuyenteEspecial
        {
            get => _isVisibleEsContribuyenteEspecial;
            set
            {
                _isVisibleEsContribuyenteEspecial = value;
                RaisePropertyChanged(() => IsVisibleEsContribuyenteEspecial);
            }
        }
        public bool IsVisiblePoseeFacturacionElectronica
        {
            get => _isVisiblePoseeFacturacionElectronica;
            set
            {
                _isVisiblePoseeFacturacionElectronica = value;
                RaisePropertyChanged(() => IsVisiblePoseeFacturacionElectronica);
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
        //public ICommand CodigoUnfocused => new Command(async () => await CodigoUnfocused_CommandAsync());
        public ICommand OpenTipoProveedorCommand => new Command(async () => await OpenTipoProveedorAsync());

        public ICommand NombreUnfocused => new Command(async () => await NombreUnfocused_CommandAsync());

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
            PageTitle = TextsTranslateManager.Translate("AddEmpresaAdministracionTitle");
            //Bind data for edit mode
            await Task.Run(() =>
            {
                if (navigationData is Empresa registro && registro != null)
                {
                    _isEditMode = registro.EsEdicion == 1 ? true : false;
                    _registro = registro;
                    PageTitle = TextsTranslateManager.Translate("EditEmpresaAdministracionTitle");
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
            


            var iregistro = new Empresa
            {
                Id = _isRegistroExiste ? Id : Generator.GenerateKey(),
                Codigo = Codigo.Value,
                Descripcion=Descripcion.Value,
                Identificacion=Identificacion.Value,
                Direccion = Direccion.Value,
                Celular =Celular.Value,
                Email=Email.Value,
                TelefonoFijo = TelefonoFijo.Value,
                Representante=Representante.Value,
                EsProveedor=EsProveedor?1:0,
                PorDefecto = PorDefecto ? 1 : 0,
                IdGaleria=_galeria!=null?_galeria.Id:IdGaleria,
                Imagen=_galeria!=null?_galeria.Image:Imagen,
                NoEsConsulta=1
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
                _galeria.Nombre = Codigo.Value;  //Nombre de imagen
                _galeria.GuardarStorage = 1;     //Almacenar Imagen en Storage            
                var resultadoimagen = await _switchIteraccion.GuardaRegistro_Galeria(_galeria, estaConectado, _isRegistroExiste);
            }
            //1. Añadir el nuevo registro
            var resultado = await _switchAdministracion.GuardaRegistro_Empresa(iregistro, estaConectado, _isRegistroExiste);
            //2. Añadir los datos de Proveedor
            if (iregistro.EsProveedor == 1)
            {
                var iregistro1 = new Proveedor
                {
                    Id = _isRegistro1Existe ? Id : Generator.GenerateKey(),
                    IdEmpresa = iregistro.Id,
                    IdTipoProveedor=TipoProveedorSeleccionado.Id,
                    EsContribuyenteEspecial = EsContribuyenteEspecial ? 1 : 0,
                    PoseeFacturaElectronica = PoseeFacturacionElectronica ? 1 : 0,
                    EsEdicion = _isRegistro1Existe ? 1 : 0

                };
                var resultado1 = await _switchAdministracion.GuardaRegistro_Proveedor(iregistro1, estaConectado, _isRegistro1Existe);
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
            informedMessage = resultado.mensaje;
            DialogService.ShowToast(informedMessage);
        }        
        /// <summary>
        /// Método de Asignación de valores recibidos por parámetros
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        private async Task BindExistingData(Empresa iregistro)
        {
            Id = iregistro.Id;
            Codigo.Value = iregistro.Codigo;
            Descripcion.Value = iregistro.Descripcion;
            Identificacion.Value = iregistro.Identificacion;
            Direccion.Value = iregistro.Direccion;
            Celular.Value = iregistro.Celular;
            Email.Value = iregistro.Email;
            TelefonoFijo.Value = iregistro.TelefonoFijo;
            Representante.Value = iregistro.Representante;
            EsProveedor = iregistro.EsProveedor==1?true:false;
            PorDefecto = iregistro.PorDefecto==1?true:false;
            IdGaleria = iregistro.IdGaleria;
            Imagen = iregistro.Imagen;
            if (iregistro.EsProveedor == 1)
            {
                var iproveedor = new Proveedor()
                {
                    IdEmpresa = iregistro.Id
                };
                bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                _proveedor=await _switchAdministracion.ConsultaProveedor(iproveedor, estaConectado);
                if (_proveedor != null)
                {
                    _isRegistro1Existe = true;
                    var icatalogo = new Catalogo()
                    {
                        Id = _proveedor.IdTipoProveedor
                    };
                    var _tipoproveedor = await _switchAdministracion.ConsultaCatalogo(icatalogo, estaConectado);
                    if (_tipoproveedor != null)
                    {
                        TipoProveedorSeleccionado = _tipoproveedor;
                    }
                    EsContribuyenteEspecial = _proveedor.EsContribuyenteEspecial == 1 ? true : false;
                    PoseeFacturacionElectronica = _proveedor.PoseeFacturaElectronica == 1 ? true : false;
                }
                
            }
            if (Imagen != null)
            {
                CargaImagen(4);
            }
        }
        private void ConvertBackModelToEntity()
        {
            _registro.Codigo = Codigo.Value;
            _registro.Descripcion = Descripcion.Value;
            _registro.Identificacion = Identificacion.Value;
            _registro.Direccion = Direccion.Value;
            _registro.Celular = Celular.Value;
            _registro.Email = Email.Value;
            _registro.TelefonoFijo = TelefonoFijo.Value;
            _registro.Representante = Representante.Value;
            _registro.EsProveedor = EsProveedor ? 1 : 0;
            _registro.PorDefecto = PorDefecto ? 1 : 0;
            _registro.IdGaleria = IdGaleria;
            _registro.Imagen = Imagen;
        }

        #endregion 4.MétodosAdministraciónDatos

        #region 5.MétodosGenerales
        /// <summary>
        /// Reiniciar valores de controles
        /// </summary>
        private void ResetForm()
        {
            Codigo.Value = "";
            Descripcion.Value = "";
            Identificacion.Value = "";
            Direccion.Value = "";
            Celular.Value = "";
            Email.Value = "";
            TelefonoFijo.Value = "";
            Representante.Value = "";
            EsProveedor = false;
            PorDefecto = false;
            IdGaleria = "";
            Imagen = "";
            IsVisibleTipoProveedor = false;
            IsVisibleEsContribuyenteEspecial = false;
            IsVisiblePoseeFacturacionElectronica = false;

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
            _codigo = new ValidatableObject<string>();
            _descripcion = new ValidatableObject<string>();
            _identificacion = new ValidatableObject<string>();
            _direccion = new ValidatableObject<string>();
            _celular = new ValidatableObject<string>();
            _email = new ValidatableObject<string>();
            _telefonoFijo = new ValidatableObject<string>();
            _representante = new ValidatableObject<string>();
            
        }
        /// <summary>
        /// Método para definir las validaciones en pantalla
        /// </summary>
        private void AddValidations()
        {
            _codigo.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _descripcion.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _identificacion.Validations.Add(new NumerosRule<string>(""));
            _direccion.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _celular.Validations.Add(new NumerosRule<string>(""));
            _email.Validations.Add(new EmailRule<string>(""));
            _telefonoFijo.Validations.Add(new NumerosRule<string>(""));
            _representante.Validations.Add(new IsNotNullOrEmptyRule<string>(""));            
        }
        /// <summary>
        /// Método para ejecutar la validación de los controles de pantalla
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            IsCodigoValid = !Codigo.Validate();
            IsDescripcionValid = !Descripcion.Validate();
            IsIdentificacionValid = !Identificacion.Validate();
            IsDireccionValid = !Direccion.Validate();
            IsCelularValid = !Celular.Validate();
            IsEmailValid = !Email.Validate();
            IsTelefonoFijoValid = !TelefonoFijo.Validate();
            IsRepresentanteValid = !Representante.Validate();

            return !IsCodigoValid
                && !IsDescripcionValid
                && !IsIdentificacionValid
                && !IsDireccionValid
                && !IsCelularValid
                && !IsEmailValid
                && !IsTelefonoFijoValid
                && !IsRepresentanteValid;
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
        private async Task eventoProveedor()
        {
            if (EsProveedor)
            {
                IsVisibleTipoProveedor = true;
                IsVisibleEsContribuyenteEspecial = true;
                IsVisiblePoseeFacturacionElectronica = true;
                //await generales.LoadListaCatalogo(new Catalogo() { CodigoCatalogo="TipoProveedor"});                
            }
            else
            {
                IsVisibleTipoProveedor = false;
                IsVisibleEsContribuyenteEspecial = false;
                IsVisiblePoseeFacturacionElectronica = false;
            }
        }
        /// <summary>
        /// Abrir pantalla para selección de tipo de proveedor
        /// </summary>
        /// <returns></returns>
        public async Task OpenTipoProveedorAsync()
        {
            TipoProveedorSeleccionado = new Catalogo()
            {
                CodigoCatalogo = "tipoproveedor"
                                                      ,
                EsJerarquico = 0
            };
            await NavigationService.NavigateToAsync<CatalogoViewModel>(TipoProveedorSeleccionado);
        }

        private async Task BrowseGalleryAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "Empresa",
                GuardarStorage=0
            };
            galeria.Nombre = Descripcion.Value == "" ? galeria.Id : Descripcion.Value;
            _galeria=await generales.PickAndShowFile(galeria);
            await CargaImagen(2);
        }

        private async Task OpenCameraAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "Empresa",
                GuardarStorage = 0
            };
            galeria.Nombre = Descripcion.Value == "" ? galeria.Id : Descripcion.Value;
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
                Descripcion.Value = displayName;//givenName + " " + middleName + " " + familyName;
                Representante.Value= givenName + " " + middleName + " " + familyName;
                Codigo.Value = id;
                Celular.Value = phones.FirstOrDefault().PhoneNumber.ToString().Replace(" ","");
                Email.Value = emails.FirstOrDefault().EmailAddress;
                Direccion.Value = string.Empty;                
            }
            catch (Exception ex)
            {
                // Handle exception here.
            }
        }

        private async Task NombreUnfocused_CommandAsync()
        {
            await eventoNombre();

        }
        private async Task eventoNombre()
        {
            
            Codigo.Value = Descripcion.Value.Replace(" ", String.Empty).ToLower();           
        }

        #endregion 5.MétodosGenerales
    }
}
