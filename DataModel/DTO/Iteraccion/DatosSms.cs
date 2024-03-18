using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Iteraccion
{
    public class DatosSms
    {   
        public int tipo { get; set; }
        public bool Enviado { get; set; }
        public string NumeroTelefono { get; set; }
        public string Mensaje { get; set; }
    }
}
