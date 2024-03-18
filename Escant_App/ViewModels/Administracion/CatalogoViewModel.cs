using DataModel.DTO.Administracion;
using DataModel.DTO.Iteraccion;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using Escant_App.ViewModels.Base;
using Escant_App.Views.Administracion;
using Newtonsoft.Json;
using Syncfusion.XForms.PopupLayout;
using Syncfusion.XForms.TreeView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Administracion
{
    public class CatalogoViewModel : ViewModelBase
    {
        /// <summary>
        /// 0. Declaración de las variables         
        /// </summary>
        /// 0.0 Fijos en cada Model
        private SwitchAdministracion _switchAdministracion;
        private Generales.Generales generales = new Generales.Generales();
        private readonly IServicioAdministracion_Catalogo _catalogoServicio;        
        private bool _isSettingExists;
        private string _facturacionSettingId;

        private Catalogo _item;
        private Catalogo _parentItem;        

        /// <summary>
        /// 0.1 Declaración de las variables de lectura y escritura para los controles
        /// </summary>
        private ObservableCollection<Catalogo> _imageNodeInfo;
        private Catalogo _itemSeleccionado;
        private bool _visible;
        private SfPopupLayout popupLayout;
        private List<Catalogo> _catalogoList = new List<Catalogo>();
        private Catalogo _catalogoSeleccionado;

        private bool _esPermitido;

        internal SfTreeView TreeView { get; set; }
        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public CatalogoViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioAdministracion_Catalogo catalogoServicio)
        {
            BindingContext = new ConectividadModelo(seguridadServicio);
            _catalogoServicio = catalogoServicio;
            _switchAdministracion = new SwitchAdministracion(_catalogoServicio);
            _item = new Catalogo();
            _parentItem = new Catalogo();
            MessagingCenter.Subscribe<string>(this, "OnDataSourceChanged", (args) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await LoadSetting();
                    IsBusy = false;
                    ItemIndex = -1;
                });
            });            
        }

        /// 2. Asignacion y Lectura de variables vinculadas a los controles
        /// <summary>
        /// Método Asignación o lectura de CodigoEstablecimiento
        /// </summary>
        public ObservableCollection<Catalogo> ImageNodeInfo
        {
            get => _imageNodeInfo;
            set
            {
                _imageNodeInfo = value;
                RaisePropertyChanged(() => ImageNodeInfo);
            }
        }

        public List<Catalogo> CatalogoList
        {
            get => _catalogoList;
            set
            {
                _catalogoList = value;
                RaisePropertyChanged(() => CatalogoList);
            }
        }
        public Catalogo CatalogoSeleccionado
        {
            get => _catalogoSeleccionado;
            set
            {
                _catalogoSeleccionado = value;
                RaisePropertyChanged(() => CatalogoSeleccionado);
                LoadArbol();
            }
        }
        public Catalogo Item
        {
            get => _item;
            set
            {
                _item = value;
                RaisePropertyChanged(() => Item);
            }
        }
        public Catalogo ParentItem
        {
            get => _parentItem;
            set
            {
                _parentItem = value;
                RaisePropertyChanged(() => ParentItem);
            }
        }
        public bool EsPermitido
        {
            get => _esPermitido;
            set
            {
                _esPermitido = value;
                RaisePropertyChanged(() => EsPermitido);
            }
        }
        /// <summary>
        /// 3. Definición de métodos de la pantalla
        /// </summary>
        /// <summary>
            #region DefinicionMetodos
        public ICommand OnAddItem => new Command(async () => await ToolbarAddCommandAsync());
        #endregion DefinicionMetodos

        /// <summary>
        /// 4. Métodos de administración de datos
        /// </summary>
        /// <returns></returns>
        #region MétodosAdministraciónDatos  
        /// 4.1. Método para Cargar información de pantalla, la llamada realizada desde el frontend al inicializar
        /// </summary>
        /// <returns></returns>
        public async Task LoadSetting()
        {
           await LoadListaCatalogo();                       
        }
        public async Task LoadListaCatalogo()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var catalogo = await _switchAdministracion.ConsultaCatalogos(new Catalogo() { EsCatalogo = 1 }, estaConectado);
            if (catalogo != null && catalogo.Count > 0)
            {
                CatalogoList = catalogo;
            }
            else
            {
                DialogService.ShowToast("Ingrese un Catálogo para empezar");
            }
        }
        public async Task LoadArbol()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var catalogo = await _switchAdministracion.ConsultaCatalogos(new Catalogo() { CodigoCatalogo = CatalogoSeleccionado.CodigoCatalogo }, estaConectado);
            if (catalogo != null && catalogo.Count>0)
            {
                _isSettingExists = true;
                //_facturacionSettingId = setting.Id;
                await armaArbol(catalogo);
                /*var count = ImageNodeInfo != null ? ImageNodeInfo.Count : 0;
                var data = ImageNodeInfo != null ? ImageNodeInfo[count - 1] : null;
                TreeView.BringIntoView(data, false, true, Syncfusion.XForms.TreeView.ScrollToPosition.MakeVisible);
                */

                //if(setting.Logo != null)
                //    StoreImageSource = ImageSource.FromStream(() => new MemoryStream(setting.Logo));
            }
            else
            {
                _isSettingExists = false;
            }
            IsBusy = false;
        }
        /// <summary>
        /// Método para iniciar la pantalla referenciada desde otro
        /// </summary>
        /// <param name="navigationData"></param>
        /// <returns></returns>
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            await LoadSetting();            
            //Bind data for edit mode
            if (navigationData is Catalogo registro && registro != null)
            {
                CatalogoSeleccionado = CatalogoList?.FirstOrDefault(x => x.CodigoCatalogo == registro.CodigoCatalogo 
                                                                      && x.EsCatalogo==1);                
            }
            if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("DMINISTRADO"))
                EsPermitido = true;
            else
                EsPermitido = false;
            IsBusy = false;

        }
        #endregion MétodosAdministraciónDatos

        #region 5.MétodosGenerales
        public async Task armaArbol(List<Catalogo> listaCatalogo)
        {
            var nodeImageInfo = new ObservableCollection<Catalogo>();
            var ldetalleGrupo = new ObservableCollection<Catalogo>();
            Catalogo grupo;
            Catalogo detalleGrupo;
            
            
            var dtRegistros = generales.ToDataTable<Catalogo>(listaCatalogo);

            if (dtRegistros != null && dtRegistros.Rows.Count > 0)
            {
                foreach (DataRow MenuPadre in dtRegistros.Select("EsCatalogo=1", "Nombre ASC"))
                {
                    grupo = new Catalogo()
                    {
                          Id = MenuPadre["Id"].ToString()
                        , IdPadre = MenuPadre["IdPadre"].ToString()
                        , CodigoCatalogo = MenuPadre["CodigoCatalogo"].ToString()
                        , EsCatalogo = MenuPadre["EsCatalogo"] != null ? Convert.ToInt32(MenuPadre["EsCatalogo"]) : 0
                        , Codigo = MenuPadre["Codigo"].ToString()
                        , Nombre = MenuPadre["Nombre"].ToString()
                        , EsMenu = MenuPadre["EsMenu"] != null ? Convert.ToInt32(MenuPadre["EsMenu"]) : 0
                        , EsMedida = MenuPadre["EsMedida"] != null ? Convert.ToInt32(MenuPadre["EsMedida"]) : 0
                        , EsCentral = MenuPadre["EsCentral"] != null ? Convert.ToInt32(MenuPadre["EsCentral"]) : 0
                        , EstaActivo = MenuPadre["EstaActivo"] != null ? Convert.ToInt32(MenuPadre["EstaActivo"]) : 0
                        , IdConversion = MenuPadre["IdConversion"].ToString()
                        , ValorConversion = MenuPadre["ValorConversion"] != null ? float.Parse(MenuPadre["ValorConversion"].ToString()) : 0
                        , Nivel = MenuPadre["Nivel"] != null ? Convert.ToInt32(MenuPadre["Nivel"]) : 0
                        , Orden = MenuPadre["Orden"] != null ? Convert.ToInt32(MenuPadre["Orden"]) : 0
                        , ImageIcon = MenuPadre["ImageIcon"].ToString()
                        , IconoMenu = MenuPadre["ImageIcon"] != null && MenuPadre["ImageIcon"].ToString() != "" ? JsonConvert.DeserializeObject<FuenteIconos>(MenuPadre["ImageIcon"].ToString()) : new FuenteIconos()
                        {
                             NombreIcono = "Configurar"
                            ,CodigoImagen = "&#xf0e8;"
                        }
                        ,EsConstantes= MenuPadre["EsConstantes"] != null ? Convert.ToInt32(MenuPadre["EsConstantes"]) : 0
                        ,
                        ValorConstanteNumerico = MenuPadre["ValorConstanteNumerico"] != null ? Convert.ToInt32(MenuPadre["ValorConstanteNumerico"]) : 0
                        ,
                        EsModulo = MenuPadre["EsModulo"] != null ? Convert.ToInt32(MenuPadre["EsModulo"]) : 0
                        ,
                        PoseeFormulario = MenuPadre["PoseeFormulario"] != null ? Convert.ToInt32(MenuPadre["PoseeFormulario"]) : 0
                        ,
                        NombreFormulario = MenuPadre["NombreFormulario"].ToString()
                        ,
                        LabelTitulo = MenuPadre["LabelTitulo"].ToString()
                        ,
                        LabelDescripcion = MenuPadre["LabelDescripcion"].ToString()
                    };
                    if (dtRegistros.Select("IdPadre='" + grupo.Id + "'").Length > 0)
                    {
                        ldetalleGrupo = new ObservableCollection<Catalogo>();                        
                        ldetalleGrupo = await armaArbolHijos(dtRegistros, grupo.Id);
                        grupo.subCatalogos = ldetalleGrupo;
                    }                   
                    nodeImageInfo.Add(grupo);
                }
            }
            ImageNodeInfo = nodeImageInfo;
        }
        private async Task<ObservableCollection<Catalogo>> armaArbolHijos(DataTable herencia,string IdPadre)
        {
            Catalogo detalleGrupo;
            var ldetalleGrupo = new ObservableCollection<Catalogo>();

            foreach (DataRow Menu in herencia.Select("IdPadre='" + IdPadre + "'", "Nombre ASC"))
            {
                detalleGrupo = new Catalogo()
                {
                    Id = Menu["Id"].ToString()
                ,   IdPadre = Menu["IdPadre"].ToString()
                ,   CodigoCatalogo = Menu["CodigoCatalogo"].ToString()
                ,   EsCatalogo = Menu["EsCatalogo"] != null ? Convert.ToInt32(Menu["EsCatalogo"]) : 0
                ,   Codigo = Menu["Codigo"].ToString()
                ,   Nombre = Menu["Nombre"].ToString()
                ,   EsMenu = Menu["EsMenu"] != null ? Convert.ToInt32(Menu["EsMenu"]) : 0
                ,   EsMedida = Menu["EsMedida"] != null ? Convert.ToInt32(Menu["EsMedida"]) : 0
                ,   EsCentral = Menu["EsCentral"] != null ? Convert.ToInt32(Menu["EsCentral"]) : 0
                ,   EstaActivo = Menu["EstaActivo"] != null ? Convert.ToInt32(Menu["EstaActivo"]) : 0
                ,   IdConversion = Menu["IdConversion"].ToString()
                ,   ValorConversion = Menu["ValorConversion"] != null ? float.Parse(Menu["ValorConversion"].ToString()) : 0
                ,   Nivel = Menu["Nivel"] != null ? Convert.ToInt32(Menu["Nivel"]) : 0
                ,   Orden = Menu["Orden"] != null ? Convert.ToInt32(Menu["Orden"]) : 0
                ,   ImageIcon = Menu["ImageIcon"].ToString()
                ,   IconoMenu= Menu["ImageIcon"] != null && Menu["ImageIcon"].ToString() != "" ? JsonConvert.DeserializeObject<FuenteIconos>(Menu["ImageIcon"].ToString()) : new FuenteIconos()
                {
                     NombreIcono = "Configurar"
                ,    CodigoImagen = "&#xf0e8;"
                }
                ,  EsConstantes = Menu["EsConstantes"] != null ? Convert.ToInt32(Menu["EsConstantes"]) : 0
                ,   ValorConstanteNumerico = Menu["ValorConstanteNumerico"] != null ? Convert.ToInt32(Menu["ValorConstanteNumerico"]) : 0
                ,
                    EsModulo = Menu["EsModulo"] != null ? Convert.ToInt32(Menu["EsModulo"]) : 0
                        ,
                    PoseeFormulario = Menu["PoseeFormulario"] != null ? Convert.ToInt32(Menu["PoseeFormulario"]) : 0
                        ,
                    NombreFormulario = Menu["NombreFormulario"].ToString()
                        ,
                    LabelTitulo = Menu["LabelTitulo"].ToString()
                        ,
                    LabelDescripcion = Menu["LabelDescripcion"].ToString()
                };
                if (herencia.Select("IdPadre='" + detalleGrupo.Id + "'").Length > 0)
                {
                    var ldetalleGrupoHijos = new ObservableCollection<Catalogo>();
                    ldetalleGrupoHijos = await armaArbolHijos(herencia,detalleGrupo.Id);
                    detalleGrupo.subCatalogos = ldetalleGrupoHijos;
                }
                ldetalleGrupo.Add(detalleGrupo);
            }
            return ldetalleGrupo;
        }

        private async Task ToolbarAddCommandAsync()
        {
            if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("DMINISTRADO"))
            {
                var oCatalogo = new Catalogo()
                {
                    EsCatalogo = 1
                    ,
                    EstaActivo = 1
                    ,
                    ImageIcon = ""
                };
                await NavigationService.NavigateToAsync<PopupAddCatalogoViewModel>(oCatalogo);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Importante", "Su perfil no posee los permisos correspondientes", "OK");
            }

        }
        public async Task EditCommandSync(Catalogo editaRegistro)
        {
            if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("DMINISTRADO"))
            {
                await NavigationService.NavigateToAsync<PopupAddCatalogoViewModel>(editaRegistro);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Importante", "Su perfil no posee los permisos correspondientes", "OK");
            }
        }
        public async Task AddCommandSync(Catalogo nuevoRegistro)
        {
            if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("DMINISTRADO"))
            {
                await NavigationService.NavigateToAsync<PopupAddCatalogoViewModel>(nuevoRegistro);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Importante", "Su perfil no posee los permisos correspondientes", "OK");
            }
        }
        public async Task AddParentCommandSync(Catalogo nuevoRegistro,Catalogo actualRegistro)
        {
            if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("DMINISTRADO"))
            {
                await NavigationService.NavigateToAsync<PopupAddCatalogoViewModel>(nuevoRegistro, actualRegistro);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Importante", "Su perfil no posee los permisos correspondientes", "OK");
            }
        }
        public async Task GoBack()
        {
            await NavigationService.NavigateToRootPage();
        }
        #endregion 5.MétodosGenerales
    }
}
