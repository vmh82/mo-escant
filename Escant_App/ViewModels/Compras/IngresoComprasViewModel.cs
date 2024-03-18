using Escant_App.Helpers;
using ManijodaServicios.Resources.Texts;
using Escant_App.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using DataModel.DTO.Productos;
using DataModel.DTO.Compras;
using DataModel.Helpers;
using DataModel.Enums;
using System.Collections.Specialized;
using System.ComponentModel;
using DataModel.DTO.Iteraccion;
using System.Windows.Input;
using Xamarin.Forms;
using Syncfusion.XForms.ProgressBar;
using DataModel.DTO.Administracion;
using Escant_App.ViewModels.Administracion;
using ManijodaServicios.Interfaces;
using DataModel.DTO;
using DataModel.DTO.Configuracion;
using Newtonsoft.Json;
using Escant_App.ViewModels.Documentos;
using Xamarin.Forms.Internals;
using Escant_App.Views.Compras;
using Rg.Plugins.Popup.Services;

namespace Escant_App.ViewModels.Compras
{
    public class IngresoComprasViewModel : ViewModelBase
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
        private int IdEstadoEdicion = ((int)Enum.Parse(typeof(EstadoOrdenCompra), "Ingresado"));
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
        #endregion 1.6 Acciones
        #endregion 1. Declaración de Variables

        #region 2.InstanciaAsignaVariablesControles
        #region 2.2 Objetos para la Clase
        public ObservableCollection<Item> Registros
        {
            get => _registros;
            set
            {
                _registros = value;
                RaisePropertyChanged(() => Registros);
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
        public IngresoComprasViewModel(IServicioSeguridad_Usuario seguridadServicio
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
            _switchConfiguracion = new SwitchConfiguracion(_settingsServicio,_servicioFirestore);
            generales = new Generales.Generales(catalogoServicio);
            MessagingCenter.Unsubscribe<Empresa>(this, Escant_App.AppSettings.Settings.EmpresaSeleccionadaIngresoComprasView);
            MessagingCenter.Subscribe<Empresa>(this, Escant_App.AppSettings.Settings.EmpresaSeleccionadaIngresoComprasView, (args) =>
            {
                ProveedorCompraSeleccionado = args;                
                ProveedorCompra=ProveedorCompraSeleccionado!=null && !string.IsNullOrEmpty(ProveedorCompraSeleccionado.Descripcion)? ProveedorCompraSeleccionado.Descripcion: "";
                IdProveedorCompra = ProveedorCompraSeleccionado != null && !string.IsNullOrEmpty(ProveedorCompraSeleccionado.Id) ? ProveedorCompraSeleccionado.Id : "";
                Identificacion = ProveedorCompraSeleccionado != null && !string.IsNullOrEmpty(ProveedorCompraSeleccionado.Identificacion) ? ProveedorCompraSeleccionado.Identificacion : "";
            });
            /*MessagingCenter.Subscribe<OrdenDetalleCompra>(this, "OnDataSourcePopupActualizaItemOrdenComprasChanged", (args) =>
            {
                OrdenDetalleCompraSeleccionado = args;
                var encontrado=OrdenSeleccionado.OrdenDetalleCompras.FirstOrDefault(x => x.Id == OrdenDetalleCompraSeleccionado.Id);
                int i = OrdenSeleccionado.OrdenDetalleCompras.IndexOf(encontrado);
                OrdenSeleccionado.OrdenDetalleCompras[i] = OrdenDetalleCompraSeleccionado;
            });*/
            MessagingCenter.Subscribe<OrdenDetalleCompra>(this, "OnDataSourcePopupReceptaItemOrdenComprasChanged", (args) =>
            {
                OrdenDetalleCompraSeleccionado = args;
                OrdenSeleccionado.OrdenDetalleComprasRecibido = OrdenSeleccionado.OrdenDetalleComprasRecibido == null ? new ObservableCollection<OrdenDetalleCompraDet>() : OrdenSeleccionado.OrdenDetalleComprasRecibido;
                var y=OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(x => x.IdOrdenDetalleCompra == OrdenDetalleCompraSeleccionado.Id).ToList();
                foreach(var item in y)
                {
                    OrdenSeleccionado.OrdenDetalleComprasRecibido.Remove(item);
                }
                foreach (var item in OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido)
                {
                    OrdenSeleccionado.OrdenDetalleComprasRecibido.Add(item);
                }
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var continuar = await validaEstado("Recibido", 2);
                    /*if (continuar)
                        await navegacion(1);*/
                });
                
            });            
            MessagingCenter.Subscribe<Orden>(this, "OnDataSourcePopupActualizaOrdenComprasChanged", (args) =>
            {
                OrdenSeleccionado = args;
                BindingData();                
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await abreOrdenEstadoAsync();
                    //await OpenPdfViewerAsync();
                    IsBusy = false;
                    //ItemIndex = -1;
                });
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
                    OrdenSeleccionado.TipoOrden = TipoOrden.Compra.ToString();
                    OrdenSeleccionado.IdEstado = ((int)EstadoOrdenCompra.Pendiente).ToString();
                    OrdenSeleccionado.CodigoEstado = EstadoOrdenCompra.Pendiente.ToString();
                    OrdenSeleccionado.CodigoEstadoEnConsulta = EstadoOrdenCompra.Ingresado.ToString();
                    OrdenSeleccionado.CodigoEstadoEnDesarrollo = EstadoOrdenCompra.Ingresado.ToString();
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
                else
                {
                    BindingData();            
                }
            }
            if (Registros == null)
            {
                await BuscarRegistros();
            }
           
