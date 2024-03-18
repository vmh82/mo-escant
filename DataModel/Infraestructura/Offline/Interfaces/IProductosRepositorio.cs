using DataModel.DTO.Productos;
using DataModel.Infraestructura.Offline.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    public interface IPrecioRepositorio : IGenericRepository<Precio>
    {
    }
    public interface IItemRepositorio : IGenericRepository<Item>
    {
    }
}

