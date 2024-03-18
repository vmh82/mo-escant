using DataModel;
using DataModel.DTO.Seguridad;
using DataModel.Infraestructura.Offline.DB;
using System;
using System.Collections.Generic;
using System.Text;


namespace DataModel.Infraestructura.Offline.Interfaces
{
    public interface ISeguridadRepositorio : IGenericRepository<Usuario>
    {
    }
    public interface IUsuarioRepositorio : IGenericRepository<Usuario>
    {
    }
    public interface IPerfilMenuRepositorio : IGenericRepository<PerfilMenu>
    {
    }
}
