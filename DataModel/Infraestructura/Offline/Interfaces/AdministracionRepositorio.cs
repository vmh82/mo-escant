using DataModel.Infraestructura.Offline.DB;
using DataModel.DTO.Administracion;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    #region Catalogo
    public class CatalogoRepositorio: GenericSqliteRepository<Catalogo>, ICatalogoRepositorio
    {
        public CatalogoRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion Catalogo
    
    #region Empresa
    public class EmpresaRepositorio : GenericSqliteRepository<Empresa>, IEmpresaRepositorio
    {
        public EmpresaRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion Empresa
    #region Proveedor
    public class ProveedorRepositorio : GenericSqliteRepository<Proveedor>, IProveedorRepositorio
    {
        public ProveedorRepositorio(DbContext context) : base(context)
        {
        }
    }
    #endregion Proveedor
    
}
