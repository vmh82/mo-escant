using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataModel.DTO.Productos
{
    public class Item : Base.BaseModel, ISyncTable<ItemDto>
    {        
        public int EsServicio { get; set; }
        public string IdPeriodoServicio { get; set; }
        public string PeriodoServicio { get; set; }
        public float CantidadPeriodoServicio { get; set; }        
        public string Codigo { get; set; }
        public string CodigoExterno { get; set; }
        public string Nombre { get; set; }
        public string IdMarca { get; set; }
        public string Marca { get; set; }
        public string IdCategoria { get; set; }
        public string Categoria { get; set; }
        public string IdUnidad { get; set; }
        public string Unidad { get; set; }
        public string IdTipoImpuesto { get; set; }
        public string TipoImpuesto { get; set; }
        public float PorcentajeImpuesto { get; set; }
        public string IdTipoAplicacionImpuesto { get; set; }
        public string TipoAplicacionImpuesto { get; set; }
        public float ValorImpuesto { get; set; }
        public string IdPeriodoIncremento { get; set; }
        public string PeriodoIncremento { get; set; }
        public float ValorPeriodoIncremento { get; set; }
        public float ValorIncremento { get; set; }
        public float ValorUnidadOriginal { get; set; }
        public int EsParticionable { get; set; }
        public string IdGaleria { get; set; }
        public string Imagen { get; set; }
        public string GaleriaImagen { get; set; }
        public int StockMinimo { get; set; }
        public float Cantidad { get; set; }
        public float CantidadDevuelta { get; set; }
        public float SaldoExistente { get; set; }
        public float PorcentajeRentabilidad { get; set; }
        public float PorcentajeGastoOperativo { get; set; }
        public float PrecioBase { get; set; }
        public float PrecioCompra { get; set; }
        public float PrecioVenta { get; set; }
        public float PrecioVentaImpuesto { get; set; }
        public float ValorRentabilidad { get; set; }
        public float ValorGastoOperativo { get; set; }
        public float ValorUtilidad { get; set; }
        public string Descripcion { get; set; }
        /* Sumarización stock*/
        [Ignore]
        public float _cantidadInventarioInicial { get; set; }
        public float CantidadInventarioInicial
        {
            get => _cantidadInventarioInicial;
            set
            {
                _cantidadInventarioInicial = value;
                OnPropertyChanged("CantidadInventarioInicial");
                CalculosStock();
            }
        }
        [Ignore]
        public float _cantidadCompra { get; set; }
        public float CantidadCompra
        {
            get => _cantidadCompra;
            set
            {
                _cantidadCompra = value;
                OnPropertyChanged("CantidadCompra");
                CalculosStock();
            }
        }
        [Ignore]
        public float _cantidadEntrada { get; set; }
        public float CantidadEntrada
        {
            get => _cantidadEntrada;
            set
            {
                _cantidadEntrada = value;
                OnPropertyChanged("CantidadEntrada");
                
            }
        }
        [Ignore]
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
                CalculosStock();
            }
        }
        [Ignore]
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
                //calculaSalida();
                //calculaSaldos();
            }
        }
        [Ignore]
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
                //calculaSalida();
                //calculaSaldos();
            }
        }
        [Ignore]
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
                //calculaSalida();
                //calculaSaldos();
            }
        }
        [Ignore]
        public float _cantidadReservada { get; set; }
        public float CantidadReservada
        {
            get => _cantidadReservada;
            set
            {
                _cantidadReservada = value;
                /*if (_cantidadReservada > 0 && _cantidadReservada + CantidadSalida > CantidadEntrada)
                    _cantidadReservada = CantidadEntrada - CantidadSalida;*/
                OnPropertyChanged("CantidadReservada");
                CalculosStock();
                //calculaSalida();
                //calculaSaldos();
            }
        }
        [Ignore]
        public float _cantidadVenta { get; set; }
        public float CantidadVenta
        {
            get => _cantidadVenta;
            set
            {
                _cantidadVenta = value;
                /*if (_cantidadVenta > 0 && _cantidadVenta + CantidadSalida > CantidadEntrada)
                    _cantidadVenta = CantidadEntrada - CantidadSalida;*/
                OnPropertyChanged("CantidadVenta");
                CalculosStock();
            }
        }
        [Ignore]
        public float _cantidadSalida { get; set; }
        public float CantidadSalida
        {
            get => _cantidadSalida;
            set
            {
                _cantidadSalida = value;
                OnPropertyChanged("CantidadSalida");
            }
        }
        [Ignore]
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
        /* Sumarización de stock */
        [Ignore]
        public ObservableCollection<ItemStock> _itemStockRegistrado { get; set; }
        [Ignore]
        public ObservableCollection<ItemStock> ItemStockRegistrado
        {
            get => _itemStockRegistrado;
            set
            {
                _itemStockRegistrado = value;
                OnPropertyChanged("ItemStockRegistrado");
            }
        }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }

        public string ItemStockRegistradoSerializado { get; set; }
        [Ignore]
        public string _fechaMinimaIngreso { get; set; }
        public string FechaMinimaIngreso
        {
            get => _fechaMinimaIngreso;
            set
            {
                _fechaMinimaIngreso = value;
                OnPropertyChanged("FechaMinimaIngreso");
            }
        }
        public string PreciosItemSerializado { get; set; }
        public int PoseeStock { get; set; }
        public int PoseeInventarioInicial { get; set; }
        /*
        public Item()
        {
            ItemStockRegistrado = new ObservableCollection<ItemStock>();
            ItemStockRegistrado.CollectionChanged += ItemStockRegistrado_CollectionChanged;
        }

        private void ItemStockRegistrado_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemPropertyChanged;
            if (e.OldItems == null) return;
            {
                foreach (var item in e.OldItems)
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemPropertyChanged;
            }
        }
        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CalculosStock();
        }
        */
        private void CalculosStock()
        {
            CantidadEntrada = CantidadInventarioInicial + CantidadCompra;
            CantidadSalida = CantidadInventarioConcilia + CantidadDevuelto + CantidadTraslado + CantidadOtros + CantidadReservada + CantidadVenta;
            CantidadSaldoFinal = PoseeStock==1?(CantidadEntrada - CantidadSalida):(EsServicio==1?1: (CantidadEntrada - CantidadSalida));
        }

        public Task BindData(ItemDto apiDto, DbContext dbContext)
        {
            Id = apiDto.Id;
            EsServicio = apiDto.EsServicio;
            Codigo = apiDto.Codigo;
            Nombre = apiDto.Nombre;
            IdMarca = apiDto.IdMarca;
            Marca = apiDto.Marca;
            IdCategoria = apiDto.IdCategoria;
            Categoria = apiDto.Categoria;
            IdUnidad = apiDto.IdUnidad;
            Unidad = apiDto.Unidad;
            ValorUnidadOriginal = (float)(apiDto.ValorUnidadOriginal ?? 0); 
            EsParticionable = apiDto.EsParticionable;
            IdGaleria = apiDto.IdGaleria;
            Imagen = apiDto.Imagen;
            GaleriaImagen = apiDto.GaleriaImagen;
            StockMinimo = apiDto.StockMinimo;
            PrecioCompra = (float)(apiDto.PrecioCompra ?? 0);
            PrecioVenta = (float)(apiDto.PrecioVenta ?? 0);
            Descripcion = apiDto.Descripcion;            
            return Task.FromResult(1);
        }
    }
    public class ItemDto : AuditableEntity
    {
        public int EsServicio { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string IdMarca { get; set; }
        public string Marca { get; set; }
        public string IdCategoria { get; set; }
        public string Categoria { get; set; }
        public string IdUnidad { get; set; }
        public string Unidad { get; set; }
        public decimal? ValorUnidadOriginal { get; set; }
        public int EsParticionable { get; set; }
        public string IdGaleria { get; set; }
        public string Imagen { get; set; }
        public string GaleriaImagen { get; set; }
        public int StockMinimo { get; set; }
        public decimal? PrecioCompra { get; set; }
        public decimal? PrecioVenta { get; set; }
        public string Descripcion { get; set; }
    }

}
