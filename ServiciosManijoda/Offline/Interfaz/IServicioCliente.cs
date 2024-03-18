using DataModel.DTO.Clientes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
namespace ManijodaServicios.Offline.Interfaz
{
    #region Cliente
    public interface IServicioClientes_Cliente
    {
        Task<ObservableCollection<Cliente>> GetAllClienteAsync(Expression<Func<Cliente, bool>> expression);
        Task<string> InsertClienteAsync(Cliente objeto);
        Task<int> ImportClienteAsync(IEnumerable<Cliente> objetos);
        Task UpdateClienteAsync(Cliente objeto);
        Task DeleteClienteAsync(Cliente objeto);
    }
    #endregion Cliente

    #region Persona
    public interface IServicioClientes_Persona
    {
        Task<ObservableCollection<Persona>> GetAllPersonaAsync(Expression<Func<Persona, bool>> expression);
        Task<string> InsertPersonaAsync(Persona objeto);
        Task<int> ImportPersonaAsync(IEnumerable<Persona> objetos);
        Task UpdatePersonaAsync(Persona objeto);
        Task DeletePersonaAsync(Persona objeto);
    }
    #endregion Persona
}
