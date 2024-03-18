using DataModel.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Facturacion
{
    public class ParametroFacturacion : BaseModel
    {
        public int Secuencial { get; set; }
        public DateTime FechaEmision { get; set; }
        public int TipoComprobante { get; set; }
        public string Ruc { get; set; }
        public string Establecimiento { get; set; }
        public string PuntoEmision { get; set; }
        public int TipoEmision { get; set; }
        public int Ambiente { get; set; }

        
    }
}
