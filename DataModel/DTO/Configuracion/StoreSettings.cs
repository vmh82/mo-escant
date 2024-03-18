using System;
namespace DataModel.DTO.Configuracion
{
    public class StoreSettings
    {
        public string StoreName { get; set; }
        public string StoreOwner { get; set; }
        public string Ruc { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsPrintBillNotIncludedTax { get; set; }
        public bool IsDisplayTotalPriceBeforeTax { get; set; }
        public int VAT { get; set; }
        public string TaxNumber { get; set; }
    }
}
