using DataModel.Enums;
using DataModel.DTO;
using DataModel.DTO.Productos;

using ManijodaServicios.Resources.Texts;
using Escant_App.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ManijodaServicios.Switch;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Interfaces;
using DataModel.DTO.Compras;
using DataModel.DTO.Administracion;
using Syncfusion.XForms.ProgressBar;
using DataModel.Helpers;
using Newtonsoft.Json;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Configuracion;
using System.Collections.Specialized;
using System.ComponentModel;
using Escant_App.Helpers;
using Rg.Plugins.Popup.Services;
using Escant_App.ViewModels.Administracion;
using Escant_App.ViewModels.Documentos;
using Xamarin.Forms.Internals;
using Escant_App.Views.Inventario;
using ManijodaServicios.AppSettings;

namespace Escant_App.ViewModels.Inventario
{
    public class DetalleInventarioViewModel:ViewModelBase
    {        

        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración
        private SwitchProductos _switchProductos;
        private SwitchAdministracion _switchAdministracion;
        private SwitchCompras _switchCompras;
        private SwitchConfiguracion _switchConfiguracion;
        private Generales.Generales generales;

        private readonly IServicioProductos_Item _itemServicio;
        private readonly IServicioAdministracion_Catalogo _catalogoServicio;
        private readonly IServicioCompras_Orden _ordenServicio;
        private readonly IServicioCompras_OrdenDetalleCompra _ordenDetalleCompraServicio;
        private readonly IServicioCompras_OrdenEstado _ordenEstadoServicio;
        private readonly IServicioFirestore _servicioFirestore;
        private readonly ISettingsService _settingsServicio;
        #endregion 1.1 Variables de Integración

        #region 1.2 Variables de Instancia de Objetos para la Clase
        private ObservableCollection<Item> _registros;
        private Item _registroSeleccionado;
        private ObservableCollection<OrdenDetalleCompra> _ordenDetalleCompra = new ObservableCollection<OrdenDetalleCompra>();
        private ObservableCollection<OrdenDetalleCompraDet> _ordenDetalleCompraRecibido = new ObservableCollection<OrdenDetalleCompraDet>();
        private OrdenDetalleCompra _ordenDetalleCompraSeleccionado;
        private Orden _ordenSeleccionado;
        private ObservableCollection<OrdenEstado> _estadoInfoCollection = new ObservableCollection<OrdenEstado>();
        private Empresa _proveedorCompraSeleccionado;
        #endregion 1.2 Variables de Instancia de Objetos para la Clase

        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        private string _imagenBotonOrden;
        private string _proveedorCompra;
        private string _idProveedorCompra;
        private string _identificacion;
        private int IdEstadoEdicion = ((int)Enum.Parse(typeof(EstadoOrdenInventario), "Ingresado"));
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo

        #region 1.4 Variables de Control de Datos
        private bool _sinProductosOrdenDetalleCompra = true;
        public bool _tieneItemsSeleccionados;
        public float _cantidadOrden;
        public float _valorTotal;
        public float _valorDescuento;
        public float _valorFinal;
        public string _resumenRegistrosOrdenCompra;
        public bool _isVisibleDetalleCarrito;
        public bool _isVisibleEstadoOrden;
        #endregion 1.4 Variables de Control de Datos

        #region 1.5 Variables de Control de Errores
        #endregion 1.5 Variables de Control de Errores

        #region 1.6 Acciones
        public Action<bool> AbreOrden { get; set; }
        public Action<bool> AbreOrdenEstado { get; set; }
        public Action<bool> SeteaNotificacion { get; set; }
        #endregion 1.6 Acciones       

        #endregion 1. Declaración de Variables

        #region 2.InstanciaAsignaVariablesControles

        #region 2.2 Objetos para la Clase
        public ObservableCollection<DataModel.DTO.Productos.Item> Registros
        {
            get => _registros;
            set
            {
                _registros = value;
                RaisePropertyChanged(() => Registros);
            }
        }
        public Item RegistroSeleccionado
        {
            get => _registroSeleccionado;
            set
            {
                _registroSeleccionado = value;
                RaisePropertyChanged(() => RegistroSeleccionado);
            }
        }
        public ObservableCollection<OrdenDetalleCompra> OrdenDetalleCompra
        {
            get => _ordenDetalleCompra;
            set
            {
                _ordenDetalleCompra = value;
                RaisePropertyChanged(() => OrdenDetalleCompra);
            }
        }
        public ObservableCollection<OrdenDetalleCompraDet> OrdenDetalleCompraRecibido
        {
            get => _ordenDetalleCompraRecibido;
            set
            {
                _ordenDetalleCompraRecibido = value;
                RaisePropertyChanged(() => OrdenDetalleCompraRecibido);
            }
        }
        public OrdenDetalleCompra OrdenDetalleCompraSeleccionado
        {
            get => _ordenDetalleCompraSeleccionado;
            set
            {
                _ordenDetalleCompraSeleccionado = value;
                RaisePropertyChanged(() => OrdenDetalleCompraSeleccionado);
            }
        }
        public Orden OrdenSeleccionado
        {
            get => _ordenSeleccionado;
            set
            {
                _ordenSeleccionado = value;
                RaisePropertyChanged(() => OrdenSeleccionado);
            }
        }

