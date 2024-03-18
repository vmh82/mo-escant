using DataModel.DTO.Seguridad;
using ManijodaServicios.Interfaces;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using Escant_App.ViewModels.Base;
using System.Threading.Tasks;

namespace Escant_App.ViewModels.Seguridad
{
    public class PasswordOlvidadoViewModel : ViewModelBase
    {
        private readonly IServicioSeguridad_Usuario _servicioSeguridad;
        private IServicioFirestore _servicioFirestore;
        private SwitchSeguridad _switchSeguridad;
        public PasswordOlvidadoViewModel(IServicioSeguridad_Usuario servicioSeguridad, IServicioFirestore servicioFirestore)
        {
            _servicioSeguridad = servicioSeguridad;
            _servicioFirestore = servicioFirestore;
            _switchSeguridad = new SwitchSeguridad(_servicioSeguridad, _servicioFirestore);
        }

        public async Task<bool> ResetearClave(string email)
        {
            bool retorno = false;
            if (string.IsNullOrEmpty(email))
            {
                await App.Current.MainPage.DisplayAlert("Warning", "Por favor ingrese su correo", "Ok");                                
            }

            bool isSend = await _switchSeguridad.ResetearPassword(new UserAuthentication() { email = email }, true);
            if (isSend)
            {
                await App.Current.MainPage.DisplayAlert("Reset Password", "El Link ha sido enviado a su correo.", "Ok");
                retorno = true;
                //await NavigationService.NavigateToModalAsync<AddUsuarioViewModel>(usuario);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Reset Password", "Link de envío errado", "Ok");
            }
            return retorno;
        }
    }
}
