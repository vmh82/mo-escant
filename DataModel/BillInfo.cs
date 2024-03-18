using System;
using System.Collections.Generic;
using System.Text;
using DataModel.DTO;
using DataModel.DTO.Configuracion;
using DataModel.Enums;

namespace DataModel
{
    public class BillInfo
    {
        public string Language { get; set; }
        public string VN_CURRENTCY_FORMAT { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string OrderDateText => OrderDate.ToString("dd/MM/yyyy h:mm tt");
        
        public float ReceivedMoney { get; set; }
        public float DeductMoney { get; set; }
        public string ReceiptHeader { get; set; }
        public string ReceiptFooter { get; set; }
        public PrinterSettings PrinterSettings { get; set; }
        //Customer info
        public ShopInfo ShopInfo { get; set; }

        public TitularInfo TitularInfo { get; set; }

        public bool FacturarEnLinea { get; set; }

        public FacturaElectronicaCabecera FacturaElectronicaCabecera{get;set;}

        public List<FormaPago> FormaPagos { get; set; }
        
    }
   
    public class FacturaElectronicaCabecera
    {
        public string Establecimiento { get; set; }
        public string PuntoEmision { get; set; }
        public string NumeroFactura { get; set; }
        public string NombreCliente { get; set; }
        public string CodigoTipoIdentificacionCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public string DireccionCliente { get; set; }
        public string TelefonoCliente { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Email { get; set; }
        public string NombreArchivo { get; set; }
        public string DescripcionArchivo { get; set; }
        public bool ArchivoGenerado { get; set; }
        public bool ArchivoFirmado { get; set; }
        public bool ArchivoAutorizado { get; set; }
        public bool EstaEnviadoMail { get; set; }
        public bool ArchivoAnulado { get; set; }
        public string NumeroAutorizacion { get; set; }
        public float TotalSinImpuestos { get; set; }
        public float TotalDescuentos { get; set; }
        public float ImporteTotal { get; set; }
        public int SecuencialEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string RucEmpresa { get; set; }
        public string DireccionMatriz { get; set; }
        public string Moneda { get; set; }
        public int SecuencialOficina { get; set; }
        public string NombreOficina { get; set; }
        public string DireccionSucursal { get; set; }
        public string Resolucion { get; set; }
        public string PathLogo { get; set; }
        public string Ambiente { get; set; }
        public string ObligadoContabilidad { get; set; }
        public string PathArchivosAutorizados { get; set; }
        public DateTime FechaAutorizacion { get; set; }
    }

    public class FormaPago
    {
        public string CodigoFormaPago { get; set; }
        public string Descripcion { get; set; }
        public int Plazo { get; set; }
        public string UnidadTiempo { get; set; }

    }

    public class ShopInfo
    {
        public string ShopName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class TitularInfo
    {
        public string EsCliente { get; set; }
        public string TitularName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
