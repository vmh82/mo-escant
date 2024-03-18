using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Productos;
using DataModel.Enums;
using DataModel.Helpers;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Interfaces;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Switch;
using Escant_App.Helpers;
using Escant_App.ViewModels.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using DataModel.DTO.Facturacion;
using DataModel.DTO.Configuracion;
using Escant_App.Common;

namespace Escant_App.ViewModels.Ventas
{
    public class PopupActualizaOrdenVentasViewModel:ViewModelBase
    {
        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración
        private SwitchCompras _switchCompras;
        private SwitchConfiguracion _switchConfiguracion;
        private SwitchProductos _switchProductos;
        private Generales.Generales generales;
        private Generales.GeneraDocumentos generaDocumentos;
        private readonly IServicioProductos_Item _itemServicio;
        private readonly IServicioCompras_Orden _ordenServicio;
        private readonly IServicioCompras_OrdenDetalleCompra _ordenDetalleCompraServicio;
        private readonly IServicioCompras_OrdenEstado _ordenEstadoServicio;
        private readonly IServicioFirestore _servicioFirestore;
        private readonly ISettingsService _settingsServicio;
        private readonly IServicioAdministracion_Catalogo _catalogoServicio;
        private PrinterSettings PrinterSettings { get; set; } = new PrinterSettings();
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase
        private bool _aplicaDescuento;
        private bool _confirmadaTransaccion;
        private bool _entregarTodo;
        private DateTime _fechaVencimiento;
        private Orden _ordenSeleccionado;
        private ObservableCollection<ItemEntrega> _ordenDatosOriginal;
        private ObservableCollection<ItemEntrega> _ordenDetalleCompraEntregadoSeleccionado;
        private List<OrdenDetalleCompra> _ordenDetalleCompraSeleccionado;
        private List<OrdenEstado> _ordenEstadoSeleccionado;
        private List<Catalogo> _lCondicionesVenta;        
        private Catalogo _condicionesVentaSeleccionado;        
        private List<Catalogo> _lFormaCancelacion;
        private List<Catalogo> _formaCancelacionSeleccionado;
        private List<Catalogo> _formaCancelacionSeleccionado1;
        private ObservableCollection<string> _oInstitucionFinanciera = new ObservableCollection<string>();
        private ObservableCollection<OrdenPago> _ordenPagoSeleccionado;
        #endregion 1.2 Variables de Instancia de Objetos para la Clase
        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        private int _listViewHeight;
        private bool _esVisibleAdjunto;
        private bool _isHiddenCondicionesCheque;
        private bool _isEnableValorDescuento;
        private bool _isVisibleCondicionesPago;
        private bool _isEnabledConfirmar;
        private Galeria _galeria;
        private float _valorCancelado;
        private float _valorDescuento;
               
        public CultureInfo Cultura => Escant_App.AppSettings.Settings.CustomCurrencySymbol == "$" ? new System.Globalization.CultureInfo("en-US") : new System.Globalization.CultureInfo("en-US");
        private ImageSource _fuenteImagenRegistro = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #region 1.4 Variables de Control de Datos
        public bool _isVisibleIngresado;
        public bool _isVisibleRecibido;
        public bool _isVisibleCancelado;
        public bool _isVisibleEntregado;
        public bool _isVisibleFacturacion;
        public bool _isVisibleCompartido;
        public bool _isVisibleCondicionesCredito;
        public bool _isVisibleCondicionesDescuento;
        public bool _isHiddenEntregaTodo;
        public float _diasCredito;
        public float _importeCredito;
        public float _valorTotal;
        public float _valorPagado;
        public float _valorCambio;
        public bool _esVisibleObservacion;
        public bool _poseeDatos;
        public string _cuotas;
        #endregion 1.4 Variables de Control de Datos
        #region 1.4 Variables de Control de Errores
        #endregion 1.4 Variables de Control de Errores
        #region 1.6 Acciones
        public Action<bool> EventoGuardar { get; set; }
        public Action<bool> EventoCancelar { get; set; }
        public Action<bool> EventoGrid { get; set; }
        #endregion 1.6 Acciones
        #endregion 1. Declaración de Variables

        #region 2.InstanciaAsignaVariablesControles
        public Orden OrdenSeleccionado
        {
            get => _ordenSeleccionado;
            set
            {
                _ordenSeleccionado = value;
                RaisePropertyChanged(() => OrdenSeleccionado);
            }

        }
        public ObservableCollection<ItemEntrega> OrdenDatosOriginal
        {
            get => _ordenDatosOriginal;
            set
            {
                _ordenDatosOriginal = value;
                RaisePropertyChanged(() => OrdenDatosOriginal);
            }

        }
        public ObservableCollection<ItemEntrega> OrdenDetalleCompraEntregadoSeleccionado
        {
            get => _ordenDetalleCompraEntregadoSeleccionado;
            set
            {
                _ordenDetalleCompraEntregadoSeleccionado = value;
                RaisePropertyChanged(() => OrdenDetalleCompraEntregadoSeleccionado);
            }

        }

        public List<OrdenDetalleCompra> OrdenDetalleCompraSeleccionado
        {
            get => _ordenDetalleCompraSeleccionado;
            set
            {
                _ordenDetalleCompraSeleccionado = value;
                RaisePropertyChanged(() => OrdenDetalleCompraSeleccionado);
            }

        }
        public List<OrdenEstado> OrdenEstadoSeleccionado
        {
            get => _ordenEstadoSeleccionado;
            set
            {
                _ordenEstadoSeleccionado = value;
                RaisePropertyChanged(() => OrdenEstadoSeleccionado);
            }

        }
        public int ListViewHeight
        {
            get => _listViewHeight;
            set
            {
                _listViewHeight = value;
                RaisePropertyChanged(() => ListViewHeight);
            }
        }
        public bool EsVisibleAdjunto
        {
            get => _esVisibleAdjunto;
            set
            {
                _esVisibleAdjunto = value;
                RaisePropertyChanged(() => EsVisibleAdjunto);
            }
        }
        public bool EsEnableValorDescuento
        {
            get => _isEnableValorDescuento;
            set
            {
                _isEnableValorDescuento = value;
                RaisePropertyChanged(() => EsEnableValorDescuento);
            }
        }
        public bool IsEnableConfirmar
        {
            get => _isEnabledConfirmar;
            set
            {
                _isEnabledConfirmar = value;
                RaisePropertyChanged(() => IsEnableConfirmar);
            }
        }
        public bool IsVisibleCondicionesPago
        {
            get => _isVisibleCondicionesPago;
            set
            {
                _isVisibleCondicionesPago = value;
                RaisePropertyChanged(() => IsVisibleCondicionesPago);
            }
        }
        public bool IsHiddenCondicionesCheque
        {
            get => _isHiddenCondicionesCheque;
            set
            {
                _isHiddenCondicionesCheque = value;
                RaisePropertyChanged(() => IsHiddenCondicionesCheque);
            }
        }
        public bool IsVisibleIngresado
        {
            get => _isVisibleIngresado;
            set
            {
                _isVisibleIngresado = value;
                RaisePropertyChanged(() => IsVisibleIngresado);
            }
        }
        public bool IsVisibleRecibido
        {
            get => _isVisibleRecibido;
            set
            {
                _isVisibleRecibido = value;
                RaisePropertyChanged(() => IsVisibleRecibido);
            }
        }
        public bool IsVisibleCancelado
        {
            get => _isVisibleCancelado;
            set
            {
                _isVisibleCancelado = value;
                RaisePropertyChanged(() => IsVisibleCancelado);
            }
        }
        public bool IsVisibleEntregado
        {
            get => _isVisibleEntregado;
            set
            {
                _isVisibleEntregado = value;
                RaisePropertyChanged(() => IsVisibleEntregado);
            }
        }
        public bool IsVisibleFacturacion
        {
            get => _isVisibleCancelado;
            set
            {
                _isVisibleCancelado = value;
                RaisePropertyChanged(() => IsVisibleFacturacion);
            }
        }
        public bool IsVisibleCompartido
        {
            get => _isVisibleCompartido;
            set
            {
                _isVisibleCompartido = value;
                RaisePropertyChanged(() => IsVisibleCompartido);
            }
        }
        public bool IsVisibleCondicionesCredito
        {
            get => _isVisibleCondicionesCredito;
            set
            {
                _isVisibleCondicionesCredito = value;
                RaisePropertyChanged(() => IsVisibleCondicionesCredito);
            }
        }

