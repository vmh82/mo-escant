using System;
using DataModel.Enums;

namespace DataModel.DTO.Configuracion
{
    public class PrinterSettings
    {
        public bool IsEnableConnectToPrinter { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public PaperSize PaperSize { get; set; }
        public string DefaultDevice { get; set; }
        public string SaleOrderHeader { get; set; }
        public string SaleOrderFooter { get; set; }
        public string PurchaseOrderHeader { get; set; }
        public bool IsEnablePrintCopy { get; set; }
        public bool IsShownCostPrice { get; set; }
        public bool IsShownAddress { get; set; }
        public bool IsShownPhoneNumber { get; set; }
        public bool IsShownBarcode { get; set; }
        public bool IsShownPrintDate { get; set; }
        public bool IsEnableConfirmBeforePrint { get; set; }
        public bool IsEnableAutoCutPaper { get; set; }
    }
}
