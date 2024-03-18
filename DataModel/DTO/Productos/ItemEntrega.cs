using DataModel.Base;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Productos
{
    public class ItemEntrega : BaseModel
    {
        public string IdOrden { get; set; }
        public string NumeroOrden { get; set; }
        public string IdOrdenOrigen { get; set; }
        public string IdItem { get; set; }
        public string NombreItem { get; set; }
        public int EsServicio { get; set; }
        public string Imagen { get; set; }
        public string Unidad { get; set; }
        public string Ubicacion { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaVencimiento { get; set; }
        public float _cantidad;
        public float cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                OnPropertyChanged("cantidad");
                //calculaSaldoEntrega();
            }
        }
        //public float cantidadEntrega { get; set; }
        
        public float _cantidadEntrega;
        public float cantidadEntrega {
            get => _cantidadEntrega;
            set
            {
                _cantidadEntrega = value;
                OnPropertyChanged("cantidadEntrega");
                calculaSaldoEntrega();
            }
        }
        public float _cantidadEntregada;
        public float cantidadEntregada
        {
            get => _cantidadEntregada;
            set
            {
                _cantidadEntregada = value;
                OnPropertyChanged("cantidadEntregada");         
            }
        }
        public float _cantidadSaldoEntrega;
        public float cantidadSaldoEntrega
        {
            get => _cantidadSaldoEntrega;
            set
            {
                _cantidadSaldoEntrega = value;
                //_cantidadSaldoEntrega = cantidad - cantidadEntregada;
                OnPropertyChanged("cantidadSaldoEntrega");
            }
        }
        public float valorUnitarioCompra { get; set; }
        public float valorUnitario { get; set; }
        public float valorCompra { get; set; }
        public float valorDescuento { get; set; }
        public float valorImpuesto { get; set; }
        public float valorFinal { get; set; }

        public void calculaSaldoEntrega() {
            if (cantidadEntrega > 0)
            {
                if (cantidadEntrega > cantidadSaldoEntrega)
                    cantidadEntrega = cantidadSaldoEntrega;
                cantidadEntregada += cantidadEntrega;
                if (cantidadEntregada > cantidad)
                    cantidadEntregada = cantidad;                
            }
            cantidadSaldoEntrega = cantidad - cantidadEntregada;            
        }
    }
}
