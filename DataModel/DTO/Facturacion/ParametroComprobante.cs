using DataModel.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Facturacion
{
    public class ParametroComprobante : BaseModel
    {
        //Datos Comprobante
        public string Ruc { get; set; }
        public string NombreEstablecimiento { get; set; }
        public string NombreResponsableLegal { get; set; }
        public string DireccionEstablecimiento { get; set; }
        public int Secuencial { get; set; }
        public DateTime FechaEmision { get; set; }
        public int TipoComprobante { get; set; }        
        public string Establecimiento { get; set; }
        public string PuntoEmision { get; set; }
        public string ContribuyenteEspecial { get; set; }
        public string ObligadoLLevarContabilidad { get; set; }
        public int TipoEmision { get; set; }
        public int Ambiente { get; set; }

        //Datos Cliente
        public string Identificacion { get; set; }
        public string NombreCliente { get; set; }
        public string TelefonoCliente { get; set; }
        public string DireccionCliente { get; set; }

        //Datos Venta
        public string OrdenDetalleCompraCanceladoSerializado { get; set; }

        //Datos Pago
        public string OrdenPagoSerializado { get; set; }

        ///Devoluciones
        public string ClaveAcceso { get; set; }
        public string XmlEnvio { get; set; }
        public string XmlEnvio64 { get; set; }
        public string XmlEnvioOrganismo { get; set; }
        public string XmlReceptaOrganismo { get; set; }
        public string Estado { get; set; }

        //Devoluciones 2do Paso
        public string XmlEnvioOrganismoAutorizacion { get; set; }
        public string XmlReceptaOrganismoAutorizacion { get; set; }
        public string EstadoAutorizacion { get; set; }
        public string NumeroAutorizacion { get; set; }
        public string FechaAutorizacion { get; set; }

        //Resultado de proceso
        public int Exito { get; set; }

        //
        public int Continuar { get; set; }

    }
}
