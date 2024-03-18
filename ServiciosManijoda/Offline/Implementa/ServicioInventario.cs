using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using DataModel.Helpers;
using DataModel.DTO.Administracion;
using DataModel.Infraestructura.Offline.Interfaces;
using ManijodaServicios.Offline.Interfaz;
using DataModel.DTO.Clientes;
using DataModel.DTO.Inventario;

namespace ManijodaServicios.Offline.Implementa
{
    #region StockResumen
    /// <summary>
    /// Métodos para la Administración de Detalle Catálogo
    /// </summary>
    public class ServicioInventario_StockResumen : IServicioInventario_StockResumen
    {
        private readonly IStockResumenRepositorio _objetoRepository;

        public ServicioInventario_StockResumen(IStockResumenRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<StockResumen>> GetAllStockResumenAsync(Expression<Func<StockResumen, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
            {
                //return objetos.ToObservableCollection();
                return new ObservableCollection<StockResumen>(objetos);
            }
            else
                return new ObservableCollection<StockResumen>();
        }

        public async Task<string> InsertStockResumenAsync(StockResumen objeto)
        {
            IEnumerable<StockResumen> resultado = await SELECT_WHERE(objeto);
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
        public async Task UpdateStockResumenAsync(StockResumen objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeleteStockResumenAsync(StockResumen objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportStockResumenAsync(IEnumerable<StockResumen> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<StockResumen>> SELECT_WHERE(StockResumen item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM StockResumen";
            sql = sql + " where IdItem=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.IdItem
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion StockResumen
}