            IsBusy = false;
        }

        private void BindingData()
        { 
            OrdenSeleccionado.OrdenDetalleCompras = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenDetalleCompraSerializado)?JsonConvert.DeserializeObject<ObservableCollection<OrdenDetalleCompra>>(OrdenSeleccionado.OrdenDetalleCompraSerializado): OrdenSeleccionado.OrdenDetalleCompras;
            OrdenSeleccionado.OrdenDetalleComprasRecibido = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenDetalleCompraRecibidoSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<OrdenDetalleCompraDet>>(OrdenSeleccionado.OrdenDetalleCompraRecibidoSerializado): OrdenSeleccionado.OrdenDetalleComprasRecibido;
            OrdenSeleccionado.OrdenEstado = JsonConvert.DeserializeObject<ObservableCollection<OrdenEstado>>(OrdenSeleccionado.OrdenEstadoCompraSerializado);
            OrdenSeleccionado.CodigoEstadoEnDesarrollo = OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.StatusProgreso == StepStatus.InProgress)!=null? OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.StatusProgreso == StepStatus.InProgress).Estado:"";
            ProveedorCompra = OrdenSeleccionado.NombreProveedor;
            TieneItemsSeleccionados = OrdenSeleccionado.OrdenDetalleCompras.Any();
            SinProductosOrdenDetalleCompra = !TieneItemsSeleccionados;
            PageTitle= string.Format(TextsTranslateManager.Translate("IngresoComprasTitulo"), OrdenSeleccionado.NumeroOrden, OrdenSeleccionado.NombreProveedor,OrdenSeleccionado.CodigoEstadoEnDesarrollo);//{ texts: Translate IngresoComprasTitulo}
            CalculoResumen();
        }

        public async Task BuscarRegistros()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var items = await _switchProductos.ConsultaItems(new Item() { Deleted = 0, PoseeStock=-1,PoseeInventarioInicial=-1 }, estaConectado);
            if (items != null && items.Count > 0)
            {
                Registros = generales.ToObservableCollection<Item>(items);
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
            SecuencialNumerico resultado=new SecuencialNumerico();
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var setting = await _switchConfiguracion.ConsultaConfiguracion(new Setting() { SettingType = (int)SettingType.SecuencialesSetting }, estaConectado);
            if (setting != null)
            {
                SecuencialSetttings comprobantesSettings = JsonConvert.DeserializeObject<SecuencialSetttings>(setting.Data);
                if (comprobantesSettings != null)
                {
                  resultado=await _switchConfiguracion.ActualizaConfiguracionSecuencial(TipoSecuencial);
                  if(!string.IsNullOrEmpty(resultado.SecuencialRecibo))
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
            var estadoPermitido = ((int)Enum.Parse(typeof(EstadoOrdenCompra), OrdenSeleccionado.CodigoEstado));

            if (estadoPermitido <= IdEstadoEdicion) //Sólo se puede editar los ítems hasta el estado Ingresado
            {

                var item = OrdenSeleccionado.OrdenDetalleCompras.FirstOrDefault(x => x.IdItem == product.Id);
                if (item != null)
                {
                    item.Cantidad += 1;
                    item.ValorCompra += product.PrecioCompra;
                    item.ValorFinal += product.PrecioCompra;
                    product.CantidadCompra = item.Cantidad;
                }
                else
                {
                    OrdenSeleccionado.OrdenDetalleCompras.Add(new OrdenDetalleCompra
                    {
                        Id = Generator.GenerateKey(),
                        IdOrden = OrdenSeleccionado.Id,
                        IdItem = product.Id,
                        CodigoItem=product.Codigo,
                        NombreItem = product.Nombre,
                        IdUnidad = product.IdUnidad,
                        Unidad = product.Unidad,
                        Imagen = product.Imagen,
                        Cantidad = 1,
                        ValorUnitario = product.PrecioCompra,
                        ValorCompra = product.PrecioCompra,
                        ValorDescuento = 0,
                        ValorFinal = product.PrecioCompra
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
        }
        public async Task OpenDetalleOrdenAsync(int origen, OrdenDetalleCompra data)
        {
            OrdenDetalleCompraSeleccionado = data;
            switch (origen)
            {
                case 1:
                        if (!OrdenSeleccionado.CodigoEstadoEnDesarrollo.Contains("Recibi"))
                        {
                            var estadoPermitido = ((int)Enum.Parse(typeof(EstadoOrdenCompra), OrdenSeleccionado.CodigoEstado));

                            if (estadoPermitido <= IdEstadoEdicion) //Sólo se puede editar los ítems hasta el estado Ingresado
                            {
                                await navegacion(3);
                                //await NavigationService.NavigateToPopupAsync<PopupActualizaItemOrdenComprasViewModel>(OrdenDetalleCompraSeleccionado);
                            }
                        }
                        else
                        {
                            await navegacion(2);
                            //await NavigationService.NavigateToPopupAsync<PopupReceptaItemOrdenComprasViewModel>(OrdenDetalleCompraSeleccionado);

                        }
                        break;
                case 2:
                        await navegacion(2);
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
                    
                    await validaEstado(estadoOrden,1);
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
                if (!OrdenSeleccionado.CodigoEstado.Contains("Pendien")) {
                    OrdenSeleccionado.CodigoEstadoEnConsulta = estadoOrden;
                    await navegacion(1);
                    //await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenComprasViewModel>(OrdenSeleccionado);
                }
            }
        }
        private async Task<bool> validaEstado(string estadoOrden,int origen)
        {
            bool continuar = true;
            switch (estadoOrden)
            {
                case string y when estadoOrden.ToString().Contains("Recib"):
                    continuar = await validaOrdenRecibida(estadoOrden,origen);
                    if (!continuar)
                        await abreOrdenAsync(!continuar, origen);
                    else
                        //await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenComprasViewModel>(OrdenSeleccionado);                                        
                        await navegacion(1);
                    break;
                default:
                    if (continuar)
                    {
                        OrdenSeleccionado.CodigoEstadoEnDesarrollo = estadoOrden;
                        OrdenSeleccionado.CodigoEstadoEnConsulta = estadoOrden;
                        OrdenSeleccionado.NombreProveedor = ProveedorCompra;
                        OrdenSeleccionado.IdProveedor = IdProveedorCompra;
                        OrdenSeleccionado.Identificacion = Identificacion;
                        await navegacion(1);
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

        private async Task<bool> validaOrdenRecibida(string estadoOrden,int origen)
        {
            bool retorno = true;
            if ((OrdenSeleccionado.OrdenDetalleComprasRecibido == null || OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.OrdenDetalleComprasRecibido == null).ToList().Count() > 0 || (OrdenSeleccionado.OrdenDetalleComprasRecibido != null && OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(a => a.TipoDescuento.Contains("Aplica")).Sum(x => x.Cantidad) != OrdenSeleccionado.OrdenDetalleCompras.Sum(y => y.Cantidad))))
            {
                if(origen==1)
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

        private async Task navegacion(int tipo)
        {
            switch (tipo)
            {
                case 1: //Abrir PopupActualizaOrdenComprasViewModel
                    /*                    var returnPage = await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenComprasViewModel>(OrdenSeleccionado);
                                        var popupPage = returnPage as PopupActualizaOrdenComprasView;
                                        var result = await popupPage.PageClosedTask;
                                        if (result != null)
                                            OrdenSeleccionado = result;*/
                    var returnPage = await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenComprasViewModel>(OrdenSeleccionado);
                    var popupPage = returnPage as PopupActualizaOrdenComprasView;
                    popupPage.CloseButtonEventHandler += async (sender, obj) =>
                    {
                        var isCancel = ((PopupActualizaOrdenComprasView)sender).IsCancel;
                        if (!isCancel)
                        {
                            var resultado = ((PopupActualizaOrdenComprasView)sender).PopupResult;
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

                    break;
                case 2: //Abrir PopupReceptaItemOrdenComprasViewModel
                    await NavigationService.NavigateToPopupAsync<PopupReceptaItemOrdenComprasViewModel>(OrdenDetalleCompraSeleccionado);
                    break;
                case 3://Abrir PopupActualizaItemOrdenComprasViewModel
                    var returnPage1 = await NavigationService.NavigateToPopupAsync<PopupActualizaItemOrdenComprasViewModel>(OrdenDetalleCompraSeleccionado);
                    var popupPage1 = returnPage1 as PopupActualizaItemOrdenComprasView;
                    popupPage1.CloseButtonEventHandler += async (sender, obj) =>
                    {
                        var isCancel = ((PopupActualizaItemOrdenComprasView)sender).IsCancel;
                        if (!isCancel)
                        {
                            var resultado = ((PopupActualizaItemOrdenComprasView)sender).PopupResult;
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
                    var result1 = await popupPage1.PageClosedTask;
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
            foreach (string name in Enum.GetNames(typeof(EstadoOrdenCompra)))
            {
                numeroestado = ((int)Enum.Parse(typeof(EstadoOrdenCompra), name)).ToString();
                OrdenSeleccionado.OrdenEstado.Add(new OrdenEstado()
                {
                    Id= Generator.GenerateKey()
                ,   IdOrden = OrdenSeleccionado.Id
                ,
                    IdEstado = ((int)Enum.Parse(typeof(EstadoOrdenCompra), name)).ToString()
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
            switch(IdEstado)
                {
                    case "0":
                        estado=StepStatus.Completed;
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
            switch ((int)Enum.Parse(typeof(EstadoOrdenCompra), name))
            {
                case 0:
                    fecha = DateTime.Now.ToString("dd MMM yyyy");
                    break;                
                default:
                    fecha="";
                    break;

            }
            return fecha;
        }

        public async Task OpenProveedorCompraAsync()
        {            
            await NavigationService.NavigateToAsync<EmpresaViewModel>();
        }

        public async Task OpenPdfViewerAsync()
        {
            try
            {
                await NavigationService.NavigateToAsync<PdfViewerViewModel>(OrdenSeleccionado);
            }
            catch(Exception ex)
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

        private async Task abreOrdenAsync(bool sigueValidando,int origen)
        {            
            if(sigueValidando)
            { 
                if(origen==1)
                    AbreOrden?.Invoke(true);
            
                /*while (OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.OrdenDetalleComprasRecibido == null).FirstOrDefault() != null)
                {*/
                var data = OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.OrdenDetalleComprasRecibido == null).FirstOrDefault();
                if(data!=null)
                    await OpenDetalleOrdenAsync(2, data);
                //}
            }
            
        }

        private async Task abreOrdenEstadoAsync()
        {
            AbreOrdenEstado?.Invoke(true);
        }

        #endregion 6.Métodos Generales
    }
}
