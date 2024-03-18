using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using DataModel.DTO.Iteraccion;
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

namespace Escant_App.ViewModels.Ventas
{
    public class PopupValidaViewModel : ViewModelBase
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
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase
        private bool _aplicaDescuento;
        private bool _confirmadaTransaccion;
        private DateTime _fechaVencimiento;
        private Orden _ordenSeleccionado;
        private List<OrdenDetalleCompra> _ordenDetalleCompraSeleccionado;
        private List<OrdenEstado> _ordenEstadoSeleccionado;
        private List<Catalogo> _lCondicionesVenta;
        private List<Catalogo> _lTipoDescuentoVenta;
        private Catalogo _condicionesVentaSeleccionado;
        private Catalogo _tipoDescuentoSeleccionado;
        private List<Catalogo> _lFormaCancelacion;
        private List<Catalogo> _formaCancelacionSeleccionado;
        private List<Catalogo> _formaCancelacionSeleccionado1;
        private ObservableCollection<string> _oInstitucionFinanciera = new ObservableCollection<string>();
        #endregion 1.2 Variables de Instancia de Objetos para la Clase
        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        private int _listViewHeight;
        private bool _esVisibleAdjunto;
        private bool _isHiddenCondicionesCheque;
        private bool _isEnableValorDescuento;
        private bool _isVisibleCondicionesPago;
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
        public bool _isVisibleFacturacion;
        public bool _isVisibleCompartido;
        public bool _isVisibleCondicionesCredito;
        public bool _isVisibleCondicionesDescuento;
        public float _diasCredito;
        public float _importeCredito;
        #endregion 1.4 Variables de Control de Datos
        #region 1.4 Variables de Control de Errores
        #endregion 1.4 Variables de Control de Errores
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
        public List<Catalogo> LTipoDescuentoVenta
        {
            get
            {
                return _lTipoDescuentoVenta;
            }
            set
            {
                _lTipoDescuentoVenta = value;
                RaisePropertyChanged(() => LTipoDescuentoVenta);
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
        public Catalogo TipoDescuentoSeleccionado
        {
            get
            {
                return _tipoDescuentoSeleccionado;
            }
            set
            {
                _tipoDescuentoSeleccionado = value;
                RaisePropertyChanged(() => TipoDescuentoSeleccionado);
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
        public bool AplicaDescuento
        {
            get
            {
                return _aplicaDescuento;
            }
            set
            {
                _aplicaDescuento = value;
                RaisePropertyChanged(() => AplicaDescuento);
                eventoAplicaDescuento();
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
        #endregion 2.InstanciaAsignaVariablesControles
        #region 3.Constructor
        public PopupValidaViewModel(IServicioSeguridad_Usuario seguridadServicio
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
            generales = new Generales.Generales(_catalogoServicio, _settingsServicio, _servicioFirestore);
            generaDocumentos = new Generales.GeneraDocumentos();
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
                                OrdenSeleccionado.Observaciones = string.Format(TextsTranslateManager.Translate("ObservacionVenta"), OrdenSeleccionado.NombreCliente, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorTotal.FormatPrice());
                                break;
                            case "Recibido":
                                var cantpro = OrdenSeleccionado.OrdenDetalleComprasRecibido != null ? OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(y => !y.TipoDescuento.Contains("Aplica")).Sum(x => x.Cantidad) : 0;
                                var descto = OrdenSeleccionado.OrdenDetalleComprasRecibido != null ? OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(y => !y.TipoDescuento.Contains("Aplica")).Sum(x => x.ValorDescuento) : 0;
                                OrdenSeleccionado.Observaciones = string.Format(TextsTranslateManager.Translate("ObservacionCompraRecibido"), OrdenSeleccionado.NombreProveedor, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorTotal.FormatPrice(), cantpro, descto.FormatPrice());
                                break;
                            case "Cancelado":
                                CalculaValores("");
                                break;
                        }
                    }

                    inicia();
                    BindingData();
                    CalculateListViewHeight();
                }
            }
            catch (NullReferenceException ex)
            {

            }
            IsBusy = false;
        }
        public async Task CalculaValores(string formapago)
        {
            IsBusy = true;
            try
            {
                float montototal = OrdenSeleccionado.ValorFinal;
                float pagoparcial = OrdenSeleccionado.OrdenPago != null && OrdenSeleccionado.OrdenPago.Count() > 0 ? OrdenSeleccionado.OrdenPago.Where(y => !y.FormaCancelacion.Contains(formapago)).Sum(x => x.ValorFinal) : 0;
                if (pagoparcial < montototal)
                {
                    float pago = OrdenSeleccionado.OrdenPago.Sum(x => x.ValorFinal);
                    float desctoPago = OrdenSeleccionado.ValorDescuento;
                    OrdenSeleccionado.ValorCancelado = pago;
                    OrdenSeleccionado.ValorDescuento = desctoPago;
                    OrdenSeleccionado.Observaciones = pago <= 0 ? string.Format(TextsTranslateManager.Translate("ObservacionVentaCanceladoPrevio"), OrdenSeleccionado.NombreCliente, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorTotal.FormatPrice(), montototal.FormatPrice(), desctoPago.FormatPrice()) : string.Format(TextsTranslateManager.Translate("ObservacionVentaCancelado"), OrdenSeleccionado.NombreCliente, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorTotal.FormatPrice(), pago.FormatPrice(), desctoPago.FormatPrice());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Importante", "El valor a pagar ya está cubierto, favor revisar los valores", "OK");
                    OrdenSeleccionado.OrdenPago.Where(x => x.FormaCancelacion == formapago).FirstOrDefault().ValorPago = 0;
                }
            }
            catch (Exception ex)
            {

            }
            IsBusy = false;
        }

        private void inicia()
        {
            try
            {
                AplicaDescuento = false;
                ConfirmadaTransaccion = false;
                IsVisibleCondicionesCredito = false;
                IsVisibleCondicionesPago = false;
                IsVisibleCondicionesDescuento = false;
                IsHiddenCondicionesCheque = true;
                IsVisibleFacturacion = false;
                IsVisibleIngresado = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Ingres") ? true : false;
                IsVisibleRecibido = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Recibi") ? true : false;
                IsVisibleCancelado = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Cancela") ? true : false;
                IsVisibleCompartido = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Recibi") ? true : false;
                if (IsVisibleIngresado)
                {
                    cargaCombo(1);
                }
                if (IsVisibleCancelado)
                {
                    cargaCombo(2);
                }
            }
            catch (NullReferenceException ex)
            {

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
            catch (NullReferenceException ex)
            {

            }
            //EsVisibleAdjunto = !string.IsNullOrEmpty(OrdenSeleccionado.IdGaleria) ? true : false;
            //OrdenSeleccionado.AdjuntoFactura= !String.IsNullOrEmpty(OrdenSeleccionado.AdjuntoFactura)? OrdenSeleccionado.AdjuntoFactura:
        }

        public void EvaluaCondicion()
        {
            if (CondicionesVentaSeleccionado.Nombre.Contains("REDITO"))
            {
                IsVisibleCondicionesCredito = true;
                DiasCredito = OrdenSeleccionado.DiasCredito > 0 ? OrdenSeleccionado.DiasCredito : 0;
                ImporteCredito = OrdenSeleccionado.ImporteCredito > 0 ? OrdenSeleccionado.ImporteCredito : 0;
                FechaVencimiento = OrdenSeleccionado.DiasCredito > 0 ? FechaVencimiento.AddDays(DiasCredito) : FechaVencimiento;
            }
            else
            {
                IsVisibleCondicionesCredito = false;
            }
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

        private async void cargaCombo(int tipo)
        {
            switch (tipo)
            {
                case 1:
                    LCondicionesVenta = SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "condicionesventa" && y.EsCatalogo == 0).ToList();
                    break;
                case 2:
                    LFormaCancelacion = SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "formacancelacion" && y.EsCatalogo == 0).ToList();
                    FormaCancelacionSeleccionado1 = LFormaCancelacion.Where(y => y.Nombre.ToUpper().Contains("EFEC")).ToList();
                    //FormaCancelacionSeleccionado = LFormaCancelacion.Where(y => y.Nombre.Contains("EFEC")).ToList();
                    OInstitucionFinanciera = new ObservableCollection<string>(SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "institucionfinanciera"
                                                                                                            && y.EsCatalogo == 0).Select(x => x.Nombre).ToList());
                    LTipoDescuentoVenta = SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "tipodescuento" && y.EsCatalogo == 0).ToList();
                    TipoDescuentoSeleccionado = LTipoDescuentoVenta.Where(x => x.Nombre.Contains("0%")).FirstOrDefault();
                    break;
            }

        }

