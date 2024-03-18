using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using DataModel.Base;

namespace DataModel.DTO.Seguridad
{
    public class Usuario : BaseModel, ISyncTable<UsuarioDto>
    {
        [Ignore]
        public string Nombre { get; set; }        
        public string IdPersona { get; set; }
        public string Nombres { get; set; }
        
        public string Apellidos { get; set; }
        
        public string Identificacion { get; set; }        
        public string Celular { get; set; }

        public string Email { get; set; }
        [Ignore]
        public string EmailAnterior { get; set; }

        public string Clave { get; set; }
        [Ignore]
        public string ClaveAnterior { get; set; }
        public string PinDigitos { get; set; }

        public string IdToken { get; set; }
        [Ignore]
        public string RefreshToken { get; set; }
        [Ignore]
        public string Telefono { get; set; }
        public string IdGaleria { get; set; }
        public string Imagen { get; set; }
        public string IdPerfil { get; set; }
        public string Perfil { get; set; }
        public int EstaActivo { get; set; }
        [Ignore]
        public string[] Roles { get; set; }
        [Ignore]
        public string[] Permisos { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public string Origen { get; set; }
        [Ignore]
        public int TipoConstructor { get; set; }
        public Task BindData(UsuarioDto apiDto, DbContext dbContext)
        {
            Id = apiDto.Id;
            Nombres = apiDto.Nombres;
            Apellidos = apiDto.Apellidos;
            Identificacion = apiDto.Identificacion;            
            Email = apiDto.Email;
            Clave = apiDto.Clave;
            IdToken = apiDto.IdToken;
            IdGaleria= apiDto.IdGaleria;
            Imagen = apiDto.Imagen;
            IdPerfil = apiDto.IdPerfil;
            Perfil = apiDto.Perfil;
            EstaActivo = apiDto.EstaActivo;
            return Task.FromResult(1);
        }

    }
    public class UsuarioDto : AuditableEntity
    {
        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Identificacion { get; set; }

        public string Email { get; set; }

        public string Clave { get; set; }

        public string IdToken { get; set; }
        public string IdGaleria { get; set; }
        public string Imagen { get; set; }
        public string IdPerfil { get; set; }
        public string Perfil { get; set; }
        public int EstaActivo { get; set; }
    }
}
