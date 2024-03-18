using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Seguridad
{
    public class UserAuthentication
    {
        public string email { get; set; }
        public string password { get; set; }
        public string passwordSinEnc { get; set; }
        public string token { get; set; }
        public bool returnSecureToken { get; set; }
    }
    public class UserUpdate
    {
        public string idToken { get; set; }
        public string password { get; set; }
        public bool returnSecureToken { get; set; }
    }
}
