using Escant_App.Helpers;
using Escant_App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Models
{
    public class ZReportModel: ViewModelBase
    {
        private float _totalNetSales;
        public float TotalNetSales
        {
            get => _totalNetSales;
            set
            {
                _totalNetSales = value;
                RaisePropertyChanged(() => TotalNetSales);
                RaisePropertyChanged(() => TotalNetSalesStr);
            }
        }
        public string TotalNetSalesStr => ((float)TotalNetSales).FormatPrice();

        private float _tax;
        public float Tax
        {
            get => _tax;
            set
            {
                _tax = value;
                RaisePropertyChanged(() => Tax);
                RaisePropertyChanged(() => TaxStr);
            }
        }
        public string TaxStr => ((float)Tax).FormatPrice();

        private float _totalSales;
        public float TotalSales
        {
            get => _totalSales;
            set
            {
                _totalSales = value;
                RaisePropertyChanged(() => TotalSales);
                RaisePropertyChanged(() => TotalSalesStr);
            }
        }
        public string TotalSalesStr => ((float)TotalSales).FormatPrice();

        private float _totalNetSalesByProduct;
        public float TotalNetSalesByProduct
        {
            get => _totalNetSalesByProduct;
            set
            {
                _totalNetSalesByProduct = value;
                RaisePropertyChanged(() => TotalNetSalesByProduct);
                RaisePropertyChanged(() => TotalNetSalesByProductStr);
            }
        }
        public string TotalNetSalesByProductStr => ((float)TotalNetSalesByProduct).FormatPrice();

        private List<SaleProductReport> _saleProducts;
        public List<SaleProductReport> SaleProducts
        {
            get => _saleProducts;
            set
            {
                _saleProducts = value;
                RaisePropertyChanged(() => SaleProducts);
            }
        }
        private DiscountReport _discountReport = new DiscountReport();
        public DiscountReport DiscountReport
        {
            get => _discountReport;
            set
            {
                _discountReport = value;
                RaisePropertyChanged(() => DiscountReport);
            }
        }
    }
    public class SaleProductReport: ViewModelBase
    {
        public string ProductName { get; set; }
        public float Quantity { get; set; }
        private float _totalPrice;
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
        public string TotalPriceStr => ((float)TotalPrice).FormatPrice();
    }
    public class DiscountReport: ViewModelBase
    {
        public int Quantity { get; set; }
        private float _total;
        public float Total
        {
            get => _total;
            set
            {
                _total = value;
                HasDiscount = (int)value > 0;
                RaisePropertyChanged(() => Total);
                RaisePropertyChanged(() => TotalStr);
            }
        }
        public string TotalStr => ((float)Total).FormatPrice();
        private bool _hasDiscount;
        public bool HasDiscount
        {
            get => _hasDiscount;
            set
            {
                _hasDiscount = value;
                RaisePropertyChanged(() => HasDiscount);
            }
        }
    }
}
