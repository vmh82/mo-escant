using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Linq;
using System.Collections.Generic;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using DataModel.DTO.Compras;
using Escant_App.ViewModels.Base;
using Escant_App.Models;
using Escant_App.Helpers;
using DataModel;
using Escant_App.Common;

namespace Escant_App.ViewModels.Reportes
{
    public class ReportesDetalleViewModel : ViewModelBase
    {
        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración
        private SwitchCompras _switchCompras;
        private SwitchProductos _switchProductos;
        private List<Orden> _ordenes;
        private readonly IServicioCompras_Orden _ordenServicio;
        private readonly IServicioProductos_Item _productoServicio;
        private Generales.Generales generales;
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase
        private string _tendenciaSeleccion;
        #endregion 1.2 Variables de Instancia de Objetos para la Clase
        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #region 1.4 Variables de Control de Datos
        private bool _esVisibleTendenciaProductos;
        private bool _esVisibleTendenciaClientes;
        private int _listViewHeight;
        #endregion 1.4 Variables de Control de Datos
        #region 1.5 Variables de Control de Errores
        #endregion 1.5 Variables de Control de Errores
        #region 1.6 Eliminar
        
        private float _totalSaleOrderMoney;
        private List<Orden> _itemOrdenes;
        private List<DataModel.DTO.Productos.Item> _items;
        private SelectReportPeriod _reportPeriod;        
        private List<ReportItem> _reportItems;
        private int _totalOrders;        
        private List<ProductReportItem> _top10Products;
        private List<ProductReportItem> _top10Clientes;
        private int _totalItems;
        private float _totalPrice;
        #endregion 1.6 Eliminar
        #endregion 1. Declaración de Variables

        #region 2.InstanciaAsignaVariablesControles
        public List<Orden> Ordenes
        {
            get => _ordenes;
            set
            {
                _ordenes = value;
                RaisePropertyChanged(() => Ordenes);
            }
        }
        public string TendenciaSeleccion
        {
            get => _tendenciaSeleccion;
            set
            {
                _tendenciaSeleccion = value;
                RaisePropertyChanged(() => TendenciaSeleccion);
            }
        }
        public bool EsVisibleTendenciaProductos
        {
            get => _esVisibleTendenciaProductos;
            set
            {
                _esVisibleTendenciaProductos = value;
                RaisePropertyChanged(() => EsVisibleTendenciaProductos);
            }
        }
        public bool EsVisibleTendenciaClientes
        {
            get => _esVisibleTendenciaClientes;
            set
            {
                _esVisibleTendenciaClientes = value;
                RaisePropertyChanged(() => EsVisibleTendenciaClientes);
            }
        }
        