        public bool IsVisibleCondicionesDescuento
        {
            get => _isVisibleCondicionesDescuento;
            set
            {
                _isVisibleCondicionesDescuento = value;
                RaisePropertyChanged(() => IsVisibleCondicionesDescuento);
            }
        }
        public bool IsHiddenEntregaTodo
        {
            get => _isHiddenEntregaTodo;
            set
            {
                _isHiddenEntregaTodo = value;
                RaisePropertyChanged(() => IsHiddenEntregaTodo);
            }
        }

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

        public float ValorCancelado
        {
            get
            {
                return _valorCancelado;
            }
            set
            {
                _valorCancelado = value;
                RaisePropertyChanged(() => ValorCancelado);
            }
        }
        public float ValorDescuento
        {
            get
            {
                return _valorDescuento;
            }
            set
            {
                _valorDescuento = value;
                RaisePropertyChanged(() => ValorDescuento);
            }
        }

        public List<Catalogo> LCondicionesVenta
        {
            get
            {
                return _lCondicionesVenta;
            }
            set
            {
                _lCondicionesVenta = value;
                RaisePropertyChanged(() => LCondicionesVenta);
            }
        }        
        public Catalogo CondicionesVentaSeleccionado
        {
            get
            {
                return _condicionesVentaSeleccionado;
            }
            set
            {
                _condicionesVentaSeleccionado = value;
                RaisePropertyChanged(() => CondicionesVentaSeleccionado);
            }
        }
        public List<Catalogo> LFormaCancelacion
        {
            get
            {
                return _lFormaCancelacion;
            }
            set
            {
                _lFormaCancelacion = value;
                RaisePropertyChanged(() => LFormaCancelacion);
            }
        }
        public List<Catalogo> FormaCancelacionSeleccionado
        {
            get
            {
                return _formaCancelacionSeleccionado;
            }
            set
            {
                _formaCancelacionSeleccionado = value;
                RaisePropertyChanged(() => FormaCancelacionSeleccionado);
            }
        }
        public List<Catalogo> FormaCancelacionSeleccionado1
        {
            get
            {
                return _formaCancelacionSeleccionado1;
            }
            set
            {
                _formaCancelacionSeleccionado1 = value;
                RaisePropertyChanged(() => FormaCancelacionSeleccionado1);
            }
        }
        public float DiasCredito
        {
            get
            {
                return _diasCredito;
            }
            set
            {
                _diasCredito = value;
                RaisePropertyChanged(() => DiasCredito);
            }
        }
        public float ImporteCredito
        {
            get
            {
                return _importeCredito;
            }
            set
            {
                _importeCredito = value;
                RaisePropertyChanged(() => ImporteCredito);
            }
        }
        public float ValorTotal
        {
            get
            {
                return _valorTotal;
            }
            set
            {
                _valorTotal = value;
                RaisePropertyChanged(() => ValorTotal);
            }
        }
        public float ValorPagado
        {
            get
            {
                return _valorPagado;
            }
            set
            {
                _valorPagado = value;
                RaisePropertyChanged(() => ValorPagado);
            }
        }
        public float ValorCambio
        {
            get
            {
                return _valorCambio;
            }
            set
            {
                _valorCambio = value;
                RaisePropertyChanged(() => ValorCambio);
            }
        }
        public bool ConfirmadaTransaccion
        {
            get
            {
                return _confirmadaTransaccion;
            }
            set
            {
                _confirmadaTransaccion = value;
                RaisePropertyChanged(() => ConfirmadaTransaccion);
            }
        }
        public bool EntregarTodo
        {
            get
            {
                return _entregarTodo;
            }
            set
            {
                _entregarTodo = value;
                RaisePropertyChanged(() => EntregarTodo);
                calculoInicial();
            }
        }
        public DateTime FechaVencimiento
        {
            get
            {
                return _fechaVencimiento;
            }
            set
            {
                _fechaVencimiento = value;
                RaisePropertyChanged(() => FechaVencimiento);
            }
        }
        public ObservableCollection<string> OInstitucionFinanciera
        {
            get { return _oInstitucionFinanciera; }
            set
            {
                _oInstitucionFinanciera = value;
                OnPropertyChanged(nameof(OInstitucionFinanciera));
            }
        }
        public ObservableCollection<OrdenPago> OrdenPagoSeleccionado
        {
            get { return _ordenPagoSeleccionado; }
            set
            {
                _ordenPagoSeleccionado = value;
                OnPropertyChanged(nameof(OrdenPagoSeleccionado));
            }
        }
        public bool EsVisibleObservacion
        {
            get
            {
                return _esVisibleObservacion;
            }
            set
            {
                _esVisibleObservacion = value;
                RaisePropertyChanged(() => EsVisibleObservacion);
            }
        }
        public bool PoseeDatos
        {
            get
            {
                return _poseeDatos;
            }
            set
            {
                _poseeDatos = value;
                RaisePropertyChanged(() => PoseeDatos);
            }
        }
        public string Cuotas
        {
            get
            {
                return _cuotas;
            }
            set
            {
                _cuotas = value;
                RaisePropertyChanged(() => Cuotas);
            }
        }
        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        public PopupActualizaOrdenVentasViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioCompras_Orden ordenServicio
                                  , IServicioCompras_OrdenDetalleCompra ordenDetalleCompraServicio
                                  , IServicioCompras_OrdenEstado ordenEstadoServicio
                                  , ISettingsService settingsServicio
                                  , IServicioFirestore servicioFirestore
                                  , IServicioProductos_Item itemServicio
                                  , IServicioAdministracion_Catalogo catalogoServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);
            //Instanciación Acorde al model
            _itemServicio = itemServicio;
            _ordenServicio = ordenServicio;
            _servicioFirestore = servicioFirestore;
            _settingsServicio = settingsServicio;
            _catalogoServicio = catalogoServicio;
            _ordenDetalleCompraServicio = ordenDetalleCompraServicio;
            _ordenEstadoServicio = ordenEstadoServicio;
            _switchCompras = new SwitchCompras(_ordenServicio, _ordenDetalleCompraServicio, _ordenEstadoServicio);
            _switchConfiguracion = new SwitchConfiguracion(_settingsServicio, _servicioFirestore);
            _switchProductos = new SwitchProductos(_itemServicio);
            //_switchFacturacion = new SwitchFacturacion();
            generales = new Generales.Generales(_catalogoServicio,_settingsServicio, _servicioFirestore);
            generaDocumentos = new Generales.GeneraDocumentos();
            //cargaCombo(2);
        }
        #endregion 3.Constructor

        #region 4.DefinicionMetodos
        public ICommand BrowseGalleryCommand => new Command(async () => await BrowseGalleryAsync());
        public ICommand OpenCameraCommand => new Command(async () => await OpenCameraAsync());
        public ICommand RemoveImageCommand => new Command(async () => await RemoveImageAsync());
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());

        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            try
            {
                if (navigationData is Orden item && item != null)
                {
                    OrdenSeleccionado = item;
                    if (OrdenSeleccionado.CodigoEstadoEnConsulta == OrdenSeleccionado.CodigoEstadoEnDesarrollo)
                    {
                        switch (OrdenSeleccionado.CodigoEstadoEnConsulta)
                        {
                            case "Ingresado":
                                EsVisibleObservacion = true;
                                OrdenSeleccionado.Observaciones = string.Format(TextsTranslateManager.Translate("ObservacionVenta"), OrdenSeleccionado.NombreCliente, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorFinal.FormatPrice());
                                break;
                            case "Recibido":
                                var cantpro = OrdenSeleccionado.OrdenDetalleComprasRecibido != null ? OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(y => !y.TipoDescuento.Contains("Aplica")).Sum(x => x.Cantidad) : 0;
                                var descto = OrdenSeleccionado.OrdenDetalleComprasRecibido != null ? OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(y => !y.TipoDescuento.Contains("Aplica")).Sum(x => x.ValorDescuento) : 0;
                                OrdenSeleccionado.Observaciones = string.Format(TextsTranslateManager.Translate("ObservacionCompraRecibido"), OrdenSeleccionado.NombreProveedor, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorFinal.FormatPrice(), cantpro, descto.FormatPrice());
                                break;
                            case string a when OrdenSeleccionado.CodigoEstadoEnConsulta.ToString().Contains("Cancelad"):
                                //case "Cancelado":
                                OrdenPagoSeleccionado = new ObservableCollection<OrdenPago>();
                                if (OrdenSeleccionado.CodigoEstadoEnConsulta.ToString().Contains("Verific"))
                                {
                                    IsEnableConfirmar = false;                                    
                                    await CalculaValores("",1);
                                }
                                else
                                {
                                    IsEnableConfirmar = true;
                                    await pagoVerificar();                                    
                                    OrdenSeleccionado.OrdenTablaPago = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenTablaPagoSerializado) ? new ObservableCollection<TablaPago>(JsonConvert.DeserializeObject<List<TablaPago>>(OrdenSeleccionado.OrdenTablaPagoSerializado)) : new ObservableCollection<TablaPago>();
                                    await CalculaValores("", 2);
                                    /*OrdenPagoSeleccionado = new ObservableCollection<TablaPago>(OrdenSeleccionado.OrdenTablaPago.Where(x => x.Seleccionada == 1
                                                                                                 && x.EstadoCuota.ToUpper().Contains("CANCELA")).ToList());*/
                                }
                                EsVisibleObservacion = false;
                                
                                break;
                            case "Entregado":
                                EsVisibleObservacion = false;
                                OrdenDetalleCompraEntregadoSeleccionado = new ObservableCollection<ItemEntrega>(JsonConvert.DeserializeObject<List<ItemEntrega>>(OrdenSeleccionado.OrdenDetalleCompraEntregadoSerializado));
                                break;
                            case "Facturado":
                                EsVisibleObservacion = true;
                                OrdenSeleccionado.Observaciones = string.Format(TextsTranslateManager.Translate("ObservacionFactura"), OrdenSeleccionado.NombreCliente, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorFinal.FormatPrice());
                                break;
                        }
                    }

                    inicia();
                    BindingData();
                    //CalculateListViewHeight();
                }
            }
            catch (NullReferenceException ex)
            {

            }
            IsBusy = false;
        }

        private void inicia()
        {
            try
            {                
                ConfirmadaTransaccion = false;
                PoseeDatos = false;
                IsVisibleCondicionesCredito = false;
                IsVisibleCondicionesPago = false;
                IsVisibleCondicionesDescuento = false;
                IsHiddenCondicionesCheque = true;                
                IsVisibleFacturacion = false;
                IsVisibleIngresado = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Ingres") ? true : false;
                IsVisibleRecibido = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Recibi") ? true : false;
                IsVisibleCancelado = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Cancela") ? true : false;
                IsVisibleEntregado = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Entrega") ? true : false;
                IsVisibleCompartido = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Recibi") ? true : false;
                if (IsVisibleIngresado)
                {
                    cargaCombo(1);
                }
                if (IsVisibleCancelado)
                {
                    cargaCombo(2);
                }
                if (IsVisibleEntregado)
                {
                    IsHiddenEntregaTodo = true;
                    EntregarTodo = true;                    
                }
            }
            catch(NullReferenceException ex)
            {

            }
        }

        private void cargaCombo(int tipo)
        {
            switch (tipo)
            {
                case 1:
                    LCondicionesVenta = SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "condicionesventa" && y.EsCatalogo == 0).ToList();
                    break;
                case 2:
                    LFormaCancelacion = SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "formacancelacion" && y.EsCatalogo == 0).ToList();
                    FormaCancelacionSeleccionado1 = OrdenSeleccionado.OrdenPago.Where(x => x.EstadoCancelado.ToUpper().Contains("VERIFICA")).ToList().Count>0
                        && !SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT") 
                        && !SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("LIENTE")
                        ? LFormaCancelacion.Where(y=>y.Nombre.Contains(OrdenSeleccionado.OrdenPago.Where(x => x.EstadoCancelado.ToUpper().Contains("VERIFICA")).ToList().FirstOrDefault().FormaCancelacion)).ToList() : LFormaCancelacion.Where(y => y.Nombre.ToUpper().Contains("EFEC")).ToList();
                    //FormaCancelacionSeleccionado = LFormaCancelacion.Where(y => y.Nombre.Contains("EFEC")).ToList();                   

                    OInstitucionFinanciera = new ObservableCollection<string>(SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "institucionfinanciera"
                                                                                                            && y.EsCatalogo == 0).Select(x => x.Nombre).ToList());                    
                    break;
            }
            
        }

        private void BindingData()
        {
            try
            {
                switch (OrdenSeleccionado.CodigoEstadoEnConsulta)
                {
                    case "Ingresado":
                        CondicionesVentaSeleccionado = LCondicionesVenta.Where(x => x.Nombre == OrdenSeleccionado.Condicion).FirstOrDefault();
                        EvaluaCondicion();
                        break;
                    case "Cancelado":
                        CargaImagen(5);
                        break;
                    case "Recibido":
                        CargaImagen(4);
                        break;

                }
                
            }
            catch(NullReferenceException ex)
            {

            }
            //EsVisibleAdjunto = !string.IsNullOrEmpty(OrdenSeleccionado.IdGaleria) ? true : false;
            //OrdenSeleccionado.AdjuntoFactura= !String.IsNullOrEmpty(OrdenSeleccionado.AdjuntoFactura)? OrdenSeleccionado.AdjuntoFactura:
        }

        private async Task Ok_CommandAsync()
        {
            string informedMessage = "";
            bool continuar = true;
            IsBusy = true;
            if (await validaAlmacenamiento())
            {
                bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                bool existe = false;
                //1.Recuperar datos de ALmacenamiento
                //OrdenDetalleCompraSeleccionado = new List<OrdenDetalleCompra>(OrdenSeleccionado.OrdenDetalleCompras);
                //OrdenEstadoSeleccionado = new List<OrdenEstado>(OrdenSeleccionado.OrdenEstado);
                if (string.IsNullOrEmpty(OrdenSeleccionado.NumeroOrden) || OrdenSeleccionado.NumeroOrden == "0")
                {
                    var secuenciales = await generales.obtieneSecuenciales("OrdenVenta");
                    OrdenSeleccionado.NumeroOrden = secuenciales.SecuencialReciboIncrementado;
                }
                OrdenSeleccionado.Imagen = OrdenSeleccionado.Imagen;

                var datosfactura=await procesarFacturacion(estaConectado);
                if (datosfactura.Continuar == 1)
                {
                    await actualizaEstadoOrdenAsync();
                    switch (OrdenSeleccionado.CodigoEstado)
                    {
                        case "Ingresado":
                            OrdenSeleccionado.OrdenTablaPagoSerializado = devuelveTablaPagoSerializado();
                            OrdenSeleccionado.OrdenDetalleCompraSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.OrdenDetalleCompras);
                            OrdenSeleccionado.Condicion = CondicionesVentaSeleccionado.Nombre;
                            OrdenSeleccionado.IdCondicion = CondicionesVentaSeleccionado.Id;
                            OrdenSeleccionado.DiasCredito = OrdenSeleccionado.Condicion.Contains("CREDIT") ? DiasCredito : OrdenSeleccionado.DiasCredito;
                            OrdenSeleccionado.ImporteCredito = OrdenSeleccionado.Condicion.Contains("CREDIT") ? ImporteCredito : OrdenSeleccionado.ImporteCredito;
                            break;
                        case "Recibido":
                            OrdenSeleccionado.OrdenDetalleCompraRecibidoSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.OrdenDetalleComprasRecibido);
                            OrdenSeleccionado.AdjuntoFactura = _galeria != null ? _galeria.Image : OrdenSeleccionado.AdjuntoFactura;
                            break;
                        case "CanceladoVerificar":
                            OrdenSeleccionado.OrdenDetalleCompraCanceladoSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.OrdenDetalleCompras);
                            await actualizaTablaPago(1);
                            await actualizaPago(1);
                            OrdenSeleccionado.AdjuntoCancelado = _galeria != null ? _galeria.Image : OrdenSeleccionado.AdjuntoCancelado;
                            break;
                        case "Cancelado":
                            if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT") || SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("LIENTE"))
                                await App.Current.MainPage.DisplayAlert("Importante", "El proceso no se encuentra permitido para este perfil", "OK");
                            else
                            {                                
                                OrdenSeleccionado.OrdenDetalleCompraCanceladoSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.OrdenDetalleCompras);                                
                                await actualizaTablaPago(2);
                                await actualizaPago(2);
                                OrdenSeleccionado.AdjuntoCancelado = _galeria != null ? _galeria.Image : OrdenSeleccionado.AdjuntoCancelado;
                            }
                            break;
                        case "Entregado":
                            if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT") || SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("LIENTE"))
                                await App.Current.MainPage.DisplayAlert("Importante", "El proceso no se encuentra permitido para este perfil", "OK");
                            else
                                    {
                                        OrdenSeleccionado.OrdenDetalleComprasEntregado = OrdenDetalleCompraEntregadoSeleccionado;
                                        OrdenSeleccionado.OrdenDetalleCompraEntregadoSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.OrdenDetalleComprasEntregado);
                                    }
                            break;
                        case "Facturado":
                            if (SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT") || SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("LIENTE"))
                                await App.Current.MainPage.DisplayAlert("Importante", "El proceso no se encuentra permitido para este perfil", "OK");
                            else
                                    {
                                        OrdenSeleccionado.FacturacionSerializado = JsonConvert.SerializeObject(datosfactura);
                                        OrdenSeleccionado.NumeroSecuencialFactura = datosfactura.Secuencial.ToString();
                                    }
                            break;
                    }

                    OrdenSeleccionado.OrdenEstadoCompraSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.OrdenEstado);

                    if (OrdenSeleccionado.CodigoEstado.Contains("Cancelado") && OrdenSeleccionado.OrdenDetalleCompras.Count > 0)
                    {
                        OrdenSeleccionado.OrdenDetalleComprasEntregado = await obtieneEntregado();
                        OrdenSeleccionado.OrdenDetalleCompraEntregadoSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.OrdenDetalleComprasEntregado);
                        var resultado1 = await _switchProductos.GuardaRegistro_StockOrden(OrdenSeleccionado, estaConectado, false);
                        //OrdenSeleccionado.OrdenDetalleCompraEntregadoSerializado = resultado1.objetoSerializado;
                    }
                    var resultado = await _switchCompras.GuardaRegistro_Orden(OrdenSeleccionado, estaConectado, existe);
                    //if(OrdenSeleccionado.CodigoEstado.ToUpper().Contains("CANCELA") || OrdenSeleccionado.CodigoEstado.ToUpper().Contains("FACTURA"))
                    //    await ImprimirOrden();
                    informedMessage = resultado.mensaje;
                    DialogService.ShowToast(informedMessage);
                    /*MessagingCenter.Send<Orden>(OrdenSeleccionado, "OnDataSourcePopupActualizaOrdenVentasChanged");
                    await NavigationService.RemovePopupAsync();*/
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Importante", "El proceso de facturación no pudo concluir con éxito", "OK");
                }
            }
            IsBusy = false;
            EventoGuardar?.Invoke(true);
        }




        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales
        private async Task ImprimirOrden()
        {

            var bluetooth = App.BluetoothAdaper;
            if (!bluetooth.IsBluetoothTurnOn)
                bluetooth.Enable();

            if (bluetooth.IsBluetoothTurnOn)
            //if(true==true)
            {

                var billInfo = OrdenSeleccionado;
                SettingsOnline.oAplicacion.Lenguaje = string.IsNullOrEmpty(Escant_App.AppSettings.Settings.LanguageSelected) ? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName : Escant_App.AppSettings.Settings.LanguageSelected;
                SettingsOnline.oAplicacion.FormatoActual = string.IsNullOrEmpty(Escant_App.AppSettings.Settings.CurrentCurrencyCustomFormat) ? SystemConstants.VN_CURRENTCY_FORMAT : Escant_App.AppSettings.Settings.CurrentCurrencyCustomFormat;
                billInfo.ParametrosImpresion = JsonConvert.SerializeObject(SettingsOnline.oAplicacion);
                //Facturación Electrónica
                //var y= await bluetooth.ProcesaFacturaElectronica(billInfo);
                //PrinterSettings = JsonConvert.DeserializeObject<PrinterSettings>(SettingsOnline.oAplicacion.ParametrosImpresion);
                //var result = await bluetooth.ConnectAndPrintOrden(PrinterSettings?.DefaultDevice, billInfo);
                //if (!result)
                //{
                //    await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("PrintError"), TextsTranslateManager.Translate("Warning"), "OK");
                //}
            }
        }


        private async Task<ParametroComprobante> procesarFacturacion(bool estaConectado)
        {
            ParametroComprobante resultado = new ParametroComprobante();
            /*if ((OrdenSeleccionado.CodigoEstadoEnDesarrollo == "Facturado" && OrdenSeleccionado.CodigoEstadoEnConsulta == "Facturado"))
            {
                if (string.IsNullOrEmpty(OrdenSeleccionado.NumeroSecuencialFactura) || OrdenSeleccionado.NumeroSecuencialFactura == "0")
                {
                    var secuenciales = await generales.obtieneSecuenciales("NumeroFactura");
                    OrdenSeleccionado.NumeroSecuencialFactura = secuenciales.SecuencialReciboIncrementado;
                }
                if (!string.IsNullOrEmpty(OrdenSeleccionado.NumeroSecuencialFactura) && Convert.ToInt32(OrdenSeleccionado.NumeroSecuencialFactura) > 0)
                {                    
                    resultado = await _switchFacturacion.FacturarSRI(OrdenSeleccionado, estaConectado);
                    if (resultado.Estado == "RECIBIDA" && resultado.EstadoAutorizacion != "AUTORIZADA")
                    {
                        OrdenSeleccionado.FacturacionSerializado = JsonConvert.SerializeObject(resultado);
                        OrdenSeleccionado.NumeroSecuencialFactura = resultado.Secuencial.ToString();
                        var resultado1 = await _switchCompras.GuardaRegistro_Orden(OrdenSeleccionado, estaConectado, true);
                    }
                    resultado.Continuar = resultado.Exito;
                }
                else
                    resultado.Continuar = 0;
            }
            else
                resultado.Continuar = 1;*/
            resultado.Continuar = 1;
            return resultado;
        }
        /// <summary>
        /// Método para generar la tabla de pago de los ítems seleccionados
        /// </summary>
        /// <returns></returns>
        private string devuelveTablaPagoSerializado()
        {
            List<TablaPago> LTablaPagoConsolidado = new List<TablaPago>();
            List<TablaPago> LTablaPago = new List<TablaPago>();
            foreach (var item in OrdenSeleccionado.OrdenDetalleCompras)
            {
                foreach(var detalle in JsonConvert.DeserializeObject<List<TablaPago>>(item.TablaPagoSerializado))
                {
                    LTablaPagoConsolidado.Add(detalle);
                }                
            }
            LTablaPago = LTablaPagoConsolidado.GroupBy(x => new { x.NumeroCuota, x.EstadoCuota, x.FechaVencimiento })
                    .Select(c => new TablaPago()
                    {
                        IdOrden = OrdenSeleccionado.Id,
                        NumeroOrden = OrdenSeleccionado.NumeroOrden,
                        NumeroCuota = c.Key.NumeroCuota,
                        EstadoCuota = c.Key.EstadoCuota,
                        FechaVencimiento = c.Key.FechaVencimiento,
                        ValorCompra = c.Sum(g => g.ValorCompra),
                        ValorIncremento = c.Sum(g => g.ValorIncremento),
                        ValorDescuento = c.Sum(g => g.ValorDescuento),
                        ValorImpuesto = c.Sum(g => g.ValorImpuesto),
                        ValorFinal = c.Sum(g => g.ValorFinal)
                    }).ToList();            

            return LTablaPago.Count > 0 ? JsonConvert.SerializeObject(LTablaPago) : null;
        }
        /// <summary>
        /// Obtener el detalle de ítem de Entrega
        /// </summary>
        /// <returns></returns>
        private async Task<ObservableCollection<ItemEntrega>> obtieneEntregado()
        {
            ObservableCollection<ItemEntrega> resultado = new ObservableCollection<ItemEntrega>();
            var recorrido = OrdenSeleccionado.OrdenDetalleCompras;
            foreach (var item in recorrido)
            {
                //var detalleItemEntrega = JsonConvert.DeserializeObject<List<TablaPago>>(item.DetalleItemEntregaSerializado);
                foreach (var item1 in JsonConvert.DeserializeObject<List<ItemEntrega>>(item.DetalleItemEntregaSerializado))
                {
                    resultado.Add(item1);
                }
            }
            return resultado;

        }
        /// <summary>
        /// Método para validar almacenamiento
        /// </summary>
        /// <returns></returns>
        private async Task<bool> validaAlmacenamiento()
        {
            bool retorno = false;
            if (OrdenSeleccionado != null && OrdenSeleccionado.CodigoEstadoEnConsulta == OrdenSeleccionado.CodigoEstadoEnDesarrollo)
            {
                switch (OrdenSeleccionado.CodigoEstadoEnDesarrollo)
                {
                    case "Ingresado":
                        if (CondicionesVentaSeleccionado.Nombre.Contains("DITO"))
                        {
                            if (DiasCredito>0)
                            {
                                retorno = true;
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Importante", "Debe ingresar a cuantos días se vence el Crédito", "OK");
                            }
                        }
                        else {
                            retorno = true;
                        }
                        
                        break;
                    case "Recibido":
                        if (!string.IsNullOrEmpty(OrdenSeleccionado.NumeroFactura))
                        {
                            if (!FuenteImagenRegistro.IsEmpty)
                            {
                                retorno = true;
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Importante", "Debe adjuntar el respaldo de factura y/o remisión", "OK");
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Importante", "Debe registrar el número de factura y/o remisión", "OK");
                        }
                        break;
                    case "CanceladoVerificar":
                    case "Cancelado":
                        //0. Validar que descuento se aplica solo a Condición de Venta Normal
                        if (OrdenSeleccionado.ValorDescuento == 0 && !OrdenSeleccionado.Condicion.ToUpper().Contains("REDI"))
                            retorno = true;
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Importante", "El descuento no se aplica a Ventas a Crédito", "OK");
                            break;
                        }
                        //1. Validar que ingrese todos los datos requeridos
                        if (FormaCancelacionSeleccionado1.Count >= 1) //Más de 1 forma de Pago
                        {
                            if(FormaCancelacionSeleccionado1.Where(x=>x.Nombre.ToUpper().Contains("CHE")
                                                                   || x.Nombre.ToUpper().Contains("TRA")).ToList().Count>=1)
                            {
                                if (!FuenteImagenRegistro.IsEmpty)
                                {
                                    retorno = true;
                                }
                                else
                                {
                                    await App.Current.MainPage.DisplayAlert("Importante", "Debe adjuntar el respaldo de la forma de pago", "OK");
                                    break;
                                }
                            }

                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Importante", "Debe registrar el o los valores de cancelación", "OK");
                            break;
                        }
                        //2. Validar Pago
                        float valorpago = ValorPagado;
                        float valorpagar = ValorTotal;
                        if (!OrdenSeleccionado.Condicion.ToUpper().Contains("REDI"))
                        {
                            if (valorpago != valorpagar)
                            {
                                await App.Current.MainPage.DisplayAlert("Importante", "Favor complete el pago, el abono solo se permite en créditos", "OK");
                                break;
                            }
                            else
                                retorno =true;
                        }

                        break;
                    case "Entregado":
                        if(OrdenDetalleCompraEntregadoSeleccionado.Sum(x=>x.cantidadEntregada)<=0)
                        { 
                            await App.Current.MainPage.DisplayAlert("Importante", "Favor seleccione si entrega parcial o totalmente su Orden", "OK");
                        }
                        else
                        {
                            return true;
                        }
                        break;
                    default:
                        retorno = true;
                        break;
                }
            }


            return retorno;
        }
        /// <summary>
        /// Método para actualizar el estado de la Orden
        /// </summary>
        /// <returns></returns>
        private async Task actualizaEstadoOrdenAsync()
        {
            OrdenEstado estadoActualiza = new OrdenEstado();
            var IdEstadoActualiza = OrdenSeleccionado.IdEstado;
            var IdEstadoSeguimiento = OrdenSeleccionado.CodigoEstadoEnDesarrollo != null ? OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.Estado == OrdenSeleccionado.CodigoEstadoEnDesarrollo).IdEstado : "";
            var IdEstadoSiguiente = OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.StatusProgreso.ToString().Contains("Progre")).IdEstado;
            /*OrdenSeleccionado.CodigoEstado = OrdenSeleccionado.CodigoEstadoEnDesarrollo != null ? OrdenSeleccionado.CodigoEstadoEnDesarrollo : OrdenSeleccionado.CodigoEstado;
            OrdenSeleccionado.IdEstado = ((int)Enum.Parse(typeof(EstadoOrdenCompra), OrdenSeleccionado.CodigoEstado)).ToString();*/
            if (IdEstadoSeguimiento == IdEstadoSiguiente)
            {
                OrdenSeleccionado.IdEstado = IdEstadoSiguiente;
                OrdenSeleccionado.CodigoEstado = OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.IdEstado == IdEstadoSiguiente).Estado;
                foreach (OrdenEstado ordenEstado in OrdenSeleccionado.OrdenEstado)
                {
                    estadoActualiza = OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.Id == ordenEstado.Id);
                    if (Convert.ToInt32(ordenEstado.IdEstado) >= Convert.ToInt32(OrdenSeleccionado.IdEstado))
                    {

                        if (Convert.ToInt32(ordenEstado.IdEstado) == (Convert.ToInt32(OrdenSeleccionado.IdEstado)))
                        {
                            estadoActualiza.StatusProgreso = Syncfusion.XForms.ProgressBar.StepStatus.Completed;
                            estadoActualiza.FechaProcesoStr = DateTime.Now.ToString("dd MMM yyyy");
                            estadoActualiza.Observaciones = OrdenSeleccionado.Observaciones;
                            switch (OrdenSeleccionado.CodigoEstado)
                            {
                                case "Recibido":
                                    estadoActualiza.Adjunto = OrdenSeleccionado.AdjuntoFactura;
                                    break;                            
                                case "CanceladoVerificar":
                                case "Cancelado":
                                    estadoActualiza.Adjunto = OrdenSeleccionado.AdjuntoCancelado;
                                    break;
                            }
                        }
                        else
                        if (Convert.ToInt32(ordenEstado.IdEstado) == (Convert.ToInt32(OrdenSeleccionado.IdEstado) + 1))
                        {
                            estadoActualiza.StatusProgreso = Syncfusion.XForms.ProgressBar.StepStatus.InProgress;
                        }
                        else
                        {
                            estadoActualiza.StatusProgreso = Syncfusion.XForms.ProgressBar.StepStatus.NotStarted;
                        }

                    }

                }
            }

        }
        /// <summary>
        /// Método para salir de la pantalla
        /// </summary>
        /// <returns></returns>
        private async Task Cancel_CommandAsync()
        {
            EventoCancelar?.Invoke(true);
        }

        private void CalculateListViewHeight()
        {
            if (OrdenSeleccionado.OrdenDetalleCompras.Any())
            {
                int itemHeight = 0;
                switch (Xamarin.Forms.Device.Idiom)
                {
                    case TargetIdiom.Tablet:
                        itemHeight = 100;
                        break;
                    case TargetIdiom.Phone:
                        itemHeight = 60;
                        break;
                    default:
                        break;
                }
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    ListViewHeight = OrdenSeleccionado.OrdenDetalleCompras.Count() * itemHeight;
                });
            }
        }
        /// <summary>
        /// Método para devolver la fecha
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string devuelveFecha(string name)
        {
            string fecha = "";
            switch ((int)Enum.Parse(typeof(EstadoOrdenCompra), name))
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
        /// <summary>
        /// Método para buscar imágenes
        /// </summary>
        /// <returns></returns>
        public async Task BrowseGalleryAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "FacturaCompra",
                GuardarStorage = 0
            };
            galeria.Nombre = OrdenSeleccionado.NumeroOrden == "" ? galeria.Id : OrdenSeleccionado.NumeroOrden;
            _galeria = await generales.PickAndShowFile(galeria);
            await CargaImagen(2);
        }
        /// <summary>
        /// Método para abrir la cámara
        /// </summary>
        /// <returns></returns>
        private async Task OpenCameraAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "FacturaCompra",
                GuardarStorage = 0
            };
            galeria.Nombre = OrdenSeleccionado.NumeroOrden == "" ? galeria.Id : OrdenSeleccionado.NumeroOrden;
            _galeria = await generales.TakePhoto(galeria);
            await CargaImagen(1);
        }
        /// <summary>
        /// Método para remover imagen
        /// </summary>
        /// <returns></returns>
        private async Task RemoveImageAsync()
        {
            await CargaImagen(3);

        }
        /// <summary>
        /// Método para cargar imagen
        /// </summary>
        /// <param name="Origen"></param>
        /// <returns></returns>
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
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(OrdenSeleccionado.AdjuntoFactura));
                        return stream;
                    });
                    break;
                case 5:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(OrdenSeleccionado.AdjuntoCancelado));
                        return stream;
                    });
                    break;
            }
        }
        public async Task pagoVerificar()
        {
            OrdenSeleccionado.OrdenPago = !string.IsNullOrEmpty(OrdenSeleccionado.OrdenPagoSerializado) ? new ObservableCollection<OrdenPago>(JsonConvert.DeserializeObject<List<OrdenPago>>(OrdenSeleccionado.OrdenPagoSerializado)) : new ObservableCollection<OrdenPago>();

        }
        /// <summary>
        /// Método para calcular valores
        /// </summary>
        /// <param name="formapago"></param>
        /// <returns></returns>
        public async Task CalculaValores(string formapago,int tipoCancela)
        {
            IsBusy = true;
            try {
                List<TablaPago> detallePagar = new List<TablaPago>();
                OrdenSeleccionado.OrdenTablaPago= !string.IsNullOrEmpty(OrdenSeleccionado.OrdenTablaPagoSerializado) ? JsonConvert.DeserializeObject<ObservableCollection<TablaPago>>(OrdenSeleccionado.OrdenTablaPagoSerializado) : new ObservableCollection<TablaPago>();
                if (tipoCancela==1)
                {
                    detallePagar = OrdenSeleccionado.OrdenTablaPago.Where(x => x.Seleccionada == 1 && (x.EstadoCuota.ToUpper().Contains("PENDIE") || x.EstadoCuota.ToUpper().Contains("ABONA"))).ToList();
                }
                else
                {
                    detallePagar = OrdenSeleccionado.OrdenTablaPago.Where(x => x.Seleccionada == 1 && (x.EstadoCuota.ToUpper().Contains("VERIFI") )).ToList();
                }
                
                detallePagar.ForEach(i => { Cuotas = i.NumeroCuota + ","; });
                float montototal = detallePagar.Sum(y=>y.ValorFinal);
                ValorDescuento= detallePagar.Sum(y => y.ValorDescuento);
                ValorTotal = montototal;
                float pagoparcial= OrdenPagoSeleccionado!=null && OrdenPagoSeleccionado.Count()>0?OrdenPagoSeleccionado.Where(y=>!y.FormaCancelacion.Contains(formapago)).Sum(x => x.ValorFinal):0;
                float pago = OrdenPagoSeleccionado.Sum(x=>x.ValorFinal);
                ValorPagado = pago;
                float valorCambio = OrdenPagoSeleccionado.Sum(x => x.ValorCambio);
                ValorCambio = valorCambio;
                if (pagoparcial<montototal)
                {                                
                    float desctoPago = ValorDescuento;
                    /*OrdenSeleccionado.ValorCancelado = pago;
                    OrdenSeleccionado.ValorDescuento = desctoPago;*/
                    OrdenSeleccionado.Observaciones = pago<=0? string.Format(TextsTranslateManager.Translate("ObservacionVentaCanceladoPrevio"), OrdenSeleccionado.NombreCliente, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorTotal.FormatPrice(), montototal.FormatPrice(), desctoPago.FormatPrice()) : string.Format(TextsTranslateManager.Translate("ObservacionVentaCancelado"), OrdenSeleccionado.NombreCliente, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorTotal.FormatPrice(), pago.FormatPrice(), desctoPago.FormatPrice());
                }         
                else
                {
                    await App.Current.MainPage.DisplayAlert("Importante", "El valor a pagar ya está cubierto, favor revisar los valores", "OK");
                    OrdenPagoSeleccionado.Where(x => x.FormaCancelacion == formapago).FirstOrDefault().ValorPago = 0;                
                }
            }
            catch(Exception ex)
            {

            }
            IsBusy = false;
        }
        /// <summary>
        /// Método para evaluar la condicíón de venta normal o crédito
        /// </summary>
        public void EvaluaCondicion()
        {
            if (CondicionesVentaSeleccionado.Nombre.Contains("REDITO"))
            {
                IsVisibleCondicionesCredito = true;
                DiasCredito = OrdenSeleccionado.DiasCredito>0? OrdenSeleccionado.DiasCredito:0;
                ImporteCredito = OrdenSeleccionado.ImporteCredito > 0 ? OrdenSeleccionado.ImporteCredito : 0;
                FechaVencimiento= OrdenSeleccionado.DiasCredito > 0 ? FechaVencimiento.AddDays(DiasCredito) : FechaVencimiento;
            }
            else
            {
                IsVisibleCondicionesCredito = false;
            }            
        }

        /// <summary>
        /// Método para evaluar la forma de cancelación
        /// </summary>
        public void EvaluaFormaCancelacion()
        {
            llenaOrdenPago();
        }
        /// <summary>
        /// Método para llenar la orden de pago
        /// </summary>
        private void llenaOrdenPago()
        {
            try
            {
                var perfil = SettingsOnline.oAplicacion.PerfilConectado.ToUpper();
                if (OrdenSeleccionado.OrdenPago.Where(x => x.EstadoCancelado.ToUpper().Contains("VERIFICA")).Count() > 0
                    && !perfil.Contains("SITANTE")
                    && !perfil.Contains("IENTE"))
                {
                    PoseeDatos = true;
                    OrdenPagoSeleccionado = new ObservableCollection<OrdenPago>(OrdenSeleccionado.OrdenPago.Where(x => x.EstadoCancelado.ToUpper().Contains("VERIFICA")).ToList());
                    //FormaCancelacionSeleccionado1 = OrdenPagoSeleccionado.Count > 0 ? LFormaCancelacion.Where(y => y.Nombre.Contains(OrdenPagoSeleccionado.FirstOrDefault().FormaCancelacion)).ToList() : FormaCancelacionSeleccionado1;
                    if (OrdenPagoSeleccionado.FirstOrDefault().FormaCancelacion.ToUpper().Contains("CHE")
                       || OrdenPagoSeleccionado.FirstOrDefault().FormaCancelacion.ToUpper().Contains("TRA")
                       )
                    {
                        IsHiddenCondicionesCheque = false;                     
                    }
                    ValorPagado = OrdenPagoSeleccionado.Sum(x => x.ValorFinal);
                    IsVisibleCompartido = true;                    
                }
                else
                {

                    List<OrdenPago> itemNuevo = FormaCancelacionSeleccionado.Where(x => !OrdenPagoSeleccionado.Where(y => y.IdFormaCancelacion == x.Id).Any())
                                                                .Select(X => new OrdenPago
                                                                {
                                                                    Id = Generator.GenerateKey()
                                                                    ,
                                                                    IdOrden = OrdenSeleccionado.Id
                                                                    ,
                                                                    NumeroOrden = OrdenSeleccionado.NumeroOrden
                                                                    ,
                                                                    Cuotas = Cuotas
                                                                    ,
                                                                    InstitucionFinanciera = !(X.Nombre.ToUpper().Contains("CHE")) && !(X.Nombre.ToUpper().Contains("TRA")) ? "No Aplica" : OInstitucionFinanciera.FirstOrDefault()
                                                                    ,
                                                                    NumeroCheque = ""
                                                                    ,
                                                                    IdFormaCancelacion = X.Id
                                                                    ,
                                                                    FormaCancelacion = X.Nombre
                                                                    ,
                                                                    MontoPagoTotal = ValorTotal
                                                                    ,
                                                                    ValorPago = 0
                                                                }).ToList();
                    List<OrdenPago> itemInexistente = OrdenPagoSeleccionado.Where(x => !FormaCancelacionSeleccionado.Where(y => y.Id == x.IdFormaCancelacion).Any()).ToList();

                    var itemAnterior = OrdenPagoSeleccionado.ToList();
                    var itemFinal = itemAnterior.Where(x => !itemInexistente.Where(y => y.IdFormaCancelacion == x.IdFormaCancelacion).Any()).ToList();
                    if (itemNuevo != null)
                    {
                        itemFinal.AddRange(itemNuevo);
                    }
                    if (itemFinal.Where(x => x.FormaCancelacion.ToUpper().Contains("CHE")
                                        || x.FormaCancelacion.ToUpper().Contains("TRA")).Count() > 0)
                    {
                        IsHiddenCondicionesCheque = false;
                        IsVisibleCompartido = true;
                    }
                    else
                    {
                        IsHiddenCondicionesCheque = true;
                        IsVisibleCompartido = false;
                    }
                    OrdenPagoSeleccionado = new ObservableCollection<OrdenPago>(itemFinal);
                }
            }
            catch(Exception ex)
            {
                var y = ex.Message.ToString();
            }
        }
        
        private async Task actualizaOrdenPago(int instanciaVerificacion)
        {
            string estadoCanceladoPago = instanciaVerificacion == 1 ? "CANCELADO POR VERIFICAR" : "CANCELADO";
            var z = OrdenSeleccionado.OrdenPago != null ? OrdenSeleccionado.OrdenPago : new ObservableCollection<OrdenPago>();
            OrdenPagoSeleccionado.ForEach(x => x.EstadoCancelado=estadoCanceladoPago);
            if (z.Where(b1 => OrdenPagoSeleccionado.Any(bi => bi.Cuotas == b1.Cuotas
                                                        && bi.EstadoCancelado == "CANCELADO")).ToList().Count() > 0)
            {
                z.Where(b1 => OrdenPagoSeleccionado.Any(bi => bi.Cuotas == b1.Cuotas
                                                        && bi.EstadoCancelado == "CANCELADO")).ToList()
                                  .ForEach(b1 => b1.EstadoCancelado = "CANCELADO");
            }
            else
            {
                OrdenPagoSeleccionado.ForEach(x => z.Add(x));
            }
            OrdenSeleccionado.OrdenPago = z;
            OrdenSeleccionado.OrdenPagoSerializado= JsonConvert.SerializeObject(OrdenSeleccionado.OrdenPago);            
            float montototal = OrdenSeleccionado.ValorFinal;
            float pago = ValorPagado;
            if (montototal != pago && pago>0 && pago<montototal)
            {
                estadoCanceladoPago = instanciaVerificacion==1? "ABONADO POR VERIFICAR":"ABONADO";
            }
            if(instanciaVerificacion==2)
                OrdenSeleccionado.ValorCancelado = OrdenSeleccionado.ValorCancelado>0? OrdenSeleccionado.ValorCancelado+pago: OrdenSeleccionado.ValorCancelado;
            OrdenSeleccionado.EstadoCancelado = estadoCanceladoPago;
        }
        private async Task actualizaPago(int instanciaVerificacion)
        {            
            var fechaCancelado = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
            if (string.IsNullOrEmpty(OrdenPagoSeleccionado.FirstOrDefault().NumeroPago) || OrdenPagoSeleccionado.FirstOrDefault().NumeroPago == "0")
            {
                var secuencialPago = await generales.obtieneSecuenciales("NumeroPago");
                foreach (var item in OrdenPagoSeleccionado)
                {
                    item.NumeroPago = secuencialPago.SecuencialReciboIncrementado;
                    item.FechaCancelado = fechaCancelado;
                }
            }
            await actualizaOrdenPago(instanciaVerificacion);
        }
        private async Task actualizaTablaPago(int instanciaVerificacion)
        {
            string estadoCuota = instanciaVerificacion == 1 ? "CANCELADA POR VERIFICAR" : "CANCELADA";
            List<string> cuotas = Cuotas.Split(',').ToList();
            if (OrdenSeleccionado.OrdenTablaPago!=null)
            {
                var CuotasPagadas= from item in OrdenSeleccionado.OrdenTablaPago
                                   where cuotas.Contains(item.NumeroCuota.ToString())
                                   select item;
                float _cantidadCuotas = CuotasPagadas!=null?CuotasPagadas.Count():0;
                int _primeraCuota = CuotasPagadas.Min(x => x.NumeroCuota);
                int _ultimaCuota = CuotasPagadas.Max(x => x.NumeroCuota);
                float _valorPagoTotal = (float)Math.Round(ValorPagado, 2);
                float _valorCuota = 0;
                float _valorPagoCuota = 0;                
                float _valorSaldoCuota = 0;
                float _valorSaldoTotal = _valorPagoTotal;

                foreach (var x in CuotasPagadas)
                {
                    _valorCuota = x.ValorFinal;
                    if (_valorCuota <= _valorSaldoTotal)
                    {
                        _valorPagoCuota = _valorCuota;
                        _valorSaldoTotal -= _valorPagoCuota;
                        _valorSaldoCuota = 0;
                    }
                    else
                    {
                        _valorPagoCuota = _valorSaldoTotal;
                        _valorSaldoTotal -= 0;
                        _valorSaldoCuota = _valorCuota-_valorPagoCuota;
                    }
                    x.ValorPagado = _valorPagoCuota;
                    x.ValorSaldo = _valorSaldoCuota;
                    x.EstadoCuota = _valorSaldoCuota == 0 ? estadoCuota : "ABONADA";                    

                }
                OrdenSeleccionado.OrdenTablaPagoSerializado=JsonConvert.SerializeObject(OrdenSeleccionado.OrdenTablaPago);
            }
            
        }

        private void calculoInicial() {
            if (EntregarTodo)
            {
                OrdenDetalleCompraEntregadoSeleccionado.ForEach(y => y.cantidadEntrega = y.cantidadSaldoEntrega);
                IsHiddenEntregaTodo = true;
            }
            else
            {
                IsHiddenEntregaTodo = false;
                foreach (ItemEntrega item in JsonConvert.DeserializeObject<List<ItemEntrega>>(OrdenSeleccionado.OrdenDetalleCompraEntregadoSerializado))
                {
                    var a = OrdenDetalleCompraEntregadoSeleccionado.Where(x => x.Id == item.Id).FirstOrDefault();
                    a.cantidadEntregada = item.cantidadEntregada;
                    a.cantidadEntrega = 0;
                }
            }
            //EventoGrid?.Invoke(true);
        }
        /*
        public async Task<bool> CalculaValoresEntrega(ItemEntrega data)
        {
            bool retorno = true;        
            var cantidadTotal = data.cantidad;
            var cantidadEntrega = data.cantidadEntrega;
            var cantidadEntregada = data.cantidadEntregada;
            var cantidadSaldoEntrega = data.cantidadSaldoEntrega;

            var item = OrdenDetalleCompraEntregadoSeleccionado.Where(x => x.Id == data.Id).FirstOrDefault();
            if (cantidadEntrega > cantidadSaldoEntrega)
            {
                cantidadEntrega = cantidadSaldoEntrega;
            }
            cantidadSaldoEntrega = cantidadSaldoEntrega - cantidadEntrega;
            cantidadEntregada = cantidadEntregada + cantidadEntrega;
            item.cantidadEntrega = cantidadEntrega;
            item.cantidadEntregada = cantidadEntregada;
            item.cantidadSaldoEntrega = cantidadSaldoEntrega;
            if (cantidadEntrega == 0)
                retorno = false;
            return retorno;
        }
        */

        #endregion 6.Métodos Generales

    }
}
