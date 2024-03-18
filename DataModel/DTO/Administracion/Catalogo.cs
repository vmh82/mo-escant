using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using Xamarin.Forms;
using System.IO;
using DataModel.Helpers;
using System.ComponentModel;
using System.Collections.ObjectModel;
using DataModel.DTO.Iteraccion;
using System.Drawing;

namespace DataModel.DTO.Administracion
{    
    /// <summary>
    /// Entidad o Clase para Colección Offline
    /// </summary>
    public class Catalogo : Base.BaseModel, ISyncTable<CatalogoDto>
    {
        public string IdPadre { get; set; }
        public string Empresa { get; set; }
        public string CodigoCatalogo { get; set; }
        public int EsCatalogo { get; set; }
        public int EsJerarquico { get; set; }
        public int EsMedida { get; set; }
        public int EsCentral { get; set; }
        public string IdConversion { get; set; }
        public float ValorConversion { get; set; }
        public int EsConstantes { get; set; }
        public float ValorConstanteNumerico { get; set; }
        public string ValorConstanteTexto { get; set; }
        [Indexed]
        public string Codigo { get; set; }
        
        public string Nombre { get; set; }
        public string ImageIcon { get; set; }
        public int Nivel { get; set; }
        public int EstaActivo { get; set; }

        public int EsMenu { get; set; }
        public int EsModulo { get; set; }
        public int PoseeFormulario { get; set; }
        public string NombreFormulario { get; set; }
        public string LabelTitulo { get; set; }
        public string LabelDescripcion { get; set; }
        public int Orden { get; set; }

        [Ignore]
        public ObservableCollection<Catalogo> subCatalogos { get; set; }
        [Ignore]
        public string NombrePadre { get; set; }
        [Ignore]
        public int EsEdicion { get; set; }
        [Ignore]
        public FuenteIconos IconoMenu { get; set; }        
        /// <summary>
        /// Constructor
        /// </summary>
        public Catalogo()
        {

        }
        public Task BindData(CatalogoDto apiDto, DbContext dbContext)
        {
            Id = apiDto.Id;
            IdPadre = apiDto.IdPadre;
            Empresa = apiDto.Empresa;
            CodigoCatalogo = apiDto.CodigoCatalogo;
            EsCatalogo = apiDto.EsCatalogo;
            EsJerarquico = apiDto.EsJerarquico;
            EsMedida = apiDto.EsMedida;
            EsCentral = apiDto.EsCentral;
            IdConversion = apiDto.IdConversion;
            ValorConversion = (float)(apiDto.ValorConversion ?? 0);
            Codigo = apiDto.Codigo;
            Nombre = apiDto.Nombre;
            if (string.IsNullOrEmpty(apiDto.UrlImageIcon))
            {
                ImageIcon = "";
            }
            Nivel = apiDto.Nivel;
            EstaActivo = apiDto.EstaActivo;
            EsConstantes = apiDto.EsConstantes;
            ValorConstanteNumerico = (float)(apiDto.ValorConstanteNumerico ?? 0);
            ValorConstanteTexto = apiDto.ValorConstanteTexto;
            EsMenu = apiDto.EsMenu;
            EsModulo = apiDto.EsModulo;
            PoseeFormulario = apiDto.PoseeFormulario;
            NombreFormulario = apiDto.NombreFormulario;
            LabelTitulo = apiDto.LabelTitulo;
            LabelDescripcion = apiDto.LabelDescripcion;
            return Task.FromResult(1);
        }        

    }
    /// <summary>
    /// Entidad o Clase para Colección Online
    /// </summary>
    public class CatalogoDto : AuditableEntity
    {
        public string IdPadre { get; set; }
        public string Empresa { get; set; }
        public string CodigoCatalogo { get; set; }
        public int EsCatalogo { get; set; }
        public int EsJerarquico { get; set; }
        public int EsMedida { get; set; }
        public int EsCentral { get; set; }
        public string IdConversion { get; set; }
        public decimal? ValorConversion { get; set; }
        public int EsConstantes { get; set; }
        public decimal? ValorConstanteNumerico { get; set; }
        public string ValorConstanteTexto { get; set; }
        public string Codigo { get; set; }

        public string Nombre { get; set; }
        public int EsMenu { get; set; }
        public int EsModulo { get; set; }
        public int PoseeFormulario { get; set; }
        public string NombreFormulario { get; set; }
        public string LabelTitulo { get; set; }
        public string LabelDescripcion { get; set; }
        public string UrlImageIcon { get; set; }
        public int Nivel { get; set; }
        public int EstaActivo { get; set; }
    }
}
