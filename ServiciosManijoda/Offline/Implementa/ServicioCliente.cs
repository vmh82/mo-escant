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

namespace ManijodaServicios.Offline.Implementa
{
    #region Cliente
    /// <summary>
    /// Métodos para la Administración de Cliente
    /// </summary>
    public class ServicioClientes_Cliente : IServicioClientes_Cliente
    {
        private readonly IClienteRepositorio _objetoRepository;

        public ServicioClientes_Cliente(IClienteRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<Cliente>> GetAllClienteAsync(Expression<Func<Cliente, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
            {
                //return objetos.ToObservableCollection();
                return new ObservableCollection<Cliente>(objetos);
            }
            else
                return new ObservableCollection<Cliente>();
        }

        public async Task<string> InsertClienteAsync(Cliente objeto)
        {
            IEnumerable<Cliente> resultado = await SELECT_WHERE(objeto);
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
        public async Task UpdateClienteAsync(Cliente objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeleteClienteAsync(Cliente objeto)
        {
            objeto.Deleted = 1;            
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportClienteAsync(IEnumerable<Cliente> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<Cliente>> SELECT_WHERE(Cliente item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM Cliente";
            sql = sql + " where Identificacion=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Identificacion
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion Cliente

    #region Persona
    /// <summary>
    /// Métodos para la Administración de Cliente
    /// </summary>
    public class ServicioClientes_Persona : IServicioClientes_Persona
    {
        private readonly IPersonaRepositorio _objetoRepository;

        public ServicioClientes_Persona(IPersonaRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<Persona>> GetAllPersonaAsync(Expression<Func<Persona, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
            {
                //return objetos.ToObservableCollection();
                return new ObservableCollection<Persona>(objetos);
            }
            else
                return new ObservableCollection<Persona>();
        }

        public async Task<string> InsertPersonaAsync(Persona objeto)
        {
            IEnumerable<Persona> resultado = await SELECT_WHERE(objeto);
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
        public async Task UpdatePersonaAsync(Persona objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeletePersonaAsync(Persona objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportPersonaAsync(IEnumerable<Persona> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<Persona>> SELECT_WHERE(Persona item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM Persona";
            sql = sql + " where Identificacion=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Identificacion
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion Persona
}