        /// <summary>
        /// Gets or sets ShipmentInfoCollection
        /// </summary>
        public ObservableCollection<OrdenEstado> EstadoInfoCollection
        {
            get => _estadoInfoCollection;
            set
            {
                _estadoInfoCollection = value;
                RaisePropertyChanged(() => EstadoInfoCollection);
            }
        }


        public Empresa ProveedorCompraSeleccionado
        {
            get => _proveedorCompraSeleccionado;
            set
            {
                _proveedorCompraSeleccionado = value;
                RaisePropertyChanged(() => ProveedorCompraSeleccionado);
            }
        }

        #endregion 2.2 Objetos para la Clase

        #region 2.3 
        //bag.png
        public string ImagenBotonOrden
        {
            get => _imagenBotonOrden;
            set
            {
                _imagenBotonOrden = value;
                RaisePropertyChanged(() => ImagenBotonOrden);
            }
        }
        public string ProveedorCompra
        {
            get => _proveedorCompra;
            set
            {
                _proveedorCompra = value;
                RaisePropertyChanged(() => ProveedorCompra);
            }
        }
        public string IdProveedorCompra
        {
            get => _idProveedorCompra;
            set
            {
                _idProveedorCompra = value;
                RaisePropertyChanged(() => IdProveedorCompra);
            }
        }
        public string Identificacion
        {
            get => _identificacion;
            set
            {
                _identificacion = value;
                RaisePropertyChanged(() => Identificacion);
            }
        }
        #endregion 2.3

