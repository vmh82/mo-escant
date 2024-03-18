using DataModel.DTO.Administracion;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Seguridad;
using DataModel.Helpers;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using Escant_App.ViewModels.Base;
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

namespace Escant_App.ViewModels.Seguridad
{
    public class PerfilMenuViewModel : ViewModelBase
    {
        #region 0.Declaración Variabales
        /// <summary>
        /// 0. Declaración de las variables         
        /// </summary>
        /// 0.0 Fijos en cada Model
        private SwitchAdministracion _switchAdministracion;
        private SwitchSeguridad _switchSeguridad;
        private Generales.Generales generales = new Generales.Generales();
        private readonly IServicioAdministracion_Catalogo _catalogoServicio;
        private readonly IServicioSeguridad_PerfilMenu _perfilMenuServicio;
        private bool _isSettingExists;
        private string _facturacionSettingId;

        private PerfilMenu _item;
        private PerfilMenu _parentItem;

        /// <summary>
        /// 0.1 Declaración de las variables de lectura y escritura para los controles
        /// </summary>
        private ObservableCollection<PerfilMenu> _perfilMenuInfo;
        private PerfilMenu _itemSeleccionado;
        private bool _visible;
        private SfPopupLayout popupLayout;
        private List<Catalogo> _catalogoList = new List<Catalogo>();
        private List<Catalogo> _menuList = new List<Catalogo>();
        private Catalogo _catalogoSeleccionado;
        private List<PerfilMenu> CatalogoPerfilMenu=new List<PerfilMenu>();

        private bool _isVisibleBotones;
        private string _menuImagen;

        internal SfTreeView TreeView { get; set; }
        #endregion 0.Declaración Variabales

        #region 1.Constructor
        public PerfilMenuViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioAdministracion_Catalogo catalogoServicio
                                 , IServicioSeguridad_PerfilMenu perfilMenuServicio)
        {
            BindingContext = new ConectividadModelo(seguridadServicio);
            _catalogoServicio = catalogoServicio;
            _perfilMenuServicio = perfilMenuServicio;
            _switchAdministracion = new SwitchAdministracion(_catalogoServicio);
            _switchSeguridad = new SwitchSeguridad(_perfilMenuServicio);
            _item = new PerfilMenu();
            _parentItem = new PerfilMenu();
            /*MessagingCenter.Subscribe<string>(this, "OnDataSourceChanged", (args) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await LoadSetting();
                    IsBusy = false;
                    ItemIndex = -1;
                });
            });*/
            IsBusy = true;
            Task.Run(async () =>
            {
                await LoadSetting();
            });

            IsBusy = false;
        }
        #endregion 1.Constructor

        #region 2.LecturaYAsignacionVariables

