using System.ComponentModel;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using SQLiteNetExtensions.Attributes;

namespace DataModel.DTO.Administracion
{
    public class Proveedor:Base.BaseModel, ISyncTable<ProveedorDto>
    {
        public string IdEmpresa { get; set; }
        public string IdTipoProveedor { get; set; }
        public int EsContribuyenteEspecial { get; set; }
        public int PoseeFacturaElectronica { get; set; }
        [ManyToOne]
        public Catalogo TipoProveedor { get; set; }
        [Ignore]
        public string NombreTipoProveedor { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        public Task BindData(ProveedorDto apiDto, DbContext dbContext)
        {

            Id = apiDto.Id;
            IdEmpresa = apiDto.IdEmpresa;
            IdTipoProveedor = apiDto.IdTipoProveedor;
            EsContribuyenteEspecial = apiDto.EsContribuyenteEspecial;
            PoseeFacturaElectronica = apiDto.PoseeFacturaElectronica;            
            return Task.FromResult(1);
        }
    }
    public class ProveedorDto : AuditableEntity
    {
        public string IdEmpresa { get; set; }
        public string IdTipoProveedor { get; set; }
        public int EsContribuyenteEspecial { get; set; }
        public int PoseeFacturaElectronica { get; set; }
    }
}
