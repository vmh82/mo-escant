using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Iteraccion
{
    public class DatosProceso
    {
        public string Id { get; set; }
        public string Tipo { get; set; }
        public string mensaje { get; set; }
        public bool respuesta { get; set; }
        public bool conClaveSinEncriptar { get; set; }

        public string objetoSerializado { get; set; }

    }
}
