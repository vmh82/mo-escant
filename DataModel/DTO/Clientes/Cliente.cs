using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using System.Collections.ObjectModel;
using DataModel.DTO.Gestion;

namespace DataModel.DTO.Clientes
{
    public class Cliente : Base.BaseModel, ISyncTable<ClienteDto>
    {
                
        public string IdPersona { get; set; }
        [Indexed]
        public string Identificacion { get; set; }        
        public string Nombre { get; set; }
        public string IdUnidadOrganizacion { get; set; }
        public string UnidadOrganizacion { get; set; }
        public string IdEmpresa { get; set; }
        public string Empresa { get; set; }
        public string IdGenero { get; set; }
        public string Genero { get; set; }
        public long? FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Celular { get; set; }
        public string TelefonoFijo { get; set; }
        public string Email { get; set; }
        public string IdPorcentajeDescuento { get; set; }
        public float? PorcentajeDescuento { get; set; }
        public string Descripcion { get; set; }
        public int EsContacto { get; set; }
        public int EsPorDefecto { get; set; }
        public string IdGaleria { get; set; }
        public string Imagen { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public string valorBusqueda { get; set; }
        [Ignore]
        public ObservableCollection<GestionProceso> ListaGestion { get; set; }
        public string GestionSerializado { get; set; }

        public Task BindData(ClienteDto apiDto, DbContext dbContext)
        {
            Id = apiDto.Id;
            IdPersona = apiDto.IdPersona;
            Identificacion = apiDto.Identificacion;
            Nombre = apiDto.Nombre;
            IdUnidadOrganizacion = apiDto.IdUnidadOrganizacion;
            UnidadOrganizacion = apiDto.UnidadOrganizacion;
            IdGenero = apiDto.IdGenero;
            Genero = apiDto.Genero;
            FechaNacimiento = apiDto.FechaNacimiento;
            Direccion = apiDto.Direccion;
            Celular = apiDto.Celular;
            TelefonoFijo = apiDto.TelefonoFijo;
            Email = apiDto.Email;
            PorcentajeDescuento = apiDto.PorcentajeDescuento;
            Descripcion = apiDto.Descripcion;
            EsContacto = apiDto.EsContacto;
            EsPorDefecto = apiDto.EsPorDefecto;
            IdGaleria = apiDto.IdGaleria;
            Imagen = apiDto.Imagen;            
            return Task.FromResult(1);
        }

    }

    public class ClienteDto : AuditableEntity
    {
        public string IdPersona { get; set; }        
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string IdUnidadOrganizacion { get; set; }
        public string UnidadOrganizacion { get; set; }
        public string IdGenero { get; set; }
        public string Genero { get; set; }
        public int FechaNacimiento { get; set; }
        public string Direccion { get; set; }
        public string Celular { get; set; }
        public string TelefonoFijo { get; set; }
        public string Email { get; set; }
        public int PorcentajeDescuento { get; set; }
        public string Descripcion { get; set; }
        public int EsContacto { get; set; }
        public int EsPorDefecto { get; set; }
        public string IdGaleria { get; set; }
        public string Imagen { get; set; }
    }

    public class ClienteImportDto
    {        
        [DisplayName("Identificación")]
        public string Identificacion { get; set; }
        [DisplayName("Nombre")]
        public string Nombre { get; set; }
        [DisplayName("Unidad de Organización")]
        public string UnidadOrganizacion { get; set; }
        [DisplayName("Género")]
        public string Genero { get; set; }
        [DisplayName("Fecha de Nacimiento")]
        public string FechaNacimiento { get; set; }
        [DisplayName("Celular")]
        public string Celular { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        [DisplayName("Teléfono Fijo")]
        public string TelefonoFijo { get; set; }
        [DisplayName("Porcentaje Descuento (%)")]
        public string PorcentajeDescuento { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        [DisplayName("Es Contacto (S/N)")]
        public string EsContacto { get; set; }

    }

}
