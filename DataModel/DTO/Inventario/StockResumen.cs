using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;

namespace DataModel.DTO.Inventario
{
    public class StockResumen : Base.BaseModel, ISyncTable<StockResumenDto>
    {        
        public string IdItem { get; set; }
        public string NombreItem { get; set; }
        public string IdUnidad { get; set; }
        public string Unidad { get; set; }
        public long? FechaVencimiento { get; set; }
        public float Cantidad { get; set; }
        public float StockMinimo { get; set; }        
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }

        public Task BindData(StockResumenDto apiDto, DbContext dbContext)
        {
            IdItem = apiDto.IdItem;
            NombreItem = apiDto.NombreItem;
            IdUnidad = apiDto.IdUnidad;
            Unidad = apiDto.Unidad;
            FechaVencimiento = apiDto.FechaVencimiento;
            Cantidad = apiDto.Cantidad;
            StockMinimo = apiDto.StockMinimo;
            CreatedBy = apiDto.CreatedBy;
            return Task.FromResult(1);
        }
    }
    public class StockResumenDto : AuditableEntity
    {
        public string IdItem { get; set; }
        public string NombreItem { get; set; }
        public string IdUnidad { get; set; }
        public string Unidad { get; set; }
        public long? FechaVencimiento { get; set; }
        public float Cantidad { get; set; }
        public float StockMinimo { get; set; }
    }
}
