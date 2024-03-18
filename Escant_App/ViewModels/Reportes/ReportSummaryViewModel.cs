using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Linq;
using System.Collections.Generic;
using Xamarin.Forms;
using DataModel.DTO.Compras;
using ManijodaServicios.Switch;
using ManijodaServicios.Offline.Interfaz;
using Escant_App.ViewModels.Base;
using Escant_App.Helpers;
using Escant_App.Models;

namespace Escant_App.ViewModels.Reportes
{
    public class ReportSummaryViewModel : ViewModelBase
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
        public List<Orden> Ordenes
        {
            get => _ordenes;
            set
            {
                _ordenes = value;
                RaisePropertyChanged(() => Ordenes);
            }
        }
        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        #endregion 3.Constructor

        #region 4.DefinicionMetodos
        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales
        #endregion 6.Métodos Generales
        private float _revenue;
        private float _totalMoneyOnSaleOrder;
        private float _deductMoneyOnSaleOrder;
        private float _purchaseOrderMoney;
        private float _profit;
        private ReportSummaryRequest _reportSummaryRequest;
        public float Revenue
        {
            get { return _revenue; }
            set
            {
                _revenue = value;
                RaisePropertyChanged(() => Revenue);
                RaisePropertyChanged(() => RevenueStr);
            }
        }
        public string RevenueStr => Revenue.FormatPriceWithoutSymbol();
        public float TotalMoneyOnSaleOrder
        {
            get { return _totalMoneyOnSaleOrder; }
            set
            {
                _totalMoneyOnSaleOrder = value;
                RaisePropertyChanged(() => TotalMoneyOnSaleOrder);
                RaisePropertyChanged(() => TotalMoneyOnSaleOrderStr);
            }
        }
        public string TotalMoneyOnSaleOrderStr => TotalMoneyOnSaleOrder.FormatPriceWithoutSymbol();
        public float DeductMoneyOnSaleOrder
        {
            get { return _deductMoneyOnSaleOrder; }
            set
            {
                _deductMoneyOnSaleOrder = value;
                RaisePropertyChanged(() => DeductMoneyOnSaleOrder);
                RaisePropertyChanged(() => DeductMoneyOnSaleOrderStr);
            }
        }
        public string DeductMoneyOnSaleOrderStr => DeductMoneyOnSaleOrder.FormatPriceWithoutSymbol();
        public float PurchaseOrderMoney
        {
            get { return _purchaseOrderMoney; }
            set
            {
                _purchaseOrderMoney = value;
                RaisePropertyChanged(() => PurchaseOrderMoney);
                RaisePropertyChanged(() => PurchaseOrderMoneyStr);
            }
        }
        public string PurchaseOrderMoneyStr => PurchaseOrderMoney.FormatPriceWithoutSymbol();
        public float Profit
        {
            get { return _profit; }
            set
            {
                _profit = value;
                RaisePropertyChanged(() => Profit);
                RaisePropertyChanged(() => ProfitStr);
            }
        }
        public string ProfitStr => Profit.FormatPriceWithoutSymbol();
        public ReportSummaryRequest ReportSummaryRequest
        {
            get { return _reportSummaryRequest; }
            set
            {
                _reportSummaryRequest = value;
                RaisePropertyChanged(() => ReportSummaryRequest);
            }
        }
        public ReportSummaryViewModel(IServicioCompras_Orden ordenServicio
                                    )
        {
            _ordenServicio = ordenServicio;            
            _switchCompras = new SwitchCompras(_ordenServicio);
            generales = new Generales.Generales();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            if (navigationData != null && navigationData is ReportSummaryRequest reportSummaryRequest)
            {
                ReportSummaryRequest = reportSummaryRequest;
                var purchaseOrders = await _switchCompras.ConsultaPeriodoOrdenes(new Orden()
                {
                    FechaDesde = reportSummaryRequest.ReportPeriod.FromDate
                                                                            ,
                    FechaHasta = reportSummaryRequest.ReportPeriod.ToDate,
                    TipoOrden = "Compra"
                }, estaConectado);
                if (reportSummaryRequest.Orders != null)
                {
                    Revenue = reportSummaryRequest.Orders.Sum(x => x.ValorFinal);
                    DeductMoneyOnSaleOrder = reportSummaryRequest.Orders.Sum(x => x.ValorDescuento);
                    TotalMoneyOnSaleOrder = Revenue - DeductMoneyOnSaleOrder;
                }
                if (purchaseOrders != null)
                {
                    ReportSummaryRequest.PurchaseOrders = purchaseOrders;
                    PurchaseOrderMoney = purchaseOrders.Sum(x => x.ValorFinal);
                }
                Profit = TotalMoneyOnSaleOrder - PurchaseOrderMoney;
            }
            IsBusy = false;
        }
    }
}
