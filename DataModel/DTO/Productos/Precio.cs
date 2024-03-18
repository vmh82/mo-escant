using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using DataModel.Helpers;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;
using DataModel.DTO.Iteraccion;

namespace DataModel.DTO.Productos
{
    public class Precio : Base.BaseModel, ISyncTable<PrecioDto>
    {        
        public string IdItem { get; set; }
        [DisplayName("Código")]
        public string CodigoItem { get; set; }
        public string NombreItem { get; set; }
        public string IdUnidad { get; set; }
        public string Unidad { get; set; }
        public float ValorUnidadOriginal { get; set; }
        public string IdUnidadPadre { get; set; }
        public string IdUnidadConvertida { get; set; }
        public string UnidadConvertida { get; set; }
        public float ValorConversion { get; set; }
        public float PrecioVentaSugerido1 { get; set; }
        public float PrecioVentaSugerido2 { get; set; }
        public float PrecioVenta { get; set; }
        public long? FechaDesde { get; set; }
        [Ignore]
        public string FechaDesdeStr => String.Format("{0:G}", DateTimeHelpers.GetDateLong(FechaDesde));
        [Ignore]
        public string FechaHastaStr => String.Format("{0:G}", DateTimeHelpers.GetDateLong(FechaHasta));
        public long? FechaHasta { get; set; }
        public int EstaActivo { get; set; }        
        [Ignore]
        public string PrecioVentaStr => PrecioVenta.FormatoPrecioSinSimbolo();
        [Ignore]
        public ObservableCollection<Precio> subCatalogos { get; set; }
        [Ignore]
        public IEnumerable<Precio> subCatalogos1 { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }
        [Ignore]
        public FuenteIconos IconoMenu { get; set; }
        [Ignore]
        public FuenteIconos IconoEstaActivo { get; set; }

        public Task BindData(PrecioDto apiDto, DbContext dbContext)
        {
            Id = apiDto.Id;
            IdItem = apiDto.IdItem;
            CodigoItem = apiDto.CodigoItem;
            NombreItem = apiDto.NombreItem;
            IdUnidad = apiDto.IdUnidad;
            Unidad = apiDto.Unidad;
            ValorUnidadOriginal = apiDto.ValorUnidadOriginal;
            IdUnidadPadre = apiDto.IdUnidadPadre;
            IdUnidadConvertida = apiDto.IdUnidadConvertida;
            UnidadConvertida = apiDto.UnidadConvertida;
            ValorConversion = apiDto.ValorConversion;
            PrecioVentaSugerido1 = apiDto.PrecioVentaSugerido1;
            PrecioVentaSugerido2 = apiDto.PrecioVentaSugerido2;
            PrecioVenta = apiDto.PrecioVenta;
            FechaDesde = apiDto.FechaDesde;
            FechaHasta = apiDto.FechaHasta;
            EstaActivo = apiDto.EstaActivo;            
            return Task.FromResult(1);
        }
    }
    public class PrecioDto : AuditableEntity
    {
        public string IdItem { get; set; }
        public string CodigoItem { get; set; }
        public string NombreItem { get; set; }
        public string IdUnidad { get; set; }
        public string Unidad { get; set; }
        public float ValorUnidadOriginal { get; set; }
        public string IdUnidadPadre { get; set; }
        public string IdUnidadConvertida { get; set; }
        public string UnidadConvertida { get; set; }
        public float ValorConversion { get; set; }
        public float PrecioVentaSugerido1 { get; set; }
        public float PrecioVentaSugerido2 { get; set; }
        public float PrecioVenta { get; set; }
        public long? FechaDesde { get; set; }
        public long? FechaHasta { get; set; }
        public int EstaActivo { get; set; }
    }
}
