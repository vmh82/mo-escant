using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using DataModel.Base;
using DataModel.DTO.Productos;
using DataModel.DTO.Gestion;
using DataModel.DTO.Facturacion;

namespace DataModel.DTO.Compras
{
    public class Orden : BaseModel, ISyncTable<OrdenDto>
    {
        public string IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string IdCliente { get; set; }
        public string NombreCliente { get; set; }        
        public string Identificacion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string IdTipoOrden { get; set; }
        public string TipoOrden { get; set; }
        public string TipoInventario { get; set; }
        [Ignore]
        public string _numeroOrden { get; set; }
        public string NumeroOrden
        {
            get => _numeroOrden;
            set
            {
                _numeroOrden = value;
                OnPropertyChanged("NumeroOrden");
            }
        }
        public string NumeroOrdenPeriodo { get; set; }
        public string IdEstado { get; set; }
        public string CodigoEstado { get; set; }
        public string IdCondicion { get; set; }
        public string Condicion { get; set; }
        public float DiasCredito { get; set; }
        public float ImporteCredito { get; set; }
        public string FechaVencimientoCredito { get; set; }
        [Ignore]
        public string CodigoEstadoEnDesarrollo { get; set; }
        [Ignore]
        public string CodigoEstadoEnConsulta { get; set; }
        public long Fecha { get; set; }
        public string Imagen { get; set; }
        public string IdGaleria { get; set; }        
        [Ignore]
        public float _cantidad { get; set; }
        public float Cantidad
        {
            get => _cantidad;
            set
            {
                _cantidad = value;
                OnPropertyChanged("Cantidad");
            }
        }
        public int CantidadPromocion { get; set; }
        public int CantidadTotal { get; set; }
        [Ignore]
        public float _valorTotal { get; set; }
        public float ValorTotal
        {
            get => _valorTotal;
            set
            {
                _valorTotal = value;
                OnPropertyChanged("ValorTotal");
            }
        }       
        public string IdTipoDescuento { get; set; }
        public string TipoDescuento { get; set; }
        public float _valorDescuento { get; set; }
        public float ValorDescuento
        {
            get => _valorDescuento;
            set
            {
                _valorDescuento = value;
                OnPropertyChanged("ValorDescuento");
                calculaValorFinal();
                aplicaOrdenPago();
            }
        }
        public float _valorImpuesto { get; set; }
        public float ValorImpuesto {
            get
            {
                return _valorImpuesto;
            }
            set
            {
                _valorImpuesto = value;
                OnPropertyChanged("ValorImpuesto");
            }
        }
        public float _valorIncremento { get; set; }
        public float ValorIncremento {
            get
            {
                return _valorIncremento;
            }
            set
            {
                _valorIncremento = value;
                OnPropertyChanged("ValorIncremento");
            }
        }
        [Ignore]
        public float _valorFinal { get; set; }
        public float ValorFinal
        {
            get
            {
                return _valorFinal;
            }
                set
            {
                _valorFinal = value;
                OnPropertyChanged("ValorFinal");
            }
        }         
        [Ignore]
        public string _observaciones { get; set; }
        public string Observaciones
        {
            get => _observaciones;
            set
            {
                _observaciones = value;
                OnPropertyChanged("Observaciones");
            }
        }

