using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Iteraccion
{
    public class Aplicacion : Base.BaseModel
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Nombre")]
        public string Nombre { get; set; }
        [JsonProperty("Administrador")]
        public string Administrador { get; set; }
        [JsonProperty("IdentificacionAdministrador")]
        public string IdentificacionAdministrador { get; set; }
        [JsonProperty("Direccion")]
        public string Direccion { get; set; }
        [JsonProperty("Telefono")]
        public string Telefono { get; set; }
        [JsonProperty("Logo")]
        public byte[] Logo { get; set; }

        [JsonProperty("IdClienteConectado")]
        public string IdClienteConectado { get; set; }
        [JsonProperty("NombreConectado")]
        public string NombreConectado { get; set; }
        [JsonProperty("IdentificacionConectado")]
        public string IdentificacionConectado { get; set; }
        [JsonProperty("PerfilConectado")]
        public string PerfilConectado { get; set; }
        [JsonProperty("ParametrosFacturacion")]
        public string ParametrosFacturacion { get; set; }
        [JsonProperty("ParametrosImpresion")]
        public string ParametrosImpresion { get; set; }
        [JsonProperty("ImagenUsuario")]
        public string ImagenUsuario { get; set; }
        [JsonProperty("Lenguaje")]
        public string Lenguaje { get; set; }
        [JsonProperty("FormatoActual")]
        public string FormatoActual { get; set; }
    }
}
