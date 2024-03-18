using DataModel.DTO.Administracion;
using DataModel.DTO.Iteraccion;
using DataModel.Helpers;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Switch;
using Escant_App.Validations;
using Escant_App.ViewModels.Base;
using Escant_App.ViewModels.Iteraccion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Administracion
{
    /// <summary>
    /// Pantalla de Interacción para el mantenimiento de catálogos y su detalle
    /// </summary>
    public class PopupAddCatalogoViewModel: ViewModelBase
    {
        #region 0.DeclaracionVariables
        /// <summary>
        /// 0. Declaración de las variables                 
        /// </summary>
        /// 0.0 Referencias a otras clases
        private SwitchAdministracion _switchAdministracion;
        private Generales.Generales generales;

        private readonly IServicioAdministracion_Catalogo _catalogoServicio;

        /// 0.1 Referencias a instancias de objetos a utilizarse en la Clase
        private List<Catalogo> _idRelacionList = new List<Catalogo>();
        private Catalogo _idRelacionSeleccionado;

        private bool _isEditMode;
        private bool _isDetalleMode;
        private Catalogo _registro;
        private Catalogo _registro1;
        private FuenteIconos _fuenteIconoSeleccionado;

        /// <summary>
        /// 0.2 Declaración de las variables de lectura y escritura para los controles
        /// </summary>
        private string _id;
        private string _idPadre;
        private string _nivel;
        private string _codigoCatalogo;
        private bool _esJerarquico;
        private bool _esMenu;
        private bool _esConstantes;        
        private bool _esMedida;
        private bool _esCentral;
        private ValidatableObject<string> _codigo;
        private ValidatableObject<string> _nombre;
        private bool _estaActivo;
        private string _idRelacion;
        private ValidatableObject<string> _valorConversion;
        private float _valorConstanteNumerico;
        private bool _esModulo;
        private bool _poseeFormulario;
        private string _nombreFormulario;
        private string _labelTitulo;
        private string _labelDescripcion;
        private string _orden;

        /// <summary>
        /// 0.3 Declaración variables control de datos
        /// </summary>
        private bool _esCatalogo;
        private bool _esNumerico;

        private bool _isVisibleEsMenu;
        private bool _isVisibleEsConstantes;
        private bool _isVisibleEsNumerico;
        private bool _isVisibleValorConstanteNumerico;
        private bool _isVisibleValorConstanteTexto;
        private bool _isVisibleEsMedida;

        private bool _isVisibleEsCentral;
        private bool _isVisibleIdRelacion;
        private bool _isVisibleValorConversion;

        private bool _isVisibleEsModulo;
        private bool _isVisiblePoseeFormulario;
        private bool _isVisibleNombreFormulario;
        private bool _isVisibleLabelTitulo;
        private bool _isVisibleLabelDescripcion;
        private bool _isVisibleOrden;


        private bool _isRegistroExiste;

        private string _nombrePadre;

        private bool _isEnabledEsMenu;
        private bool _isEnabledEsConstantes;
        private bool _isEnabledEsMedida;
        private bool _isEnabledEsJerarquico;
        private bool _isEnabledEsCentral;

        /// <summary>
        /// 0.4 Declaración de las variables de control de errores
        /// </summary>
        private bool _isCodigoValid;
        private bool _isNombreValid;
        private bool _isValorConversionValid;

        #endregion DeclaracionVariables

        #region 1.Constructor
        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public PopupAddCatalogoViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioAdministracion_Catalogo catalogoServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);                   
            //Instanciación Acorde al model
            _catalogoServicio = catalogoServicio;
            _switchAdministracion = new SwitchAdministracion(_catalogoServicio);
            generales = new Generales.Generales(_catalogoServicio);
            IsVisibleIdRelacion = false;
            inicioControlesValidacion();
            AddValidations();
            MessagingCenter.Unsubscribe<FuenteIconos>(this, Escant_App.AppSettings.Settings.FuenteIconosSeleccionadoAddCatalogoView);
            MessagingCenter.Subscribe<FuenteIconos>(this, Escant_App.AppSettings.Settings.FuenteIconosSeleccionadoAddCatalogoView, (args) =>
            {
                FuenteIconoSeleccionado = args;
                App.Current.Resources["ColorImagen"] = Color.FromHex(FuenteIconoSeleccionado.CodigoColor);
                App.Current.Resources["FuenteImagen"] = FuenteIconoSeleccionado.AliasFuente;
            });
        }
        #endregion 1.Constructor       

        #region 2.InstanciaAsignaVariablesControles
        /// 2. Asignacion y Lectura de variables vinculadas a los controles
        /// <summary>
        /// Método Asignación o lectura de CodigoEstablecimiento
        /// </summary>
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(() => Id);
            }
        }
        public string IdPadre
        {
            get => _idPadre;
            set
            {
                _idPadre = value;
                RaisePropertyChanged(() => IdPadre);
            }
        }
        public string Nivel
        {
            get => _nivel;
            set
            {
                _nivel = value;
                RaisePropertyChanged(() => Nivel);
            }
        }
        public string CodigoCatalogo
        {
            get => _codigoCatalogo;
            set
            {
                _codigoCatalogo = value;
                RaisePropertyChanged(() => CodigoCatalogo);
            }
        }
        public bool EsJerarquico
        {
            get => _esJerarquico;
            set
            {
                _esJerarquico = value;
                RaisePropertyChanged(() => EsJerarquico);
            }
        }

        public bool EsMedida
        {
            get => _esMedida;
            set
            {
                _esMedida = value;
                RaisePropertyChanged(() => EsMedida);
                eventoMedida();                
            }
        }
        public bool EsConstantes
        {
            get => _esConstantes;
            set
            {
                _esConstantes = value;
                RaisePropertyChanged(() => EsConstantes);
                eventoConstantes();
            }
        }
        public bool EsCentral
        {
            get => _esCentral;
            set
            {
                _esCentral = value;
                RaisePropertyChanged(() => EsCentral);
                eventoCentral();
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
        public ValidatableObject<string> Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                RaisePropertyChanged(() => Nombre);
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
        public Catalogo IdRelacionSeleccionado
        {
            get => _idRelacionSeleccionado;
            set
            {
                _idRelacionSeleccionado = value;
                RaisePropertyChanged(() => IdRelacionSeleccionado);
            }
        }
        public ValidatableObject<string> ValorConversion
        {
            get => _valorConversion;
            set
            {
                _valorConversion = value;
                RaisePropertyChanged(() => ValorConversion);
            }
        }
        public float ValorConstanteNumerico
        {
            get => _valorConstanteNumerico;
            set
            {
                _valorConstanteNumerico = value;
                RaisePropertyChanged(() => ValorConstanteNumerico);
            }
        }
        public bool EsCatalogo
        {
            get => _esCatalogo;
            set
            {
                _esCatalogo = value;
                RaisePropertyChanged(() => EsCatalogo);                
            }
        }

        public bool EsMenu
        {
            get => _esMenu;
            set
            {
                _esMenu = value;
                RaisePropertyChanged(() => EsMenu);
                eventoMenu();
            }
        }

        public bool EsNumerico
        {
            get => _esNumerico;
            set
            {
                _esNumerico = value;
                RaisePropertyChanged(() => EsNumerico);
                eventoEsNumerico();
            }
        }
        public string NombrePadre
        {
            get => _nombrePadre;
            set
            {
                _nombrePadre = value;
                RaisePropertyChanged(() => NombrePadre);
            }
        }
        public bool EsModulo
        {
            get => _esModulo;
            set
            {
                _esModulo = value;
                RaisePropertyChanged(() => EsModulo);
            }
        }

        public bool PoseeFormulario
        {
            get => _poseeFormulario;
            set
            {
                _poseeFormulario = value;
                RaisePropertyChanged(() => PoseeFormulario);
                eventoPoseeFormulario();
            }
        }
        public string NombreFormulario
        {
            get => _nombreFormulario;
            set
            {
                _nombreFormulario = value;
                RaisePropertyChanged(() => NombreFormulario);
            }
        }

        public string LabelTitulo
        {
            get => _labelTitulo;
            set
            {
                _labelTitulo = value;
                RaisePropertyChanged(() => LabelTitulo);
            }
        }

        public string LabelDescripcion
        {
            get => _labelDescripcion;
            set
            {
                _labelDescripcion = value;
                RaisePropertyChanged(() => LabelDescripcion);
            }
        }
        public string Orden
        {
            get => _orden;
            set
            {
                _orden = value;
                RaisePropertyChanged(() => Orden);
            }
        }
        public bool IsVisibleIdRelacion
        {
            get => _isVisibleIdRelacion;
            set
            {
                _isVisibleIdRelacion = value;
                RaisePropertyChanged(() => IsVisibleIdRelacion);
            }
        }
        public bool IsVisibleValorConversion
        {
            get => _isVisibleValorConversion;
            set
            {
                _isVisibleValorConversion = value;
                RaisePropertyChanged(() => IsVisibleValorConversion);
            }
        }
        public bool IsVisibleEsCentral
        {
            get => _isVisibleEsCentral;
            set
            {
                _isVisibleEsCentral = value;
                RaisePropertyChanged(() => IsVisibleEsCentral);
            }
        }
        public bool IsVisibleEsMenu
        {
            get => _isVisibleEsMenu;
            set
            {
                _isVisibleEsMenu = value;
                RaisePropertyChanged(() => IsVisibleEsMenu);
            }
        }
        public bool IsVisibleEsMedida
        {
            get => _isVisibleEsMedida;
            set
            {
                _isVisibleEsMedida = value;
                RaisePropertyChanged(() => IsVisibleEsMedida);
            }
        }
        public bool IsVisibleEsConstantes
        {
            get => _isVisibleEsConstantes;
            set
            {
                _isVisibleEsConstantes = value;
                RaisePropertyChanged(() => IsVisibleEsConstantes);
            }
        }
        public bool IsVisibleEsNumerico
        {
            get => _isVisibleEsNumerico;
            set
            {
                _isVisibleEsNumerico = value;
                RaisePropertyChanged(() => IsVisibleEsNumerico);
            }
        }
        public bool IsVisibleValorConstanteNumerico
        {
            get => _isVisibleValorConstanteNumerico;
            set
            {
                _isVisibleValorConstanteNumerico = value;
                RaisePropertyChanged(() => IsVisibleValorConstanteNumerico);
            }
        }
        public bool IsVisibleValorConstanteTexto
        {
            get => _isVisibleValorConstanteTexto;
            set
            {
                _isVisibleValorConstanteTexto = value;
                RaisePropertyChanged(() => IsVisibleValorConstanteTexto);
            }
        }
        public List<Catalogo> IdRelacionList
        {
            get => _idRelacionList;
            set
            {
                _idRelacionList = value;
                RaisePropertyChanged(() => IdRelacionList);
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
        public bool IsNombreValid
        {
            get => _isNombreValid;
            set
            {
                _isNombreValid = value;
                RaisePropertyChanged(() => IsNombreValid);
            }
        }
        public bool IsValorConversionValid
        {
            get => _isValorConversionValid;
            set
            {
                _isValorConversionValid = value;
                RaisePropertyChanged(() => IsValorConversionValid);
            }
        }

        public FuenteIconos FuenteIconoSeleccionado
        {
            get => _fuenteIconoSeleccionado;
            set
            {
                _fuenteIconoSeleccionado = value;
                RaisePropertyChanged(() => FuenteIconoSeleccionado);
            }
        }

        public bool IsEnabledEsMenu
        {
            get => _isEnabledEsMenu;
            set
            {
                _isEnabledEsMenu = value;
                RaisePropertyChanged(() => IsEnabledEsMenu);
            }
        }
        public bool IsEnabledEsJerarquico
        {
            get => _isEnabledEsJerarquico;
            set
            {
                _isEnabledEsJerarquico = value;
                RaisePropertyChanged(() => IsEnabledEsJerarquico);
            }
        }
        public bool IsEnabledEsMedida
        {
            get => _isEnabledEsMedida;
            set
            {
                _isEnabledEsMedida = value;
                RaisePropertyChanged(() => IsEnabledEsMedida);
            }
        }
        public bool IsEnabledEsConstantes
        {
            get => _isEnabledEsConstantes;
            set
            {
                _isEnabledEsConstantes = value;
                RaisePropertyChanged(() => IsEnabledEsConstantes);
            }
        }
        public bool IsVisibleEsModulo
        {
            get => _isVisibleEsModulo;
            set
            {
                _isVisibleEsModulo = value;
                RaisePropertyChanged(() => IsVisibleEsModulo);
            }
        }
        public bool IsVisiblePoseeFormulario
        {
            get => _isVisiblePoseeFormulario;
            set
            {
                _isVisiblePoseeFormulario = value;
                RaisePropertyChanged(() => IsVisiblePoseeFormulario);
            }
        }
        public bool IsVisibleNombreFormulario
        {
            get => _isVisibleNombreFormulario;
            set
            {
                _isVisibleNombreFormulario = value;
                RaisePropertyChanged(() => IsVisibleNombreFormulario);
            }
        }
        public bool IsVisibleLabelTitulo
        {
            get => _isVisibleLabelTitulo;
            set
            {
                _isVisibleLabelTitulo = value;
                RaisePropertyChanged(() => IsVisibleLabelTitulo);
            }
        }
        public bool IsVisibleLabelDescripcion
        {
            get => _isVisibleLabelDescripcion;
            set
            {
                _isVisibleLabelDescripcion = value;
                RaisePropertyChanged(() => IsVisibleLabelDescripcion);
            }
        }
        public bool IsVisibleOrden
        {
            get => _isVisibleOrden;
            set
            {
                _isVisibleOrden = value;
                RaisePropertyChanged(() => IsVisibleOrden);
            }
        }




        #endregion 2.InstanciaAsignaVariablesControles        

        #region 3.DefinicionMetodos
        /// <summary>
        /// 3. Definición de métodos de la pantalla
        /// </summary>
        /// <summary>
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());
        //public ICommand CodigoUnfocused => new Command(async () => await CodigoUnfocused_CommandAsync());
        public ICommand NombreUnfocused => new Command(async () => await NombreUnfocused_CommandAsync());
        public ICommand OpenFuenteIconoCommand => new Command(async () => await OpenFuenteIconoAsync());

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
            IdPadre = EsCatalogo ? "0" : "1";
            Nivel = EsCatalogo ? "0" : "1";
            EsJerarquico = false;
            EsMedida = false;
            EsMenu = false;
            IsVisibleEsMenu = true;
            IsVisibleEsMedida = true;
            IsVisibleEsConstantes = true;
            EstaActivo = true;
            Codigo.Value = "";
            Nombre.Value = "";
            _isEditMode = false;
            FuenteIconoSeleccionado = new FuenteIconos()
            {
                NombreIcono = "Configurar"
               ,
                CodigoImagen = "&#xf0e8;"
            };
        }
        /// <summary>
        /// 4.3 Método iniciar Pantalla con 1 parámetro
        /// </summary>
        /// <param name="navigationData"></param>
        /// <returns></returns>
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            PageTitle = TextsTranslateManager.Translate("AddCatalogoAdministracionTitle");
            //Bind data for edit mode
                if (navigationData is Catalogo registro && registro != null)
                {
                    _isEditMode = registro.EsEdicion==1 ?true:false;
                    _registro = registro;
                    PageTitle = !_isEditMode?TextsTranslateManager.Translate("AddCatalogoAdministracion1Title"): TextsTranslateManager.Translate("EditCatalogoAdministracion1Title");
                    await BindExistingData(registro);
                }
                else
                {
                    await Load();
                }

            IsBusy = false;
        }
        /// <summary>
        /// Método iniciar Pantalla con 2 parámetros
        /// </summary>
        /// <param name="navigationData"></param>
        /// <param name="navigationData1"></param>
        /// <returns></returns>
        public override async Task InitializeAsync(object navigationData,object navigationData1)
        {
            IsBusy = true;
            PageTitle = TextsTranslateManager.Translate("AddCatalogoAdministracionTitle");
            //Bind data for edit mode
            await Task.Run(() =>
            {
                if (navigationData is Catalogo registro && registro != null)
                {
                    _isEditMode = registro.EsEdicion == 1 ? true : false;
                    _registro = registro;
                    if(navigationData1 is Catalogo registro1 && registro1 != null)
                    {
                        _registro1 = registro1;
                    }
                    PageTitle = TextsTranslateManager.Translate("AddCatalogoAdministracion1Title");
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
            bool result = Validate();
            if (!result)
            {
                IsBusy = false;
                return;
            }
            _isRegistroExiste = _isEditMode;
            
            var iregistro = new Catalogo
            {
                Id = _isRegistroExiste?Id:Generator.GenerateKey(),
                IdPadre = IdPadre,
                Empresa = "",
                CodigoCatalogo = CodigoCatalogo,
                EsCatalogo = EsCatalogo?1:0,
                EsMenu= EsMenu ? 1 : 0,                
                EsJerarquico = EsJerarquico ? 1 : 0,
                EsConstantes = EsConstantes ? 1 : 0,
                ValorConstanteNumerico=EsNumerico?ValorConstanteNumerico:0,
                EsMedida = EsMedida ? 1 : 0,
                EsCentral = EsCentral ? 1 : 0,                
                Codigo = Codigo.Value,
                Nombre = Nombre.Value,                
                ImageIcon= JsonConvert.SerializeObject(FuenteIconoSeleccionado),
                EsModulo= EsModulo ? 1 : 0,
                PoseeFormulario = PoseeFormulario ? 1 : 0,
                NombreFormulario =NombreFormulario,
                LabelTitulo=LabelTitulo,
                LabelDescripcion = LabelDescripcion,
                Nivel=Convert.ToInt32(Nivel),
                Orden = Convert.ToInt32(Orden),
                EstaActivo = EstaActivo ? 1 : 0
            };
            iregistro.IdConversion = EsMedida ? (!EsCatalogo && !EsCentral? IdRelacionSeleccionado.Id: iregistro.Id) : "";
            iregistro.ValorConversion = EsMedida ? (!EsCatalogo && !EsCentral ? float.Parse(ValorConversion.Value) : 1) : 0;
            iregistro.EsEdicion = _isRegistroExiste ? 1 : 0;
            string informedMessage = "";
            /*if (_isSettingExists)
            {
                setting.Id = _facturacionSettingId;
            }*/            
            //1. Añadir el nuevo registro
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var resultado = await _switchAdministracion.GuardaRegistro_Catalogo(iregistro, estaConectado, _isRegistroExiste);
            if(_registro1!=null && _registro1.Id!=null && _registro1.Id != "")
            {
                _registro1.IdPadre = iregistro.Id;
                _isRegistroExiste = true;
                resultado = await _switchAdministracion.GuardaRegistro_Catalogo(_registro1, estaConectado, _isRegistroExiste);
            }            
            IsBusy = false;
            //MessagingCenter.Send<string>("", "StoreLogoChanged");
            //await NavigationService.RemoveLastFromBackStackAsync();            
            informedMessage = resultado.mensaje; 
            DialogService.ShowToast(informedMessage);
            MessagingCenter.Send<string>(string.Empty, "OnDataSourceChanged");
            SettingsOnline.oLCatalogo = await generales.LoadListaCatalogo(new DataModel.DTO.Administracion.Catalogo() { Deleted = 0 });
            //await NavigationService.RemovePopupAsync();
        }
        /// <summary>
        /// Método para cargar los datos del combo
        /// </summary>
        /// <returns></returns>
        public async Task LoadListaCatalogo()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var catalogo = await _switchAdministracion.ConsultaCatalogos(new Catalogo() { EsMedida = 1, EsCentral=1 }, estaConectado);
            if (catalogo != null && catalogo.Count > 0)
            {
                IdRelacionList = catalogo;                
            }
            else
            {
                DialogService.ShowToast("Ingrese un ítem central");
            }
        }
        /// <summary>
        /// Método de Asignación de valores recibidos por parámetros
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        private async Task BindExistingData(Catalogo registro)
        {
            if(registro.EsMedida==1 && registro.EsCatalogo==0)
            {
                await LoadListaCatalogo();
            }
            EsCatalogo = registro.EsCatalogo == 1 ? true : false;
            if (EsCatalogo)
            {
                IsEnabledEsMenu = true;
                IsEnabledEsMedida = true;
                IsEnabledEsConstantes = true;
                IsEnabledEsJerarquico = false;
            }
            else
            {
                IsEnabledEsMenu = false;
                IsEnabledEsMedida = false;
                IsEnabledEsConstantes = false;
                IsEnabledEsJerarquico = false;
            }
            _isDetalleMode = EsCatalogo?false:true;
            
            IdPadre = EsCatalogo ? "0" : registro.IdPadre;
            NombrePadre= EsCatalogo ? "Catálogos" : registro.NombrePadre;
            CodigoCatalogo = registro.CodigoCatalogo;            
            EsJerarquico = registro.EsJerarquico==1?true: EsJerarquico;
            EsMedida = registro.EsMedida == 1 ? true : EsMedida;
            EstaActivo = registro.EstaActivo==1?true: EstaActivo;
            EsMenu = registro.EsMenu == 1 ? true : EsMenu;
            EsConstantes = registro.EsConstantes == 1 ? true : EsConstantes;
            ValorConstanteNumerico = registro.ValorConstanteNumerico;            
            IsVisibleEsMedida = EsMenu ? false : (EsConstantes ? false:true);
            IsVisibleEsConstantes = EsMenu ? false : (EsMedida ? false : true);
            EsModulo = registro.EsModulo == 1 ? true : EsModulo;
            PoseeFormulario =registro.PoseeFormulario == 1 ? true : PoseeFormulario;
            NombreFormulario = registro.NombreFormulario;
            LabelTitulo = registro.LabelTitulo;
            LabelDescripcion = registro.LabelDescripcion;
            Orden = Convert.ToString(registro.Orden);
            FuenteIconoSeleccionado = registro.ImageIcon!=null && registro.ImageIcon!=""?JsonConvert.DeserializeObject<FuenteIconos>(registro.ImageIcon): new FuenteIconos()
            {
                NombreIcono = "Configurar"
               ,
                CodigoImagen = "&#xf0e8;"
            };
            if (_isEditMode)
            {
                Id = registro.Id;
                Codigo.Value = registro.Codigo;
                Nombre.Value = registro.Nombre;
                Nivel = registro.Nivel.ToString();
                IdRelacionSeleccionado = !string.IsNullOrEmpty(registro.IdConversion) ? IdRelacionList.Find(x => x.Id == registro.IdConversion) : null;
                ValorConversion.Value = registro.ValorConversion.ToString();
            }
            else
            { 
                Codigo.Value = "";
                Nombre.Value = "";
                Nivel = EsCatalogo ? "0" : (registro.Nivel + 1).ToString();
            }
            
        }

        #endregion 4.MétodosAdministraciónDatos

        #region 5.MétodosGenerales
        /// <summary>
        /// Método para iniciar instancia de Controles de Validación
        /// </summary>
        public void inicioControlesValidacion()
        {
            _codigo = new ValidatableObject<string>();
            _nombre = new ValidatableObject<string>();
            _valorConversion = new ValidatableObject<string>();
            _idRelacionSeleccionado = new Catalogo();
        }
        /// <summary>
        /// Método para definir las validaciones en pantalla
        /// </summary>
        private void AddValidations()
        {
            _codigo.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _nombre.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
            _valorConversion.Validations.Add(new IsNotNullOrEmptyRule<string>(""));
        }
        /// <summary>
        /// Método para ejecutar la validación de los controles de pantalla
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            IsCodigoValid = !Codigo.Validate();
            IsNombreValid = !Nombre.Validate();
            if(EsMedida && !EsCatalogo && !EsCentral)
                   IsValorConversionValid= !ValorConversion.Validate();

            return !IsCodigoValid
                && !IsNombreValid
                && (EsMedida && !EsCatalogo && !EsCentral ? !IsValorConversionValid:true);                
        }        
        /// <summary>
        /// Método para salir de la pantalla
        /// </summary>
        /// <returns></returns>
        private async Task Cancel_CommandAsync()
        {
            MessagingCenter.Send<string>(string.Empty, "OnDataSourceChanged");
            await NavigationService.RemovePopupAsync();
        }
        /// <summary>
        /// Método para el evento onfocus del Código
        /// </summary>
        /// <returns></returns>
        private async Task CodigoUnfocused_CommandAsync()
        {
            if (!_isDetalleMode) { 
                await eventoCodigo();
            }
        }
        /// <summary>
        /// Método para el evento onfocus del nombre
        /// </summary>
        /// <returns></returns>
        private async Task NombreUnfocused_CommandAsync()
        {
                await eventoNombre();

        }
        /// <summary>
        /// Método cuando se cambia el check que determina si Es Medida o no
        /// </summary>
        /// <returns></returns>
        private async Task eventoMedida()
        {
            if (EsMedida)
            {                
                IsVisibleEsConstantes = false;
                IsVisibleEsMenu = false;
                EsJerarquico = false;                
                EsConstantes = false;
                EsMenu = false;
                if (!EsCatalogo)
                    IsVisibleEsCentral = true;
                if (!EsCentral)
                {
                    IsVisibleIdRelacion = true;
                    IsVisibleValorConversion = true;
                    await LoadListaCatalogo();
                }
            }
            else
            {

                IsVisibleEsCentral = false;
                IsVisibleIdRelacion = false;
                IsVisibleValorConversion = false;
            }
        }
        /// <summary>
        /// Método para el evento cuando se cambia el check como unidad Central
        /// </summary>
        /// <returns></returns>
        private async Task eventoCentral()
        {
            if (!EsCentral)
            {                
                IsVisibleIdRelacion = true;
                IsVisibleValorConversion = true;
                await LoadListaCatalogo();
            }
            else
            {
                IsVisibleIdRelacion = false;
                IsVisibleValorConversion = false;
            }
        }        
        /// <summary>
        /// Métdodo para el cambio de valor en el Código
        /// </summary>
        /// <returns></returns>
        private async Task eventoCodigo()
        {
            CodigoCatalogo = Codigo.Value;
        }
        private async Task eventoNombre()
        {
            Codigo.Value=Nombre.Value.Replace(" ", String.Empty).ToLower();
            if (!_isDetalleMode)
            {
                CodigoCatalogo = Codigo.Value;
            }
        }
        private async Task eventoMenu()
        {
            if (EsMenu)
            {
                IsVisibleEsMedida = false;
                IsVisibleEsConstantes = false;
                EsJerarquico = true;
                EsMedida = false;
                EsConstantes = false;
                IsVisibleEsModulo = true;
                IsVisiblePoseeFormulario = true;
                IsVisibleLabelTitulo = true;
                IsVisibleLabelDescripcion = true;
                IsVisibleOrden = true;
            }
            else
            {
                IsVisibleEsMedida = EsConstantes?false:true;
                IsVisibleEsConstantes = EsMedida ? false : true;
                IsVisibleEsModulo = false;
                IsVisiblePoseeFormulario = false;
                IsVisibleLabelTitulo = false;
                IsVisibleLabelDescripcion = false;
                IsVisibleOrden = false;
            }
        }
        private async Task eventoPoseeFormulario()
        {
            if (PoseeFormulario)
            {
                IsVisibleNombreFormulario = true;
                NombreFormulario = Nombre.Value.Replace(" ", String.Empty) + "View";
                LabelTitulo = Nombre.Value.Replace(" ", String.Empty) + "Titulo";
                LabelDescripcion = Nombre.Value.Replace(" ", String.Empty) + "Descripcion";
            }
            else
            {
                IsVisibleNombreFormulario = false;
                NombreFormulario = "";
                LabelTitulo = "";
                LabelDescripcion = "";
            }

        }
        private async Task eventoConstantes()
        {
            if (EsConstantes)
            {
                IsVisibleEsMenu = false;
                IsVisibleEsMedida = false;                
                EsJerarquico = false;                
                EsMedida = false;
                EsMenu = false;
                IsVisibleEsNumerico = EsCatalogo?false:true;
            }
            else
            {
                IsVisibleEsMenu = EsMedida ? false : true;
                IsVisibleEsMedida = EsMenu ? false : true;
                IsVisibleEsNumerico = false;
            }
        }
        private async Task eventoEsNumerico()
        {
            if (EsNumerico)
            {
                IsVisibleValorConstanteNumerico = true;
                IsVisibleValorConstanteTexto = false;
            }
            else
            {
                IsVisibleValorConstanteNumerico = false;
                IsVisibleValorConstanteTexto = true;
            }
        }
        public async Task OpenFuenteIconoAsync()
        {            
            await NavigationService.NavigateToAsync<GaleriaViewModel>(FuenteIconoSeleccionado);
        }
        #endregion 5.MétodosGenerales
    }
}
