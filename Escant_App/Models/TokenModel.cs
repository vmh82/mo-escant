using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Models
{
    public class TokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
