using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using DataModel.Base;
using DataModel.Helpers;
using System.Collections.ObjectModel;
using DataModel.DTO.Productos;

namespace DataModel.DTO.Compras
{
    public class OrdenDetalleCompra : BaseModel, ISyncTable<OrdenDetalleCompraDto>
    {
        public string IdOrden { get; set; }
        public string IdItem { get; set; }
        public string CodigoItem { get; set; }
        public string NombreItem { get; set; }
        public int EsServicio { get; set; }
        public string IdUnidad { get; set; }
        public string Unidad { get; set; }        
        public int EsParticionable { get; set; }
        public float _porcentajeImpuesto { get; set; }
        public float PorcentajeImpuesto {
            get => _porcentajeImpuesto;
            set
            {
                _porcentajeImpuesto = value;
                OnPropertyChanged("PorcentajeImpuesto");
                calculaValorImpuesto();                
            }
        }
        public string IdTipoAplicacionImpuesto { get; set; }
        public string TipoAplicacionImpuesto { get; set; }
        [Ignore]
        public string Ubicacion { get; set; }
        [Ignore]
        public string FechaVencimientoItem { get; set; }
        [Ignore]
        public string _fechaIngresoItem { get; set; }
        public string FechaIngresoItem
        {
            get => _fechaIngresoItem;
            set
            {
                _fechaIngresoItem = value;
                OnPropertyChanged("FechaIngresoItem");
                CalculaIncremento();
            }
        }
        public float ValorConversion { get; set; }
        public string PeriodoIncremento { get; set; }
        public float CantidadPeriodoIncremento { get; set; }
        public float ValorPeriodoIncremento { get; set; }
        public float _valorConversionMinimaUnidad { get; set; }
        public float ValorConversionMinimaUnidad {
            get => _valorConversionMinimaUnidad;
            set
            {
                _valorConversionMinimaUnidad = value;
                OnPropertyChanged("ValorConversionMinimaUnidad");
                CalculaIncremento();
            }
        }

