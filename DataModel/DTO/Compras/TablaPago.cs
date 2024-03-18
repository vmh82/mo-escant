using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Compras
{
    public class TablaPago : Base.BaseModel
    {
        public string IdOrden { get; set; }
        public string NumeroOrden { get; set; }        
        public int NumeroCuota { get; set; }
        public string EstadoCuota { get; set; }        
        public string FechaVencimiento { get; set; }        
        public float ValorCompra { get; set; }
        public float ValorIncremento { get; set; }
        public float ValorDescuento { get; set; }
        public float ValorImpuesto { get; set; }
        public float ValorPagado { get; set; }
        public float ValorSaldo { get; set; }
        public float ValorFinal { get; set; }
        public int Seleccionada { get; set; }
    }
}
