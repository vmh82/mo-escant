using DataModel.DTO.Seguridad;
using ManijodaServicios.AppSettings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ManijodaServicios.Helpers
{
    public static class TokenHelpers
    {
        private static JwtSecurityToken ParseToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var readableToken = jwtHandler.CanReadToken(token);
            if (!readableToken)
                return null;
            var jwtToken = jwtHandler.ReadJwtToken(token);
            return jwtToken;
        }
        public static string GetToken()
        {
            string token = string.Empty;
            if (GlobalSettings.User != null && !string.IsNullOrEmpty(GlobalSettings.User.IdToken))
                token = GlobalSettings.User.IdToken;
            return token;
        }
        public static bool Valid(this Usuario user)
        {
            if (user == null)
                return false;
            var token = user.IdToken;
            if (string.IsNullOrEmpty(token))
                return false;
            var jwtToken = ParseToken(token);
            if (jwtToken == null || jwtToken.ValidTo.ToLocalTime() < DateTime.Now)
            if (jwtToken == null )
                return false;
            return true;
        }
    }
}