        /// 2. Asignacion y Lectura de variables vinculadas a los controles
        /// <summary>
        /// Método Asignación o lectura de CodigoEstablecimiento
        /// </summary>
        public ObservableCollection<PerfilMenu> PerfilMenuInfo
        {
            get => _perfilMenuInfo;
            set
            {
                _perfilMenuInfo = value;
                RaisePropertyChanged(() => PerfilMenuInfo);
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
        public List<Catalogo> MenuList
        {
            get => _menuList;
            set
            {
                _menuList = value;
                RaisePropertyChanged(() => MenuList);
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
        public PerfilMenu Item
        {
            get => _item;
            set
            {
                _item = value;
                RaisePropertyChanged(() => Item);
            }
        }
        public PerfilMenu ParentItem
        {
            get => _parentItem;
            set
            {
                _parentItem = value;
                RaisePropertyChanged(() => ParentItem);
            }
        }

        public bool IsVisibleBotones {
            get => _isVisibleBotones;
            set
            {
                _isVisibleBotones = value;
                RaisePropertyChanged(() => IsVisibleBotones);
            }
        }
        public string MenuImagen
        {
            get => _menuImagen;
            set
            {
                _menuImagen = value;
                RaisePropertyChanged(() => MenuImagen);
            }
        }

        
        /// <summary>
        /// 3. Definición de métodos de la pantalla
        /// </summary>
        /// <summary>
        #endregion 2.LecturaYAsignacionVariables

        #region 3.DefinicionMetodos
        //public ICommand OnAddItem => new Command(async () => await ToolbarAddCommandAsync());
        #endregion 3.DefinicionMetodos

        /// <summary>
        /// 4. Métodos de administración de datos
        /// </summary>
        /// <returns></returns>
        #region 4.MétodosAdministraciónDatos  
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
            var catalogo = await _switchAdministracion.ConsultaCatalogos(new Catalogo() { CodigoCatalogo = "perfil" }, estaConectado);
            if (catalogo != null && catalogo.Count > 0)
            {
                CatalogoList = catalogo.Where(x => x.EsCatalogo == 0).ToList();
            }
            else
            {
                DialogService.ShowToast("Ingrese un Perfil en el Catálogo");
            }
        }
        public async Task LoadArbol()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            CatalogoPerfilMenu = await _switchSeguridad.ConsultaPerfilesMenu(new PerfilMenu() { CodigoPerfil = CatalogoSeleccionado.Codigo }, estaConectado);
            if (CatalogoPerfilMenu != null && CatalogoPerfilMenu.Count > 0)
            {
                _isSettingExists = true;
                //_facturacionSettingId = setting.Id;
                await armaArbol(CatalogoPerfilMenu);
                /*var count = ImageNodeInfo != null ? ImageNodeInfo.Count : 0;
                var data = ImageNodeInfo != null ? ImageNodeInfo[count - 1] : null;
                TreeView.BringIntoView(data, false, true, Syncfusion.XForms.TreeView.ScrollToPosition.MakeVisible);
                */

                //if(setting.Logo != null)
                //    StoreImageSource = ImageSource.FromStream(() => new MemoryStream(setting.Logo));
            }
            else
            {
                PerfilMenuInfo = new ObservableCollection<PerfilMenu>();
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
            await LoadListaCatalogo();
            //Bind data for edit mode
            if (navigationData is Catalogo registro && registro != null)
            {
                CatalogoSeleccionado = CatalogoList?.FirstOrDefault(x => x.CodigoCatalogo == registro.CodigoCatalogo
                                                                      && x.EsCatalogo == 1);
            }

            IsBusy = false;

        }

        public async Task CreaPerfilMenu()
        {
            IsBusy = true;
            if (CatalogoSeleccionado != null && CatalogoPerfilMenu.Count()==0)
            {
                bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                var menu = await _switchAdministracion.ConsultaCatalogos(new Catalogo() { CodigoCatalogo = "menu" }, estaConectado);
                //MenuList = menu.Where(x => x.EsCatalogo == 0).ToList();
                List<PerfilMenu> perfilMenus = menu.Where(x => x.EsCatalogo == 0).Select(X => new PerfilMenu
                {
                    Id = Generator.GenerateKey()
                                                                                  ,
                    IdPerfil = CatalogoSeleccionado.Id
                                                                                  ,
                    CodigoPerfil = CatalogoSeleccionado.Codigo
                                                                                  ,
                    IdMenu = X.Id
                                                                                  ,
                    IdMenuPadre = X.IdPadre
                                                                                  ,
                    NombreMenu = X.Nombre
                                                                                  ,
                    NombreFormulario = X.NombreFormulario
                                                                                  ,
                    LabelTitulo = X.LabelTitulo
                                                                                  ,
                    LabelDescripcion = X.LabelDescripcion
                                                                                  ,
                    Nivel = X.Nivel

                                                                                  ,
                    Orden = X.Orden,
                    ImageIcon = X.ImageIcon
                                                                                  ,
                    EstaActivo = 0
                }).ToList();
                await _switchSeguridad.GuardaRegistro_PerfilesMenu(perfilMenus, estaConectado, false);
                await LoadArbol();
                
            }
            else {
                DialogService.ShowToast("Seleccione un Perfil");
            }
            IsBusy = false;
        }

        public async Task ActualizaPerfilMenu()
        {
            IsBusy = true;
            if (CatalogoSeleccionado != null && CatalogoPerfilMenu.Count() > 0)
            {
                bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                var menu = await _switchAdministracion.ConsultaCatalogos(new Catalogo() { CodigoCatalogo = "menu" }, estaConectado);
                //MenuList = menu.Where(x => x.EsCatalogo == 0).ToList();
                //Shifts.Where(s => !EmployeeShifts.Where(es => es.ShiftID == s.ShiftID).Any());
                List<PerfilMenu> perfilMenus = menu.Where(x => x.EsCatalogo == 0
                                                        && !CatalogoPerfilMenu.Where(y=>y.IdMenu==x.Id).Any()).Select(X => new PerfilMenu
                {
                    Id = Generator.GenerateKey()
                                                                                  ,
                    IdPerfil = CatalogoSeleccionado.Id
                                                                                  ,
                    CodigoPerfil = CatalogoSeleccionado.Codigo
                                                                                  ,
                    IdMenu = X.Id
                                                                                  ,
                    IdMenuPadre = X.IdPadre
                                                                                  ,
                    NombreMenu = X.Nombre,
                    NombreFormulario = X.NombreFormulario,
                    LabelTitulo = X.LabelTitulo,
                    LabelDescripcion = X.LabelDescripcion,
                    Nivel = X.Nivel,
                    Orden = X.Orden,
                    ImageIcon = X.ImageIcon
                                                                                  ,
                    EstaActivo = 0
                }).ToList();
                await _switchSeguridad.GuardaRegistro_PerfilesMenu(perfilMenus, estaConectado, false);
                await LoadArbol();
                        
            }
            else
            {
                DialogService.ShowToast("Seleccione un Perfil");
            }
            IsBusy = false;
        }

        public async Task EditCommandSync(PerfilMenu item, PerfilMenu parentItem)
        {

            IsBusy = true;
            if (PerfilMenuInfo != null && PerfilMenuInfo.Count() > 0)
            {
                bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                List<PerfilMenu> perfilMenus = CatalogoPerfilMenu.Where(x => x.Id == item.Id        //si mismo
                                                                || x.IdMenuPadre == item.IdMenu   //hijos
                                                                || (parentItem!=null
                                                                && x.Id == parentItem.Id
                                                                && ((CatalogoPerfilMenu.Where(y=>y.IdMenuPadre==parentItem.IdMenu  
                                                                                          && y.EstaActivo==item.EstaActivo
                                                                                          && y.Id!=item.Id
                                                                                            ).Count()==0
                                                                   && item.EstaActivo==1)
                                                                   || (CatalogoPerfilMenu.Where(y => y.IdMenuPadre == parentItem.IdMenu
                                                                                           && y.EstaActivo == item.EstaActivo
                                                                                           && y.Id != item.Id
                                                                                            ).Count() >= 0
                                                                   && item.EstaActivo == 0)
                                                                   )

                                                                )      //PAdre

                                                                ).Select(X => new PerfilMenu
                                                                {
                                                                    Id = X.Id
                                                                                  ,
                                                                    IdPerfil = X.IdPerfil
                                                                                  ,
                                                                    CodigoPerfil = X.CodigoPerfil
                                                                                  ,
                                                                    IdMenu = X.IdMenu
                                                                                  ,
                                                                    IdMenuPadre = X.IdMenuPadre
                                                                                  ,
                                                                    NombreMenu = X.NombreMenu
                                                                                  ,
                                                                    NombreFormulario = X.NombreFormulario
                                                                                  ,
                                                                    LabelTitulo = X.LabelTitulo
                                                                                  ,
                                                                    LabelDescripcion = X.LabelDescripcion
                                                                                  ,
                                                                    Nivel = X.Nivel
                                                                    ,
                                                                    Orden = X.Orden
                                                                                  ,
                                                                    ImageIcon = X.ImageIcon
                                                                                  ,
                                                                    EstaActivo = item.EstaActivo == 1 ? 0 : 1,
                                                                    EsEdicion = 1
                                                                }).ToList();
                await _switchSeguridad.GuardaRegistro_PerfilesMenu(perfilMenus, estaConectado, true);
                await LoadArbol();                            
            }
        }
        #endregion 4.MétodosAdministraciónDatos

        #region 5.MétodosGenerales
        public async Task armaArbol(List<PerfilMenu> listaPerfilMenu)
        {
            var nodeImageInfo = new ObservableCollection<PerfilMenu>();
            var ldetalleGrupo = new ObservableCollection<PerfilMenu>();
            PerfilMenu grupo;
            PerfilMenu detalleGrupo;


            var dtRegistros = generales.ToDataTable<PerfilMenu>(listaPerfilMenu);

            if (dtRegistros != null && dtRegistros.Rows.Count > 0)
            {
                foreach (DataRow MenuPadre in dtRegistros.Select("Nivel=1", "Orden ASC"))
                {
                    grupo = new PerfilMenu()
                    {
                        Id = MenuPadre["Id"].ToString()
                        ,
                        IdMenu = MenuPadre["IdMenu"].ToString()
                        ,
                        IdMenuPadre = MenuPadre["IdMenuPadre"].ToString()
                        ,
                        NombreMenu = MenuPadre["NombreMenu"].ToString()
                        ,
                        NombreFormulario = MenuPadre["NombreFormulario"].ToString()
                        ,
                        LabelTitulo = MenuPadre["LabelTitulo"].ToString()
                        ,
                        LabelDescripcion = MenuPadre["LabelDescripcion"].ToString()                        
                        ,                                                
                        Nivel = MenuPadre["Nivel"] != null ? Convert.ToInt32(MenuPadre["Nivel"]) : 0
                        ,
                        Orden = MenuPadre["Orden"] != null ? Convert.ToInt32(MenuPadre["Orden"]) : 0
                        ,
                        ImageIcon = MenuPadre["ImageIcon"].ToString()
                        ,
                        IconoMenu = MenuPadre["ImageIcon"] != null && MenuPadre["ImageIcon"].ToString() != "" ? JsonConvert.DeserializeObject<FuenteIconos>(MenuPadre["ImageIcon"].ToString()) : new FuenteIconos()
                        {
                            NombreIcono = "Configurar"
                            ,
                            CodigoImagen = "&#xf0e8;"

                        },                        
                        EstaActivo = MenuPadre["EstaActivo"] != null && MenuPadre["EstaActivo"].ToString() == "1" ? 1 : 0
                        ,
                        IconoEstaActivo = MenuPadre["EstaActivo"] != null && MenuPadre["EstaActivo"].ToString() == "1" ? new FuenteIconos()
                        {
                            NombreIcono = "Activo"
                            ,
                            CodigoImagen = "&#xe5ca;"
                            ,
                            CodigoIcono = "\ue5ca"
                        } : new FuenteIconos()
                        {
                            NombreIcono = "Inactivo"
                            ,
                            CodigoImagen = "&#xe5c9;"
                            ,
                            CodigoIcono = "\ue5c9"
                        }

                    };
                    if (dtRegistros.Select("IdMenuPadre='" + grupo.IdMenu + "'").Length > 0)
                    {
                        ldetalleGrupo = new ObservableCollection<PerfilMenu>();
                        ldetalleGrupo = await armaArbolHijos(dtRegistros, grupo.IdMenu);
                        grupo.SubCatalogos = ldetalleGrupo;
                    }
                    nodeImageInfo.Add(grupo);
                }
            }
            PerfilMenuInfo = nodeImageInfo;
        }
        private async Task<ObservableCollection<PerfilMenu>> armaArbolHijos(DataTable herencia, string IdPadre)
        {
            PerfilMenu detalleGrupo;
            var ldetalleGrupo = new ObservableCollection<PerfilMenu>();

            foreach (DataRow Menu in herencia.Select("IdMenuPadre='" + IdPadre + "'", "Orden ASC"))
            {
                detalleGrupo = new PerfilMenu()
                {
                    Id = Menu["Id"].ToString()
                        ,
                    IdMenu = Menu["IdMenu"].ToString()
                        ,
                    IdMenuPadre = Menu["IdMenuPadre"].ToString()
                        ,
                    NombreMenu = Menu["NombreMenu"].ToString()
                        ,
                    NombreFormulario = Menu["NombreFormulario"].ToString()
                        ,
                    LabelTitulo = Menu["LabelTitulo"].ToString()
                        ,
                    LabelDescripcion = Menu["LabelDescripcion"].ToString()
                        ,
                    Nivel = Menu["Nivel"] != null ? Convert.ToInt32(Menu["Nivel"]) : 0
                    ,
                    Orden = Menu["Orden"] != null ? Convert.ToInt32(Menu["Orden"]) : 0
                        ,
                    ImageIcon = Menu["ImageIcon"].ToString()
                    ,
                    IconoMenu = Menu["ImageIcon"] != null && Menu["ImageIcon"].ToString() != "" ? JsonConvert.DeserializeObject<FuenteIconos>(Menu["ImageIcon"].ToString()) : new FuenteIconos()
                    {
                        NombreIcono = "Configurar"
                        ,
                        CodigoImagen = "&#xf0e8;"
                    },
                    EstaActivo = Menu["EstaActivo"] != null && Menu["EstaActivo"].ToString() == "1" ? 1 : 0
                        ,
                    IconoEstaActivo = Menu["EstaActivo"] != null && Menu["EstaActivo"].ToString() == "1" ? new FuenteIconos()
                    {
                        NombreIcono = "Activo"
                            ,
                        CodigoImagen = "&#xe5ca;"
                            ,
                        CodigoIcono = "\ue5ca"
                    } : new FuenteIconos()
                    {
                        NombreIcono = "Inactivo"
                            ,
                        CodigoImagen = "&#xe5c9;"
                            ,
                        CodigoIcono = "\ue5c9"
                    }
                };
                if (herencia.Select("IdMenuPadre='" + detalleGrupo.IdMenu + "'").Length > 0)
                {
                    var ldetalleGrupoHijos = new ObservableCollection<PerfilMenu>();
                    ldetalleGrupoHijos = await armaArbolHijos(herencia, detalleGrupo.IdMenu);
                    detalleGrupo.SubCatalogos = ldetalleGrupoHijos;
                }
                ldetalleGrupo.Add(detalleGrupo);
            }
            return ldetalleGrupo;
        }
        /*
        private async Task ToolbarAddCommandAsync()
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
        public async Task EditCommandSync(Catalogo editaRegistro)
        {
            await NavigationService.NavigateToAsync<PopupAddCatalogoViewModel>(editaRegistro);
        }
        public async Task AddCommandSync(Catalogo nuevoRegistro)
        {
            await NavigationService.NavigateToAsync<PopupAddCatalogoViewModel>(nuevoRegistro);
        }
        public async Task AddParentCommandSync(Catalogo nuevoRegistro, Catalogo actualRegistro)
        {
            await NavigationService.NavigateToPopupAsync<PopupAddCatalogoViewModel>(nuevoRegistro, actualRegistro);
        }*/
        public async Task GoBack()
        {
            await NavigationService.NavigateToRootPage();
        }
        #endregion 5.MétodosGenerales
    }
}
