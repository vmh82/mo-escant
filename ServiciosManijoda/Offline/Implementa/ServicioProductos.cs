using DataModel.DTO.Productos;
using DataModel.Infraestructura.Offline.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using DataModel.Helpers;
using ManijodaServicios.Offline.Interfaz;

namespace ManijodaServicios.Offline.Implementa
{
    #region Item
    /// <summary>
    /// Métodos para la Administración de Cliente
    /// </summary>
    public class ServicioProductos_Item : IServicioProductos_Item
    {
        private readonly IItemRepositorio _objetoRepository;

        public ServicioProductos_Item(IItemRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<Item>> GetAllItemAsync(Expression<Func<Item, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
            {
                //return objetos.ToObservableCollection();
                return new ObservableCollection<Item>(objetos);
            }
            else
                return new ObservableCollection<Item>();
        }

        public async Task<string> InsertItemAsync(Item objeto)
        {
            IEnumerable<Item> resultado = await SELECT_WHERE(objeto);
            if (resultado.Count() > 0)
            {
                //Datos de Auditoría
                objeto.CreatedBy = resultado.FirstOrDefault().CreatedBy;

                await _objetoRepository.UpdateAsync(objeto);
                return "0";
            }
            else
            {
                return await _objetoRepository.AddAsync(objeto);
            }
        }
        public async Task UpdateItemAsync(Item objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeleteItemAsync(Item objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportItemAsync(IEnumerable<Item> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<Item>> SELECT_WHERE(Item item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM Item";
            sql = sql + " where Id=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Id
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion Item

    #region Precio
    /// <summary>
    /// Métodos para la Administración de Cliente
    /// </summary>
    public class ServicioProductos_Precio : IServicioProductos_Precio
    {
        private readonly IPrecioRepositorio _objetoRepository;

        public ServicioProductos_Precio(IPrecioRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<Precio>> GetAllPrecioAsync(Expression<Func<Precio, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
            {
                //return objetos.ToObservableCollection();
                return new ObservableCollection<Precio>(objetos);
            }
            else
                return new ObservableCollection<Precio>();
        }

        public async Task<string> InsertPrecioAsync(Precio objeto)
        {
            IEnumerable<Precio> resultado = await SELECT_WHERE(objeto);
            if (resultado.Count() > 0)
            {
                //Datos de Auditoría
                objeto.CreatedBy = resultado.FirstOrDefault().CreatedBy;

                await _objetoRepository.UpdateAsync(objeto);
                return "0";
            }
            else
            {
                return await _objetoRepository.AddAsync(objeto);
            }
        }
        public async Task UpdatePrecioAsync(Precio objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeletePrecioAsync(Precio objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportPrecioAsync(IEnumerable<Precio> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<Precio>> SELECT_WHERE(Precio item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM Precio";
            sql = sql + " where Id=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Id
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion Precio
}
