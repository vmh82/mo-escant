using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataModel.DTO.Iteraccion
{
    public class Transaccion : Base.BaseModel
    {
        public ImageSource Imagen { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string CantidadTotal { get; set; }
        public string CantidadHoy { get; set; }
    }
}
