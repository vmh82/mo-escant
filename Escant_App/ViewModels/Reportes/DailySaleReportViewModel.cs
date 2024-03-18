using DataModel.DTO.Compras;
using Escant_App.Models;
using Escant_App.Services;
using Escant_App.ViewModels.Base;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Reportes
{
    public class DailySaleReportViewModel : ViewModelBase
    {
        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración
        private SwitchCompras _switchCompras;
        private List<Orden> _ordenes;
        private readonly IServicioCompras_Orden _ordenServicio;
        private Generales.Generales generales;
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase
        #endregion 1.2 Variables de Instancia de Objetos para la Clase
        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #region 1.4 Variables de Control de Datos
        #endregion 1.4 Variables de Control de Datos
        #region 1.5 Variables de Control de Errores
        #endregion 1.5 Variables de Control de Errores
        #endregion 1. Declaración de Variables

        #region 2.InstanciaAsignaVariablesControles
        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        #endregion 3.Constructor

        #region 4.DefinicionMetodos
        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales
        #endregion 6.Métodos Generales
        
        public DailySaleReportViewModel(IServicioCompras_Orden ordenServicio
                                        )
        {
            _ordenServicio = ordenServicio;
            _switchCompras = new SwitchCompras(_ordenServicio);            
            generales = new Generales.Generales();
        }

        private DateTime _selectDate;
        public DateTime SelectDate
        {
            get => _selectDate;
            set
            {
                _selectDate = value;
                RaisePropertyChanged(() => SelectDate);
            }
        }
        private ZReportModel _zReportData = new ZReportModel();
        public ZReportModel ZReportData
        {
            get => _zReportData;
            set
            {
                _zReportData = value;
                RaisePropertyChanged(() => ZReportData);
            }
        }

        public override async Task InitializeAsync(object navigationData)
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            if (navigationData != null && navigationData is DateTime reportDate)
            {
                IsBusy = true;
                SelectDate = reportDate;
                //var data = await _Escant_AppService.GetZReportData(reportDate);
                var data = await _switchCompras.ConsultaFechaOrden(new Orden()
                {
                    FechaReporte = SelectDate
                                                                            ,
                    TipoOrden = "Venta"
                }, estaConectado);
                GenerateZReportData(data);
                IsBusy = false;
            }
        }
        private float _productSaleListViewHeight;
        public float ProductSaleListViewHeight
        {
            get => _productSaleListViewHeight;
            set
            {
                _productSaleListViewHeight = value;
                RaisePropertyChanged(() => ProductSaleListViewHeight);
            }
        }
        private void GenerateZReportData(List<Orden> orders)
        {
            List<OrdenDetalleCompra> ordenDetalleCompra = new List<OrdenDetalleCompra>();
            /*foreach(var item in orders)
            {
                var item1= JsonConvert.DeserializeObject<List<OrdenDetalleCompra>>(item.OrdenDetalleCompraCanceladoSerializado);
                item1.ForEach(x => ordenDetalleCompra.Add(x));
            }*/
            orders.ForEach(x => JsonConvert.DeserializeObject<List<OrdenDetalleCompra>>(x.OrdenDetalleCompraCanceladoSerializado).ForEach(y => ordenDetalleCompra.Add(y)));
            _zReportData.TotalSales = orders.Sum(x => x.ValorFinal);
            _zReportData.Tax = ordenDetalleCompra.Sum(x => x.ValorImpuesto);
            _zReportData.TotalNetSales = _zReportData.TotalSales - _zReportData.Tax;
            _zReportData.SaleProducts = ordenDetalleCompra.GroupBy(a => a.NombreItem).Select(x => new SaleProductReport
            {
                ProductName = x.Key,
                Quantity = x.Sum(y => y.Cantidad),
                TotalPrice = x.Sum(y => y.ValorFinal)
            }).ToList();
            _zReportData.DiscountReport = new DiscountReport
            {
                Quantity = ordenDetalleCompra.Where(x => x.ValorDescuento > 0).Count(),
                Total = ordenDetalleCompra.Where(x => x.ValorDescuento > 0).Sum(x => x.ValorDescuento)
            };
            switch (Device.Idiom)
            {
                case TargetIdiom.Desktop:
                    ProductSaleListViewHeight = _zReportData.SaleProducts.Count * 40;
                    break;
                case TargetIdiom.Tablet:
                    ProductSaleListViewHeight = _zReportData.SaleProducts.Count * 40;
                    break;
                default:
                    ProductSaleListViewHeight = _zReportData.SaleProducts.Count * 30;
                    break;
            }
        }
    }
}
