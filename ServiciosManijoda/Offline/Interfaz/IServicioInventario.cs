using DataModel.DTO.Inventario;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Offline.Interfaz
{
    #region StockResumen
    public interface IServicioInventario_StockResumen
    {
        Task<ObservableCollection<StockResumen>> GetAllStockResumenAsync(Expression<Func<StockResumen, bool>> expression);
        Task<string> InsertStockResumenAsync(StockResumen objeto);
        Task<int> ImportStockResumenAsync(IEnumerable<StockResumen> objetos);
        Task UpdateStockResumenAsync(StockResumen objeto);
        Task DeleteStockResumenAsync(StockResumen objeto);
    }
    #endregion StockResumen
}
