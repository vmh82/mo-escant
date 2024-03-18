using DataModel.DTO.Compras;
using DataModel.Infraestructura.Offline.DB;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    #region Orden
    public class OrdenRepositorio : GenericSqliteRepository<Orden>, IOrdenRepositorio
    {
        public OrdenRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion Orden
    #region OrdenDetalleCompra
    public class OrdenDetalleCompraRepositorio : GenericSqliteRepository<OrdenDetalleCompra>, IOrdenDetalleCompraRepositorio
    {
        public OrdenDetalleCompraRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion OrdenDetalleCompra
    #region OrdenEstado
    public class OrdenEstadoRepositorio : GenericSqliteRepository<OrdenEstado>, IOrdenEstadoRepositorio
    {
        public OrdenEstadoRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion OrdenEstado
}
