using DataModel.DTO.Productos;
using DataModel.Infraestructura.Offline.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    public class ProductosRepositorio
    {
        #region Item
        /// <summary>
        /// Usuario
        /// </summary>
        public class ItemRepositorio : GenericSqliteRepository<Item>, IItemRepositorio
        {
            public ItemRepositorio(DbContext context) : base(context)
            {
            }
        }
        #endregion Item
        #region Precio
        public class PrecioRepositorio : GenericSqliteRepository<Precio>, IPrecioRepositorio
        {
            public PrecioRepositorio(DbContext context) : base(context)
            {
            }
        }
        #endregion Precio
    }
}
