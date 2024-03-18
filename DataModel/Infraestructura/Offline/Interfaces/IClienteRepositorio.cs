using DataModel;
using DataModel.DTO.Clientes;
using DataModel.Infraestructura.Offline.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    public interface IClienteRepositorio : IGenericRepository<Cliente>
    {
    }
    public interface IPersonaRepositorio : IGenericRepository<Persona>
    {
    }
}
