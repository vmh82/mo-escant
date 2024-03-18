using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using static DataModel.DTO.Compras.OrdenPago;

namespace DataModel.DTO.Compras
{
    public class OrdenPago : Base.BaseModel, ISyncTable<OrdenPagoDto>
    {
        public string IdOrden { get; set; }
        public string NumeroOrden { get; set; }
        public string NumeroPago { get; set; }
        public string Cuotas { get; set; }
        public string AdjuntoCancelado { get; set; }
        public string EstadoCancelado { get; set; }
        public string IdFormaCancelacion { get; set; }
        public string FormaCancelacion { get; set; }
        public string FechaVencimientoPago { get; set; }
        public string InstitucionFinanciera { get; set; }
        public string NumeroCheque { get; set; }        
        public float _montoPagoTotal { get; set; }
        public float MontoPagoTotal {
            get { return _montoPagoTotal; }
            set
            {
                _montoPagoTotal = value;
                OnPropertyChanged("MontoPagoTotal");                
            }
        }
        public float _valorPago { get; set; }
        public float ValorPago
        {
            get { return _valorPago; }
            set
            {
                _valorPago = value;
                OnPropertyChanged("ValorPago");
                calculaValores();                                
            }
        }

        public float _valorCambio;
        public float ValorCambio
        {
            get { return _valorCambio; }
            set
            {
                _valorCambio = value;
                OnPropertyChanged("ValorCambio");
            }
        }
        public float _valorFinal;
        public float ValorFinal
        {
            get { return _valorFinal; }
            set
            {
                _valorFinal = value;
                OnPropertyChanged("ValorFinal");
            }
        }
        [Ignore]
        public string _fechaCancelado { get; set; }
        public string FechaCancelado
        {
            get => _fechaCancelado;
            set
            {
                _fechaCancelado = value;
                OnPropertyChanged("FechaCancelado");
            }
        }

        public void calculaValores()
        {
            ValorCambio=(float)Math.Round(ValorPago > MontoPagoTotal ? ValorPago - MontoPagoTotal : 0,2);
            ValorFinal = (float)Math.Round(ValorPago - ValorCambio,2);
        }


        public Task BindData(OrdenPagoDto apiDto, DbContext dbContext)
        {
            Id = apiDto.Id;
            IdOrden = apiDto.IdOrden;
            NumeroOrden = apiDto.NumeroOrden;
            IdFormaCancelacion = apiDto.IdFormaCancelacion;
            FormaCancelacion = apiDto.FormaCancelacion;
            FechaVencimientoPago = apiDto.FechaVencimientoPago;
            InstitucionFinanciera = apiDto.InstitucionFinanciera;
            NumeroCheque = apiDto.NumeroCheque;
            ValorPago = apiDto.ValorPago;
            ValorCambio = apiDto.ValorCambio;
            ValorFinal = apiDto.ValorFinal;
            return Task.FromResult(1);
        }
        public class OrdenPagoDto : AuditableEntity
        {
            public string IdOrden { get; set; }
            public string NumeroOrden { get; set; }
            public string IdFormaCancelacion { get; set; }
            public string FormaCancelacion { get; set; }
            public string FechaVencimientoPago { get; set; }
            public string InstitucionFinanciera { get; set; }
            public string NumeroCheque { get; set; }
            public float ValorPago { get; set; }
            public float ValorCambio { get; set; }
            public float ValorFinal { get; set; }
        }

    }
}
