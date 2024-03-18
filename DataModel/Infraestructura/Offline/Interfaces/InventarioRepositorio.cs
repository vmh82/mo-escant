using DataModel.Infraestructura.Offline.DB;
using DataModel.DTO.Inventario;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    #region StockResumenRepositorio
    public class StockResumenRepositorio : GenericSqliteRepository<StockResumen>, IStockResumenRepositorio
    {
        public StockResumenRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion StockResumenRepositorio
}
