using DataModel.DTO.Iteraccion;
using DataModel.Infraestructura.Offline.DB;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    #region IGaleriaRepositorio
    /// <summary>
    /// Interfaz de Repositorio para Galeria
    /// </summary>
    public interface IGaleriaRepositorio : IGenericRepository<Galeria>
    {
    }
    #endregion IGaleriaRepositorio
    #region TablasBdd
    /// <summary>
    /// Interfaz de Repositorio para TablasBdd
    /// </summary>
    public interface ITablasBddRepositorio : IGenericRepository<TablasBdd>
    {
    }
    #endregion TablasBdd
}
