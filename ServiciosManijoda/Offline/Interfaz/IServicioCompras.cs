using DataModel.DTO.Compras;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Offline.Interfaz
{
    #region Orden
    public interface IServicioCompras_Orden
    {
        Task<ObservableCollection<Orden>> GetAllOrdenAsync(Expression<Func<Orden, bool>> expression);
        Task<string> InsertOrdenAsync(Orden objeto);
        Task<int> ImportOrdenAsync(IEnumerable<Orden> objetos);
        Task UpdateOrdenAsync(Orden objeto);
        Task DeleteOrdenAsync(Orden objeto);
    }
    #endregion Orden

    #region OrdenDetalleCompra
    public interface IServicioCompras_OrdenDetalleCompra
    {
        Task<ObservableCollection<OrdenDetalleCompra>> GetAllOrdenDetalleCompraAsync(Expression<Func<OrdenDetalleCompra, bool>> expression);
        Task<string> InsertOrdenDetalleCompraAsync(OrdenDetalleCompra objeto);
        Task<int> ImportOrdenDetalleCompraAsync(IEnumerable<OrdenDetalleCompra> objetos);
        Task UpdateOrdenDetalleCompraAsync(OrdenDetalleCompra objeto);
        Task DeleteOrdenDetalleCompraAsync(OrdenDetalleCompra objeto);
    }
    #endregion OrdenDetalleCompra

    #region OrdenEstado
    public interface IServicioCompras_OrdenEstado
    {
        Task<ObservableCollection<OrdenEstado>> GetAllOrdenEstadoAsync(Expression<Func<OrdenEstado, bool>> expression);
        Task<string> InsertOrdenEstadoAsync(OrdenEstado objeto);
        Task<int> ImportOrdenEstadoAsync(IEnumerable<OrdenEstado> objetos);
        Task UpdateOrdenEstadoAsync(OrdenEstado objeto);
        Task DeleteOrdenEstadoAsync(OrdenEstado objeto);
    }
    #endregion OrdenEstado

}
