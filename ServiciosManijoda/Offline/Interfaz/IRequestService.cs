using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Offline.Interfaz
{
    public interface IRequestService
    {
        Task<TResult> GetAsync<TResult>(string uri);

        Task<TResult> PostAsync<TResult>(string uri, TResult data);

        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data);
        Task<TResult> AuthPostAsync<TRequest, TResult>(string uri, string request);

        Task<TResult> PutAsync<TResult>(string uri, TResult data);

        Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data);
    }
}
