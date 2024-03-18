using DataModel.DTO.Seguridad;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Offline.Interfaz
{
    public interface IAuthenticationService
    {
        bool IsAuthenticated { get; }

        Usuario AuthenticatedUser { get; }        
    }
}