        public ObservableCollection<OrdenEstado> _ordenEstado;
        [Ignore]
        public ObservableCollection<OrdenEstado> OrdenEstado
        {
            get => _ordenEstado;
            set
            {
                _ordenEstado = value;
                OnPropertyChanged("OrdenEstado");
            }
        }
        public ObservableCollection<OrdenDetalleCompra> _ordenDetalleCompras;
        [Ignore]
        public ObservableCollection<OrdenDetalleCompra> OrdenDetalleCompras {
            get => _ordenDetalleCompras;
            set
            {
                _ordenDetalleCompras = value;
                OnPropertyChanged("OrdenDetalleCompras");                                
            }
        }
        public ObservableCollection<OrdenDetalleCompra> _ordenDetalleComprasCancelado;
        [Ignore]
        public ObservableCollection<OrdenDetalleCompra> OrdenDetalleComprasCancelado
        {
            get => _ordenDetalleComprasCancelado;
            set
            {
                _ordenDetalleComprasCancelado = value;
                OnPropertyChanged("OrdenDetalleComprasCancelado");
            }
        }
        public ObservableCollection<ItemEntrega> _ordenDetalleComprasEntregado;
        [Ignore]
        public ObservableCollection<ItemEntrega> OrdenDetalleComprasEntregado
        {
            get => _ordenDetalleComprasEntregado;
            set
            {
                _ordenDetalleComprasEntregado = value;
                OnPropertyChanged("OrdenDetalleComprasEntregado");
            }
        }
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
        [Ignore]
        public float CantidadTmp => OrdenDetalleCompras!=null?OrdenDetalleCompras.Sum(x => x.Cantidad):0;
        [Ignore]
        public float ValorCompraTmp => OrdenDetalleCompras != null ? OrdenDetalleCompras.Sum(x => x.ValorCompra):0;
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }
        public ObservableCollection<OrdenPago> _ordenPago;
        [Ignore]
        public ObservableCollection<OrdenPago> OrdenPago
        {
            get => _ordenPago;
            set
            {
                _ordenPago = value;
                OnPropertyChanged("OrdenPago");
            }
        }
        public string OrdenDetalleCompraCanceladoSerializado { get; set; }
        public string OrdenDetalleCompraEntregadoSerializado { get; set; }
        public string OrdenDetalleCompraSerializado { get; set; }
        public string OrdenDetalleCompraRecibidoSerializado { get; set; }
        public string OrdenEstadoCompraSerializado { get; set; }
        [Ignore]
        public ObservableCollection<TablaPago> OrdenTablaPago { get; set; }
        public string OrdenTablaPagoSerializado { get; set; }
        public string OrdenPagoSerializado { get; set; }
        public string NumeroFactura { get; set; }
        public string AdjuntoFactura { get; set; }
        public string AdjuntoCancelado { get; set; }
        [Ignore]
        public float _valorCancelado { get; set; }
        public float ValorCancelado
        {
            get => _valorCancelado;
            set
            {
                _valorCancelado = value;
                OnPropertyChanged("ValorCancelado");
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

        [Ignore]
        public float _saldoCancelado { get; set; }
        public float SaldoCancelado
        {
            get => _saldoCancelado;
            set
            {
                _saldoCancelado = value;
                OnPropertyChanged("SaldoCancelado");
            }
        }
        public string EstadoCancelado { get; set; }
        public int ConfirmadaTransaccionPago { get; set; }
        [Ignore]
        public int ContinuaNavegacion { get; set; }
        [Ignore]
        public ObservableCollection<GestionProceso> ListaGestion { get; set; }
        public string GestionSerializado { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string FechaFinalizado { get; set; }
        [Ignore]
        public string _numeroSecuencialFactura { get; set; }
        public string NumeroSecuencialFactura
        {
            get => _numeroSecuencialFactura;
            set
            {
                _numeroSecuencialFactura = value;
                OnPropertyChanged("NumeroSecuencialFactura");
            }
        }
        [Ignore]
        public ObservableCollection<ParametroComprobante> oFacturacion { get; set; }
        public string FacturacionSerializado { get; set; }
        [Ignore]
        public DateTime FechaDesde { get; set; }
        [Ignore]
        public DateTime FechaHasta { get; set; }
        [Ignore]
        public DateTime OrdenFechaValue => new DateTime(1970, 1, 1).AddMilliseconds(Fecha);
        [Ignore]
        public DateTime FechaReporte { get; set; }
        [Ignore]
        public string ParametrosImpresion { get; set; }
        [Ignore]
        public int Seleccionada { get; set; }
        public void calculaValorFinal()
        {
            float _valorDescuento = ValorDescuento;
            float _valorIncremento = ValorIncremento;
            float _valorImpuesto = ValorImpuesto;
            float _valorCompra = ValorTotal;
            float _valorFinal = _valorCompra + _valorIncremento - _valorDescuento + _valorImpuesto;
            ValorFinal = _valorFinal;
        }
        public void aplicaOrdenPago()
        {
            if(OrdenPago!=null && OrdenPago.Count>0)
            {
                foreach(var item in OrdenPago)
                {
                    item.MontoPagoTotal = ValorFinal;
                }
                
            }
        }
        public Task BindData(OrdenDto apiDto, DbContext dbContext)
        {
            IdProveedor = apiDto.IdProveedor;
            IdCliente = apiDto.IdCliente;
            NombreCliente = apiDto.NombreCliente;
            NombreProveedor = apiDto.NombreProveedor;
            Identificacion = apiDto.Identificacion;
            IdTipoOrden = apiDto.IdTipoOrden;
            TipoOrden = apiDto.TipoOrden;
            NumeroOrden = apiDto.NumeroOrden;
            NumeroOrdenPeriodo = apiDto.NumeroOrdenPeriodo;
            IdEstado = apiDto.IdEstado;
            CodigoEstado = apiDto.CodigoEstado;
            Fecha = apiDto.Fecha;
            ValorTotal = apiDto.ValorTotal;
            IdTipoDescuento = apiDto.IdTipoDescuento;
            TipoDescuento = apiDto.TipoDescuento;
            ValorDescuento = apiDto.ValorDescuento;
            ValorImpuesto = apiDto.ValorImpuesto;
            ValorFinal = apiDto.ValorFinal;
            Observaciones = apiDto.Observaciones;
            return Task.FromResult(1);
        }        
    }
    public class OrdenDto : AuditableEntity
    {
        public string IdProveedor { get; set; }
        public string NombreProveedor { get; set; }                
        public string IdCliente { get; set; }
        public string NombreCliente { get; set; }        
        public string Identificacion { get; set; }
        public string IdTipoOrden { get; set; }
        public string TipoOrden { get; set; }
        public string NumeroOrden { get; set; }
        public string NumeroOrdenPeriodo { get; set; }
        public string IdEstado { get; set; }
        public string CodigoEstado { get; set; }
        public long Fecha { get; set; }
        public float ValorTotal { get; set; }
        public string IdTipoDescuento { get; set; }
        public string TipoDescuento { get; set; }
        public float ValorDescuento { get; set; }
        public float ValorImpuesto { get; set; }
        public float ValorFinal { get; set; }
        public string Observaciones { get; set; }
    }
}
