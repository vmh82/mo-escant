using DataModel;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DataModel.DTO;
using DataModel.DTO.Clientes;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    public class ClienteRepositorio : GenericSqliteRepository<Cliente>, IClienteRepositorio
    {
        public ClienteRepositorio(DbContext context) : base(context)
        {
        }
    }
    public class PersonaRepositorio : GenericSqliteRepository<Persona>, IPersonaRepositorio
    {
        public PersonaRepositorio(DbContext context) : base(context)
        {
        }
    }
}
