using DataModel;
using DataModel.DTO.Seguridad;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Offline.Interfaz
{
    public interface IServicioSeguridad_Usuario
    {
        Task<ObservableCollection<Usuario>> GetAllUsuarioAsync(Expression<Func<Usuario, bool>> expression);

        Task<bool> ConsultaUsuario(Usuario item);
        Task<string> InsertUsuarioAsync(Usuario objeto);
        Task<int> ImportUsuarioAsync(IEnumerable<Usuario> objetos);
        Task UpdateUsuarioAsync(Usuario objeto);
        Task DeleteUsuarioAsync(Usuario objeto);
    }
    public interface IServicioSeguridad_PerfilMenu
    {
        Task<ObservableCollection<PerfilMenu>> GetAllPerfilMenuAsync(Expression<Func<PerfilMenu, bool>> expression);

        Task<string> InsertPerfilMenuAsync(PerfilMenu objeto);
        Task<int> ImportPerfilMenuAsync(IEnumerable<PerfilMenu> objetos);
        Task UpdatePerfilMenuAsync(PerfilMenu objeto);
        Task DeletePerfilMenuAsync(PerfilMenu objeto);
    }
}
