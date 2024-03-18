using DataModel.DTO.Administracion;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Productos;
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
using Escant_App.Views.Productos;
using Rg.Plugins.Popup.Services;

namespace Escant_App.ViewModels.Productos
{
    public class PrecioViewModel : ViewModelBase
    {
        #region 0.Declaración Variabales
        /// <summary>
        /// 0. Declaración de las variables         
        /// </summary>
        /// 0.0 Fijos en cada Model
        private SwitchAdministracion _switchAdministracion;
        private SwitchProductos _switchProductos;
        private Generales.Generales generales = new Generales.Generales();
        private readonly IServicioAdministracion_Catalogo _catalogoServicio;
        private readonly IServicioProductos_Precio _precioServicio;
        private bool _isSettingExists;
        private string _facturacionSettingId;

        private Precio _item;
        private Precio _parentItem;

        /// <summary>
        /// 0.1 Declaración de las variables de lectura y escritura para los controles
        /// </summary>
        private ObservableCollection<Precio> _precioInfo;
        private Precio _itemSeleccionado;
        private bool _visible;
        private SfPopupLayout popupLayout;
        private List<Precio> _precioList = new List<Precio>();
        private List<Catalogo> _menuList = new List<Catalogo>();
        private Catalogo _catalogoSeleccionado;
        private Precio _precioSeleccionado;
        private List<Precio> CatalogoPrecio = new List<Precio>();

        private bool _isVisibleBotones;
        private string _menuImagen;

        internal SfTreeView TreeView { get; set; }
        #endregion 0.Declaración Variabales

