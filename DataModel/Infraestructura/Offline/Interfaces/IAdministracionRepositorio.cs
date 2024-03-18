using DataModel.DTO.Administracion;
using DataModel.Infraestructura.Offline.DB;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    /// <summary>
    /// Interfaz de Repositorio para Catálogo
    /// </summary>
    public interface ICatalogoRepositorio : IGenericRepository<Catalogo>
    {
    }
    
    /// <summary>
    /// Interfaz de Repositorio para Empresa
    /// </summary>
    public interface IEmpresaRepositorio : IGenericRepository<Empresa>
    {
    }
    /// <summary>
    /// Interfaz de Repositorio para Proveedor
    /// </summary>
    public interface IProveedorRepositorio : IGenericRepository<Proveedor>
    {
    }
    
}
