using DataModel;
using DataModel.DTO.Administracion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Offline.Interfaz
{
    #region Catálogo
    /// <summary>
    /// Interfaz método Servicio de Administración de Catálogos
    /// </summary>
    public interface IServicioAdministracion_Catalogo
    {
        Task<ObservableCollection<Catalogo>> GetAllCatalogoAsync(Expression<Func<Catalogo, bool>> expression);
        Task<string> InsertCatalogoAsync(Catalogo objeto);
        Task<int> ImportCatalogoAsync(IEnumerable<Catalogo> objetos);
        Task UpdateCatalogoAsync(Catalogo objeto);
        Task DeleteCatalogoAsync(Catalogo objeto);        
    }
    #endregion Catálogo

    #region Empresa
    public interface IServicioAdministracion_Empresa
    {
        Task<ObservableCollection<Empresa>> GetAllEmpresaAsync(Expression<Func<Empresa, bool>> expression);
        Task<string> InsertEmpresaAsync(Empresa objeto);
        Task<int> ImportEmpresaAsync(IEnumerable<Empresa> objetos);
        Task UpdateEmpresaAsync(Empresa objeto);
        Task DeleteEmpresaAsync(Empresa objeto);
    }
    #endregion Empresa

    #region Proveedor
    public interface IServicioAdministracion_Proveedor
    {
        Task<ObservableCollection<Proveedor>> GetAllProveedorAsync(Expression<Func<Proveedor, bool>> expression);
        Task<string> InsertProveedorAsync(Proveedor objeto);
        Task<int> ImportProveedorAsync(IEnumerable<Proveedor> objetos);
        Task UpdateProveedorAsync(Proveedor objeto);
        Task DeleteProveedorAsync(Proveedor objeto);
    }
    #endregion Proveedor

}
