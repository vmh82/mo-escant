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
//


namespace ManijodaServicios.Offline.Implementa
{
    #region Catalogo
    /// <summary>
    /// Métodos para la Administración de Catálogo
    /// </summary>
    public class ServicioAdministracion_Catalogo : IServicioAdministracion_Catalogo
    {
        private readonly ICatalogoRepositorio _objetoRepository;

        public ServicioAdministracion_Catalogo(ICatalogoRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<Catalogo>> GetAllCatalogoAsync(Expression<Func<Catalogo, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
                //return objetos.ToObservableCollection();
                return new ObservableCollection<Catalogo>(objetos);
            else
                return new ObservableCollection<Catalogo>();
        }

        public async Task<string> InsertCatalogoAsync(Catalogo objeto)
        {            
            IEnumerable<Catalogo> resultado = await SELECT_WHERE(objeto);
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
        public async Task UpdateCatalogoAsync(Catalogo objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeleteCatalogoAsync(Catalogo objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportCatalogoAsync(IEnumerable<Catalogo> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }
                
        private async Task<IEnumerable<Catalogo>> SELECT_WHERE(Catalogo item)
        {
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync("SELECT * FROM Catalogo WHERE codigo=?"
                                                                            , new object[] { item.Codigo })
                                                                              ).AsEnumerable();
        }

    }

    #endregion Catalogo
    
    #region Empresa
    /// <summary>
    /// Métodos para la Administración de Detalle Catálogo
    /// </summary>
    public class ServicioAdministracion_Empresa : IServicioAdministracion_Empresa
    {
        private readonly IEmpresaRepositorio _objetoRepository;

        public ServicioAdministracion_Empresa(IEmpresaRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<Empresa>> GetAllEmpresaAsync(Expression<Func<Empresa, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
            {                
                //return objetos.ToObservableCollection();
                return new ObservableCollection<Empresa>(objetos);
            }
            else
                return new ObservableCollection<Empresa>();
        }

        public async Task<string> InsertEmpresaAsync(Empresa objeto)
        {
            IEnumerable<Empresa> resultado = await SELECT_WHERE(objeto);
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
        public async Task UpdateEmpresaAsync(Empresa objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeleteEmpresaAsync(Empresa objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportEmpresaAsync(IEnumerable<Empresa> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<Empresa>> SELECT_WHERE(Empresa item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM Empresa";
            sql = sql + " where Codigo=?";            
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Codigo                                                                               
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion Empresa

    #region Proveedor
    /// <summary>
    /// Métodos para la Administración de Detalle Catálogo
    /// </summary>
    public class ServicioAdministracion_Proveedor : IServicioAdministracion_Proveedor
    {
        private readonly IProveedorRepositorio _objetoRepository;

        public ServicioAdministracion_Proveedor(IProveedorRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<Proveedor>> GetAllProveedorAsync(Expression<Func<Proveedor, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
                //return objetos.ToObservableCollection();
                return new ObservableCollection<Proveedor>(objetos);
            else
                return new ObservableCollection<Proveedor>();
        }

        public async Task<string> InsertProveedorAsync(Proveedor objeto)
        {
            IEnumerable<Proveedor> resultado = await SELECT_WHERE(objeto);
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
        public async Task UpdateProveedorAsync(Proveedor objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeleteProveedorAsync(Proveedor objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportProveedorAsync(IEnumerable<Proveedor> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<Proveedor>> SELECT_WHERE(Proveedor item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM Proveedor";
            sql = sql + " where IdEmpresa=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.IdEmpresa
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion Proveedor

}