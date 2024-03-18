using DataModel.DTO.Seguridad;
using ManijodaServicios.Helpers;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Switch;

namespace ManijodaServicios.Offline.Implementa
{
    public class AuthenticationService : IAuthenticationService
    {
        public bool IsAuthenticated => GlobalSettings.User != null && GlobalSettings.User.Valid();
        public bool IsExisteMenu => SettingsOnline.EsMenuVacio;
        public Usuario AuthenticatedUser => GlobalSettings.User;
        private readonly IRequestService _requestService;
        private readonly IDialogService _dialogService;
        private SwitchSeguridad _switchSeguridad;
        public AuthenticationService(IRequestService requestService, IDialogService dialogService)
        {
            _requestService = requestService;
            _dialogService = dialogService;
        }        
        void SaveAuthenticationResult(TokenModel result)
        {
            var user = AuthenticationResultHelper.GetUserFromResult(result);
            GlobalSettings.User = user;
        }

    }
}
