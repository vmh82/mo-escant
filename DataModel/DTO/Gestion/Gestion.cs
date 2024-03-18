using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Gestion
{
    public class GestionProceso : Base.BaseModel
    {
        public string IdGestionPadre { get; set; }
        public string IdOrden { get; set; }
        public string IdClienteProveedor { get; set; }
        public string NombreClienteProveedor { get; set; }
        public string Identificacion { get; set; }
        public string IdTipoGestion { get; set; }
        public string TipoGestion { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string IdProceso { get; set; }
        public string Proceso { get; set; }
        public string NumeroTelefono { get; set; }
        public string Mensaje { get; set; }
        public string IdRespuesta { get; set; }
        public string Respuesta { get; set; }
        public string FechaGestion { get; set; }
        public string Observacion { get; set; }
        public int EstaGestionado { get; set; }
    }
}
