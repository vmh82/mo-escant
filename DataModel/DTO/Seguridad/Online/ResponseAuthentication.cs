using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Seguridad.Online
{
    public class ResponseAuthentication
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("localId")]
        public string LocalId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("idToken")]
        public string IdToken { get; set; }

        [JsonProperty("registered")]
        public bool Registered { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("expiresIn")]
        public long ExpiresIn { get; set; }
    }
    public class ResponseError
    {
        [JsonProperty("error")]
        public Error error { get; set; }        
        
    }
    public class Error
    {
        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }

    }
    
}
