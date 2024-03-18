using DataModel.Base;
using DataModel.DTO.Administracion;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Compras
{
    public class OrdenDetalleCompraDet:BaseModel
    {
        public string IdOrdenDetalleCompra { get; set; }
        public string IdOrden { get; set; }
        public string IdItem { get; set; }
        public string NombreItem { get; set; }
        public string CodigoExterno { get; set; }
        public string IdUnidad { get; set; }
        public string Unidad { get; set; }

        public float _cantidad;
        public float Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                OnPropertyChanged("Cantidad");
                otroDescuento();
                calculaValorFinal();             
            }
        }
        
        public float ValorUnitario { get; set; }

        public float _valorCompraTmp;
        [Ignore]
        //public float ValorCompraTmp => Cantidad * ValorUnitario;
        public float ValorCompraTmp { get { return ValorUnitario * _cantidad; } set { _valorCompraTmp = value; OnPropertyChanged("ValorCompraTmp"); } }
        //public float ValorCompraTmp => ValorUnitario * _cantidad;
        public float _valorCompra;
        public float ValorCompra { get => _valorCompra; set { _valorCompra = value; OnPropertyChanged("ValorCompra"); } }

        public string _tipoDescuento;
        public string TipoDescuento
        {
            get => _tipoDescuento;
            set
            {
                _tipoDescuento = value;
                OnPropertyChanged("TipoDescuento");
                revisaValores();
            }
        }
        public float _valorDescuento;
        public float ValorDescuento {
            get => _valorDescuento;
            set
            {
                _valorDescuento = value;                
                OnPropertyChanged("ValorDescuento");                
                calculaValorFinal();
            }
        }
        [Ignore]
        public float _valorFinal { get; set; }
        public float ValorFinal
        {
            get => _valorFinal;
            set
            {
                _valorFinal = value;
                OnPropertyChanged("ValorFinal");
            }
        }
        public long? FechaVencimiento { get; set; }

        public string FechaVencimientoStr { get; set; }
        [Ignore]
        public string Imagen { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }
        [Ignore]
        public string CodigoEstadoOrden { get; set; }
        [Ignore]
        public Catalogo _tipoDescuentoSeleccion { get; set; }
        [Ignore]
        public Catalogo TipoDescuentoSeleccion
        {
            get => _tipoDescuentoSeleccion;
            set
            {
                _tipoDescuentoSeleccion = value;
                OnPropertyChanged("TipoDescuentoSeleccion");
                cambiarTipoDescuento();
            }
        }

        public string _ubicacion;
        public string Ubicacion
        {
            get => _ubicacion;
            set
            {
                _ubicacion = value;
                OnPropertyChanged("Ubicacion");
            }
        }

        public void revisaValores()
        {
            if (TipoDescuento != null)
            {
                if (TipoDescuento.Contains("+"))
                {
                    string[] pValorNuevo = TipoDescuento.Split('+');
                    var cantidadDescontada = Convert.ToInt32(pValorNuevo[1]);
                    var cantidadRequerida = Convert.ToInt32(pValorNuevo[0]);
                    if (cantidadRequerida == Cantidad)
                    {
                        Cantidad = cantidadDescontada;
                        ValorDescuento = Cantidad * ValorUnitario;
                    }
                }
                if(TipoDescuento.Contains("Desc"))
                {                    
                    ValorDescuento = Cantidad * ValorUnitario;
                }
            }
        }
        public void calculaValorFinal()
        {
            ValorCompra=ValorUnitario* Cantidad;
            ValorFinal =ValorCompra - ValorDescuento;
        }
        public void otroDescuento()
        {            
            ValorDescuento = TipoDescuento!=null && TipoDescuento.Contains("Otro Desc") ? Cantidad * ValorUnitario : ValorDescuento;
        }
        public void cambiarTipoDescuento()
        {
            TipoDescuento = TipoDescuentoSeleccion != null ? TipoDescuentoSeleccion.Nombre : "No Aplica";
        }

    }
}
