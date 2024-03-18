using DataModel.DTO.Iteraccion;
using DataModel.DTO.Seguridad;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace ManijodaServicios.Interfaces
{
    public interface IServicioFirestore
    {
        Task<bool> LoginAsync(string username, string password);
        //Task<bool> SendOtpCodeAsync(string phoneNumber);
        //Task<bool> VerifyOtpCodeAsync(string code);

        //Task<Usuario> GetUserAsync();
        Task<bool> ResetPasswordAsync(string email);
        Task<SecuencialNumerico> GetIncremental(string secuencial);
    }
}
