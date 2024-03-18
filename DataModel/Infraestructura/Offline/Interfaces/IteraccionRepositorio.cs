using DataModel.Infraestructura.Offline.DB;
using DataModel.DTO.Iteraccion;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    #region Galeria
    public class GaleriaRepositorio : GenericSqliteRepository<Galeria>, IGaleriaRepositorio
    {
        public GaleriaRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion Galeria    

    #region TablasBdd
    public class TablasBddRepositorio : GenericSqliteRepository<TablasBdd>, ITablasBddRepositorio
    {
        public TablasBddRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion TablasBdd
}