        public float _cantidad;
        public float Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                OnPropertyChanged("Cantidad");
                CalculaIncremento();
                CalculaValores();                                
            }
        }
        public int CantidadPromocion { get; set; }
        public int CantidadTotal { get; set; }

        public float CantidadSaldoExistente { get; set; }

        public float _cantidadSaldoFinal;
        public float CantidadSaldoFinal { 
            get { 
                return _cantidadSaldoFinal; 
            } 
            set { _cantidadSaldoFinal = value; 
                    OnPropertyChanged("CantidadSaldoFinal"); 
            } 
        }
        /*
        public int Cantidad
        {
            get { return _cantidad; }
            set { SetField(ref _cantidad, value, () => Cantidad, () => ValorCompraTmp); }
        }
        
        public float _valorUnitario;
        public float ValorUnitario
        {
            get { return _valorUnitario; }
            set { SetField(ref _valorUnitario, value, () => ValorUnitario, () => ValorCompraTmp); }
        }*/
        public float ValorUnitario { get; set; }
        public float _valorIncremento { get; set; }
        public float ValorIncremento { 
            get { return _valorIncremento; }
            set {
                _valorIncremento = value;
                OnPropertyChanged("ValorIncremento");
                calculaValorImpuesto();
            }
        }
        
        //public float ValorCompraTmp => ValorUnitario * _cantidad;
        public float _valorCompra;
        public float ValorCompra { 
            get { return _valorCompra; } 
            set { _valorCompra = value; 
                OnPropertyChanged("ValorCompra");
                calculaValorImpuesto();
            } 
        }
        public float _valorImpuesto;
        public float ValorImpuesto { 
            get {
                return _valorImpuesto;
                } 
            set { _valorImpuesto = value; 
                OnPropertyChanged("ValorImpuesto");
                calculaValorFinal();                
            } 
        }
        
        public float ValorCompraImpuesto { get; set; }
        
        [Ignore]
        public string ValorCompraStr => ValorCompra.FormatoPrecioSinSimbolo();

        [Ignore]
        public float _valorDescuento { get; set; }
        public float ValorDescuento {
            get { return _valorDescuento; }
            set
            {
                _valorDescuento = value;
                OnPropertyChanged("ValorDescuento");
                calculaValorImpuesto();
            }
        }
        [Ignore]
        public float _valorFinal { get; set; }
        public float ValorFinal
        {
            get { return _valorFinal; }
            set
            {
                _valorFinal = value;
                OnPropertyChanged("ValorFinal");
            }
        }        
        public long? FechaVencimiento { get; set; }

        public ObservableCollection<OrdenDetalleCompraDet> _ordenDetalleComprasRecibido;
        [Ignore]
        public ObservableCollection<OrdenDetalleCompraDet> OrdenDetalleComprasRecibido
        {
            get => _ordenDetalleComprasRecibido;
            set
            {
                _ordenDetalleComprasRecibido = value;
                OnPropertyChanged("OrdenDetalleComprasRecibido");
            }
        }
        public ObservableCollection<ItemEntrega> _detalleItemEntrega;
        [Ignore]
        public ObservableCollection<ItemEntrega> DetalleItemEntrega
        {
            get => _detalleItemEntrega;
            set
            {
                _detalleItemEntrega = value;
                OnPropertyChanged("DetalleItemEntrega");
            }
        }
        public string Observaciones { get; set; }
        public string DetalleItemEntregaSerializado { get; set; }
        public string IdPeriodoPago { get; set; }
        public string NombrePeriodoPago { get; set; }
        public string TablaPagoSerializado { get; set; }
        [Ignore]
        public string Imagen { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }
        [Ignore]
        public string CodigoEstadoOrden { get; set; }
        [Ignore]
        public string IdPeriodoServicio { get; set; }
        [Ignore]
        public string PeriodoServicio { get; set; }
        [Ignore]
        public float CantidadPeriodoServicio { get; set; }
        [Ignore]
        public float CantidadEntregada { get; set; }
        [Ignore]
        public float CantidadSaldoEntrega { get; set; }
        [Ignore]
        public int PoseeStock { get; set; }
        [Ignore]
        public int PoseeInventarioInicial { get; set; }
        [Ignore]
        public string TipoOrden { get; set; }
        [Ignore]
        public string TipoInventario { get; set; }

        public void CalculaIncremento()
        {
            float _incremento = 0;
            float _cantidad = Cantidad;
            float _valorIncremento = ValorPeriodoIncremento;
            string _periodoIncremento = PeriodoIncremento;
            float _valorPeriodoIncremento = CantidadPeriodoIncremento;
            float _valorConversion = ValorConversion;
            float _valorConversionMinimo = ValorConversionMinimaUnidad;
            float diferencia = 0;
            float _vecesIncremento = 0;
            if (_valorIncremento > 0 && !string.IsNullOrEmpty(FechaIngresoItem))
            {
                string _fecha = FechaIngresoItem;
                switch (_periodoIncremento)
                {
                    case string a when _periodoIncremento.ToUpper().Contains("DIA"):
                        diferencia = (float)(Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy")) - Convert.ToDateTime(_fecha)).TotalDays;
                        _vecesIncremento = (float)Math.Round(diferencia / _valorPeriodoIncremento,0);
                        break;
                    case string b when _periodoIncremento.ToUpper().Contains("AÑO"):
                        diferencia = (float)((DateTime.Now).Year - Convert.ToDateTime(_fecha).Year);
                        _vecesIncremento = diferencia / _valorPeriodoIncremento;
                        break;
                    case string b when _periodoIncremento.ToUpper().Contains("MES"):
                        diferencia = ((float)((DateTime.Now).Year - Convert.ToDateTime(_fecha).Year)) * 12 + (DateTime.Now).Month - Convert.ToDateTime(_fecha).Month;
                        _vecesIncremento = diferencia / _valorPeriodoIncremento;
                        break;
                }

                _incremento = (float)Math.Round(_cantidad * (_valorConversion/ _valorConversionMinimo) * _vecesIncremento * _valorIncremento, 2);
            }
            ValorIncremento = _incremento;
        }
        public void calculaValorImpuesto()
        {
            float _valorImpuesto= (float)Math.Round(!string.IsNullOrEmpty(TipoAplicacionImpuesto) && PorcentajeImpuesto > 0 //si posee porcentajeimpuesto mayor a cero
                    ? (ValorCompra + ValorIncremento - ValorDescuento) * (PorcentajeImpuesto / 100) //Adicional
                    : 0,2); //Caso que no tenga definido Impuesto
            float _diferencia = 0;
            if (!string.IsNullOrEmpty(IdPeriodoServicio))
                _diferencia=(float)Math.Round((ValorCompra + ValorIncremento - ValorDescuento + _valorImpuesto) - (Cantidad * ValorCompraImpuesto),2);
            ValorImpuesto = _diferencia>0? (float)Math.Round(_valorImpuesto -_diferencia,2):_valorImpuesto;
        }

        public void calculaValorFinal()
        {
            float _valorDescuento = ValorDescuento;
            float _valorIncremento = ValorIncremento;
            float _valorImpuesto = ValorImpuesto;
            float _valorCompra = ValorCompra;
            float _valorFinal = _valorCompra + _valorIncremento - _valorDescuento + _valorImpuesto;
            ValorFinal = _valorFinal;
        }

        public void CalculaValores()
        {
            float _valorCompra = (float)Math.Round(ValorUnitario * Cantidad,2);
            float _cantidadSaldoFinal = PoseeStock == 1 ? 
                                                         (string.IsNullOrEmpty(TipoOrden)? CantidadSaldoExistente - (float)Math.Round((Cantidad * ValorConversion), 4) 
                                                         :(TipoOrden.ToUpper().Contains("NVENTAR")? CantidadSaldoExistente + (float)Math.Round((Cantidad * ValorConversion), 4)
                                                            : CantidadSaldoExistente - (float)Math.Round((Cantidad * ValorConversion), 4))
                                                         )
                                                        : 1;            
            ValorCompra = _valorCompra;
            CantidadSaldoFinal = PoseeStock == 1 ? _cantidadSaldoFinal :1;
        }

        public Task BindData(OrdenDetalleCompraDto apiDto, DbContext dbContext)
        {            
            IdOrden = apiDto.IdOrden;
            IdItem = apiDto.IdItem;
            NombreItem = apiDto.NombreItem;
            IdUnidad = apiDto.IdUnidad;
            Unidad = apiDto.Unidad;
            Cantidad = apiDto.Cantidad;
            ValorUnitario = apiDto.ValorUnitario;
            //ValorCompra = apiDto.ValorCompra;
            ValorDescuento = apiDto.ValorDescuento;
            ValorFinal = apiDto.ValorFinal;
            FechaVencimiento = apiDto.FechaVencimiento;            
            return Task.FromResult(1);
        }
        
    }
    public class OrdenDetalleCompraDto : AuditableEntity
    {
        public string IdOrden { get; set; }
        public string IdItem { get; set; }
        public string NombreItem { get; set; }
        public string IdUnidad { get; set; }
        public string Unidad { get; set; }
        public int Cantidad { get; set; }
        public float ValorUnitario { get; set; }
        public float ValorCompra { get; set; }
        public float ValorDescuento { get; set; }
        public float ValorFinal { get; set; }
        public long? FechaVencimiento { get; set; }
    }

    public class OrdenDetalleCompraSerializado
    {
        public string IdOrden { get; set; }
        public string IdItem { get; set; }
        public string NombreItem { get; set; }
        public string Imagen { get; set; }
        public string IdUnidad { get; set; }
        public string Unidad { get; set; }
        public string FechaVencimiento { get; set; }
        public string Cantidad { get; set; }
        public string ValorUnitario { get; set; }
        public string ValorCompra { get; set; }
        public string ValorIncremento { get; set; }
        public string TipoDescuento { get; set; }
        public string ValorDescuento { get; set; }
        public string ValorImpuesto { get; set; }        
        public string ValorFinal { get; set; }
        public string MontoPagoTotal { get; set; }
        public string EstadoCuota { get; set; }

    }


}
