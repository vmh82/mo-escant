using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataModel.DTO.Iteraccion
{
    public class Galeria : Base.BaseModel
    {
        public string Directorio { get; set; }
        public string Nombre { get; set; }        
        public string UrlImagen { get; set; }
        public string UrlImagenOffline { get; set; }
        public string Image { get; set; }
        [Ignore]
        public byte[] fByte { get; set; }
        [Ignore]
        public Stream fStream { get; set; }
        [Ignore]
        public string Extension { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public int GuardarStorage { get; set; }        
    }
}
