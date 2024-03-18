using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using Syncfusion.XForms.ProgressBar;

namespace DataModel.DTO.Compras
{
    public class OrdenEstado : Base.BaseModel, ISyncTable<OrdenEstadoDto>
    {
        public string IdOrden { get; set; }
        public string NumeroOrden { get; set; }
        public string IdEstado { get; set; }
        public string Estado { get; set; }
        public string Status { get; set; }
        public string Adjunto { get; set; }
        public string PoseeAdjunto { get; set; }
        public string Observaciones { get; set; }
        public long? FechaProceso { get; set; }
        [Ignore]
        public string FechaProcesoStr { get; set; }
        [Ignore]
        public StepStatus StatusProgreso { get; set; }
        [Ignore]
        public int ValorProgreso { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }

        public Task BindData(OrdenEstadoDto apiDto, DbContext dbContext)
        {
            Id = apiDto.Id;
            IdOrden = apiDto.IdOrden;
            NumeroOrden = apiDto.NumeroOrden;
            IdEstado = apiDto.IdEstado;
            Estado = apiDto.Estado;
            FechaProceso = apiDto.FechaProceso;
            return Task.FromResult(1);
        }
    }
    public class OrdenEstadoDto : AuditableEntity
    {
        public string IdOrden { get; set; }
        public string NumeroOrden { get; set; }
        public string IdEstado { get; set; }
        public string Estado { get; set; }
        public long? FechaProceso { get; set; }
    }
}
