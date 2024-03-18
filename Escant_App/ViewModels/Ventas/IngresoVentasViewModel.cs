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
using Escant_App.ViewModels.Compras;
using Escant_App.ViewModels.Clientes;
using DataModel.DTO.Clientes;
using Syncfusion.ListView.XForms;
using DataModel;
using Xamarin.Forms.Internals;
using ManijodaServicios.AppSettings;
using Escant_App.ViewModels.Cajas;
using Escant_App.Views.Ventas;
using Escant_App.Views.Cajas;
using Rg.Plugins.Popup.Services;

namespace Escant_App.ViewModels.Ventas
{
    public class IngresoVentasViewModel : ViewModelBase
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
        private ObservableCollection<OrdenPago> _pagoInfoCollection = new ObservableCollection<OrdenPago>();
        private Cliente _clienteCompraSeleccionado;
        #endregion 1.2 Variables de Instancia de Objetos para la Clase

        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        private string _imagenBotonOrden;
        private string _clienteCompra;
        private string _idClienteCompra;
        private string _identificacion;
        private string _direccion;
        private string _telefono;
        private int IdEstadoEdicion = ((int)Enum.Parse(typeof(EstadoOrdenCompra), "Ingresado"));
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo

        #region 1.4 Variables de Control de Datos
        private bool _sinProductosOrdenDetalleCompra = true;
        public bool _tieneItemsSeleccionados;
        public float _cantidadOrden;
        public float _valorTotal;
        public float _valorIncremento;
        public float _valorDescuento;
        public float _valorImpuesto;
        public float _valorFinal;
        public string _resumenRegistrosOrdenCompra;
        public bool _isVisibleDetalleCarrito;
        public bool _isVisibleEstadoOrden;
        public bool EstaSeleccionadaCondicion = false;        
        internal SfListView ListView { get; set; }
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
        public ObservableCollection<OrdenPago> PagoInfoCollection
        {
            get => _pagoInfoCollection;
            set
            {
                _pagoInfoCollection = value;
                RaisePropertyChanged(() => PagoInfoCollection);
            }
        }

