using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using DataModel.Helpers;
using DataModel.Infraestructura.Offline.Interfaces;
using ManijodaServicios.Offline.Interfaz;
using DataModel.DTO.Iteraccion;

namespace ManijodaServicios.Offline.Implementa
{
    public class ServicioIteraccion
    {
        #region Galeria
        /// <summary>
        /// Métodos para la Administración de Catálogo
        /// </summary>
        public class ServicioIteraccion_Galeria : IServicioIteraccion_Galeria
        {
            private readonly IGaleriaRepositorio _objetoRepository;

            public ServicioIteraccion_Galeria(IGaleriaRepositorio generalRepositorio)
            {
                _objetoRepository = generalRepositorio;
            }

            public async Task<ObservableCollection<Galeria>> GetAllGaleriaAsync(Expression<Func<Galeria, bool>> expression)
            {
                var objetos = await _objetoRepository.GetAsync(expression);
                if (objetos != null)
                    //return objetos.ToObservableCollection();
                    return new ObservableCollection<Galeria>(objetos);
                else
                    return new ObservableCollection<Galeria>();
            }

            public async Task<string> InsertGaleriaAsync(Galeria objeto)
            {
                IEnumerable<Galeria> resultado = await SELECT_WHERE(objeto);
                if (resultado.Count() > 0)
                {
                    await _objetoRepository.UpdateAsync(objeto);
                    return "0";
                }
                else
                {
                    return await _objetoRepository.AddAsync(objeto);
                }
            }
            public async Task UpdateGaleriaAsync(Galeria objeto)
            {
                await _objetoRepository.UpdateAsync(objeto);
            }
            public async Task DeleteGaleriaAsync(Galeria objeto)
            {
                objeto.Deleted = 1;
                objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
                await _objetoRepository.UpdateAsync(objeto);
            }
            public async Task<int> ImportGaleriaAsync(IEnumerable<Galeria> objetos)
            {
                return await _objetoRepository.AddRangeAsync(objetos);
            }

            private async Task<IEnumerable<Galeria>> SELECT_WHERE(Galeria item)
            {
                return (await _objetoRepository.ExecuteScriptsWithParameterAsync("SELECT * FROM Galeria WHERE Id=?"
                                                                                , new object[] { item.Id })
                                                                                  ).AsEnumerable();
            }

        }

        #endregion Galeria

        #region TablasBdd
        /// <summary>
        /// Métodos para la Administración de Catálogo
        /// </summary>
        public class ServicioIteraccion_TablasBdd : IServicioIteraccion_TablasBdd
        {
            private readonly ITablasBddRepositorio _objetoRepository;

            public ServicioIteraccion_TablasBdd(ITablasBddRepositorio generalRepositorio)
            {
                _objetoRepository = generalRepositorio;
            }

            public async Task<ObservableCollection<TablasBdd>> GetAllTablasBddAsync(Expression<Func<TablasBdd, bool>> expression)
            {
                var objetos = await _objetoRepository.GetAsync(expression);
                if (objetos != null)
                    //return objetos.ToObservableCollection();
                    return new ObservableCollection<TablasBdd>(objetos);
                else
                    return new ObservableCollection<TablasBdd>();
            }

            public async Task<ObservableCollection<TablasBdd>> GetAllTablasBddFullAsync(TablasBdd objeto)
            {
                IEnumerable<TablasBdd> resultado = await SELECT_WHEREALL(objeto);
                if (resultado != null && resultado.Count() > 0)
                {                    
                    resultado.ToList().ForEach(async i => i.cantidadDatosSecundaria = await SELECT_ROWCOUNT(i.nombreTabla));
                    /*foreach(var registro in resultado.ToList())
                    {
                        registro.cantidadDatosSecundaria = await SELECT_ROWCOUNT(registro.nombreTabla);
                    }*/
                    //return objetos.ToObservableCollection();

                    return new ObservableCollection<TablasBdd>(resultado);
                }
                else
                    return new ObservableCollection<TablasBdd>();
            }

            public async Task<string> InsertTablasBddAsync(TablasBdd objeto)
            {
                IEnumerable<TablasBdd> resultado = await SELECT_WHERE(objeto);
                if (resultado.Count() > 0)
                {
                    await _objetoRepository.UpdateAsync(objeto);
                    return "0";
                }
                else
                {
                    return await _objetoRepository.AddAsync(objeto);
                }
            }
            public async Task UpdateTablasBddAsync(TablasBdd objeto)
            {
                await _objetoRepository.UpdateAsync(objeto);
            }
            public async Task DeleteTablasBddAsync(TablasBdd objeto)
            {
                objeto.Deleted = 1;
                objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
                await _objetoRepository.UpdateAsync(objeto);
            }
            public async Task DeleteTablasBddAllDataAsync(string nombreTabla)
            {
                string query = string.Format("DELETE FROM {0}", nombreTabla);
                await _objetoRepository.ExecuteScriptsAsync(query);
            }
            public async Task<int> ImportTablasBddAsync(IEnumerable<TablasBdd> objetos)
            {
                return await _objetoRepository.AddRangeAsync(objetos);
            }

            private async Task<IEnumerable<TablasBdd>> SELECT_WHERE(TablasBdd item)
            {
                return (await _objetoRepository.ExecuteScriptsWithParameterAsync("SELECT * FROM TablasBdd WHERE Id=?"
                                                                                , new object[] { item.Id })
                                                                                  ).AsEnumerable();
            }

            private async Task<IEnumerable<TablasBdd>> SELECT_WHEREALL(TablasBdd item)
            {
                //return (await _objetoRepository.ExecuteScriptsWithParameterAsync("DROP TABLE IF EXISTS TablasBdd; CREATE TABLE TablasBdd AS SELECT a.name as nombreTabla,0 as cantidadDatosPrincipal, 0 as cantidadDatosSecundaria, 0 as sincronizada FROM sqlite_schema a WHERE type = 'table' AND name NOT LIKE 'sqlite_%';select *from TablasBdd;  "
                return (await _objetoRepository.ExecuteScriptsWithParameterAsync("select *from TablasBdd;  "
                                                                                , new object[] { item.Id })
                                                                                  ).AsEnumerable();
            }
            private async Task<int> SELECT_ROWCOUNT(string item)
            {
                string query = string.Format("SELECT count(1) FROM {0}", item);
                return (await _objetoRepository.ExecuteScriptsScalarAsync(query, new object[] { item}));
            }

        }

        #endregion TablasBdd

    }
}
