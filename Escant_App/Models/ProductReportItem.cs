
using Escant_App.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using DataModel.DTO;

namespace Escant_App.Models
{
    public class ProductReportItem : DataModel.DTO.Productos.Item
    {
        public float Total { get; set; }
        public string TotalStr => Total.FormatPriceWithoutSymbol();
    }
}