        public List<ReportItem> ReportItems
        {
            get => _reportItems;
            set
            {
                _reportItems = value;
                RaisePropertyChanged(() => ReportItems);
            }
        }
        public List<ProductReportItem> Top10Products
        {
            get => _top10Products;
            set
            {
                _top10Products = value;
                RaisePropertyChanged(() => Top10Products);
            }
        }
        public List<ProductReportItem> Top10Clientes
        {
            get => _top10Clientes;
            set
            {
                _top10Clientes = value;
                RaisePropertyChanged(() => Top10Clientes);
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
        public float TotalSaleOrderMoney
        {
            get => _totalSaleOrderMoney;
            set
            {
                _totalSaleOrderMoney = value;
                RaisePropertyChanged(() => TotalSaleOrderMoney);
            }
        }
        public int TotalItems
        {
            get => _totalItems;
            set
            {
                _totalItems = value;
                RaisePropertyChanged(() => TotalItems);
            }
        }
        public float TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                RaisePropertyChanged(() => TotalPrice);
                RaisePropertyChanged(() => TotalPriceStr);
            }
        }
        public string TotalPriceStr => TotalPrice.FormatPriceWithoutSymbol();
        public int TotalOrders
        {
            get => _totalOrders;
            set
            {
                _totalOrders = value;
                RaisePropertyChanged(() => TotalOrders);
            }
        }
        private string _reportTypeHeader;
        public string ReportTypeHeader
        {
            get => _reportTypeHeader;
            set
            {
                _reportTypeHeader = value;
                RaisePropertyChanged(() => ReportTypeHeader);
            }
        }
        public SelectReportPeriod ReportPeriod
        {
            get => _reportPeriod;
            set
            {
                _reportPeriod = value;
                RaisePropertyChanged(() => ReportPeriod);
            }
        }        
        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        public ReportesDetalleViewModel(IServicioCompras_Orden ordenServicio,
                                IServicioProductos_Item productoServicio)
        {
            _ordenServicio = ordenServicio;
            _productoServicio = productoServicio;            
            _switchCompras = new SwitchCompras(_ordenServicio);
            _switchProductos = new SwitchProductos(_productoServicio);
            generales = new Generales.Generales();
            MessagingCenter.Unsubscribe<SelectReportPeriod>(this, nameof(SelectReportPeriod));
            MessagingCenter.Subscribe<SelectReportPeriod>(this, nameof(SelectReportPeriod), (args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Task.Run(async () =>
                    {
                        IsBusy = true;
                        ReportPeriod = args;
                        ReportTypeHeader = GenerateReportTypeHeader.GetReportHeader(args.PeriodType, args.FromDate, args.ToDate);                        
                        await GeneraReporte(args.FromDate, args.ToDate);
                        IsBusy = false;
                    });
                });
            });
        }
        #endregion 3.Constructor

        #region 4.DefinicionMetodos
        public ICommand SelectDatePopupCommand => new Command(async () => await SelectDatePopupAsync());
        public ICommand SelectPeriodPopupCommand => new Command(async () => await SelectPeriodPopupAsync());
        public ICommand GotoSummaryReportCommand => new Command(async () => await GotoSummaryReport());
        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales
        #endregion 6.Métodos Generales




        private async Task SelectDatePopupAsync()
        {
            await NavigationService.NavigateToPopupAsync<PopupSelectDateViewModel>();
        }

        

        private async Task SelectPeriodPopupAsync()
        {
            await NavigationService.NavigateToPopupAsync<PopupSelectDateRangeViewModel>(ReportPeriod);
        }

        
        private async Task GotoSummaryReport()
        {
            var reportSummaryRequest = new ReportSummaryRequest
            {
                ReportPeriod = ReportPeriod,
                Orders = Ordenes
            };
            await NavigationService.NavigateToAsync<ReportSummaryViewModel>(reportSummaryRequest);
        }

        
        

               

        private async Task GeneraReporte(DateTime fromDate, DateTime toDate)
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            if (toDate < fromDate)
            {
                return;
            }
            Ordenes = await _switchCompras.ConsultaPeriodoOrdenes(new Orden()
            {
                FechaDesde = fromDate
                                                                            ,
                FechaHasta = toDate,
                TipoOrden="Venta"
            },estaConectado);
            if (Ordenes == null)
            {
                return;
            }
            List<ReportItem> reportItems = new List<ReportItem>();
            while (fromDate <= toDate)
            {
                var reportItem = new ReportItem
                {
                    Day = fromDate.ToString(SystemConstants.DATE_FORMAT_DDMM)
                };
                reportItem.Revenue = Ordenes.GroupBy(x => x.OrdenFechaValue.Date)
                                     .OrderBy(x => x.Key)
                                     .Where(x => x.Key == fromDate.Date)
                                     .Select(x => x.Sum(y => y.ValorFinal))
                                     .Sum();
                reportItems.Add(reportItem);


                fromDate = fromDate.AddDays(1);
            }
            TotalSaleOrderMoney = (float)Math.Round(Ordenes.Sum(x => x.ValorFinal), 2);
            TotalOrders = Ordenes.Count;
            ReportItems = reportItems;
        }
        

        public async Task LoadTop10ProductoPorCantidadAsync()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var orderDetails = await _switchProductos.ConsultaTopItems(new DataModel.DTO.Productos.Item() { },estaConectado);
            if (orderDetails == null)
            {
                return;
            }
            var top10Products = orderDetails.GroupBy(x => x.Id)
                                             .Select(x => new
                                             {
                                                 ProductId = x.Key,
                                                 TotalQuantity = x.Sum(y => y.CantidadVenta)
                                             }).OrderByDescending(x => x.TotalQuantity)
                                             .Take(10)
                                             .ToList();
            List<string> productIds = top10Products.Select(y => y.ProductId).ToList();
            _items = orderDetails;
            Top10Products = top10Products
                    .Join(_items,
                          ord => ord.ProductId,
                          prd => prd.Id,
                          (ord, prd) => new ProductReportItem
                          {
                              Nombre = prd.Nombre,
                              Unidad = prd.Unidad,
                              Imagen = prd.Imagen,
                              Total = ord.TotalQuantity
                          }).ToList();
            CalculateListViewHeight(1);
        }

        private bool _isLoading = false;
        private bool _isLoadingCliente = false;
        public async Task LoadTop10ProductoPorVentaAsync()
        {
            if (_isLoading)
                return;
            _isLoading = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var orderDetails = await _switchProductos.ConsultaTopItems(new DataModel.DTO.Productos.Item() { }, estaConectado);
            if (orderDetails == null)
            {
                return;
            }
            var top10Products = orderDetails.GroupBy(x => x.Id)
                                             .Select(x => new
                                             {
                                                 ProductId = x.Key,
                                                 TotalRevenue = x.Sum(y => y.CantidadVenta * y.PrecioVenta)
                                             }).OrderByDescending(x => x.TotalRevenue)
                                             .Take(10)
                                             .ToList();
            List<string> productIds = top10Products.Select(y => y.ProductId).ToList();
            _items = orderDetails;
            Top10Products = top10Products
                    .Join(_items,
                          ord => ord.ProductId,
                          prd => prd.Id,
                          (ord, prd) => new ProductReportItem
                          {
                              Nombre = prd.Nombre,
                              Unidad = prd.Unidad,
                              Imagen = prd.Imagen,
                              Total = ord.TotalRevenue
                          }).ToList();
            CalculateListViewHeight(1);
            _isLoading = false;
        }

        public async Task LoadTop10ClientesPorVentaAsync()
        {
            if (_isLoadingCliente)
                return;
            _isLoadingCliente = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var orderDetails = await _switchCompras.ConsultaOrdenPorTipo(new Orden()
            {
                TipoOrden = "Venta"            
            }, estaConectado);
            if (orderDetails == null)
            {
                return;
            }
            var top10Products = orderDetails.GroupBy(x => x.Identificacion)
                                             .Select(x => new
                                             {
                                                 ProductId = x.Key,
                                                 TotalRevenue = x.Sum(y => y.ValorFinal)
                                             }).OrderByDescending(x => x.TotalRevenue)
                                             .Take(10)
                                             .ToList();
            List<string> productIds = top10Products.Select(y => y.ProductId).ToList();
            var _itemOrdenes = orderDetails.Select(x => new {
                Identificacion = x.Identificacion
                                                        ,
                Nombre = x.NombreCliente
                                                        ,
                Direccion = x.Direccion
                                                        ,
                Telefono = x.Telefono
            }).ToList()
                                            .Distinct();
            Top10Clientes = top10Products
                    .Join(_itemOrdenes,
                          ord => ord.ProductId,
                          prd => prd.Identificacion,
                          (ord, prd) => new ProductReportItem
                          {
                              Nombre = prd.Nombre,
                              Unidad = prd.Direccion,
                              Imagen = prd.Telefono,
                              Total = ord.TotalRevenue
                          }).ToList();
            CalculateListViewHeight(2);
            _isLoadingCliente = false;
        }
        public async Task LoadTop10ClientesPorCantidadAsync()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var orderDetails = await _switchCompras.ConsultaOrdenPorTipo(new Orden()
            {
                TipoOrden = "Venta"
            }, estaConectado);
            if (orderDetails == null)
            {
                return;
            }
            var top10Products = orderDetails.GroupBy(x => x.Identificacion)
                                             .Select(x => new
                                             {
                                                 ProductId = x.Key,
                                                 TotalQuantity = x.Count()
                                             }).OrderByDescending(x => x.TotalQuantity)
                                             .Take(10)
                                             .ToList();
            List<string> productIds = top10Products.Select(y => y.ProductId).ToList();
            var _itemOrdenes = orderDetails.Select(x=>new { Identificacion=x.Identificacion
                                                        ,Nombre=x.NombreCliente
                                                        ,Direccion=x.Direccion
                                                        ,Telefono=x.Telefono}).ToList()
                                            .Distinct();
            Top10Clientes = top10Products
                    .Join(_itemOrdenes,
                          ord => ord.ProductId,
                          prd => prd.Identificacion,
                          (ord, prd) => new ProductReportItem
                          {
                              Nombre = prd.Nombre,
                              Unidad = prd.Direccion,
                              Imagen = prd.Telefono,
                              Total = ord.TotalQuantity
                          }).ToList();
            CalculateListViewHeight(2);
        }

        private async Task CalculateStockBalance()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var stockBalances = await _switchProductos.ConsultaStockItems(new DataModel.DTO.Productos.Item() { }, estaConectado);                        
            var products = stockBalances;

            if (stockBalances != null && products != null)
            {
                var result = stockBalances
                    .Join(products,
                          stb => stb.Id,
                          prd => prd.Id,
                          (stb, prd) => new
                          {
                              Total = stb.CantidadSaldoFinal * prd.PrecioVenta
                          }).ToList();
                if (result != null)
                {
                    TotalItems = result.Count;
                    TotalPrice = result.Sum(x => x.Total);
                }
            }
        }

        

        private void CalculateListViewHeight(int tipo)
        {
            switch(tipo)
            {
                case 1:
                    if (Top10Products.Any())
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
                            ListViewHeight = Top10Products.Count() * itemHeight;
                        });
                    }
                    break;
                case 2:
                    if (Top10Clientes.Any())
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
                            ListViewHeight = Top10Clientes.Count() * itemHeight;
                        });
                    }
                    break;

            }
            
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            Task.Run(async () =>
            {
                ReportPeriod = new SelectReportPeriod
                {
                    PeriodType = PeriodType.Today,
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today.AddDays(1)
                };
                ReportTypeHeader = GenerateReportTypeHeader.GetReportHeader(ReportPeriod.PeriodType, ReportPeriod.FromDate, ReportPeriod.ToDate);
                await GeneraReporte(ReportPeriod.FromDate, ReportPeriod.ToDate);
                await CalculateStockBalance();
                IsBusy = false;
            });
            return Task.FromResult(true);
        }
    }

}
