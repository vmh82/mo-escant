using System.ComponentModel;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using Xamarin.Forms;
using System.IO;
using System;

namespace DataModel.DTO.Administracion
{
    public class Empresa : Base.BaseModel, ISyncTable<EmpresaDto>
    {

        [Indexed]
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Identificacion { get; set; }
        public string Direccion { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string TelefonoFijo { get; set; }
        public string Representante { get; set; }
        public int EsProveedor { get; set; }
        public int PorDefecto { get; set; }
        public string IdGaleria { get; set; }
        public string Imagen { get; set; }        
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public int NoEsConsulta { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }        

        public Task BindData(EmpresaDto apiDto, DbContext dbContext)
        {

            Id = apiDto.Id;
            Codigo = apiDto.Codigo;
            Descripcion = apiDto.Descripcion;
            Direccion = apiDto.Direccion;
            Identificacion = apiDto.Identificacion;
            Celular = apiDto.Celular;
            Email = apiDto.Email;
            TelefonoFijo = apiDto.TelefonoFijo;
            Representante = apiDto.Representante;
            EsProveedor = apiDto.EsProveedor;
            PorDefecto = apiDto.PorDefecto;
            IdGaleria = apiDto.IdGaleria;
            Imagen = apiDto.Imagen;
            return Task.FromResult(1);
        }
        private Stream ByteArrayToStream(byte[] input)
        {
            return new MemoryStream(input);
        }

    }
    public class EmpresaDto : AuditableEntity
    {
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Identificacion { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string TelefonoFijo { get; set; }
        public string Representante { get; set; }
        public int EsProveedor { get; set; }
        public int PorDefecto { get; set; }
        public string IdGaleria { get; set; }
        public string Imagen { get; set; }
    }


    public class EmpresaImportDto
    {
        [DisplayName("Código")]
        public string Codigo { get; set; }
        
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        [DisplayName("Identificación")]
        public string Identificacion { get; set; }
        [DisplayName("Celular")]
        public string Celular { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Teléfono Fijo")]
        public string TelefonoFijo { get; set; }
        [DisplayName("Representante")]
        public string Representante { get; set; }
        [DisplayName("Es Proveedor (S/N)")]
        public string EsProveedor { get; set; }
        [DisplayName("Por Default (S/N)")]
        public string PorDefecto { get; set; }
        
    }

}
