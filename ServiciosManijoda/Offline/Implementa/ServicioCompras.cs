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
using DataModel.DTO.Compras;

namespace ManijodaServicios.Offline.Implementa
{
    #region Orden
    /// <summary>
    /// Métodos para la Administración de Detalle Catálogo
    /// </summary>
    public class ServicioCompras_Orden : IServicioCompras_Orden
    {
        private readonly IOrdenRepositorio _objetoRepository;

        public ServicioCompras_Orden(IOrdenRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<Orden>> GetAllOrdenAsync(Expression<Func<Orden, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
            {
                //return objetos.ToObservableCollection();
                return new ObservableCollection<Orden>(objetos);
            }
            else
                return new ObservableCollection<Orden>();
        }

        public async Task<string> InsertOrdenAsync(Orden objeto)
        {
            IEnumerable<Orden> resultado = await SELECT_WHERE(objeto);
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
        public async Task UpdateOrdenAsync(Orden objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeleteOrdenAsync(Orden objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportOrdenAsync(IEnumerable<Orden> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<Orden>> SELECT_WHERE(Orden item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM Orden";
            sql = sql + " where Id=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Id
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion Orden

    #region OrdenDetalleCompra
    /// <summary>
    /// Métodos para la Administración de Detalle Catálogo
    /// </summary>
    public class ServicioCompras_OrdenDetalleCompra : IServicioCompras_OrdenDetalleCompra
    {
        private readonly IOrdenDetalleCompraRepositorio _objetoRepository;

        public ServicioCompras_OrdenDetalleCompra(IOrdenDetalleCompraRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<OrdenDetalleCompra>> GetAllOrdenDetalleCompraAsync(Expression<Func<OrdenDetalleCompra, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
            {
                //return objetos.ToObservableCollection();
                return new ObservableCollection<OrdenDetalleCompra>(objetos);
            }
            else
                return new ObservableCollection<OrdenDetalleCompra>();
        }

        public async Task<string> InsertOrdenDetalleCompraAsync(OrdenDetalleCompra objeto)
        {
            IEnumerable<OrdenDetalleCompra> resultado = await SELECT_WHERE(objeto);
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
        public async Task UpdateOrdenDetalleCompraAsync(OrdenDetalleCompra objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeleteOrdenDetalleCompraAsync(OrdenDetalleCompra objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportOrdenDetalleCompraAsync(IEnumerable<OrdenDetalleCompra> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<OrdenDetalleCompra>> SELECT_WHERE(OrdenDetalleCompra item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM OrdenDetalleCompra";
            sql = sql + " where Id=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Id
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion OrdenDetalleCompra

    #region OrdenEstado
    /// <summary>
    /// Métodos para la Administración de Detalle Catálogo
    /// </summary>
    public class ServicioCompras_OrdenEstado : IServicioCompras_OrdenEstado
    {
        private readonly IOrdenEstadoRepositorio _objetoRepository;

        public ServicioCompras_OrdenEstado(IOrdenEstadoRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<OrdenEstado>> GetAllOrdenEstadoAsync(Expression<Func<OrdenEstado, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
            {
                //return objetos.ToObservableCollection();
                return new ObservableCollection<OrdenEstado>(objetos);
            }
            else
                return new ObservableCollection<OrdenEstado>();
        }

        public async Task<string> InsertOrdenEstadoAsync(OrdenEstado objeto)
        {
            IEnumerable<OrdenEstado> resultado = await SELECT_WHERE(objeto);
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
        public async Task UpdateOrdenEstadoAsync(OrdenEstado objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeleteOrdenEstadoAsync(OrdenEstado objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportOrdenEstadoAsync(IEnumerable<OrdenEstado> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<OrdenEstado>> SELECT_WHERE(OrdenEstado item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM OrdenEstado";
            sql = sql + " where Id=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Id
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion OrdenEstado
}
