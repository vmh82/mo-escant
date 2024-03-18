using DataModel;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DataModel.DTO;
using DataModel.DTO.Seguridad;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    public class SeguridadRepositorio : GenericSqliteRepository<Usuario>, ISeguridadRepositorio
    {
        public SeguridadRepositorio(DbContext context) : base(context)
        {
        }
    }
    #region PerfilMenu
    /// <summary>
    /// Usuario
    /// </summary>
    public class PerfilMenuRepositorio : GenericSqliteRepository<PerfilMenu>, IPerfilMenuRepositorio
    {
        public PerfilMenuRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion PerfilMenu
    #region Usuario
    /// <summary>
    /// Usuario
    /// </summary>
    public class UsuarioRepositorio : GenericSqliteRepository<Usuario>, IUsuarioRepositorio
    {
        public UsuarioRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion Usuario
}