        #region 2.4 Control de Datos
        public float CantidadOrden
        {
            get => _cantidadOrden;
            set
            {
                _cantidadOrden = value;
                RaisePropertyChanged(() => CantidadOrden);
            }

        }
        public float ValorTotal
        {
            get => _valorTotal;
            set
            {
                _valorTotal = value;
                RaisePropertyChanged(() => ValorTotal);
            }
        }
        public float ValorDescuento
        {
            get => _valorDescuento;
            set
            {
                _valorDescuento = value;
                RaisePropertyChanged(() => ValorDescuento);
            }
        }
        public float ValorFinal
        {
            get => _valorFinal;
            set
            {
                _valorFinal = value;
                RaisePropertyChanged(() => ValorFinal);
            }
        }
        public string ResumenRegistrosOrdenCompra
        {
            get => _resumenRegistrosOrdenCompra;
            set
            {
                _resumenRegistrosOrdenCompra = value;
                RaisePropertyChanged(() => ResumenRegistrosOrdenCompra);
            }
        }
        public bool SinProductosOrdenDetalleCompra
        {
            get => _sinProductosOrdenDetalleCompra;
            set
            {
                _sinProductosOrdenDetalleCompra = value;
                RaisePropertyChanged(() => SinProductosOrdenDetalleCompra);
            }
        }
        public bool TieneItemsSeleccionados
        {
            get => _tieneItemsSeleccionados;
            set
            {
                _tieneItemsSeleccionados = value;
                RaisePropertyChanged(() => TieneItemsSeleccionados);
            }
        }
        public bool IsVisibleDetalleCarrito
        {
            get => _isVisibleDetalleCarrito;
            set
            {
                _isVisibleDetalleCarrito = value;
                RaisePropertyChanged(() => IsVisibleDetalleCarrito);
            }
        }
        public bool IsVisibleEstadoOrden
        {
            get => _isVisibleEstadoOrden;
            set
            {
                _isVisibleEstadoOrden = value;
                RaisePropertyChanged(() => IsVisibleEstadoOrden);
            }
        }
        #endregion 2.4 Control de Datos       

        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        public DetalleInventarioViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioProductos_Item itemServicio
                                  , IServicioAdministracion_Catalogo catalogoServicio
                                  , IServicioCompras_Orden ordenServicio
                                  , IServicioCompras_OrdenDetalleCompra ordenDetalleCompraServicio
                                  , IServicioCompras_OrdenEstado ordenEstadoServicio
                                  , ISettingsService settingsServicio
                                  , IServicioFirestore servicioFirestore

                                  )
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);
            //Instanciación Acorde al model
            _itemServicio = itemServicio;
            _catalogoServicio = catalogoServicio;
            _ordenServicio = ordenServicio;
            _servicioFirestore = servicioFirestore;
            _settingsServicio = settingsServicio;
            _ordenDetalleCompraServicio = ordenDetalleCompraServicio;
            _ordenEstadoServicio = ordenEstadoServicio;
            _switchProductos = new SwitchProductos(_itemServicio);
            _switchAdministracion = new SwitchAdministracion(_catalogoServicio);
            _switchCompras = new SwitchCompras(_ordenServicio, _ordenDetalleCompraServicio, _ordenEstadoServicio);
            _switchConfiguracion = new SwitchConfiguracion(_settingsServicio, _servicioFirestore);
            generales = new Generales.Generales(catalogoServicio);
            MessagingCenter.Unsubscribe<Empresa>(this, Escant_App.AppSettings.Settings.EmpresaSeleccionadaIngresoComprasView);
            MessagingCenter.Subscribe<Empresa>(this, Escant_App.AppSettings.Settings.EmpresaSeleccionadaIngresoComprasView, (args) =>
            {
                ProveedorCompraSeleccionado = args;
                ProveedorCompra = ProveedorCompraSeleccionado != null && !string.IsNullOrEmpty(ProveedorCompraSeleccionado.Descripcion) ? ProveedorCompraSeleccionado.Descripcion : "";
                IdProveedorCompra = ProveedorCompraSeleccionado != null && !string.IsNullOrEmpty(ProveedorCompraSeleccionado.Id) ? ProveedorCompraSeleccionado.Id : "";
                Identificacion = ProveedorCompraSeleccionado != null && !string.IsNullOrEmpty(ProveedorCompraSeleccionado.Identificacion) ? ProveedorCompraSeleccionado.Identificacion : "";
            });            
            MessagingCenter.Subscribe<Orden>(this, "PdfViewerIngresoComprasView", (args) =>
            {
                OrdenSeleccionado = args;
                BindingData();
            });
            /*inicioControlesValidacion();
            AddValidations();*/

        }



        #endregion 3.Constructor

        #region 4.DefinicionMetodos
        public ICommand TappedCommand => new Command<StepTappedEventArgs>(TappedCommandAsync);
        public ICommand OpenProveedorCompraCommand => new Command(async () => await OpenProveedorCompraAsync());
        public ICommand CancelOrderCommand => new Command(async () => await CancelOrderAsync());
        public ICommand ToolbarInventarioCommand => new Command(async () => await ToolbarInventarioCommandAsync());        
        public ICommand ViewDetalleInventarioCommand => new Command<Item>(ViewDetalleInventarioAsync);
      

        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is Orden item && item != null)
            {
                OrdenSeleccionado = item;
                if (OrdenSeleccionado.EsEdicion == 0)
                {
                    OrdenSeleccionado.Id = Generator.GenerateKey();
                    OrdenSeleccionado.TipoOrden = TipoOrden.Inventario.ToString();
                    OrdenSeleccionado.IdEstado = ((int)EstadoOrdenInventario.Pendiente).ToString();
                    OrdenSeleccionado.CodigoEstado = EstadoOrdenInventario.Pendiente.ToString();
                    OrdenSeleccionado.CodigoEstadoEnConsulta = EstadoOrdenInventario.Ingresado.ToString();
                    OrdenSeleccionado.CodigoEstadoEnDesarrollo = EstadoOrdenInventario.Ingresado.ToString();
                    OrdenSeleccionado.Fecha = DateTimeHelpers.GetDate(DateTimeHelpers.GetDateActual());
                    OrdenSeleccionado.OrdenEstado = EstadoInfoCollection;
                    OrdenSeleccionado.OrdenDetalleCompras = OrdenDetalleCompra;
                    OrdenSeleccionado.OrdenDetalleComprasRecibido = OrdenDetalleCompraRecibido;
                    OrdenSeleccionado.OrdenDetalleCompras.CollectionChanged += OnCollectionChanged;
                    //2. OrdenEstado
                    LlenaEstadoOrden();
                    ResetForm();
                    PageTitle = "Orden";
                }
            }
            if (Registros == null)
            {
                await BuscarRegistros();
            }
            BindingData();
            IsBusy = false;
        }

        private void BindingData()
        {
            if (OrdenSeleccionado.EsEdicion == 1)
            {
                OrdenSeleccionado.OrdenDetalleCompras = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenDetalleCompraSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<OrdenDetalleCompra>>(OrdenSeleccionado.OrdenDetalleCompraSerializado) : OrdenSeleccionado.OrdenDetalleCompras;
                OrdenSeleccionado.OrdenDetalleComprasRecibido = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenDetalleCompraRecibidoSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<OrdenDetalleCompraDet>>(OrdenSeleccionado.OrdenDetalleCompraRecibidoSerializado) : OrdenSeleccionado.OrdenDetalleComprasRecibido;
                OrdenSeleccionado.OrdenEstado = JsonConvert.DeserializeObject<ObservableCollection<OrdenEstado>>(OrdenSeleccionado.OrdenEstadoCompraSerializado);
                OrdenSeleccionado.CodigoEstadoEnDesarrollo = OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.StatusProgreso == StepStatus.InProgress) != null ? OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.StatusProgreso == StepStatus.InProgress).Estado : "";
                ProveedorCompra = OrdenSeleccionado.NombreProveedor;
                TieneItemsSeleccionados = OrdenSeleccionado.OrdenDetalleCompras.Any();
                SinProductosOrdenDetalleCompra = !TieneItemsSeleccionados;
                PageTitle = string.Format(TextsTranslateManager.Translate("IngresoComprasTitulo"), OrdenSeleccionado.NumeroOrden, OrdenSeleccionado.NombreProveedor, OrdenSeleccionado.CodigoEstadoEnDesarrollo);//{ texts: Translate IngresoComprasTitulo}
                CalculoResumen();
                if (OrdenSeleccionado.OrdenDetalleComprasRecibido != null)
                    Registros.Where(d => (OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(y => y.IdItem == d.Id).Count() > 0)).ToList().ForEach(x => x.CantidadInventarioInicial = ((OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(y => y.IdItem == x.Id).Sum(l => l.Cantidad))));
                else
                    Registros.Where(d => (OrdenSeleccionado.OrdenDetalleCompras.Where(y => y.IdItem == d.Id).Count() > 0)).ToList().ForEach(x => x.CantidadInventarioInicial = ((OrdenSeleccionado.OrdenDetalleCompras.Where(y => y.IdItem == x.Id).Sum(l => l.Cantidad))));
            }
        }

        public async Task BuscarRegistros()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var seleccion = new DataModel.DTO.Productos.Item()
            {
                Deleted = 0,
                PoseeStock = 1
            };
            switch (OrdenSeleccionado.TipoInventario)
            {
                case string a when OrdenSeleccionado.TipoInventario.ToString().Contains("Inici"):
                    seleccion.PoseeInventarioInicial = 0;
                    break;
                case string b when OrdenSeleccionado.TipoInventario.ToString().Contains("Conci"):
                    seleccion.PoseeInventarioInicial = 1;
                    break;
                case string c when OrdenSeleccionado.TipoInventario.ToString().Contains("Transfe"):
                    seleccion.PoseeInventarioInicial = 1;
                    break;
            }
            var items = await _switchProductos.ConsultaItems(seleccion, estaConectado);
            if (items != null && items.Count > 0)
            {
                Registros = generales.ToObservableCollection<DataModel.DTO.Productos.Item>(items);
            }
            IsBusy = false;
        }
        /// <summary>
        /// Obtener Secuencial Orden Compra
        /// </summary>
        /// <param name="TipoSecuencial"></param>
        /// <returns></returns>
        public async Task<SecuencialNumerico> obtieneSecuenciales(string TipoSecuencial)
        {
            SecuencialNumerico resultado = new SecuencialNumerico();
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var setting = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.SecuencialesSetting }, estaConectado);
            if (setting != null)
            {
                SecuencialSetttings comprobantesSettings = JsonConvert.DeserializeObject<SecuencialSetttings>(setting.Data);
                if (comprobantesSettings != null)
                {
                    resultado = await _switchConfiguracion.ActualizaConfiguracionSecuencial(TipoSecuencial);
                    if (!string.IsNullOrEmpty(resultado.SecuencialRecibo))
                    {
                        comprobantesSettings.SecuencialOrdenCompra = resultado.SecuencialReciboIncrementado;
                        setting.Data = JsonConvert.SerializeObject(comprobantesSettings);
                        await _switchConfiguracion.GuardaConfiguracion(setting, estaConectado, true);
                    }
                }
            }
            return resultado;
        }

        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales
        /// <summary>
        /// Método para resetear valores de almacenamiento
        /// </summary>
        private void ResetForm()
        {
            TieneItemsSeleccionados = false;
            CantidadOrden = OrdenDetalleCompra.Sum(x => x.Cantidad);
            ValorTotal = OrdenDetalleCompra.Sum(x => x.ValorCompra);
            OrdenSeleccionado.Cantidad = CantidadOrden;
            OrdenSeleccionado.ValorTotal = ValorTotal;
            IsVisibleDetalleCarrito = false;
            IsVisibleEstadoOrden = false;
            if(OrdenSeleccionado.TipoInventario.ToUpper().Contains("NICIAL"))
            {
                ProveedorCompra = SettingsOnline.oAplicacion.Nombre;
                Identificacion = SettingsOnline.oAplicacion.IdentificacionAdministrador;
            }
        }
        public async Task OpenItemAdjust(Item product)
        {

        }
        /// <summary>
        /// Método para añadir ítems a la Compra
        /// </summary>
        /// <param name="product"></param>
        public void AddItemOrdenCompra(Item product)
        {
            var estadoPermitido = ((int)Enum.Parse(typeof(EstadoOrdenInventario), OrdenSeleccionado.CodigoEstado));

            if (estadoPermitido <= IdEstadoEdicion) //Sólo se puede editar los ítems hasta el estado Ingresado
            {

                var item = OrdenSeleccionado.OrdenDetalleCompras.FirstOrDefault(x => x.IdItem == product.Id);
                if (item != null)
                {
                    item.Cantidad = product.CantidadInventarioInicial;
                    product.CantidadCompra = item.Cantidad;
                }
                else
                {
                    OrdenSeleccionado.OrdenDetalleCompras.Add(new OrdenDetalleCompra
                    {
                        Id = Generator.GenerateKey(),
                        IdOrden = OrdenSeleccionado.Id,
                        IdItem = product.Id,
                        NombreItem = product.Nombre,
                        IdUnidad = product.IdUnidad,
                        Unidad = product.Unidad,
                        Imagen = product.Imagen,
                        TipoOrden=OrdenSeleccionado.TipoOrden,
                        TipoInventario = OrdenSeleccionado.TipoInventario,
                        PoseeStock = product.PoseeStock,
                        ValorConversion=1,
                        Cantidad = product.CantidadInventarioInicial,
                        ValorUnitario = product.PrecioCompra,
                        //ValorCompra = product.PrecioCompra,
                        ValorDescuento = 0
                        //ValorFinal = product.PrecioCompra,                                                
                    });
                    product.CantidadCompra += 1;
                }
                TieneItemsSeleccionados = OrdenSeleccionado.OrdenDetalleCompras.Any();
                SinProductosOrdenDetalleCompra = !TieneItemsSeleccionados;
                CalculoResumen();
            }
            else
            {
                DialogService.ShowToast("Sólo se puede editar el detalle de la Orden en tanto permanezca Ingresada");
            }
        }
        private void CalculoResumen()
        {
            CantidadOrden = OrdenSeleccionado.OrdenDetalleCompras.Sum(x => x.Cantidad);
            ValorTotal = OrdenSeleccionado.OrdenDetalleCompras.Sum(x => x.ValorCompra);
            ValorDescuento = OrdenSeleccionado.OrdenDetalleCompras.Sum(x => x.ValorDescuento);
            OrdenSeleccionado.Cantidad = CantidadOrden;
            OrdenSeleccionado.ValorTotal = ValorTotal;
            OrdenSeleccionado.ValorFinal = ValorTotal - ValorDescuento;
            ResumenRegistrosOrdenCompra = string.Format(TextsTranslateManager.Translate("ItemAdded"), CantidadOrden, ValorTotal.FormatPrice());
            SeteaNotificacion?.Invoke(true);
        }
        public async Task OpenDetalleOrdenAsync(int origen, OrdenDetalleCompra data)
        {
            OrdenDetalleCompraSeleccionado = data;
            switch (origen)
            {
                case 1:
                    if (!OrdenSeleccionado.CodigoEstadoEnDesarrollo.ToUpper().Contains("DISTRIB"))
                    {
                        var estadoPermitido = ((int)Enum.Parse(typeof(EstadoOrdenInventario), OrdenSeleccionado.CodigoEstado));

                        if (estadoPermitido <= IdEstadoEdicion) //Sólo se puede editar los ítems hasta el estado Ingresado
                        {
                            await navegacion(origen,2);
                            //await NavigationService.NavigateToPopupAsync<PopupActualizaItemOrdenComprasViewModel>(OrdenDetalleCompraSeleccionado);
                        }
                    }
                    else
                    {
                        await navegacion(origen, 3);
                        //await NavigationService.NavigateToPopupAsync<PopupReceptaItemOrdenComprasViewModel>(OrdenDetalleCompraSeleccionado);

                    }
                    break;
                case 2:
                    await navegacion(origen, 3);
                    //await NavigationService.NavigateToPopupAsync<PopupReceptaItemOrdenComprasViewModel>(OrdenDetalleCompraSeleccionado);
                    break;
            }
        }
        private async void TappedCommandAsync(StepTappedEventArgs args)
        {
            var estadoOrden = args.Item.PrimaryFormattedText.Spans[0].Text;

            if (args.Item.Status.ToString().Contains("Progress"))
            {
                if (OrdenSeleccionado.CantidadTmp > 0 && !string.IsNullOrEmpty(ProveedorCompra))
                {

                    await validaEstado(estadoOrden, 1);
                    /*
                    if (continuar)
                    {
                        OrdenSeleccionado.CodigoEstadoEnDesarrollo = estadoOrden;
                        OrdenSeleccionado.CodigoEstadoEnConsulta = estadoOrden;
                        OrdenSeleccionado.NombreProveedor = ProveedorCompra;
                        OrdenSeleccionado.IdProveedor = IdProveedorCompra;
                        OrdenSeleccionado.Identificacion = Identificacion;

                        await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenComprasViewModel>(OrdenSeleccionado);
                    }*/
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Importante", "Debe al menos seleccionar el Proveedor y tener ingresado un ítem a su Orden", "OK");
                }
            }
            if (args.Item.Status.ToString().Contains("Complete"))
            {
                if (!OrdenSeleccionado.CodigoEstado.Contains("Pendien"))
                {
                    OrdenSeleccionado.CodigoEstadoEnConsulta = estadoOrden;
                    await navegacion(2, 1);
                    //await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenComprasViewModel>(OrdenSeleccionado);
                }
            }
        }
        private async Task<bool> validaEstado(string estadoOrden, int origen)
        {
            bool continuar = true;
            switch (estadoOrden)
            {
                case string y when estadoOrden.ToString().ToUpper().Contains("DISTRI"):
                    continuar = await validaOrdenRecibida(estadoOrden, origen);
                    if (!continuar)
                        await abreOrdenAsync(!continuar, origen);
                    else
                        //await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenComprasViewModel>(OrdenSeleccionado);                                        
                        await navegacion(2, 1);
                    break;
                default:
                    if (continuar)
                    {
                        OrdenSeleccionado.CodigoEstadoEnDesarrollo = estadoOrden;
                        OrdenSeleccionado.CodigoEstadoEnConsulta = estadoOrden;
                        OrdenSeleccionado.NombreProveedor = ProveedorCompra;
                        OrdenSeleccionado.IdProveedor = IdProveedorCompra;
                        OrdenSeleccionado.Identificacion = Identificacion;
                        await navegacion(2,1);
                    }
                    break;
            }
            /*if (estadoOrden.Contains("Recib") && (OrdenSeleccionado.OrdenDetalleComprasRecibido==null || OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.OrdenDetalleComprasRecibido == null).ToList().Count()>0 || (OrdenSeleccionado.OrdenDetalleComprasRecibido != null && OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(a=>a.TipoDescuento.Contains("Aplica")).Sum(x=>x.Cantidad) != OrdenSeleccionado.OrdenDetalleCompras.Sum(y=>y.Cantidad))))
            {               
                await App.Current.MainPage.DisplayAlert("Importante", "Debe registrar el detalle adicional de la recepción de los ítems solicitados", "OK");                
                await abreOrdenAsync();
                if((OrdenSeleccionado.OrdenDetalleComprasRecibido == null || OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.OrdenDetalleComprasRecibido == null).ToList().Count() > 0 || (OrdenSeleccionado.OrdenDetalleComprasRecibido != null && OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(a => a.TipoDescuento.Contains("Aplica")).Sum(x => x.Cantidad) != OrdenSeleccionado.OrdenDetalleCompras.Sum(y => y.Cantidad))))
                    continuar = false;                
            }*/

            return continuar;
        }

        private async Task<bool> validaOrdenRecibida(string estadoOrden, int origen)
        {
            bool retorno = true;
            if ((OrdenSeleccionado.OrdenDetalleComprasRecibido == null || OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.OrdenDetalleComprasRecibido == null).ToList().Count() > 0 || (OrdenSeleccionado.OrdenDetalleComprasRecibido != null && OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(a => a.TipoDescuento.Contains("Aplica")).Sum(x => x.Cantidad) != OrdenSeleccionado.OrdenDetalleCompras.Sum(y => y.Cantidad))))
            {
                if (origen == 1)
                    await App.Current.MainPage.DisplayAlert("Importante", "Debe registrar el detalle adicional de la recepción de los ítems solicitados", "OK");
                retorno = false;
            }
            else
            {
                OrdenSeleccionado.CodigoEstadoEnDesarrollo = estadoOrden;
                OrdenSeleccionado.CodigoEstadoEnConsulta = estadoOrden;
                OrdenSeleccionado.NombreProveedor = ProveedorCompra;
                OrdenSeleccionado.IdProveedor = IdProveedorCompra;
                OrdenSeleccionado.Identificacion = Identificacion;
            }
            return retorno;
        }

        private async Task navegacion(int origen, int tipo)
        {
            bool evaluaEstado = false;
            switch (tipo)
            {
                case 1: //Abrir PopupActualizaOrdenComprasViewModel
                    /*                    var returnPage = await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenComprasViewModel>(OrdenSeleccionado);
                                        var popupPage = returnPage as PopupActualizaOrdenComprasView;
                                        var result = await popupPage.PageClosedTask;
                                        if (result != null)
                                            OrdenSeleccionado = result;*/
                    var returnPage = await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenInventarioViewModel>(OrdenSeleccionado);
                    var popupPage = returnPage as PopupActualizaOrdenInventarioView;
                    popupPage.CloseButtonEventHandler += async (sender, obj) =>
                    {
                        var isCancel = ((PopupActualizaOrdenInventarioView)sender).IsCancel;
                        if (!isCancel)
                        {
                            var resultado = ((PopupActualizaOrdenInventarioView)sender).PopupResult;
                            if (resultado != null)
                            {
                                OrdenSeleccionado = resultado;
                                BindingData();
                                /*Device.BeginInvokeOnMainThread(async () =>
                                {
                                    IsBusy = true;
                                    await OpenPdfViewerAsync();
                                    IsBusy = false;
                                    //ItemIndex = -1;
                                });*/
                            }
                        }
                        await PopupNavigation.Instance.PopAsync();
                    };
                    var result = await popupPage.PageClosedTask;
                    await abreOrdenEstadoAsync();
                    break;
                case 2: //Abrir PopupReceptaItemOrdenComprasViewModel
                    var returnPage2 = await NavigationService.NavigateToPopupAsync<PopupActualizaDetalleInventarioViewModel>(OrdenDetalleCompraSeleccionado);
                    var popupPage2 = returnPage2 as PopupActualizaDetalleInventarioView;
                    popupPage2.CloseButtonEventHandler += async (sender, obj) =>
                    {
                        var isCancel = ((PopupActualizaDetalleInventarioView)sender).IsCancel;
                        if (!isCancel)
                        {
                            var resultado = ((PopupActualizaDetalleInventarioView)sender).PopupResult;
                            if (resultado != null)
                            {
                                OrdenDetalleCompraSeleccionado = resultado;
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    IsBusy = true;
                                    var encontrado = OrdenSeleccionado.OrdenDetalleCompras.FirstOrDefault(x => x.Id == OrdenDetalleCompraSeleccionado.Id);
                                    int i = OrdenSeleccionado.OrdenDetalleCompras.IndexOf(encontrado);
                                    OrdenSeleccionado.OrdenDetalleCompras[i] = OrdenDetalleCompraSeleccionado;
                                    IsBusy = false;
                                    //ItemIndex = -1;
                                });
                            }
                        }
                        await PopupNavigation.Instance.PopAsync();
                    };
                    var result2 = await popupPage2.PageClosedTask;
                    break;
                case 3://Abrir PopupActualizaItemOrdenComprasViewModel
                    var returnPage1 = await NavigationService.NavigateToPopupAsync<PopupDistribuyeItemOrdenInventarioViewModel>(OrdenDetalleCompraSeleccionado);
                    var popupPage1 = returnPage1 as PopupDistribuyeItemOrdenInventarioView;
                    popupPage1.CloseButtonEventHandler += async (sender, obj) =>
                    {
                        var isCancel = ((PopupDistribuyeItemOrdenInventarioView)sender).IsCancel;
                        if (!isCancel)
                        {
                            var resultado = ((PopupDistribuyeItemOrdenInventarioView)sender).PopupResult;
                            if (resultado != null)
                            {
                                OrdenDetalleCompraSeleccionado = resultado;
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                        OrdenSeleccionado.OrdenDetalleComprasRecibido = OrdenSeleccionado.OrdenDetalleComprasRecibido == null ? new ObservableCollection<OrdenDetalleCompraDet>() : OrdenSeleccionado.OrdenDetalleComprasRecibido;
                                        var y = OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(x => x.IdOrdenDetalleCompra == OrdenDetalleCompraSeleccionado.Id).ToList();
                                        foreach (var item in y)
                                        {
                                            OrdenSeleccionado.OrdenDetalleComprasRecibido.Remove(item);
                                        }
                                        foreach (var item in OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido)
                                        {
                                            OrdenSeleccionado.OrdenDetalleComprasRecibido.Add(item);
                                        }
                                });
                                /*
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    var continuar = await validaEstado("Recibido", 2);
    
                                });*/
                            }
                            if (origen == 2)
                                evaluaEstado = true;
                        }
                        await PopupNavigation.Instance.PopAsync();
                    };
                    var result1 = await popupPage1.PageClosedTask;
                    if (evaluaEstado)
                        await validaEstado("Distribuido", 2);
                    break;



            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
            if (e.OldItems == null) return;
            {
                foreach (var item in e.OldItems)
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CalculoResumen();
        }


        /// <summary>
        /// Populate shipment details
        /// </summary>
        public void LlenaEstadoOrden()
        {
            string numeroestado = "";
            foreach (string name in Enum.GetNames(typeof(EstadoOrdenInventario)))
            {
                numeroestado = ((int)Enum.Parse(typeof(EstadoOrdenInventario), name)).ToString();
                OrdenSeleccionado.OrdenEstado.Add(new OrdenEstado()
                {
                    Id = Generator.GenerateKey()
                ,
                    IdOrden = OrdenSeleccionado.Id
                ,
                    IdEstado = ((int)Enum.Parse(typeof(EstadoOrdenInventario), name)).ToString()
                ,
                    Estado = name
                ,
                    StatusProgreso = devuelveStatus(numeroestado)
                ,
                    FechaProcesoStr = devuelveFecha(name)
                });
            }
        }

        public StepStatus devuelveStatus(string IdEstado)
        {
            StepStatus estado = StepStatus.NotStarted;
            switch (IdEstado)
            {
                case "0":
                    estado = StepStatus.Completed;
                    break;
                case "1":
                    estado = StepStatus.InProgress;
                    break;
            }
            return estado;
        }

        public string devuelveFecha(string name)
        {
            string fecha = "";
            switch ((int)Enum.Parse(typeof(EstadoOrdenInventario), name))
            {
                case 0:
                    fecha = DateTime.Now.ToString("dd MMM yyyy");
                    break;
                default:
                    fecha = "";
                    break;

            }
            return fecha;
        }

        public async Task OpenProveedorCompraAsync()
        {
            if(!OrdenSeleccionado.TipoInventario.ToUpper().Contains("NICIAL"))
                await NavigationService.NavigateToAsync<EmpresaViewModel>();
        }

        public async Task OpenPdfViewerAsync()
        {
            try
            {
                await NavigationService.NavigateToAsync<PdfViewerViewModel>(OrdenSeleccionado);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task CancelOrderAsync()
        {
            var result = await DialogService.ShowConfirmedAsync(TextsTranslateManager.Translate("ConfirmDiscardPurchase"), TextsTranslateManager.Translate("LegalInfo"), "OK", "Cancel");
            if (result)
            {
                OrdenSeleccionado.OrdenDetalleCompras = new ObservableCollection<OrdenDetalleCompra>();
                Registros.ForEach(i => i.CantidadReservada = 0);
                CalculoResumen();
            }
        }

        private async Task ToolbarInventarioCommandAsync()
        {
            await abreOrdenEstadoAsync();
        }

        public async void ViewDetalleInventarioAsync(Item dato)
        {
            IsBusy = true;
            try
            {
                if (ItemIndex >= 0)
                {
                    var registro = OrdenSeleccionado.OrdenDetalleCompras.Where(x=>x.IdItem==dato.Id).FirstOrDefault();
                    registro.EsEdicion = 1;
                    OrdenDetalleCompraSeleccionado = registro;
                    await navegacion(2,2);
                    //await NavigationService.NavigateToAsync<PdfViewerViewModel>(RegistroSeleccionado);                    
                    //ItemIndex = -1;
                }
            }
            catch (Exception ex)
            {

            }
            IsBusy = false;
        }


        private async Task abreOrdenAsync(bool sigueValidando, int origen)
        {
            if (sigueValidando)
            {
                var data = OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.OrdenDetalleComprasRecibido == null).FirstOrDefault();
                if (data != null)
                    await OpenDetalleOrdenAsync(2, data);
            }

        }

        private async Task abreOrdenEstadoAsync()
        {
            AbreOrdenEstado?.Invoke(true);
        }


        #endregion 6.Métodos Generales



    }
}