        #region 1.Constructor
        public PrecioViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioAdministracion_Catalogo catalogoServicio
                                 , IServicioProductos_Precio precioServicio)
        {
            BindingContext = new ConectividadModelo(seguridadServicio);
            _catalogoServicio = catalogoServicio;
            _precioServicio = precioServicio;
            _switchAdministracion = new SwitchAdministracion(_catalogoServicio);
            _switchProductos = new SwitchProductos(_precioServicio);
            _item = new Precio();
            _parentItem = new Precio();
            /*MessagingCenter.Unsubscribe<Orden>(this, "OnDataSourcePopupAddPrecioChanged");
            MessagingCenter.Subscribe<string>(this, "OnDataSourcePopupAddPrecioChanged", (args) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await LoadArbol();
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
        public ObservableCollection<Precio> PrecioInfo
        {
            get => _precioInfo;
            set
            {
                _precioInfo = value;
                RaisePropertyChanged(() => PrecioInfo);
            }
        }

        public List<Precio> PrecioList
        {
            get => _precioList;
            set
            {
                _precioList = value;
                RaisePropertyChanged(() => PrecioList);
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
        public Precio PrecioSeleccionado
        {
            get => _precioSeleccionado;
            set
            {
                _precioSeleccionado = value;
                RaisePropertyChanged(() => PrecioSeleccionado);
                LoadArbol();
            }
        }
        public Precio Item
        {
            get => _item;
            set
            {
                _item = value;
                RaisePropertyChanged(() => Item);
            }
        }
        public Precio ParentItem
        {
            get => _parentItem;
            set
            {
                _parentItem = value;
                RaisePropertyChanged(() => ParentItem);
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
            await LoadListaProductos();
        }
        public async Task LoadListaProductos()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var precios = await _switchProductos.ConsultaPrecios(new Precio() { ValorConversion=1 }, estaConectado);
            if (precios != null && precios.Count > 0)
            {
                PrecioList = precios.OrderBy(x=>x.NombreItem).ToList();
            }
            else
            {
                DialogService.ShowToast("Ingrese un ítem, producto, servicio o artículo en el Sistema para definir los precios");
            }
        }
        public async Task LoadArbol()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            CatalogoPrecio = await _switchProductos.ConsultaPrecios(new Precio() { IdItem = PrecioSeleccionado.IdItem }, estaConectado);
            if (CatalogoPrecio != null && CatalogoPrecio.Count > 0)
            {
                _isSettingExists = true;
                //_facturacionSettingId = setting.Id;
                await armaArbol(CatalogoPrecio);
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
            await LoadListaProductos();
            //Bind data for edit mode
            if (navigationData is Precio registro && registro != null)
            {
                PrecioSeleccionado = PrecioList?.FirstOrDefault(x => x.CodigoItem == registro.CodigoItem
                                                                      && x.ValorConversion == 1);
            }

            IsBusy = false;

        }

        public async Task CreaPrecio()
        {
            IsBusy = true;
            if (PrecioSeleccionado != null && PrecioSeleccionado.Id!="")
            {
                bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                var unidad = await _switchAdministracion.ConsultaCatalogos(new Catalogo() { CodigoCatalogo = "unidad" }, estaConectado);
                //MenuList = menu.Where(x => x.EsCatalogo == 0).ToList();
                List<Precio> Precios = unidad.Where(x => x.Id == PrecioSeleccionado.IdUnidad).Select(X => new Precio
                {
                     Id = PrecioSeleccionado.Id
                    ,IdItem = PrecioSeleccionado.IdItem
                    ,IdUnidadPadre = X.IdPadre
                    ,IdUnidad = PrecioSeleccionado.IdUnidad
                    ,CodigoItem = PrecioSeleccionado.CodigoItem
                    ,NombreItem = PrecioSeleccionado.NombreItem
                    ,Unidad = PrecioSeleccionado.Unidad
                    ,ValorUnidadOriginal = PrecioSeleccionado.ValorUnidadOriginal
                    ,IdUnidadConvertida = PrecioSeleccionado.IdUnidadConvertida
                    ,UnidadConvertida = PrecioSeleccionado.UnidadConvertida
                    ,ValorConversion = PrecioSeleccionado.ValorConversion
                    ,PrecioVentaSugerido1 = PrecioSeleccionado.PrecioVentaSugerido1
                    ,PrecioVentaSugerido2 = PrecioSeleccionado.PrecioVentaSugerido2
                    ,PrecioVenta = PrecioSeleccionado.PrecioVenta
                    ,FechaDesde = PrecioSeleccionado.FechaDesde
                    ,FechaHasta = PrecioSeleccionado.FechaHasta
                    ,EstaActivo=PrecioSeleccionado.EstaActivo
                    ,subCatalogos=GetChildren(unidad,PrecioSeleccionado.IdUnidad)
                }).ToList();
                var listadoPrecios = GetAItems(Precios.FirstOrDefault());
                //List<Precio> result = Precios.SelectMany(x => x.subCatalogos).ToList();
                //var flatList = SelectRecursive<Precio>(Precios,f => f.subCatalogos1).ToList();
                await _switchProductos.GuardaRegistro_Precios(listadoPrecios, estaConectado, false);
                //var PreciosItemSerializado = JsonConvert.SerializeObject(listadoPrecios);
                //await _switchProductos.GuardaRegistro_PreciosItem(new Item() { Id=PrecioSeleccionado.IdItem,PreciosItemSerializado=PreciosItemSerializado}, estaConectado, true);
                await LoadArbol();
                /*await Task.WhenAll(
                          _switchProductos.GuardaRegistro_Precios(listadoPrecios, estaConectado, false),
                           LoadArbol()
                        );*/
            }
            else
            {
                DialogService.ShowToast("Seleccione un Perfil");
            }
            IsBusy = false;
        }

        List<Precio> GetAItems(Precio item)
        {
            List<Precio> list = new List<Precio>();

            GetItems(item, list);
            return list;
        }

        void GetItems(Precio item, List<Precio> list)
        {
            foreach (Precio affectedItem in item.subCatalogos)
            {
                GetItems(affectedItem, list);
            }

            list.Add(item);
        }


        public IEnumerable<T> SelectRecursive<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> recursiveSelector)
        {
            foreach (var i in source)
            {
                yield return i;

                var directChildren = recursiveSelector(i);
                var allChildren = SelectRecursive(directChildren, recursiveSelector);

                foreach (var c in allChildren)
                {
                    yield return c;
                }
            }
        }

        public ObservableCollection<Precio> GetChildren(List<Catalogo> precios, string parentId)
        {
            return generales.ToObservableCollection<Precio>(precios
                    .Where(c => c.IdPadre == parentId)
                    .Select(c => new Precio
                    {
                        Id = Generator.GenerateKey()
                    ,
                        IdItem = PrecioSeleccionado.IdItem
                    ,
                        IdUnidadPadre = c.IdPadre
                    ,
                        IdUnidad = PrecioSeleccionado.IdUnidad
                    ,
                        CodigoItem = PrecioSeleccionado.CodigoItem
                    ,
                        NombreItem = PrecioSeleccionado.NombreItem
                    ,
                        Unidad = PrecioSeleccionado.Unidad
                    ,
                        ValorUnidadOriginal = PrecioSeleccionado.ValorUnidadOriginal
                    ,
                        IdUnidadConvertida = c.Id
                    ,
                        UnidadConvertida = c.Nombre
                    ,
                        ValorConversion = (float)Math.Round(c.ValorConversion/PrecioSeleccionado.ValorUnidadOriginal,4)
                    ,
                        PrecioVentaSugerido1 = (float)Math.Round(PrecioSeleccionado.PrecioVenta* (float)Math.Round(c.ValorConversion / PrecioSeleccionado.ValorUnidadOriginal, 4),2)
                    ,
                        PrecioVentaSugerido2 = (float)Math.Round(PrecioSeleccionado.PrecioVenta * (float)Math.Round(c.ValorConversion / PrecioSeleccionado.ValorUnidadOriginal, 4)+0.03, 2)
                    ,
                        PrecioVenta = 0
                    ,
                        FechaDesde = PrecioSeleccionado.FechaDesde
                    ,
                        FechaHasta = PrecioSeleccionado.FechaHasta
                    ,
                        EstaActivo = 0
                    ,
                        subCatalogos = GetChildren(precios, c.Id)
                    })
                    .ToList());
        }

        public async Task ActualizaPrecio()
        {
            IsBusy = true;
            /*if (CatalogoSeleccionado != null && CatalogoPerfilMenu.Count() > 0)
            {
                bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                var menu = await _switchAdministracion.ConsultaCatalogos(new Catalogo() { CodigoCatalogo = "menu" }, estaConectado);
                //MenuList = menu.Where(x => x.EsCatalogo == 0).ToList();
                //Shifts.Where(s => !EmployeeShifts.Where(es => es.ShiftID == s.ShiftID).Any());
                List<PerfilMenu> perfilMenus = menu.Where(x => x.EsCatalogo == 0
                                                        && !CatalogoPerfilMenu.Where(y => y.IdMenu == x.Id).Any()).Select(X => new PerfilMenu
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
            }*/
            IsBusy = false;
        }

        public async Task EditCommandSync(Precio item, Precio parentItem)
        {

            //await NavigationService.NavigateToPopupAsync<PopupAddPrecioItemViewModel>(item);
            var returnPage = await NavigationService.NavigateToPopupAsync<PopupAddPrecioItemViewModel>(item);
            var popupPage = returnPage as PopupAddPrecioItemView;
            popupPage.CloseButtonEventHandler += async (sender, obj) =>
            {
                var isCancel = ((PopupAddPrecioItemView)sender).IsCancel;
                if (!isCancel)
                {
                    var resultado = ((PopupAddPrecioItemView)sender).PopupResult;
                    if (resultado != null)
                    {
                        PrecioSeleccionado = resultado;                        
                    }                    
                }                
                await PopupNavigation.Instance.PopAsync();
            };
            var result = await popupPage.PageClosedTask;            
            //await LoadArbol();



            /*if (PerfilMenuInfo != null && PerfilMenuInfo.Count() > 0)
            {
                bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                List<PerfilMenu> perfilMenus = CatalogoPerfilMenu.Where(x => x.Id == item.Id        //si mismo
                                                                || x.IdMenuPadre == item.IdMenu   //hijos
                                                                || (parentItem != null
                                                                && x.Id == parentItem.Id
                                                                && ((CatalogoPerfilMenu.Where(y => y.IdMenuPadre == parentItem.IdMenu
                                                                                          && y.EstaActivo == item.EstaActivo
                                                                                          && y.Id != item.Id
                                                                                            ).Count() == 0
                                                                   && item.EstaActivo == 1)
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
            }*/
        }
        #endregion 4.MétodosAdministraciónDatos

        #region 5.MétodosGenerales
        public async Task armaArbol(List<Precio> listaPrecio)
        {
            var nodeImageInfo = new ObservableCollection<Precio>();
            var ldetalleGrupo = new ObservableCollection<Precio>();
            Precio grupo;
            Precio detalleGrupo;

            try
            {
                var dtRegistros = generales.ToDataTable<Precio>(listaPrecio);

                if (dtRegistros != null && dtRegistros.Rows.Count > 0)
                {
                    foreach (DataRow MenuPadre in dtRegistros.Select("IdUnidadPadre='0'", "Unidad ASC"))
                    {
                        grupo = new Precio()
                        {
                            Id = MenuPadre["Id"].ToString()
                            ,
                            IdItem = MenuPadre["IdItem"].ToString()
                            ,
                            IdUnidadPadre = MenuPadre["IdUnidadPadre"].ToString()
                            ,
                            IdUnidad = MenuPadre["IdUnidad"].ToString()
                            ,
                            CodigoItem = MenuPadre["CodigoItem"].ToString()
                            ,
                            NombreItem = MenuPadre["NombreItem"].ToString()
                            ,
                            Unidad = MenuPadre["Unidad"].ToString()
                            ,
                            ValorUnidadOriginal = MenuPadre["ValorUnidadOriginal"] != null ? float.Parse((MenuPadre["ValorUnidadOriginal"]).ToString()) : 0
                            ,
                            IdUnidadConvertida = MenuPadre["IdUnidadConvertida"].ToString()
                            ,
                            UnidadConvertida = MenuPadre["UnidadConvertida"].ToString()
                            ,
                            ValorConversion = MenuPadre["ValorConversion"] != null ? float.Parse((MenuPadre["ValorConversion"]).ToString()) : 0
                            ,
                            PrecioVentaSugerido1 = MenuPadre["PrecioVentaSugerido1"] != null ? float.Parse((MenuPadre["PrecioVentaSugerido1"]).ToString()) : 0
                            ,
                            PrecioVentaSugerido2 = MenuPadre["PrecioVentaSugerido2"] != null ? float.Parse((MenuPadre["PrecioVentaSugerido2"]).ToString()) : 0
                            ,
                            PrecioVenta = MenuPadre["PrecioVenta"] != null ? float.Parse((MenuPadre["PrecioVenta"]).ToString()) : 0
                            ,
                            FechaDesde = MenuPadre["FechaDesde"] != null ? long.Parse((MenuPadre["FechaDesde"]).ToString()) : 0
                            ,
                            FechaHasta = MenuPadre["FechaHasta"] != null ? long.Parse((MenuPadre["FechaHasta"]).ToString()) : 0
                            ,
                            EstaActivo = MenuPadre["EstaActivo"] != null && MenuPadre["EstaActivo"].ToString() == "1" ? 1 : 0
                            ,
                            IconoMenu = /*MenuPadre["ImageIcon"] != null && MenuPadre["ImageIcon"].ToString() != "" ? JsonConvert.DeserializeObject<FuenteIconos>(MenuPadre["ImageIcon"].ToString()) : */new FuenteIconos()
                            {
                                NombreIcono = "Configurar"
                                ,
                                CodigoImagen = "&#xf0e8;"

                            }
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
                        if (dtRegistros.Select("IdUnidadPadre='" + grupo.IdUnidadConvertida + "'").Length > 0)
                        {
                            ldetalleGrupo = new ObservableCollection<Precio>();
                            ldetalleGrupo = await armaArbolHijos(dtRegistros, grupo.IdUnidadConvertida);
                            grupo.subCatalogos = ldetalleGrupo;
                        }
                        nodeImageInfo.Add(grupo);
                    }
                }
                PrecioInfo = nodeImageInfo;
            }
            catch(Exception ex)
            {
                var y = ex.Message.ToString();
            }
        }
        private async Task<ObservableCollection<Precio>> armaArbolHijos(DataTable herencia, string IdPadre)
        {
            Precio detalleGrupo;
            var ldetalleGrupo = new ObservableCollection<Precio>();

            try
            {
                foreach (DataRow Menu in herencia.Select("IdUnidadPadre='" + IdPadre + "'", "Unidad ASC"))
                {
                    detalleGrupo = new Precio()
                    {
                        Id = Menu["Id"].ToString()
                        ,
                        IdItem = Menu["IdItem"].ToString()
                            ,
                        IdUnidadPadre = Menu["IdUnidadPadre"].ToString()
                            ,
                        IdUnidad = Menu["IdUnidad"].ToString()
                            ,
                        CodigoItem = Menu["CodigoItem"].ToString()
                            ,
                        NombreItem = Menu["NombreItem"].ToString()
                            ,
                        Unidad = Menu["Unidad"].ToString()
                            ,
                        ValorUnidadOriginal = Menu["ValorUnidadOriginal"] != null ? float.Parse((Menu["ValorUnidadOriginal"]).ToString()) : 0
                            ,
                        IdUnidadConvertida = Menu["IdUnidadConvertida"].ToString()
                            ,
                        UnidadConvertida = Menu["UnidadConvertida"].ToString()
                            ,
                        ValorConversion = Menu["ValorConversion"] != null ? float.Parse((Menu["ValorConversion"]).ToString()) : 0
                            ,
                        PrecioVentaSugerido1 = Menu["PrecioVentaSugerido1"] != null ? float.Parse((Menu["PrecioVentaSugerido1"]).ToString()) : 0
                            ,
                        PrecioVentaSugerido2 = Menu["PrecioVentaSugerido2"] != null ? float.Parse((Menu["PrecioVentaSugerido2"]).ToString()) : 0
                            ,
                        PrecioVenta = Menu["PrecioVenta"] != null ? float.Parse((Menu["PrecioVenta"]).ToString()) : 0
                            ,
                        FechaDesde = Menu["FechaDesde"] != null ? long.Parse((Menu["FechaDesde"]).ToString()) : 0
                            ,
                        FechaHasta = Menu["FechaHasta"] != null ? long.Parse((Menu["FechaHasta"]).ToString()) : 0
                            ,
                        IconoMenu = /*Menu["ImageIcon"] != null && Menu["ImageIcon"].ToString() != "" ? JsonConvert.DeserializeObject<FuenteIconos>(Menu["ImageIcon"].ToString()) : */new FuenteIconos()
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
                    if (herencia.Select("IdUnidadPadre='" + detalleGrupo.IdUnidadConvertida + "'").Length > 0)
                    {
                        var ldetalleGrupoHijos = new ObservableCollection<Precio>();
                        ldetalleGrupoHijos = await armaArbolHijos(herencia, detalleGrupo.IdUnidadConvertida);
                        detalleGrupo.subCatalogos = ldetalleGrupoHijos;
                    }
                    ldetalleGrupo.Add(detalleGrupo);
                }
            }
            catch(Exception ex)
            {
                var z = ex.Message.ToString();
            }
            return ldetalleGrupo;
        }
        public async Task GoBack()
        {
            await NavigationService.NavigateToRootPage();
        }
        #endregion 5.MétodosGenerales
    }
}