        private async Task Ok_CommandAsync()
        {
            string informedMessage = "";
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
                await actualizaEstadoOrdenAsync();
                switch (OrdenSeleccionado.CodigoEstado)
                {
                    case "Ingresado":
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
                    case "Cancelado":
                        OrdenSeleccionado.AdjuntoCancelado = _galeria != null ? _galeria.Image : OrdenSeleccionado.AdjuntoCancelado;
                        OrdenSeleccionado.ValorCancelado = ValorCancelado;
                        break;
                }

                OrdenSeleccionado.OrdenEstadoCompraSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.OrdenEstado);

                var resultado = await _switchCompras.GuardaRegistro_Orden(OrdenSeleccionado, estaConectado, existe);
                if (OrdenSeleccionado.CodigoEstado.Contains("Recibido") && OrdenSeleccionado.OrdenDetalleComprasRecibido.Count > 0)
                {
                    var resultado1 = await _switchProductos.GuardaRegistro_StockOrden(OrdenSeleccionado, estaConectado, false);
                }
                /*if (resultado != null)
                {
                    //2.2 Almacenar la Orden detalle Compra                    
                    var resultado1 = await _switchCompras.GuardaRegistro_ListaOrdenDetalleCompra( OrdenDetalleCompraSeleccionado, estaConectado, existe);
                    //2.3 Almacenar la Orden Estado
                    var resultado2 = await _switchCompras.GuardaRegistro_ListaOrdenEstado(OrdenEstadoSeleccionado, estaConectado, existe);

                }*/
                informedMessage = resultado.mensaje;
                DialogService.ShowToast(informedMessage);
                MessagingCenter.Send<Orden>(OrdenSeleccionado, "OnDataSourcePopupActualizaOrdenVentasChanged");
                await NavigationService.RemovePopupAsync();
            }
            IsBusy = false;

        }
        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales

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
                            if (DiasCredito > 0)
                            {
                                retorno = true;
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Importante", "Debe ingresar a cuantos días se vence el Crédito", "OK");
                            }
                        }
                        else
                        {
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
                    case "Cancelado":
                        if (OrdenSeleccionado.ValorCancelado > 0)
                        {
                            if (!FuenteImagenRegistro.IsEmpty)
                            {
                                retorno = true;
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Importante", "Debe adjuntar el respaldo de pago ", "OK");
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Importante", "Debe registrar el valor Cancelado", "OK");
                        }
                        break;
                    default:
                        retorno = true;
                        break;
                }
            }


            return retorno;
        }

        private async Task Cancel_CommandAsync()
        {
            MessagingCenter.Send<string>(string.Empty, "OnDataSourcePopupActualizaOrdenVentasChanged");
            await NavigationService.RemovePopupAsync();
        }

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

        public void EvaluaTipoDescuento()
        {
            if (TipoDescuentoSeleccionado.Nombre.ToUpper().Contains("TROS"))
            {
                EsEnableValorDescuento = true;
            }
            else
            {
                EsEnableValorDescuento = false;
                calculoDescuento(1);
            }
        }
        public void calculoDescuento(int tipo)
        {
            float valorPago = OrdenSeleccionado.ValorTotal;
            float porcentajeDescuento = 0;
            float valorDescuento = 0;
            switch (tipo)
            {
                case 1:
                    porcentajeDescuento = TipoDescuentoSeleccionado.ValorConstanteNumerico;
                    valorDescuento = valorPago - (valorPago - ((porcentajeDescuento * valorPago) / 100));
                    ValorDescuento = valorDescuento;
                    break;
                case 2:
                    valorDescuento = ValorDescuento;
                    break;
            }
            OrdenSeleccionado.ValorDescuento = ValorDescuento;
            CalculaValores("");
        }
        public void EvaluaFormaCancelacion()
        {
            llenaOrdenPago();
        }
        private void llenaOrdenPago()
        {
            try
            {
                List<OrdenPago> itemNuevo = FormaCancelacionSeleccionado.Where(x => !OrdenSeleccionado.OrdenPago.Where(y => y.IdFormaCancelacion == x.Id).Any())
                                                            .Select(X => new OrdenPago
                                                            {
                                                                IdOrden = OrdenSeleccionado.Id
                                                                ,
                                                                NumeroOrden = OrdenSeleccionado.NumeroOrden
                                                                ,
                                                                InstitucionFinanciera = !(X.Nombre.ToUpper().Contains("CHE")) && !(X.Nombre.ToUpper().Contains("TRA")) ? "No Aplica" : OInstitucionFinanciera.FirstOrDefault()
                                                                ,
                                                                NumeroCheque = ""
                                                                ,
                                                                IdFormaCancelacion = X.Id
                                                                ,
                                                                FormaCancelacion = X.Nombre
                                                                ,
                                                                MontoPagoTotal = OrdenSeleccionado.ValorFinal
                                                                ,
                                                                ValorPago = 0
                                                            }).ToList();
                List<OrdenPago> itemInexistente = OrdenSeleccionado.OrdenPago.Where(x => !FormaCancelacionSeleccionado.Where(y => y.Id == x.IdFormaCancelacion).Any()).ToList();

                var itemAnterior = OrdenSeleccionado.OrdenPago.ToList();
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
                OrdenSeleccionado.OrdenPago = new ObservableCollection<OrdenPago>(itemFinal);
            }
            catch (Exception ex)
            {
                var y = ex.Message.ToString();
            }
        }
        private async void eventoAplicaDescuento()
        {
            if (AplicaDescuento)
            {
                IsVisibleCondicionesDescuento = true;
            }
            else
            {
                IsVisibleCondicionesDescuento = false;
                if(LTipoDescuentoVenta!=null)
                    TipoDescuentoSeleccionado = LTipoDescuentoVenta.Where(x => x.Nombre.Contains("0%")).FirstOrDefault();
            }
        }
        #endregion 6.Métodos Generales

    }
}
