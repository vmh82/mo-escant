using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Iteraccion
{
    public class TablasBdd: Base.BaseModel
    {
        public string nombreTabla { get; set; }
        public int cantidadDatosPrincipal { get; set; }
        public int cantidadDatosSecundaria { get; set; }
        public int sincronizada { get; set; }
    }
}
