using DataModel.DTO.Compras;
using DataModel.Infraestructura.Offline.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    public interface IOrdenRepositorio : IGenericRepository<Orden>
    {
    }
    public interface IOrdenDetalleCompraRepositorio : IGenericRepository<OrdenDetalleCompra>
    {
    }
    public interface IOrdenEstadoRepositorio : IGenericRepository<OrdenEstado>
    {
    }
}
