using DataModel.Base;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Productos
{
    public class ItemStock : BaseModel
    {
        public string IdItem { get; set; }
        public string NombreItem { get; set; }
        public string Imagen { get; set; }
        public string Unidad { get; set; }
        public string IdOrden { get; set; }
        public string NumeroOrden { get; set; }
        public string Ubicacion { get; set; }
        public string FechaIngreso { get; set; }
        public string FechaVencimiento { get; set; }
        public float ValorUnitario { get; set; }
        public float _cantidadInventarioInicial { get; set; }
        public float CantidadInventarioInicial
        {
            get => _cantidadInventarioInicial;
            set
            {
                _cantidadInventarioInicial = value;
                OnPropertyChanged("CantidadInventarioInicial");
                calculaEntrada();
            }
        }
        public float _cantidadCompra { get; set; }
        public float CantidadCompra
        {
            get => _cantidadCompra;
            set
            {
                _cantidadCompra = value;
                OnPropertyChanged("CantidadCompra");
                calculaEntrada();
            }
        }
        public float _cantidadEntrada { get; set; }
        public float CantidadEntrada
        {
            get => _cantidadEntrada;
            set
            {
                _cantidadEntrada = value;
                OnPropertyChanged("CantidadEntrada");
                calculaSaldos();
            }
        }
        public float _cantidadInventarioConcilia { get; set; }
        public float CantidadInventarioConcilia
        {
            get => _cantidadInventarioConcilia;
            set
            {
                _cantidadInventarioConcilia = value;
                /*if (_cantidadInventarioConcilia>0 && _cantidadInventarioConcilia + CantidadSalida > CantidadEntrada)
                    _cantidadInventarioConcilia = CantidadEntrada - CantidadSalida;*/
                OnPropertyChanged("CantidadInventarioConcilia");
                calculaSalida();                
            }
        }
        public float _cantidadDevuelto { get; set; }
        public float CantidadDevuelto
        {
            get => _cantidadDevuelto;
            set
            {
                _cantidadDevuelto = value;
                /*if (_cantidadDevuelto > 0 && _cantidadDevuelto + CantidadSalida > CantidadEntrada)
                    _cantidadDevuelto = CantidadEntrada - CantidadSalida;*/
                OnPropertyChanged("CantidadDevuelto");
                calculaSalida();                
            }
        }
        public float _cantidadTraslado { get; set; }
        public float CantidadTraslado
        {
            get => _cantidadTraslado;
            set
            {
                _cantidadTraslado = value;
                /*if (_cantidadTraslado > 0 && _cantidadTraslado + CantidadSalida > CantidadEntrada)
                    _cantidadTraslado = CantidadEntrada - CantidadSalida;*/
                OnPropertyChanged("CantidadTraslado");
                calculaSalida();                
            }
        }
        public float _cantidadOtros { get; set; }
        public float CantidadOtros
        {
            get => _cantidadOtros;
            set
            {
                _cantidadOtros = value;
                /*if (_cantidadOtros > 0 && _cantidadOtros + CantidadSalida > CantidadEntrada)
                    _cantidadOtros = CantidadEntrada - CantidadSalida;*/
                OnPropertyChanged("CantidadOtros");
                calculaSalida();                
            }
        }
        public float _cantidadVenta { get; set; }
        public float CantidadVenta
        {
            get => _cantidadVenta;
            set
            {
                _cantidadVenta = value;
                /*if (_cantidadVenta>0 && _cantidadVenta + CantidadSalida > CantidadEntrada)
                    _cantidadVenta = CantidadEntrada - CantidadSalida;*/
                OnPropertyChanged("CantidadVenta");
                calculaSalida();                
            }
        }
        public float _cantidadSalida { get; set; }
        public float CantidadSalida
        {
            get => _cantidadSalida;
            set
            {
                _cantidadSalida = value;
                OnPropertyChanged("CantidadSalida");
                calculaSaldos();
            }
        }
        public float _cantidadSaldoFinal { get; set; }
        public float CantidadSaldoFinal
        {
            get => _cantidadSaldoFinal;
            set
            {
                _cantidadSaldoFinal = value;
                OnPropertyChanged("CantidadSaldoFinal");
            }
        }


        public void calculaEntrada()
        {
            CantidadEntrada = CantidadInventarioInicial + CantidadCompra;
        }
        public void calculaSalida()
        {
            CantidadSalida = CantidadInventarioConcilia + CantidadDevuelto+CantidadTraslado+CantidadOtros+CantidadVenta;
        }
        public void calculaSaldos()
        {
            CantidadSaldoFinal = CantidadEntrada - CantidadSalida;
        }
    }
}
