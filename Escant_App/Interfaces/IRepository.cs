using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Escant_App.Interfaces
{
    public interface IIdentifiable
    {
        string Id { get; set; }
    }

    public interface IRepository<T> where T : IIdentifiable
    {
        Task<T> Get(string id);
        Task<IList<T>> GetAll();
        Task<string> Save(T item);
        Task<bool> Delete(T item);
    }
}
