using System.ComponentModel;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using Xamarin.Forms;
using System.IO;
using System;

namespace DataModel.DTO.Clientes
{
    public class Persona : Base.BaseModel, ISyncTable<PersonaDto>
    {        
        public string Identificacion { get; set; }        
        public int EsNatural { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }
        public Task BindData(PersonaDto apiDto, DbContext dbContext)
        {

            Id = apiDto.Id;
            Identificacion = apiDto.Identificacion;
            EsNatural = apiDto.EsNatural;            
            return Task.FromResult(1);
        }

    }
    public class PersonaDto : AuditableEntity
    {
        public string Identificacion { get; set; }
        public int EsNatural { get; set; }
    }
}
