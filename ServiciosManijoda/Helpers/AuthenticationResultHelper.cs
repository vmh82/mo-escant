using DataModel.DTO.Seguridad;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManijodaServicios.Helpers
{
    public static class AuthenticationResultHelper
    {
        public static Usuario GetUserFromResult(TokenModel authResult)
        {
            var data = ParseIdToken(authResult.AccessToken);
            var user = new Usuario
            {
                Id = GetTokenValue(data, "sub"),
                IdToken = authResult.AccessToken,
                RefreshToken = authResult.RefreshToken,
                Email = GetTokenValue(data, "email"),
                Nombre = GetTokenValue(data, "fullname"),
                Telefono = GetTokenValue(data, "phone_number"),
                Roles = GetRoles(data, "role")
            };
            return user;
        }
        private static string[] GetRoles(JObject data, string key)
        {
            try
            {
                var token = data[key];
                return token.HasValues ? token.ToArray().Select(x=>x.ToString())?.ToArray() : null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting token data from B2C: {ex}");
            }
            return null;
        }
        static JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }

        static string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());

            return decoded;
        }

        static string GetTokenValue(JObject data, string key)
        {
            var value = string.Empty;

            try
            {
                var token = data[key];

                value = token.HasValues
                    ? token.First.ToString()
                    : token.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting token data from B2C: {ex}");
            }
            return value;
        }
    }
}