        public Cliente ClienteCompraSeleccionado
        {
            get => _clienteCompraSeleccionado;
            set
            {
                _clienteCompraSeleccionado = value;
                RaisePropertyChanged(() => ClienteCompraSeleccionado);
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
        public string ClienteCompra
        {
            get => _clienteCompra;
            set
            {
                _clienteCompra = value;
                RaisePropertyChanged(() => ClienteCompra);
            }
        }
        public string IdClienteCompra
        {
            get => _idClienteCompra;
            set
            {
                _idClienteCompra = value;
                RaisePropertyChanged(() => IdClienteCompra);
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
        public string Direccion
        {
            get => _direccion;
            set
            {
                _direccion = value;
                RaisePropertyChanged(() => Direccion);
            }
        }
        public string Telefono
        {
            get => _telefono;
            set
            {
                _telefono = value;
                RaisePropertyChanged(() => Telefono);
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
        public float ValorIncremento
        {
            get => _valorIncremento;
            set
            {
                _valorIncremento = value;
                RaisePropertyChanged(() => ValorIncremento);
            }
        }
        public float ValorImpuesto
        {
            get => _valorImpuesto;
            set
            {
                _valorImpuesto = value;
                RaisePropertyChanged(() => ValorImpuesto);
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
        public IngresoVentasViewModel(IServicioSeguridad_Usuario seguridadServicio
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
            MessagingCenter.Unsubscribe<Cliente>(this, Escant_App.AppSettings.Settings.ClienteSeleccionadaIngresoVentasView);
            MessagingCenter.Subscribe<Cliente>(this, Escant_App.AppSettings.Settings.ClienteSeleccionadaIngresoVentasView, (args) =>
            {
                ClienteCompraSeleccionado = args;
                ClienteCompra = ClienteCompraSeleccionado != null && !string.IsNullOrEmpty(ClienteCompraSeleccionado.Nombre) ? ClienteCompraSeleccionado.Nombre : "";
                IdClienteCompra = ClienteCompraSeleccionado != null && !string.IsNullOrEmpty(ClienteCompraSeleccionado.Id) ? ClienteCompraSeleccionado.Id : "";
                Identificacion = ClienteCompraSeleccionado != null && !string.IsNullOrEmpty(ClienteCompraSeleccionado.Identificacion) ? ClienteCompraSeleccionado.Identificacion : "";
                Direccion = ClienteCompraSeleccionado != null && !string.IsNullOrEmpty(ClienteCompraSeleccionado.Direccion) ? ClienteCompraSeleccionado.Direccion : "";
                Telefono = ClienteCompraSeleccionado != null && !string.IsNullOrEmpty(ClienteCompraSeleccionado.Celular) ? ClienteCompraSeleccionado.Celular : "";
            });
            /*
            MessagingCenter.Subscribe<OrdenDetalleCompra>(this, "OnDataSourcePopupActualizaItemOrdenVentasChanged", (args) =>
            {
                OrdenDetalleCompraSeleccionado = args;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await actualizaOrdenDetalleCompra();
                    await validaEstado("Ingresado", 2);
                    IsBusy = false;
                    //ItemIndex = -1;
                });                
                
            });
            MessagingCenter.Unsubscribe<Orden>(this, "OnDataSourcePopupOrdenTablaPagoChanged");
            MessagingCenter.Subscribe<Orden>(this, "OnDataSourcePopupOrdenTablaPagoChanged", (args) =>
            {
                OrdenSeleccionado = args;
                if(OrdenSeleccionado.ContinuaNavegacion==1)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        IsBusy = true;
                        await navegacion(1); 
                        IsBusy = false;
                        //ItemIndex = -1;
                    });                    
                }
            });*/
            MessagingCenter.Subscribe<OrdenDetalleCompra>(this, "OnDataSourcePopupReceptaItemOrdenComprasChanged", (args) =>
            {
                OrdenDetalleCompraSeleccionado = args;
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
            /*MessagingCenter.Subscribe<Orden>(this, "OnDataSourcePopupActualizaOrdenVentasChanged", (args) =>
            {
                OrdenSeleccionado = args;
                BindingData();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await OpenPdfViewerAsync();
                    IsBusy = false;
                    //ItemIndex = -1;
                });
            });*/
            MessagingCenter.Subscribe<Orden>(this, "PdfViewerIngresaVentasView", (args) =>
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
        public ICommand OpenClienteCompraCommand => new Command(async () => await OpenClienteCompraAsync());
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
                    OrdenDetalleCompra = new ObservableCollection<OrdenDetalleCompra>();
                    OrdenDetalleCompraRecibido = new ObservableCollection<OrdenDetalleCompraDet>();
                    EstadoInfoCollection = new ObservableCollection<OrdenEstado>();
                    PagoInfoCollection = new ObservableCollection<OrdenPago>();                    
                    OrdenSeleccionado.Id = Generator.GenerateKey();
                    OrdenSeleccionado.TipoOrden = TipoOrden.Venta.ToString();
                    OrdenSeleccionado.IdEstado = ((int)EstadoOrdenVenta.Pendiente).ToString();
                    OrdenSeleccionado.CodigoEstado = EstadoOrdenVenta.Pendiente.ToString();
                    OrdenSeleccionado.CodigoEstadoEnConsulta = EstadoOrdenVenta.Ingresado.ToString();
                    OrdenSeleccionado.CodigoEstadoEnDesarrollo = EstadoOrdenVenta.Ingresado.ToString();                    
                    OrdenSeleccionado.Fecha = DateTimeHelpers.GetDate(DateTimeHelpers.GetDateActual());
                    OrdenSeleccionado.OrdenEstado = EstadoInfoCollection;
                    OrdenSeleccionado.OrdenPago = PagoInfoCollection;
                    OrdenSeleccionado.EstadoCancelado = "PENDIENTE";
                    OrdenSeleccionado.OrdenDetalleCompras = OrdenDetalleCompra;
                    OrdenSeleccionado.OrdenDetalleComprasRecibido = OrdenDetalleCompraRecibido;
                    OrdenSeleccionado.OrdenDetalleCompras.CollectionChanged += OnCollectionChanged;
                    //2. OrdenEstado
                    //LlenaEstadoOrden();
                    ResetForm();
                    PageTitle = "Orden";
                }
                else
                {
                    await BindingData();
                }
            }
            if (Registros == null)
            {
                await BuscarRegistros();
            }

            IsBusy = false;
        }

        private async Task BindingData()
        {
            EstaSeleccionadaCondicion = true;
            OrdenSeleccionado.OrdenDetalleCompras = JsonConvert.DeserializeObject<ObservableCollection<OrdenDetalleCompra>>(OrdenSeleccionado.OrdenDetalleCompraSerializado);
            OrdenSeleccionado.OrdenDetalleComprasCancelado = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenDetalleCompraCanceladoSerializado)?JsonConvert.DeserializeObject<ObservableCollection<OrdenDetalleCompra>>(OrdenSeleccionado.OrdenDetalleCompraCanceladoSerializado):new ObservableCollection<OrdenDetalleCompra>();
            OrdenSeleccionado.OrdenDetalleComprasEntregado = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenDetalleCompraEntregadoSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<ItemEntrega>>(OrdenSeleccionado.OrdenDetalleCompraEntregadoSerializado) : new ObservableCollection<ItemEntrega>();            
            OrdenSeleccionado.OrdenTablaPago = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenTablaPagoSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<TablaPago>>(OrdenSeleccionado.OrdenTablaPagoSerializado) : new ObservableCollection<TablaPago>();            
            OrdenSeleccionado.OrdenPago = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenPagoSerializado) ?JsonConvert.DeserializeObject<ObservableCollection<OrdenPago>>(OrdenSeleccionado.OrdenPagoSerializado): (OrdenSeleccionado.OrdenPago!=null? OrdenSeleccionado.OrdenPago:PagoInfoCollection);
            OrdenSeleccionado.OrdenEstado = validaPagosPendientes();
            OrdenSeleccionado.CodigoEstadoEnDesarrollo = OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.StatusProgreso == StepStatus.InProgress)!=null?OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.StatusProgreso == StepStatus.InProgress).Estado:"";
            ClienteCompra = OrdenSeleccionado.NombreCliente;
            IdClienteCompra = OrdenSeleccionado.IdCliente;
            Direccion = OrdenSeleccionado.Direccion;
            Telefono = OrdenSeleccionado.Telefono;
            TieneItemsSeleccionados = OrdenSeleccionado.OrdenDetalleCompras.Any();
            SinProductosOrdenDetalleCompra = !TieneItemsSeleccionados;
            PageTitle = string.Format(TextsTranslateManager.Translate("IngresoComprasTitulo"), OrdenSeleccionado.NumeroOrden, OrdenSeleccionado.NombreProveedor, OrdenSeleccionado.CodigoEstadoEnDesarrollo);//{ texts: Translate IngresoComprasTitulo}            
            CalculoResumen();
        }
        public ObservableCollection<OrdenEstado> validaPagosPendientes()
        {
            var ordenEstado=JsonConvert.DeserializeObject<ObservableCollection<OrdenEstado>>(OrdenSeleccionado.OrdenEstadoCompraSerializado);
            if (OrdenSeleccionado.OrdenTablaPago.Where(x=>x.EstadoCuota.ToUpper().Contains("ERIFICAR")).Count()==0
                && OrdenSeleccionado.OrdenTablaPago.Where(x => x.EstadoCuota.ToUpper().Contains("ENDIENTE")).Count() > 0)
            {

                OrdenSeleccionado.CodigoEstado = "Ingresado";
                OrdenSeleccionado.IdEstado= ((int)EstadoOrdenVenta.Ingresado).ToString();
                //OrdenSeleccionado.CodigoEstadoEnDesarrollo = OrdenSeleccionado.OrdenEstado.Where(x=>x.Estado.ToUpper().Contains("VERIFIC")).Count()>0? "CanceladoVerificar":"Cancelado";                
                ordenEstado.Where(b1=>b1.Estado.ToUpper().Contains("VERIFIC")).ToList()
                                             .ForEach(b1 => b1.StatusProgreso = (StepStatus)1);
                ordenEstado.Where(b1 => Convert.ToInt32(b1.IdEstado)> ((int)EstadoOrdenVentaCliente.CanceladoVerificar)
                                                ).ToList()
                                             .ForEach(b1 => b1.StatusProgreso = (StepStatus)0);


            }
            return ordenEstado;
        }

        public async Task BuscarRegistros()
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var items = await _switchProductos.ConsultaItems(new Item() { Deleted = 0, PoseeStock = -1, PoseeInventarioInicial = -1 }, estaConectado);
            

            if (items != null && items.Count > 0)
            {
                Registros = generales.ToObservableCollection<Item>(items.OrderByDescending(x=>x.CantidadSaldoFinal).ToList());
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
            if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT"))
            {
                ClienteCompra = SettingsOnline.oAplicacion.NombreConectado;
                Identificacion = SettingsOnline.oAplicacion.IdentificacionConectado;
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
            var estadoPermitido = ((int)Enum.Parse(typeof(EstadoOrdenVenta), OrdenSeleccionado.CodigoEstado));
            OrdenDetalleCompra item;

            if (estadoPermitido <= IdEstadoEdicion) //Sólo se puede editar los ítems hasta el estado Ingresado
            {

                if (product.CantidadSaldoFinal > 0 || product.EsServicio==1)
                {

                    item = OrdenSeleccionado.OrdenDetalleCompras.FirstOrDefault(x => x.IdItem == product.Id
                                                                                      && x.Unidad == product.Unidad
                                                                                      );
                    if (item != null)
                    {
                        item.Cantidad += 1;
                        item.ValorCompra += product.PrecioVenta;
                        item.ValorFinal += product.PrecioVenta;
                        product.CantidadReservada = item.Cantidad;
                    }
                    else
                    {
                        item = new OrdenDetalleCompra
                        {
                            Id = Generator.GenerateKey(),
                            IdOrden = OrdenSeleccionado.Id,
                            IdItem = product.Id,
                            CodigoItem = product.Codigo,
                            EsServicio = product.EsServicio,
                            PoseeStock = product.PoseeStock,
                            NombreItem = product.Nombre,
                            IdUnidad = product.IdUnidad,
                            Unidad = product.Unidad,
                            TipoAplicacionImpuesto = product.TipoAplicacionImpuesto,
                            PorcentajeImpuesto = product.PorcentajeImpuesto,
                            Imagen = product.Imagen,
                            Cantidad = 1,
                            ValorUnitario = product.PrecioVenta,
                            ValorCompraImpuesto = product.PrecioVentaImpuesto,
                            ValorCompra = product.PrecioVenta,
                            ValorDescuento = 0,
                            ValorFinal = product.PrecioVenta,
                            CantidadSaldoExistente = product.EsServicio == 0 ? product.CantidadSaldoFinal : 1,
                            ValorConversion = 1,
                            ValorConversionMinimaUnidad = obtieneValorMinimo(product.Id),
                            PeriodoIncremento = product.PeriodoIncremento,
                            CantidadPeriodoIncremento = product.ValorPeriodoIncremento,
                            ValorPeriodoIncremento = product.ValorIncremento,
                            FechaIngresoItem = calculaFechaMinimaIngreso(product.ItemStockRegistradoSerializado),
                            Observaciones = product.Descripcion,
                            IdPeriodoServicio = product.IdPeriodoServicio,
                            PeriodoServicio = product.PeriodoServicio,
                            CantidadPeriodoServicio = product.CantidadPeriodoServicio,
                            EsParticionable = product.EsParticionable

                        };
                        OrdenSeleccionado.OrdenDetalleCompras.Add(item);
                        product.CantidadReservada += 1;
                    }
                    TieneItemsSeleccionados = OrdenSeleccionado.OrdenDetalleCompras.Any();
                    SinProductosOrdenDetalleCompra = !TieneItemsSeleccionados;
                    CalculoResumen();
                    ///Almacenar información extra
                    //AlmacenaDatosAdicionalesDetalle(item);

                }
                else
                {
                    DialogService.ShowToast("El ítem no posee existencias");
                }
            }
            else
            {
                DialogService.ShowToast("Sólo se puede editar el detalle de la Orden en tanto permanezca Ingresada");
            }
        }

        private float obtieneValorMinimo(string IdItem)
        {
                float retorno = 1;
                var LPrecios = SettingsOnline.oLPrecio.Where(x => x.IdItem == IdItem
                                                        && x.EstaActivo == 1).OrderByDescending(y => y.ValorConversion).ToList();                
                var PrecioMinimo = LPrecios!=null? LPrecios.OrderBy(x => x.ValorConversion).FirstOrDefault():null;
                retorno = PrecioMinimo != null ? PrecioMinimo.ValorConversion : retorno;
                return retorno;

        }

        private string calculaFechaMinimaIngreso(string ItemStockRegistradoSerializado)
        {
            var _fechaMinimaIngreso = DateTime.Now.ToString("dd/MM/yyyy");
            var ItemStockRegistrado = !string.IsNullOrEmpty(ItemStockRegistradoSerializado)?JsonConvert.DeserializeObject<ObservableCollection<ItemStock>>(ItemStockRegistradoSerializado):null;
            var itemStockSaldo = ItemStockRegistrado!=null?ItemStockRegistrado.Where(x => x.CantidadSaldoFinal > 0).OrderBy(y => y.FechaIngreso).ToList():null;
            _fechaMinimaIngreso = itemStockSaldo != null && itemStockSaldo.Count > 0 ? itemStockSaldo.FirstOrDefault().FechaIngreso : _fechaMinimaIngreso;
            return _fechaMinimaIngreso;
        }

        private void CalculoResumen()
        {
            CantidadOrden = OrdenSeleccionado.OrdenDetalleCompras.Sum(x => (float)Math.Round((x.Cantidad * x.ValorConversion), 4));
            ValorTotal = OrdenSeleccionado.OrdenDetalleCompras.Sum(x => x.ValorCompra);
            ValorIncremento = OrdenSeleccionado.OrdenDetalleCompras.Sum(x => x.ValorIncremento);
            ValorImpuesto = OrdenSeleccionado.OrdenDetalleCompras.Sum(x => x.ValorImpuesto);
            ValorDescuento = OrdenSeleccionado.OrdenDetalleCompras.Sum(x => x.ValorDescuento);
            ValorFinal= ValorTotal + ValorIncremento + ValorImpuesto - ValorDescuento;
            OrdenSeleccionado.Cantidad = CantidadOrden;
            OrdenSeleccionado.ValorTotal = ValorTotal;
            OrdenSeleccionado.ValorIncremento = ValorIncremento;
            OrdenSeleccionado.ValorImpuesto = ValorImpuesto;
            OrdenSeleccionado.ValorDescuento = ValorDescuento;
            OrdenSeleccionado.ValorFinal = ValorFinal;
            ResumenRegistrosOrdenCompra = string.Format(TextsTranslateManager.Translate("ItemAdded"), CantidadOrden, ValorFinal.FormatPrice());
        }

        private void AlmacenaDatosAdicionalesDetalle(OrdenDetalleCompra item)
        {            
            //OrdenDetalleCompraSeleccionado.IdPeriodoPago = PeriodoPagoSeleccionado.Id;
            //OrdenDetalleCompraSeleccionado.NombrePeriodoPago = PeriodoPagoSeleccionado.Nombre;
            //Almacena Item Entrega
            //await almacenaItemEntrega();
            //await almacenaTablaPago();
        }
        
        private async void TappedCommandAsync(StepTappedEventArgs args)
        {
            bool continuar = true;
            var estadoOrden = args.Item.PrimaryFormattedText.Spans[0].Text;
            if (args.Item.Status.ToString().Contains("Progress"))
            {
                if (estadoOrden.ToUpper()=="CANCELADO" || estadoOrden.ToUpper().Contains("NTREGAD") || estadoOrden.ToUpper().Contains("CTURAD"))
                {
                    if(estadoOrden.ToUpper().Contains("CTURAD"))
                    {
                        await App.Current.MainPage.DisplayAlert("Importante", "El proceso no se encuentra permitido para este perfil", "OK");
                        return;
                    }

                    if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT") || SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("LIENTE"))
                    {
                        await App.Current.MainPage.DisplayAlert("Importante", "El proceso no se encuentra permitido para este perfil", "OK");
                        return;
                    }
                }
                if (OrdenSeleccionado.CantidadTmp > 0 && !string.IsNullOrEmpty(ClienteCompra))
                {

                    continuar = await validaEstado(estadoOrden,2);
                    /*
                    if (continuar)
                    {
                        OrdenSeleccionado.CodigoEstadoEnDesarrollo = estadoOrden;
                        OrdenSeleccionado.CodigoEstadoEnConsulta = estadoOrden;
                        OrdenSeleccionado.NombreCliente = ClienteCompra;
                        OrdenSeleccionado.IdCliente = IdClienteCompra;
                        OrdenSeleccionado.Identificacion = Identificacion;
                        
                        if(estadoOrden.ToUpper().Contains("CANCE"))
                        {
                            OrdenSeleccionado.ContinuaNavegacion = 0;
                            var returnPage = await NavigationService.NavigateToPopupAsync<PopupOrdenTablaPagoViewModel>(OrdenSeleccionado);
                            var popupPage = returnPage as PopupOrdenTablaPagoView;
                            popupPage.CloseButtonEventHandler += async (sender, obj) =>
                            {
                                var isCancel = ((PopupOrdenTablaPagoView)sender).IsCancel;
                                if (!isCancel)
                                {
                                    var resultado = ((PopupOrdenTablaPagoView)sender).PopupResult;
                                    if (resultado != null)
                                    {
                                        OrdenSeleccionado = resultado;
                                        Device.BeginInvokeOnMainThread(async () =>
                                        {
                                            IsBusy = true;
                                            await navegacion(2,1);
                                            IsBusy = false;
                                            //ItemIndex = -1;
                                        });
                                    }
                                }
                                await PopupNavigation.Instance.PopAsync();
                            };
                            var result=await popupPage.PageClosedTask;
                                               
                        }
                        else
                        {
                            //await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenVentasViewModel>(OrdenSeleccionado);
                            await navegacion(2,1);
                        }
                        
                    //await NavigationService.NavigateToPopupAsync<PopupValidaViewModel>(OrdenSeleccionado);
                }*/
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Importante", "Debe al menos seleccionar el Cliente y tener ingresado un ítem a su Orden", "OK");
                }
            }
            if (args.Item.Status.ToString().Contains("Complete"))
            {
                if (!OrdenSeleccionado.CodigoEstado.Contains("Pendien"))
                {
                    OrdenSeleccionado.CodigoEstadoEnConsulta = estadoOrden;
                    await navegacion(2,1);
                    //await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenVentasViewModel>(OrdenSeleccionado);
                }
            }
        }        

        private async Task<bool> validaEstado(string estadoOrden, int origen)
        {
            bool continuar = true;
            OrdenSeleccionado.CodigoEstadoEnDesarrollo = estadoOrden;
            OrdenSeleccionado.CodigoEstadoEnConsulta = estadoOrden;
            OrdenSeleccionado.NombreCliente = !string.IsNullOrEmpty(ClienteCompra)? ClienteCompra:OrdenSeleccionado.NombreCliente;
            OrdenSeleccionado.IdCliente = !string.IsNullOrEmpty(IdClienteCompra) ? IdClienteCompra : OrdenSeleccionado.IdCliente;
            OrdenSeleccionado.Identificacion = !string.IsNullOrEmpty(Identificacion) ? Identificacion : OrdenSeleccionado.Identificacion;
            OrdenSeleccionado.Direccion = !string.IsNullOrEmpty(Direccion) ? Direccion : OrdenSeleccionado.Direccion;
            OrdenSeleccionado.Telefono = !string.IsNullOrEmpty(Telefono) ? Telefono : OrdenSeleccionado.Telefono;
            switch (estadoOrden)
            {
                case string y when estadoOrden.ToString().Contains("Ingre"):
                    continuar = await validaStockItem(estadoOrden, origen);
                    if (!continuar)
                        await abreOrdenAsync(!continuar, origen);
                    else
                        await navegacion(origen,1);                        
                    break;
                case string z when estadoOrden.ToUpper().Contains("CANCE"):                    
                    await navegacion(origen, 2); //Llama a pantalla de tabla de pagos
                    break;
                default:
                    await navegacion(origen, 1);
                    break;
            }
         
            return continuar;
        }

        private async Task<bool> validaStockItem(string estadoOrden, int origen)
        {
            bool retorno = true;
            if (OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.DetalleItemEntrega == null).ToList().Count() > 0 )
            {
                if (origen == 2)
                    await App.Current.MainPage.DisplayAlert("Importante", "Debe confirmar el detalle de su orden", "OK");
                retorno = false;
            }
            else
            {
                OrdenSeleccionado.CodigoEstadoEnDesarrollo = estadoOrden;
                OrdenSeleccionado.CodigoEstadoEnConsulta = estadoOrden;
                OrdenSeleccionado.NombreCliente = !string.IsNullOrEmpty(ClienteCompra) ? ClienteCompra : OrdenSeleccionado.NombreCliente;
                OrdenSeleccionado.IdCliente = !string.IsNullOrEmpty(IdClienteCompra) ? IdClienteCompra : OrdenSeleccionado.IdCliente;
                OrdenSeleccionado.Identificacion = !string.IsNullOrEmpty(Identificacion) ? Identificacion : OrdenSeleccionado.Identificacion;
                OrdenSeleccionado.Direccion = !string.IsNullOrEmpty(Direccion) ? Direccion : OrdenSeleccionado.Direccion;
                OrdenSeleccionado.Telefono = !string.IsNullOrEmpty(Telefono) ? Telefono : OrdenSeleccionado.Telefono;
            }
            return retorno;
        }
        /// <summary>
        /// Parámetros 
        /// sigueValidando  true, false
        /// Origen:1. Desde el detalle de la Orden, 2. desde el proceso de la Orden
        /// </summary>
        /// <param name="sigueValidando"></param>
        /// <param name="origen"></param>
        /// <returns></returns>
        private async Task abreOrdenAsync(bool sigueValidando, int origen)
        {
            if (sigueValidando)
            {
                if (origen == 2)
                    AbreOrden?.Invoke(true);

                /*while (OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.OrdenDetalleComprasRecibido == null).FirstOrDefault() != null)
                {*/
                var data = OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.DetalleItemEntrega == null).FirstOrDefault();
                await OpenDetalleOrdenAsync(2, data);
                //}
            }

        }

        private async Task abreOrdenEstadoAsync()
        {
            AbreOrdenEstado?.Invoke(true);
        }
        /// <summary>
        /// Método para abrir el detalle de la Orden
        /// Parámetros Origen:1. Desde el detalle de la Orden, 2. desde el proceso de la Orden
        /// </summary>
        /// <param name="origen"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task OpenDetalleOrdenAsync(int origen, OrdenDetalleCompra data)
        {
            OrdenDetalleCompraSeleccionado = data;
            var estadoPermitido = ((int)Enum.Parse(typeof(EstadoOrdenVenta), OrdenSeleccionado.CodigoEstado));
            switch (origen)
            {
                case 1:
                    
                    switch(OrdenSeleccionado.CodigoEstadoEnDesarrollo)
                    {
                        case string a when OrdenSeleccionado.CodigoEstadoEnDesarrollo.Contains("Ingre"):
                            if (estadoPermitido <= IdEstadoEdicion) //Sólo se puede editar los ítems hasta el estado Ingresado
                            {
                                await navegacion(origen,3);
                            }
                            break;
                    }
                    break;
                case 2:
                    switch (OrdenSeleccionado.CodigoEstadoEnDesarrollo)
                    {
                        case string a when OrdenSeleccionado.CodigoEstadoEnDesarrollo.Contains("Ingre"):
                            if (estadoPermitido <= IdEstadoEdicion) //Sólo se puede editar los ítems hasta el estado Ingresado
                            {
                                await navegacion(origen,3);
                            }
                            break;
                    }
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
            actualizaItem(((OrdenDetalleCompra)sender).IdItem, ((OrdenDetalleCompra)sender).Cantidad, ((OrdenDetalleCompra)sender).ValorConversion);            
            CalculoResumen();
        }

        public void actualizaItem(string IdItem,float Cantidad,float ValorConversion)
        {
            var item = Registros.Where(x => x.Id == IdItem).FirstOrDefault();
            item.CantidadReservada = (float)Math.Round(Cantidad*ValorConversion,2);
        }

        /// <summary>
        /// Populate shipment details
        /// </summary>
        public void LlenaEstadoOrden(int tipo)
        {
            string numeroestado = "";
            switch (tipo)
            {
                //Crédito
                case 1:
                    foreach (string name in Enum.GetNames(typeof(EstadoOrdenVentaCredito)))
                    {
                        numeroestado = ((int)Enum.Parse(typeof(EstadoOrdenVentaCredito), name)).ToString();
                        OrdenSeleccionado.OrdenEstado.Add(new OrdenEstado()
                        {
                            Id = Generator.GenerateKey()
                        ,
                            IdOrden = OrdenSeleccionado.Id
                        ,
                            IdEstado = ((int)Enum.Parse(typeof(EstadoOrdenVentaCredito), name)).ToString()
                        ,
                            Estado = name
                        ,
                            StatusProgreso = devuelveStatus(numeroestado)
                        ,
                            FechaProcesoStr = devuelveFecha(name,tipo)
                        });
                    }
                    break;
                    //Normal
                case 3:
                    foreach (string name in Enum.GetNames(typeof(EstadoOrdenVentaCliente)))
                    {
                        numeroestado = ((int)Enum.Parse(typeof(EstadoOrdenVentaCliente), name)).ToString();
                        OrdenSeleccionado.OrdenEstado.Add(new OrdenEstado()
                        {
                            Id = Generator.GenerateKey()
                        ,
                            IdOrden = OrdenSeleccionado.Id
                        ,
                            IdEstado = ((int)Enum.Parse(typeof(EstadoOrdenVentaCliente), name)).ToString()
                        ,
                            Estado = name
                        ,
                            StatusProgreso = devuelveStatus(numeroestado)
                        ,
                            FechaProcesoStr = devuelveFecha(name,tipo)
                        });
                    }
                    break;
                default:
                    foreach (string name in Enum.GetNames(typeof(EstadoOrdenVenta)))
                    {
                        numeroestado = ((int)Enum.Parse(typeof(EstadoOrdenVenta), name)).ToString();
                        OrdenSeleccionado.OrdenEstado.Add(new OrdenEstado()
                        {
                            Id = Generator.GenerateKey()
                        ,
                            IdOrden = OrdenSeleccionado.Id
                        ,
                            IdEstado = ((int)Enum.Parse(typeof(EstadoOrdenVenta), name)).ToString()
                        ,
                            Estado = name
                        ,
                            StatusProgreso = devuelveStatus(numeroestado)
                        ,
                            FechaProcesoStr = devuelveFecha(name, tipo)
                        });
                    }
                    break;
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

        public string devuelveFecha(string name,int tipo)
        {
            string fecha = "";
            switch(tipo)
            {
                case 1:
                    switch ((int)Enum.Parse(typeof(EstadoOrdenVentaCredito), name))
                    {
                        case 0:
                            fecha = DateTime.Now.ToString("dd MMM yyyy");
                            break;
                        default:
                            fecha = "";
                            break;

                    }
                    break;
                case 3:
                    switch ((int)Enum.Parse(typeof(EstadoOrdenVentaCliente), name))
                    {
                        case 0:
                            fecha = DateTime.Now.ToString("dd MMM yyyy");
                            break;
                        default:
                            fecha = "";
                            break;

                    }
                    break;
                default:
                    switch ((int)Enum.Parse(typeof(EstadoOrdenVenta), name))
                    {
                        case 0:
                            fecha = DateTime.Now.ToString("dd MMM yyyy");
                            break;
                        default:
                            fecha = "";
                            break;

                    }
                    break;
            }
            
            return fecha;
        }

        public async Task OpenClienteCompraAsync()
        {
            if (!SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT"))
                await NavigationService.NavigateToAsync<ClienteViewModel>();
        }

        private async Task CancelOrderAsync()
        {
            var result = await DialogService.ShowConfirmedAsync(TextsTranslateManager.Translate("ConfirmDiscardPurchase"), TextsTranslateManager.Translate("LegalInfo"), "OK", "Cancel");
            if (result)
            {
                /*MessagingCenter.Send<FooterSummary>(new FooterSummary
                {
                    IsCancel = true
                }, Escant_App.AppSettings.Settings.PurchaseOrderItemsChanged);                
                */
                OrdenSeleccionado.OrdenDetalleCompras = new ObservableCollection<OrdenDetalleCompra>();
                Registros.ForEach(i => i.CantidadReservada = 0);
                CalculoResumen();
            }
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

        public async Task<bool> escogeTipoCondicion()
        {
            bool retorno = true;
            try
            {
                if (OrdenSeleccionado.OrdenEstado.Count<=0 && !EstaSeleccionadaCondicion)
                {
                    var answer = await App.Current.MainPage.DisplayAlert("Condición de Venta", "Escoja la condición de Venta, recuerde que no puede ser modificada", "CREDITO", "NORMAL");
                    if (answer)
                    {
                        LlenaEstadoOrden(1);
                        OrdenSeleccionado.Condicion = "CREDITO";
                        //Do something
                    }
                    else
                    {
                        if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT") || SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("LIENTE"))
                            LlenaEstadoOrden(3);
                        else
                            LlenaEstadoOrden(2);
                        OrdenSeleccionado.Condicion = "NORMAL";
                    }
                    EstaSeleccionadaCondicion = true;
                }
            }
            catch(Exception ex)
            {

            }
            return retorno;
        }

        public void cancelaTipoCondicion()
        {            
            try
            {
                if (!EstaSeleccionadaCondicion)
                {
                    DialogService.ShowToast("Sólo se puede resetear la Condición de Venta de la Orden en tanto ya haya sido seleccionada");
                }
                else
                {
                    var estadoPermitido = !string.IsNullOrEmpty(OrdenSeleccionado.Condicion) && OrdenSeleccionado.Condicion.Contains("NORMAL") ? ((int)Enum.Parse(typeof(EstadoOrdenVenta), OrdenSeleccionado.CodigoEstado)) : ((int)Enum.Parse(typeof(EstadoOrdenVentaCredito), OrdenSeleccionado.CodigoEstado));

                    if (estadoPermitido <= IdEstadoEdicion) //Sólo se puede editar los ítems hasta el estado Ingresado
                    {
                        EstadoInfoCollection = new ObservableCollection<OrdenEstado>();
                        OrdenSeleccionado.OrdenEstado = EstadoInfoCollection;
                        EstaSeleccionadaCondicion = false;
                        DialogService.ShowToast("La Condición de Venta ha sido cancelada para continuar debe seleccionar de nuevo");
                    }
                    else
                    {
                        DialogService.ShowToast("Sólo se puede editar la Condición de Venta de la Orden en tanto permanezca Ingresada");
                    }
                }
            }
            catch (Exception ex)
            {

            }         
        }

        private async Task navegacion(int origen,int tipo)
        {
            bool evaluaEstado = false;
            switch (tipo)
            {
                case 1:
                    
                    var returnPage = await NavigationService.NavigateToPopupAsync<PopupActualizaOrdenVentasViewModel>(OrdenSeleccionado);
                    var popupPage = returnPage as PopupActualizaOrdenVentasView;
                    popupPage.CloseButtonEventHandler += async (sender, obj) =>
                    {
                        var isCancel = ((PopupActualizaOrdenVentasView)sender).IsCancel;
                        if (!isCancel)
                        {
                            var resultado = ((PopupActualizaOrdenVentasView)sender).PopupResult;
                            if (resultado != null)
                            {
                                OrdenSeleccionado = resultado;
                                BindingData();
                                await abreOrdenEstadoAsync();
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    IsBusy = true;
                                    //await OpenPdfViewerAsync();
                                    IsBusy = false;
                                    //ItemIndex = -1;
                                });
                            }
                        }
                        await PopupNavigation.Instance.PopAsync();
                    };
                    var result = await popupPage.PageClosedTask;
                    break;
                case 2:                    
                    var returnPage2 = await NavigationService.NavigateToPopupAsync<PopupOrdenTablaPagoViewModel>(OrdenSeleccionado);
                    var popupPage2 = returnPage2 as PopupOrdenTablaPagoView;
                    popupPage2.CloseButtonEventHandler += async (sender, obj) =>
                    {
                        var isCancel = ((PopupOrdenTablaPagoView)sender).IsCancel;
                        if (!isCancel)
                        {
                            var resultado = ((PopupOrdenTablaPagoView)sender).PopupResult;
                            if (resultado != null)
                            {
                                OrdenSeleccionado = resultado;
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    IsBusy = true;
                                    await navegacion(2, 1);
                                    IsBusy = false;
                                    //ItemIndex = -1;
                                });
                            }
                        }
                        await PopupNavigation.Instance.PopAsync();
                    };
                    var result2 = await popupPage2.PageClosedTask;
                    break;



                case 3:
                    var returnPage1 = await NavigationService.NavigateToPopupAsync<PopupActualizaItemOrdenVentasViewModel>(OrdenDetalleCompraSeleccionado);
                    var popupPage1 = returnPage1 as PopupActualizaItemOrdenVentasView;
                    popupPage1.CloseButtonEventHandler += async (sender, obj) =>
                    {
                        var isCancel = ((PopupActualizaItemOrdenVentasView)sender).IsCancel;
                        if (!isCancel)
                        {
                            var resultado = ((PopupActualizaItemOrdenVentasView)sender).PopupResult;
                            if (resultado != null)
                            {
                                OrdenDetalleCompraSeleccionado = resultado;
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    IsBusy = true;
                                    await actualizaOrdenDetalleCompra();                                    
                                    IsBusy = false;
                                    //ItemIndex = -1;
                                });
                            }
                            if(origen==2)
                                evaluaEstado = true;
                        }
                        await PopupNavigation.Instance.PopAsync();
                    };
                    var result1 = await popupPage1.PageClosedTask;
                    if (evaluaEstado)
                        await validaEstado("Ingresado", 2);                    
                    break;
            }
        }

        private async Task actualizaOrdenDetalleCompra()
        {
            var encontrado = OrdenSeleccionado.OrdenDetalleCompras.FirstOrDefault(x => x.Id == OrdenDetalleCompraSeleccionado.Id);
            int i = OrdenSeleccionado.OrdenDetalleCompras.IndexOf(encontrado);
            OrdenSeleccionado.OrdenDetalleCompras[i] = OrdenDetalleCompraSeleccionado;
        }

        #endregion 6.Métodos Generales
    }

}
