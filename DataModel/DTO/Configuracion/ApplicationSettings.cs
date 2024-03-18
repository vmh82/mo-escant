using System;
namespace DataModel.DTO.Configuracion
{
    public class ApplicationSettings
    {
        public bool IsEnableSelectCustomer { get; set; }
        public bool IsEnableSelectSupplier { get; set; }
        public bool IsEnableSelectStaff { get; set; }
        public bool IsEnableDelivery { get; set; }
        public bool IsEnableRevenue { get; set; }
        public bool IsDisplayStockBalanceQty { get; set; }
        public bool IsEnableHotKeyWhenSearchProduct { get; set; }
        public bool IsEnableAutoBackup { get; set; }
        public bool IsEnablePromotionPrice { get; set; }
        public bool IsEnableStaffCannotAdjustSalePrice { get; set; }
        public bool IsEnableStaffCannotAdjustInboundPrice { get; set; }
        public bool IsPreventSellWhenNegative { get; set; }
        public bool? IsUseDefaultCurrencyFormat { get; set; }
        public string CurrentCurrencyCustomFormat { get; set; }
        public string LanguageSelected { get; set; }
        public string CustomCurrencySymbol { get; set; }
    }
}
