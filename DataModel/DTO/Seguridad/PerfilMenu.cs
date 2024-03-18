using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using DataModel.Base;
using System.Collections.ObjectModel;
using DataModel.DTO.Iteraccion;

namespace DataModel.DTO.Seguridad
{
    public class PerfilMenu : BaseModel, ISyncTable<PerfilMenuDto>
    {        
        public string IdPerfil { get; set; }
        public string CodigoPerfil { get; set; }
        public string IdMenu { get; set; }
        public string IdMenuPadre { get; set; }
        public string NombreMenu { get; set; }
        public string NombreFormulario { get; set; }
        public string LabelTitulo { get; set; }
        public string LabelDescripcion { get; set; }
        public string ImageIcon { get; set; }
        public int Nivel { get; set; }
        public int Orden { get; set; }
        public int EstaActivo { get; set; }
        [Ignore]
        public ObservableCollection<PerfilMenu> SubCatalogos { get; set; }
        [Ignore]
        public string ValorBusqueda { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public FuenteIconos IconoMenu { get; set; }
        [Ignore]
        public FuenteIconos IconoEstaActivo { get; set; }
        public Task BindData(PerfilMenuDto apiDto, DbContext dbContext)
        {
            Id = apiDto.Id;
            IdPerfil = apiDto.IdPerfil;
            CodigoPerfil = apiDto.CodigoPerfil;
            IdMenu = apiDto.IdMenu;
            NombreMenu = apiDto.NombreMenu;
            IdMenuPadre = apiDto.IdMenuPadre;
            NombreFormulario = apiDto.NombreFormulario;
            LabelTitulo = apiDto.LabelTitulo;
            LabelDescripcion = apiDto.LabelDescripcion;
            ImageIcon = apiDto.ImageIcon;
            Nivel = apiDto.Nivel;
            EstaActivo = apiDto.EstaActivo;            
            return Task.FromResult(1);
        }

    }
    public class PerfilMenuDto : AuditableEntity
    {        
        public string IdPerfil { get; set; }
        public string CodigoPerfil { get; set; }
        public string IdMenu { get; set; }
        public string IdMenuPadre { get; set; }
        public string NombreMenu { get; set; }
        public string NombreFormulario { get; set; }
        public string LabelTitulo { get; set; }
        public string LabelDescripcion { get; set; }
        public string ImageIcon { get; set; }
        public int Nivel { get; set; }
        public int EstaActivo { get; set; }

    }
}
