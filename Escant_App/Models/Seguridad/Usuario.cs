using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Escant_App.Models.Seguridad
{
    public class Usuario
    {
        [MaxLength(255)]
        public string Id { get; set; }
        [MaxLength(255)]
        public string Nombre { get; set; }
        [MaxLength(255)]
        public string Nombres { get; set; }
        [MaxLength(255)]
        public string Apellidos { get; set; }
        [MaxLength(255)]
        public string Documento { get; set; }
        [MaxLength(255)]
        public string Email { get; set; }
        [MaxLength(255)]
        public string Clave { get; set; }
        [MaxLength(255)]
        public string Token { get; set; }
        [MaxLength(255)]
        public string RefreshToken { get; set; }
        [MaxLength(255)]
        public string Telefono { get; set; }
        [MaxLength(255)]
        public string[] Roles { get; set; }
        [MaxLength(255)]
        public string[] Permisos { get; set; }

    }
}
