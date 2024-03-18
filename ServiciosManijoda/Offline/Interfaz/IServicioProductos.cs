using DataModel.DTO.Productos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Offline.Interfaz
{
    public interface IServicioProductos_Item
    {
        Task<ObservableCollection<Item>> GetAllItemAsync(Expression<Func<Item, bool>> expression);

        Task<string> InsertItemAsync(Item objeto);
        Task<int> ImportItemAsync(IEnumerable<Item> objetos);
        Task UpdateItemAsync(Item objeto);
        Task DeleteItemAsync(Item objeto);
    }

    public interface IServicioProductos_Precio
    {
        Task<ObservableCollection<Precio>> GetAllPrecioAsync(Expression<Func<Precio, bool>> expression);

        Task<string> InsertPrecioAsync(Precio objeto);
        Task<int> ImportPrecioAsync(IEnumerable<Precio> objetos);
        Task UpdatePrecioAsync(Precio objeto);
        Task DeletePrecioAsync(Precio objeto);
    }
}
