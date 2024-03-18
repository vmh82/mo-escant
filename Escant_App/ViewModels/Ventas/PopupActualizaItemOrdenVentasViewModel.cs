using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Productos;
using DataModel.Enums;
using DataModel.Helpers;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using Escant_App.ViewModels.Base;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Ventas
{
    public class PopupActualizaItemOrdenVentasViewModel:ViewModelBase
    {
        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración
        private SwitchProductos _switchProductos;
        private readonly IServicioProductos_Item _itemServicio;
        private Generales.Generales generales;
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase        
        private Orden _ordenSeleccionado;
        private OrdenDetalleCompra _ordenDetalleCompraSeleccionado;
        private List<Precio> _lPrecios;
        private Precio _precioSeleccionado;
        private Precio _precioMinimo;
        private List<ItemStock> _lItemStock;
        private List<FechaItem> _lFecha;
        private FechaItem _fechaSeleccionado;
        private float _valorPeriodoIncremento;
        private string _periodoIncremento;
        private float _valorIncremento;
        private float _incremento;
        private float _cantidad;
        private float _costo;
        private float _existenteFecha;
        private float _porcentajeImpuesto;
        private string _idTipoAplicacionImpuesto;
        private string _tipoAplicacionImpuesto;
        private string _fechaLabel;
        private string _impuestoLabel;
        private float _valorImpuesto;
        private float _precioCompra;
        private string _periodoPagoLabel;
        private List<Catalogo> _lPeriodoPago;
        private List<Catalogo> _lTipoDescuentoVenta;
        private Catalogo _periodoPagoSeleccionado;
        private Catalogo _tipoDescuentoSeleccionado;
        private string _fechaPrimerPago;
        private string _minimaFechaPago;
        private string _maximaFechaPago;
        #endregion 1.2 Variables de Instancia de Objetos para la Clase
        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        private bool _isVisibleCondicionesPeriodoPago;        
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #region 1.4 Variables de Control de Datos
        public CultureInfo Cultura => Escant_App.AppSettings.Settings.CustomCurrencySymbol == "$" ? new System.Globalization.CultureInfo("es-EC") : new System.Globalization.CultureInfo("en-US");
        private bool _esEditableCosto;
        public bool _esActivadaCalculo = false;
        public string _fuenteImagenPago;
        public bool _aplicaDescuento;
        private bool _isVisibleCondicionesDescuento;
        private bool _esEnableValorDescuento;
        private float _valorDescuento;
        private bool _esEnableUnidad;
        private bool _esHabilitadoPeriodoPago;
        #endregion 1.4 Variables de Control de Datos
        #region 1.5 Variables de Control de Errores
        #endregion 1.5 Variables de Control de Errores
        #region 1.6 Acciones
        public Action<bool> EventoGuardar { get; set; }
        public Action<bool> EventoCancelar { get; set; }
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
        public OrdenDetalleCompra OrdenDetalleCompraSeleccionado
        {
            get => _ordenDetalleCompraSeleccionado;
            set
            {
                _ordenDetalleCompraSeleccionado = value;
                RaisePropertyChanged(() => OrdenDetalleCompraSeleccionado);
            }

        }
        public List<Precio> LPrecios
        {
            get => _lPrecios;
            set
            {
                _lPrecios = value;
                RaisePropertyChanged(() => LPrecios);
            }

        }
        public Precio PrecioSeleccionado
        {
            get => _precioSeleccionado;
            set
            {
                _precioSeleccionado = value;
                RaisePropertyChanged(() => PrecioSeleccionado);
            }

        }
        public Precio PrecioMinimo
        {
            get => _precioMinimo;
            set
            {
                _precioMinimo = value;
                RaisePropertyChanged(() => PrecioMinimo);
            }

        }

        public List<ItemStock> LItemStock
        {
            get => _lItemStock;
            set
            {
                _lItemStock = value;
                RaisePropertyChanged(() => LItemStock);
            }

        }

        public List<FechaItem> LFecha
        {
            get => _lFecha;
            set
            {
                _lFecha = value;
                RaisePropertyChanged(() => LFecha);
            }

        }
        public FechaItem FechaSeleccionado
        {
            get => _fechaSeleccionado;
            set
            {
                _fechaSeleccionado = value;
                RaisePropertyChanged(() => FechaSeleccionado);
            }

        }

        public float Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                RaisePropertyChanged(() => Cantidad);
            }

        }
        public float Costo
        {
            get => _costo;
            set
            {
                _costo = value;
                RaisePropertyChanged(() => Costo);
            }

        }
        public float ExistenteFecha
        {
            get => _existenteFecha;
            set
            {
                _existenteFecha = value;
                RaisePropertyChanged(() => ExistenteFecha);
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
        public string PeriodoIncremento
        {
            get => _periodoIncremento;
            set
            {
                _periodoIncremento = value;
                RaisePropertyChanged(() => PeriodoIncremento);
            }

        }
        public float ValorPeriodoIncremento
        {
            get => _valorPeriodoIncremento;
            set
            {
                _valorPeriodoIncremento = value;
                RaisePropertyChanged(() => ValorPeriodoIncremento);
            }

        }
        public float Incremento
        {
            get => _incremento;
            set
            {
                _incremento = value;
                RaisePropertyChanged(() => Incremento);
            }

        }
        public float PrecioCompra
        {
            get => _precioCompra;
            set
            {
                _precioCompra = value;
                RaisePropertyChanged(() => PrecioCompra);
            }

        }
        public string PeriodoPagoLabel
        {
            get => _periodoPagoLabel;
            set
            {
                _periodoPagoLabel = value;
                RaisePropertyChanged(() => PeriodoPagoLabel);
            }

        }
        public List<Catalogo> LPeriodoPago
        {
            get => _lPeriodoPago;
            set
            {
                _lPeriodoPago = value;
                RaisePropertyChanged(() => LPeriodoPago);
            }

        }
        public Catalogo PeriodoPagoSeleccionado
        {
            get => _periodoPagoSeleccionado;
            set
            {
                _periodoPagoSeleccionado = value;
                RaisePropertyChanged(() => PeriodoPagoSeleccionado);
            }

        }
        public string FechaPrimerPago
        {
            get => _fechaPrimerPago;
            set
            {
                _fechaPrimerPago = value;
                RaisePropertyChanged(() => FechaPrimerPago);
            }

        }
        
        public bool EsEditableCosto
        {
            get => _esEditableCosto;
            set
            {
                _esEditableCosto = value;
                RaisePropertyChanged(() => EsEditableCosto);
            }

        }
        public bool EsHabilitadoPeriodoPago
        {
            get => _esHabilitadoPeriodoPago;
            set
            {
                _esHabilitadoPeriodoPago = value;
                RaisePropertyChanged(() => EsHabilitadoPeriodoPago);
            }

        }
        public string FechaLabel
        {
            get => _fechaLabel;
            set
            {
                _fechaLabel = value;
                RaisePropertyChanged(() => FechaLabel);
            }
        }
        public string ImpuestoLabel
        {
            get => _impuestoLabel;
            set
            {
                _impuestoLabel = value;
                RaisePropertyChanged(() => ImpuestoLabel);
            }
        }
        public float PorcentajeImpuesto
        {
            get => _porcentajeImpuesto;
            set
            {
                _porcentajeImpuesto = value;
                RaisePropertyChanged(() => PorcentajeImpuesto);
            }
        }
        public string TipoAplicacionImpuesto
        {
            get => _tipoAplicacionImpuesto;
            set
            {
                _tipoAplicacionImpuesto = value;
                RaisePropertyChanged(() => TipoAplicacionImpuesto);
            }
        }
        public string IdTipoAplicacionImpuesto
        {
            get => _idTipoAplicacionImpuesto;
            set
            {
                _idTipoAplicacionImpuesto = value;
                RaisePropertyChanged(() => IdTipoAplicacionImpuesto);
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
        public bool IsVisibleCondicionesPeriodoPago
        {
            get => _isVisibleCondicionesPeriodoPago;
            set
            {
                _isVisibleCondicionesPeriodoPago = value;
                RaisePropertyChanged(() => IsVisibleCondicionesPeriodoPago);
            }
        }
        public string MinimaFechaPago
        {
            get => _minimaFechaPago;
            set
            {
                _minimaFechaPago = value;
                RaisePropertyChanged(() => MinimaFechaPago);
            }
        }
        public string MaximaFechaPago
        {
            get => _maximaFechaPago;
            set
            {
                _maximaFechaPago = value;
                RaisePropertyChanged(() => MaximaFechaPago);
            }
        }
        public string FuenteImagenPago
        {
            get => _fuenteImagenPago;
            set
            {
                if (this._fuenteImagenPago == value) return;

                _fuenteImagenPago = value;
                RaisePropertyChanged(() => FuenteImagenPago);
            }
            
        }
        public bool AplicaDescuento
        {
            get => _aplicaDescuento;
            set
            {
                _aplicaDescuento = value;
                RaisePropertyChanged(() => AplicaDescuento);
                eventoAplicaDescuento();
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
        public bool EsEnableValorDescuento
        {
            get => _esEnableValorDescuento;
            set
            {
                _esEnableValorDescuento = value;
                RaisePropertyChanged(() => EsEnableValorDescuento);
            }

        }
        public bool EsEnableUnidad
        {
            get => _esEnableUnidad;
            set
            {
                _esEnableUnidad = value;
                RaisePropertyChanged(() => EsEnableUnidad);
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

        public List<Catalogo> LTipoDescuentoVenta
        {
            get => _lTipoDescuentoVenta;
            set
            {
                _lTipoDescuentoVenta = value;
                RaisePropertyChanged(() => LTipoDescuentoVenta);
            }

        }
        public Catalogo TipoDescuentoSeleccionado
        {
            get => _tipoDescuentoSeleccionado;
            set
            {
                _tipoDescuentoSeleccionado = value;
                RaisePropertyChanged(() => TipoDescuentoSeleccionado);
            }

        }

        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        public PopupActualizaItemOrdenVentasViewModel(IServicioSeguridad_Usuario seguridadServicio
                                                     ,IServicioProductos_Item itemServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);

            _itemServicio = itemServicio;
            _switchProductos = new SwitchProductos(_itemServicio);
            generales = new Generales.Generales();
        }
        #endregion 3.Constructor

        #region 4.DefinicionMetodos
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());
        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        /// <summary>
        /// Método de inicio de la pantalla, ejecuta métodos: inicia y llena info
        /// </summary>
        /// <param name="navigationData"></param>
        /// <returns></returns>
        #region 5.1 InitializaAsync        
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is OrdenDetalleCompra item && item != null)
            {
                OrdenDetalleCompraSeleccionado = item;
                await inicia();                
                llenainfo(); 
            }
            IsBusy = false;
        }
        #endregion 5.1 InitializaAsync
        /// <summary>
        /// Procesos que se ejecutan luego de recibir el parámetro en la apertura de la pantalla
        /// Métodos : cargaFechaPago,cargaPrecios,cargaCombo,consultaDetalleStock
        /// </summary>
        /// <returns></returns>
        #region 5.1.1 Inicia
        public async Task inicia()
        {
            AplicaDescuento = false;
            IsVisibleCondicionesDescuento = false;
            IsVisibleCondicionesPeriodoPago = string.IsNullOrEmpty(OrdenDetalleCompraSeleccionado.IdPeriodoServicio) ? false : true;
            EsEnableUnidad = OrdenDetalleCompraSeleccionado.EsParticionable == 1 ? true : false;
            EsEditableCosto = OrdenDetalleCompraSeleccionado.EsParticionable == 1 ? false : true;
            await cargaFechaPago();
            await cargaPrecios();            
            await cargaCombo(2);
            await consultaDetalleStock();
        }
        #endregion 5.1.1 Inicia
        /// <summary>
        /// Método para cargar los períodos de Pago
        /// </summary>
        /// <returns></returns>
        #region 5.1.1.1 cargaFechaPago
        public async Task cargaFechaPago()
        {
            FuenteImagenPago = "arena_inicial";
            LPeriodoPago = SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "periodopago" && y.EsCatalogo == 0).ToList();
            if (!string.IsNullOrEmpty(OrdenDetalleCompraSeleccionado.IdPeriodoPago))
            {
                PeriodoPagoSeleccionado = LPeriodoPago != null && LPeriodoPago.Count > 0 ? LPeriodoPago.Where(x => x.Id == OrdenDetalleCompraSeleccionado.IdPeriodoPago).FirstOrDefault() : new Catalogo();
            }
            else
            {
                PeriodoPagoSeleccionado = LPeriodoPago != null && LPeriodoPago.Count > 0 ? (!string.IsNullOrEmpty(OrdenDetalleCompraSeleccionado.PeriodoServicio) && OrdenDetalleCompraSeleccionado.PeriodoServicio.ToUpper().Contains("AS") ? LPeriodoPago.Where(x => x.Nombre.ToUpper().Contains("NO APL")).FirstOrDefault() : LPeriodoPago.FirstOrDefault()) : new Catalogo();
                EsHabilitadoPeriodoPago = string.IsNullOrEmpty(OrdenDetalleCompraSeleccionado.PeriodoServicio) || OrdenDetalleCompraSeleccionado.PeriodoServicio.ToUpper().Contains("AS") ? false : true;
            }
        }
        #endregion 5.1.1.1 cargaFechaPago

        /// <summary>
        /// Método para cargar los precios del ítem o servicio que se está modificando
        /// </summary>
        /// <returns></returns>
        #region 5.1.1.2 cargaPrecios
        public async Task cargaPrecios()
        {
            LPrecios = SettingsOnline.oLPrecio.Where(x => x.IdItem == OrdenDetalleCompraSeleccionado.IdItem
                                                    && x.EstaActivo == 1).OrderByDescending(y => y.ValorConversion).ToList();
            EsEditableCosto = LPrecios.Count() > 1 ? false : true;
            if (OrdenDetalleCompraSeleccionado.ValorConversionMinimaUnidad == 0)
            {
                PrecioMinimo = LPrecios.OrderBy(x => x.ValorConversion).FirstOrDefault();
            }
            else
            {
                PrecioMinimo = LPrecios.Where(x => x.ValorConversion == OrdenDetalleCompraSeleccionado.ValorConversionMinimaUnidad).FirstOrDefault();
            }
            OrdenDetalleCompraSeleccionado.ValorConversionMinimaUnidad = PrecioMinimo.ValorConversion;
        }
        #endregion 5.1.1.2 cargaPrecios

        /// <summary>
        /// Método para cargar los combos TipoDEscuentoVenta
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        #region 5.1.1.3 cargaCombo
        private async Task cargaCombo(int tipo)
        {
            switch (tipo)
            {
                case 2:
                    LTipoDescuentoVenta = SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "tipodescuento" && y.EsCatalogo == 0).ToList();
                    TipoDescuentoSeleccionado = LTipoDescuentoVenta.Where(x => x.Nombre.Contains("0%")).FirstOrDefault();
                    break;
            }

        }
        #endregion 5.1.1.3 cargaCombo

        /// <summary>
        /// Método para consultar el detalle del stock del ítem o Servicio
        /// Almacena: LFecha, FechaSeleccionado
        /// </summary>
        /// <returns></returns>
        #region 5.1.1.4 consultaDetalleStock
        public async Task consultaDetalleStock()
        {            
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var itemProducto = (await _switchProductos.ConsultaItems(new DataModel.DTO.Productos.Item() { Id = OrdenDetalleCompraSeleccionado.IdItem }, estaConectado)).FirstOrDefault();
            if (itemProducto.PoseeStock == 1)
            {
                var retornoStock = !string.IsNullOrEmpty(itemProducto.ItemStockRegistradoSerializado) ? new ObservableCollection<ItemStock>(JsonConvert.DeserializeObject<List<ItemStock>>(itemProducto.ItemStockRegistradoSerializado)) : new ObservableCollection<ItemStock>();
                LItemStock = retornoStock.Where(x => x.IdItem == OrdenDetalleCompraSeleccionado.IdItem
                                                        && x.CantidadSaldoFinal > 0).OrderBy(y => y.FechaVencimiento).ToList();
                if (itemProducto.ValorIncremento > 0)
                {
                    LFecha = LItemStock.GroupBy(x => new { x.FechaIngreso, x.CantidadSaldoFinal })
                                        .Select(c => new
                                                    FechaItem()
                                        {
                                            Fecha = c.Key.FechaIngreso,
                                            Cantidad = c.Sum(p => p.CantidadSaldoFinal)
                                        }).ToList();
                    FechaLabel = "Fecha Ingreso";                    
                }
                else
                {
                    LFecha = LItemStock.GroupBy(x => new { x.FechaVencimiento, x.CantidadSaldoFinal })
                                    .Select(c => new
                                                FechaItem()
                                    {
                                        Fecha = c.Key.FechaVencimiento,
                                        Cantidad = c.Sum(p => p.CantidadSaldoFinal)
                                    }).ToList();
                    FechaLabel = "Fecha Vencimiento";                    
                }
                /*.Select(c => new
                       {
                           Fecha = c.Key,
                           Total = c.Sum(p => p.CantidadSaldoFinal)
                       });*/

            }
            else
            {
                LFecha = new List<FechaItem>();
                LFecha.Add(new FechaItem()
                                    {
                                        Fecha = obtieneFechaServicio(),
                                        Cantidad = OrdenDetalleCompraSeleccionado.Cantidad
                                    });
                FechaLabel = "Fecha Vencimiento";
                ValorIncremento = 0;
                PeriodoIncremento = "";
                ValorPeriodoIncremento = 0;
            }
            FechaSeleccionado = LFecha.FirstOrDefault();
            PrecioCompra = itemProducto.PrecioCompra;
            OrdenDetalleCompraSeleccionado.FechaIngresoItem = FechaSeleccionado.Fecha;

        }
        #endregion 5.1.1.4 consultaDetalleStock

        /// <summary>
        /// Método para llenar información predefinida
        /// Método ejecuta: calculaLimiteFechas
        /// </summary>
        #region 5.1.2 llenainfo
        public void llenainfo()
        {
            try { 
            calculaLimiteFechas(1);
            _esActivadaCalculo = true;
            PrecioSeleccionado = LPrecios.Where(x => x.UnidadConvertida == OrdenDetalleCompraSeleccionado.Unidad).FirstOrDefault();
            Cantidad = OrdenDetalleCompraSeleccionado.Cantidad;
            Costo = OrdenDetalleCompraSeleccionado.ValorCompra;
            }
            catch(Exception ex)
            {

            }
        }
        #endregion 5.1.2 llenainfo
        /// <summary>
        /// Calcula Límite de Fechas de Pago
        /// </summary>
        /// <param name="tipo"></param>
        #region 5.1.2.1 calculaLimiteFechas
        public void calculaLimiteFechas(int tipo)
        {
            if (IsVisibleCondicionesPeriodoPago)
            {
                MinimaFechaPago = DateTime.Now.ToString("dd/MM/yyyy");
                MaximaFechaPago = obtieneFechaPago();
                if (tipo == 2)
                {
                    if (FuenteImagenPago == "arena_inicial")
                    {
                        FuenteImagenPago = "arena_final";
                        FechaPrimerPago = MaximaFechaPago;
                    }
                    else
                    {
                        FuenteImagenPago = "arena_inicial";
                        FechaPrimerPago = MinimaFechaPago;
                    }
                }
                else
                {
                    FechaPrimerPago = MinimaFechaPago;
                }
            }
            else
            {
                FechaPrimerPago = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        #endregion 5.1.2.1 calculaLimiteFechas
        public string obtieneFechaServicio()
        {
            string retorno = DateTime.Now.ToString("dd/MM/yyyy");
            if(IsVisibleCondicionesPeriodoPago)
            {
                switch(OrdenDetalleCompraSeleccionado.PeriodoServicio)
                {
                    case string a when OrdenDetalleCompraSeleccionado.PeriodoServicio.ToString().ToUpper().Contains("AÑO"):
                        var y = Convert.ToDateTime(retorno).AddYears((int)OrdenDetalleCompraSeleccionado.CantidadPeriodoServicio);
                        retorno = y.ToString("dd/MM/yyyy");
                        break;
                }
            }            
            return retorno;
        }
 

        public async Task almacenaItemEntrega()
        {
            ObservableCollection<ItemEntrega> devuelveStockCancelado = new ObservableCollection<ItemEntrega>();
            if (OrdenDetalleCompraSeleccionado.EsServicio== 0)
            {                

                float saldo = 0;
                float cantidadVendida = 0;
                float saldoFinal = 0;
                float saldoVendido = 0;
                float cantidadSaldo = 0;
                float valorMontoItem = (float)Math.Round(OrdenDetalleCompraSeleccionado.ValorFinal / OrdenDetalleCompraSeleccionado.Cantidad, 2);                
                foreach (var rstock in LItemStock)
                {
                    cantidadVendida = OrdenDetalleCompraSeleccionado.Cantidad - saldo;
                    saldoVendido= OrdenDetalleCompraSeleccionado.ValorFinal - saldoFinal;
                    cantidadSaldo = rstock.CantidadSaldoFinal;
                    if (cantidadSaldo >= cantidadVendida)
                    {
                        rstock.CantidadVenta += cantidadVendida;
                        devuelveStockCancelado.Add(new ItemEntrega()
                        {
                            Id=rstock.Id,
                            IdOrden = OrdenDetalleCompraSeleccionado.IdOrden,
                            IdOrdenOrigen = rstock.IdOrden
                                                                       ,
                            IdItem = rstock.IdItem
                                                                       ,
                            NombreItem = rstock.NombreItem,
                            EsServicio=0,
                            Unidad = rstock.Unidad
                                                                       ,
                            Imagen = rstock.Imagen
                                                                       ,
                            Ubicacion = rstock.Ubicacion,
                            FechaIngreso = rstock.FechaIngreso
                                                                       ,
                            FechaVencimiento = rstock.FechaVencimiento
                                                                       ,
                            cantidad = cantidadVendida
                                                                       ,
                            valorUnitario = OrdenDetalleCompraSeleccionado.ValorUnitario
                                                                       ,
                            valorUnitarioCompra = rstock.ValorUnitario,
                            valorFinal=saldoVendido
                        });
                        break;
                    }
                    else
                    {
                        cantidadVendida = cantidadSaldo;
                        saldoVendido = (float)Math.Round(cantidadVendida * valorMontoItem,2);
                        rstock.CantidadVenta += cantidadSaldo;
                        devuelveStockCancelado.Add(new ItemEntrega()
                        {
                            Id = rstock.Id,
                            IdOrden = OrdenDetalleCompraSeleccionado.IdOrden,
                            IdOrdenOrigen = rstock.IdOrden
                                                                       ,

                            IdItem = rstock.IdItem
                                                                       ,
                            NombreItem = rstock.NombreItem,
                            EsServicio = 0,
                            Unidad = rstock.Unidad
                                                                       ,
                            Imagen = rstock.Imagen
                                                                       ,
                            Ubicacion = rstock.Ubicacion,
                            FechaIngreso = rstock.FechaIngreso
                                                                       ,
                            FechaVencimiento = rstock.FechaVencimiento
                                                                       ,
                            cantidad = cantidadVendida
                                                                       ,
                            valorUnitario = OrdenDetalleCompraSeleccionado.ValorUnitario
                                                                       ,
                            valorUnitarioCompra = rstock.ValorUnitario,
                            valorFinal = saldoVendido
                        });
                        saldo += (cantidadSaldo);
                        saldoFinal += (saldoVendido);
                    }

                }
            }
            else
            {
                devuelveStockCancelado.Add(new ItemEntrega()
                {
                    Id=Generator.GenerateKey(),
                    IdOrden = OrdenDetalleCompraSeleccionado.IdOrden,
                    IdOrdenOrigen = ""
                                                                       ,
                    IdItem = OrdenDetalleCompraSeleccionado.IdItem
                                                                       ,
                    NombreItem = OrdenDetalleCompraSeleccionado.NombreItem
                                                                       ,
                    EsServicio=1,
                    Imagen = OrdenDetalleCompraSeleccionado.Imagen
                                                                       ,
                    Ubicacion = "LOCAL"
                                                                       ,
                    FechaVencimiento = ""
                                                                       ,
                    cantidad = OrdenDetalleCompraSeleccionado.Cantidad
                                                                       ,
                    valorUnitario = OrdenDetalleCompraSeleccionado.ValorUnitario
                                                                       ,
                    valorUnitarioCompra = PrecioCompra,
                    valorFinal = OrdenDetalleCompraSeleccionado.ValorFinal
                });
            }
            var stockEntrega = devuelveStockCancelado.GroupBy(x => new { x.Id, x.IdItem, x.NombreItem, x.Ubicacion, x.FechaVencimiento,x.valorUnitario,x.valorUnitarioCompra,x.IdOrden,x.IdOrdenOrigen,x.EsServicio })
                .Select(c => new ItemEntrega()
                {
                    Id=c.Key.Id,
                    IdOrden = c.Key.IdOrden,
                    IdOrdenOrigen = c.Key.IdOrdenOrigen,
                    IdItem=c.Key.IdItem,
                    NombreItem = c.Key.NombreItem,
                    EsServicio=c.Key.EsServicio,
                    Ubicacion = c.Key.Ubicacion,
                    FechaVencimiento = c.Key.FechaVencimiento,
                    valorUnitario=c.Key.valorUnitario,
                    valorUnitarioCompra = c.Key.valorUnitarioCompra,
                    cantidad =c.Sum(g=>g.cantidad),
                    valorFinal = c.Sum(g => g.valorFinal)
                }).ToList();

                
            OrdenDetalleCompraSeleccionado.DetalleItemEntrega = new ObservableCollection<ItemEntrega>(stockEntrega);
            OrdenDetalleCompraSeleccionado.DetalleItemEntregaSerializado = JsonConvert.SerializeObject(OrdenDetalleCompraSeleccionado.DetalleItemEntrega);
        }

        public async Task almacenaTablaPago()
        {
            List<TablaPago> _tablaPago = new List<TablaPago>();
            float _cantidadCuotas = Cantidad;
            if (IsVisibleCondicionesPeriodoPago && _cantidadCuotas>1) //Es Servicio y tiene condiciones de Periodo de Pago
            {                
                float _numeroCuota = 0;
                string _fechaVencimiento = FechaPrimerPago;
                string _fechaVencimientoInicial = FechaPrimerPago;
                float _valorCompraCuota =(float)Math.Round(OrdenDetalleCompraSeleccionado.ValorCompra / _cantidadCuotas,2);
                float _valorDiferenciaCompraCuota = (float)Math.Round(_valorCompraCuota * _cantidadCuotas, 2) - OrdenDetalleCompraSeleccionado.ValorCompra;
                float _valorCompraCuotaUltima = _valorDiferenciaCompraCuota!=0?(_valorCompraCuota-_valorDiferenciaCompraCuota):_valorCompraCuota;

                float _valorIncrementoCuota = (float)Math.Round(OrdenDetalleCompraSeleccionado.ValorIncremento / _cantidadCuotas,2);
                float _valorDiferenciaIncrementoCuota = (float)Math.Round(_valorIncrementoCuota * _cantidadCuotas, 2) - OrdenDetalleCompraSeleccionado.ValorIncremento;
                float _valorIncrementoCuotaUltima = _valorDiferenciaIncrementoCuota != 0 ? (_valorIncrementoCuota - _valorDiferenciaIncrementoCuota) : _valorIncrementoCuota;

                float _valorDescuentoCuota = (float)Math.Round(OrdenDetalleCompraSeleccionado.ValorDescuento / _cantidadCuotas, 2);
                float _valorDiferenciaDescuentoCuota = (float)Math.Round(_valorDescuentoCuota * _cantidadCuotas, 2) - OrdenDetalleCompraSeleccionado.ValorDescuento;
                float _valorDescuentoCuotaUltima = _valorDiferenciaDescuentoCuota != 0 ? (_valorDescuentoCuota - _valorDiferenciaDescuentoCuota) : _valorDescuentoCuota;

                float _valorImpuestoCuota = (float)Math.Round(OrdenDetalleCompraSeleccionado.ValorImpuesto / _cantidadCuotas, 2);
                float _valorDiferenciaImpuestoCuota = (float)Math.Round(_valorImpuestoCuota * _cantidadCuotas, 2) - OrdenDetalleCompraSeleccionado.ValorImpuesto;
                float _valorImpuestoCuotaUltima = _valorDiferenciaImpuestoCuota != 0 ? (_valorImpuestoCuota - _valorDiferenciaImpuestoCuota) : _valorImpuestoCuota;

                float _valorFinalCuota = (float)Math.Round(OrdenDetalleCompraSeleccionado.ValorFinal / _cantidadCuotas, 2);
                float _valorDiferenciaFinalCuota = (float)Math.Round(_valorFinalCuota * _cantidadCuotas, 2) - OrdenDetalleCompraSeleccionado.ValorFinal;
                float _valorFinalCuotaUltima = _valorDiferenciaFinalCuota != 0 ? (_valorFinalCuota - _valorDiferenciaFinalCuota) : _valorFinalCuota;

               
                string _periodoPago = PeriodoPagoSeleccionado.Nombre;

                        
                DateTime y=DateTime.Now;
                

                while (_tablaPago.Count<_cantidadCuotas)
                {
                    _numeroCuota += 1;
                    //_fechaVencimiento = devuelveFechaVencimiento(_numeroCuota);

                    switch (_periodoPago)
                    {
                        case string a when _periodoPago.ToString().ToUpper().Contains("ANU"):
                             y= Convert.ToDateTime(_fechaVencimientoInicial).AddYears((int)_numeroCuota);
                            _fechaVencimiento = y.ToString("dd/MM/yyyy");
                            break;
                        case string b when _periodoPago.ToString().ToUpper().Contains("SEMES"):
                            y = Convert.ToDateTime(_fechaVencimientoInicial).AddMonths((int)_numeroCuota*6);
                            _fechaVencimiento = y.ToString("dd/MM/yyyy");
                            break;
                        case string c when _periodoPago.ToString().ToUpper().Contains("CUAT"):
                            y = Convert.ToDateTime(_fechaVencimientoInicial).AddMonths((int)_numeroCuota * 4);
                            _fechaVencimiento = y.ToString("dd/MM/yyyy");
                            break;
                        case string d when _periodoPago.ToString().ToUpper().Contains("TRIMESTRA"):
                            y = Convert.ToDateTime(_fechaVencimientoInicial).AddMonths((int)_numeroCuota * 3);
                            _fechaVencimiento = y.ToString("dd/MM/yyyy");
                            break;
                        case string e when _periodoPago.ToString().ToUpper().Contains("MENS"):
                            y = Convert.ToDateTime(_fechaVencimientoInicial).AddMonths((int)_numeroCuota * 1);
                            _fechaVencimiento = y.ToString("dd/MM/yyyy");
                            break;
                        case string f when _periodoPago.ToString().ToUpper().Contains("SEMAN"):
                            y = Convert.ToDateTime(_fechaVencimientoInicial).AddDays((int)_numeroCuota * 7);
                            _fechaVencimiento = y.ToString("dd/MM/yyyy");
                            break;
                        case string g when _periodoPago.ToString().ToUpper().Contains("DIAR"):
                            y = Convert.ToDateTime(_fechaVencimientoInicial).AddDays((int)_numeroCuota * 1);
                            _fechaVencimiento = y.ToString("dd/MM/yyyy");
                            break;
                    }

                    _tablaPago.Add(new TablaPago()
                    {
                        IdOrden = OrdenDetalleCompraSeleccionado.IdOrden,
                        NumeroCuota = (int)_numeroCuota,
                        EstadoCuota = Enum.GetName(typeof(EstadoCuotaPago), 0), //Pendiente
                        FechaVencimiento = _fechaVencimiento,
                        ValorCompra = _numeroCuota==_cantidadCuotas-1?_valorCompraCuotaUltima:_valorCompraCuota,
                        ValorIncremento = _numeroCuota == _cantidadCuotas - 1 ? _valorIncrementoCuotaUltima : _valorIncrementoCuota,
                        ValorDescuento = _numeroCuota == _cantidadCuotas - 1 ? _valorDescuentoCuotaUltima : _valorDescuentoCuota,
                        ValorImpuesto = _numeroCuota == _cantidadCuotas - 1 ? _valorImpuestoCuotaUltima : _valorImpuestoCuota,
                        ValorFinal = _numeroCuota == _cantidadCuotas - 1 ? _valorFinalCuotaUltima : _valorFinalCuota
                    });
                }                
            }
            else
            {
                _tablaPago.Add(new TablaPago()
                {
                    IdOrden = OrdenDetalleCompraSeleccionado.IdOrden,
                    NumeroCuota = 1,
                    EstadoCuota= Enum.GetName(typeof(EstadoCuotaPago),0),
                    FechaVencimiento=FechaPrimerPago,
                    ValorCompra=OrdenDetalleCompraSeleccionado.ValorCompra,
                    ValorIncremento = OrdenDetalleCompraSeleccionado.ValorIncremento,
                    ValorDescuento = OrdenDetalleCompraSeleccionado.ValorDescuento,
                    ValorImpuesto = OrdenDetalleCompraSeleccionado.ValorImpuesto,
                    ValorFinal = OrdenDetalleCompraSeleccionado.ValorFinal
                });
                
            }
            OrdenDetalleCompraSeleccionado.TablaPagoSerializado = JsonConvert.SerializeObject(_tablaPago);
        }                    
        
        
        
        private async Task Ok_CommandAsync()
        {
            EventoGuardar?.Invoke(true);

        }
        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales
        private async Task Cancel_CommandAsync()
        {
            EventoCancelar?.Invoke(true);
        }
        public async Task almacenaDatos()
        {
            OrdenDetalleCompraSeleccionado.IdUnidad = PrecioSeleccionado.IdUnidadConvertida;
            OrdenDetalleCompraSeleccionado.Unidad = PrecioSeleccionado.UnidadConvertida;
            OrdenDetalleCompraSeleccionado.IdPeriodoPago = PeriodoPagoSeleccionado.Id;
            OrdenDetalleCompraSeleccionado.NombrePeriodoPago = PeriodoPagoSeleccionado.Nombre;
            //Almacena Item Entrega
            await almacenaItemEntrega();
            await almacenaTablaPago();
        }
        public async void calculoValores(int origen)
        {
            var precio = PrecioSeleccionado.PrecioVenta;            
            //float conversionTotal = 0;
            float cantidad = 0;
            float costo = 0;
            switch (origen)
            {
                case 1:
                    if(Cantidad>ExistenteFecha)
                    {
                        Cantidad = ExistenteFecha;
                        await App.Current.MainPage.DisplayAlert("Importante", "La cantidad no puede ser mayor a la existente", "OK");
                    }
                    cantidad = Cantidad;
                    costo = precio * cantidad;
                    Costo = costo;
                    //conversionTotal = cantidad * conversion;
                    OrdenDetalleCompraSeleccionado.ValorConversion = PrecioSeleccionado.ValorConversion;
                    OrdenDetalleCompraSeleccionado.ValorUnitario = precio;
                    OrdenDetalleCompraSeleccionado.Cantidad = Cantidad;
                    //CalculaIncremento();
                    break;
                case 2:
                    PrecioSeleccionado = LPrecios.OrderBy(x => x.ValorConversion).FirstOrDefault();
                    _esActivadaCalculo = true;
                    precio = PrecioSeleccionado.PrecioVenta;                    
                    costo = Costo;
                    cantidad = (float)Math.Round(Costo / precio,2);
                    Cantidad = cantidad;
                    OrdenDetalleCompraSeleccionado.ValorUnitario = precio;
                    OrdenDetalleCompraSeleccionado.Cantidad = Cantidad;
                    OrdenDetalleCompraSeleccionado.ValorCompra = Costo;
                    OrdenDetalleCompraSeleccionado.ValorConversion = PrecioSeleccionado.ValorConversion;
                    break;
            }
            CalculaExistentes();

        }
        public void CalculaExistentes()
        {
            CalculaExistenteFecha();
            //CalculaIncremento();
            CalculaImpuesto();
        }
        public void CalculaExtraServicio()
        {
            if (IsVisibleCondicionesPeriodoPago)
            {
                SeteaValoresCantidad();
                CalculaExistenteFecha();
                //CalculaIncremento();
                //CalculaImpuesto();
                calculoValores(1);
            }
        }
        public void SeteaValoresCantidad()
        {
            bool cambiaCantidad = false;
            float _cantidad = 0;
            string _periodo = OrdenDetalleCompraSeleccionado.PeriodoServicio;
            float _cantidadPeriodoServicio = OrdenDetalleCompraSeleccionado.CantidadPeriodoServicio;
            string _periodoPago = PeriodoPagoSeleccionado.Nombre;
            float _cantidadPeriodo = 0;
            switch (_periodo) { 
                case string o when _periodo.ToString().ToUpper().Contains("AÑ"):
                    switch (_periodoPago)
                        {
                            case string a when _periodoPago.ToString().ToUpper().Contains("ANU"):
                                _cantidadPeriodo = 1*_cantidadPeriodoServicio;
                                _cantidad = _cantidadPeriodo;
                                break;
                            case string b when _periodoPago.ToString().ToUpper().Contains("SEMES"):
                                _cantidadPeriodo = 2 * _cantidadPeriodoServicio;
                                _cantidad = _cantidadPeriodo;
                                break;
                        case string c when _periodoPago.ToString().ToUpper().Contains("CUAT"):
                            _cantidadPeriodo = 3 * _cantidadPeriodoServicio;
                            _cantidad = _cantidadPeriodo;
                            break;
                        case string d when _periodoPago.ToString().ToUpper().Contains("TRIMESTRA"):
                            _cantidadPeriodo = 4 * _cantidadPeriodoServicio;
                            _cantidad = _cantidadPeriodo;
                            break;
                        case string e when _periodoPago.ToString().ToUpper().Contains("MENS"):
                            _cantidadPeriodo = 12 * _cantidadPeriodoServicio;
                            _cantidad = _cantidadPeriodo;
                            break;
                        case string f when _periodoPago.ToString().ToUpper().Contains("SEMAN"):
                            _cantidadPeriodo = 48 * _cantidadPeriodoServicio;
                            _cantidad = _cantidadPeriodo;
                            break;
                        case string g when _periodoPago.ToString().ToUpper().Contains("DIA"):
                            _cantidadPeriodo = 360 * _cantidadPeriodoServicio;
                            _cantidad = _cantidadPeriodo;
                            break;                            
                    }
                    cambiaCantidad = true;
                    break;
                case string p when _periodo.ToString().ToUpper().Contains("AS"):
                    cambiaCantidad = false;
                    break;
            }
            LFecha.FirstOrDefault().Cantidad = cambiaCantidad?_cantidad: LFecha.FirstOrDefault().Cantidad;
            Cantidad = cambiaCantidad ? _cantidad : Cantidad;                        
        }
        
        public string obtieneFechaPago()
        {
            string retorno = DateTime.Now.ToString("dd/MM/yyyy");
            if (IsVisibleCondicionesPeriodoPago)
            {
                string _periodo = OrdenDetalleCompraSeleccionado.PeriodoServicio;
                float _cantidadPeriodoServicio = OrdenDetalleCompraSeleccionado.CantidadPeriodoServicio;
                string _periodoPago = PeriodoPagoSeleccionado.Nombre;
                DateTime y = DateTime.Now;
                switch (_periodo)
                {
                    case string o when _periodo.ToString().ToUpper().Contains("AÑ"):
                        switch (_periodoPago)
                        {
                            case string a when _periodoPago.ToString().ToUpper().Contains("ANU"):
                                y = Convert.ToDateTime(retorno).AddYears((int)_cantidadPeriodoServicio);
                                retorno = y.ToString("dd/MM/yyyy");
                                break;
                            case string b when _periodoPago.ToString().ToUpper().Contains("SEMES"):
                                y = Convert.ToDateTime(retorno).AddMonths(6);
                                retorno = y.ToString("dd/MM/yyyy");
                                break;
                            case string c when _periodoPago.ToString().ToUpper().Contains("CUAT"):
                                y = Convert.ToDateTime(retorno).AddMonths(4);
                                retorno = y.ToString("dd/MM/yyyy");
                                break;
                            case string d when _periodoPago.ToString().ToUpper().Contains("TRIMESTRA"):
                                y = Convert.ToDateTime(retorno).AddMonths(3);
                                retorno = y.ToString("dd/MM/yyyy");
                                break;
                            case string e when _periodoPago.ToString().ToUpper().Contains("MENS"):
                                y = Convert.ToDateTime(retorno).AddMonths(1);
                                retorno = y.ToString("dd/MM/yyyy");
                                break;
                            case string f when _periodoPago.ToString().ToUpper().Contains("SEMAN"):
                                y = Convert.ToDateTime(retorno).AddDays(7);
                                retorno = y.ToString("dd/MM/yyyy");
                                break;
                            case string g when _periodoPago.ToString().ToUpper().Contains("DIAR"):
                                y = Convert.ToDateTime(retorno).AddDays(1);
                                retorno = y.ToString("dd/MM/yyyy");
                                break;
                        }
                        break;
                    case string o when _periodo.ToString().ToUpper().Contains("AS"):
                        y = Convert.ToDateTime(retorno).AddDays((int)_cantidadPeriodoServicio);
                        retorno = y.ToString("dd/MM/yyyy");
                        break;
                }
            }
            return retorno;
        }
        public void CalculaExistenteFecha()
        {
            float _cantidadExistente = 0;
            float _cantidadFecha = FechaSeleccionado.Cantidad;
            float _valorConversion = PrecioSeleccionado.ValorConversion;
            _cantidadExistente = (float)Math.Round(_cantidadFecha / _valorConversion,2);
            ExistenteFecha = _cantidadExistente;
        }
        
        public void CalculaImpuesto()
        {
            /*float _valorImpuesto = 0;
            float _porcentajeImpuesto = PorcentajeImpuesto;
            float _valorCompra = OrdenDetalleCompraSeleccionado.ValorCompra;*/
            string _tipoAplicacionImpuesto = TipoAplicacionImpuesto;
            string _idTipoAplicacionImpuesto = IdTipoAplicacionImpuesto;
            /*_valorImpuesto = (float)Math.Round(!string.IsNullOrEmpty(_tipoAplicacionImpuesto) && _porcentajeImpuesto > 0 //si posee porcentajeimpuesto mayor a cero
                    ? (_valorCompra + _valorIncremento) * (_porcentajeImpuesto / 100) //Adicional
                    : 0,2); //Caso que no tenga definido Impuesto;
            ValorImpuesto = _valorImpuesto;*/
            ImpuestoLabel = string.IsNullOrEmpty(_idTipoAplicacionImpuesto)? "Impuesto" : (_tipoAplicacionImpuesto.ToUpper().Contains("ADI")?"Impuesto Adic.": "Impuesto Incl.");
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
                if (LTipoDescuentoVenta != null)
                    TipoDescuentoSeleccionado = LTipoDescuentoVenta.Where(x => x.Nombre.Contains("0%")).FirstOrDefault();
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
            float valorPago = Costo;
            float porcentajeDescuento = 0;
            float valorDescuento = 0;
            switch (tipo)
            {
                case 1:
                    porcentajeDescuento = TipoDescuentoSeleccionado.ValorConstanteNumerico;
                    valorDescuento = (float)Math.Round(valorPago - (valorPago - ((porcentajeDescuento * valorPago) / 100)),2);
                    ValorDescuento = valorDescuento;
                    break;
                case 2:
                    valorDescuento = ValorDescuento;
                    break;
            }
            OrdenDetalleCompraSeleccionado.ValorDescuento = ValorDescuento;
            //OrdenSeleccionado.ValorDescuento += ValorDescuento;
            //aplicaDescuentoDetalle();
        }

        #endregion 6.Métodos Generales

    }
}
