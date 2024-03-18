using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using Escant_App.ViewModels.Base;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Escant_App.ViewModels.Reportes
{
    public class PopupOpcionesImpresionViewModel: ViewModelBase
    {
        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración
        private ObservableCollection<Catalogo> _opcionesImpresionInfo;
        private Catalogo _itemSeleccionado;
        private Orden _ordenSeleccionado;
        private Generales.Generales generales = new Generales.Generales();
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase
        #endregion 1.2 Variables de Instancia de Objetos para la Clase
        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #region 1.4 Variables de Control de Datos
        #endregion 1.4 Variables de Control de Datos
        #region 1.5 Variables de Control de Errores
        #endregion 1.5 Variables de Control de Errores
        #region 1.6 Acciones
        public Action<bool> EventoGuardar { get; set; }
        public Action<bool> EventoCancelar { get; set; }
        #endregion 1.6 Acciones
        #endregion 1. Declaración de Variables

        #region 2.InstanciaAsignaVariablesControles
        public ObservableCollection<Catalogo> OpcionesImpresionInfo
        {
            get => _opcionesImpresionInfo;
            set
            {
                _opcionesImpresionInfo = value;
                RaisePropertyChanged(() => OpcionesImpresionInfo);
            }
        }
        public Catalogo ItemSeleccionado
        {
            get => _itemSeleccionado;
            set
            {
                _itemSeleccionado = value;
                RaisePropertyChanged(() => ItemSeleccionado);
            }
        }
        public Orden OrdenSeleccionado
        {
            get => _ordenSeleccionado;
            set
            {
                _ordenSeleccionado = value;
                RaisePropertyChanged(() => OrdenSeleccionado);
            }
        }
        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        public PopupOpcionesImpresionViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  )
        {
            BindingContext = new ConectividadModelo(seguridadServicio);                               
            
        }
        #endregion 3.Constructor

        #region 4.DefinicionMetodos
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is Orden item && item != null)
            {
                OrdenSeleccionado = item;
                await LoadArbol();
            }
            IsBusy = false;

        }
        public async Task LoadArbol()
        {
            IsBusy = true;
            var catalogo = SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "opcionesimpresion" && y.EsCatalogo == 0).ToList();
            if (catalogo != null && catalogo.Count > 0)
            {
                OrdenSeleccionado.OrdenDetalleCompras = JsonConvert.DeserializeObject<ObservableCollection<OrdenDetalleCompra>>(OrdenSeleccionado.OrdenDetalleCompraSerializado);
                if (string.IsNullOrEmpty(OrdenSeleccionado.OrdenDetalleCompraSerializado))
                    catalogo.Remove(catalogo.Where(x => x.Nombre.Contains("Ingresado") == true).FirstOrDefault());
                if (string.IsNullOrEmpty(OrdenSeleccionado.OrdenDetalleCompraCanceladoSerializado))                
                    catalogo.Remove(catalogo.Where(x => x.Nombre.Contains("Cancelado") == true).FirstOrDefault());                
                if (string.IsNullOrEmpty(OrdenSeleccionado.OrdenDetalleCompraEntregadoSerializado))
                    catalogo.Remove(catalogo.Where(x => x.Nombre.Contains("Entregado") == true).FirstOrDefault());
                if (string.IsNullOrEmpty(OrdenSeleccionado.FacturacionSerializado))
                {
                    foreach (var item in catalogo.Where(x => x.Nombre.Contains("Facturado") == true).ToList())
                    {
                        catalogo.Remove(item);
                    }
                }
                if (OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.EsServicio == 1).ToList().Count > 0)
                {
                    if (OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.NombreItem.ToUpper().Contains("CERTIFIC")).ToList().Count == 0)
                        catalogo.Remove(catalogo.Where(x => x.Nombre.Contains("Certificado") == true).FirstOrDefault());
                    if (OrdenSeleccionado.OrdenDetalleCompras.Where(x => !x.NombreItem.ToUpper().Contains("CERTIFIC")).ToList().Count == 0)
                        catalogo.Remove(catalogo.Where(x => x.Nombre.Contains("Contrato") == true).FirstOrDefault());                    
                }
                else
                {
                    catalogo.Remove(catalogo.Where(x => x.Nombre.Contains("Servicio") == true).FirstOrDefault());
                    catalogo.Remove(catalogo.Where(x => x.Nombre.Contains("Contrato") == true).FirstOrDefault());
                    catalogo.Remove(catalogo.Where(x => x.Nombre.Contains("Certificado") == true).FirstOrDefault());
                }
                    


                await armaArbol(catalogo);
            }
            IsBusy = false;
        }
        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales
        public async Task armaArbol(List<Catalogo> listaCatalogo)
        {
            var nodeImageInfo = new ObservableCollection<Catalogo>();
            var ldetalleGrupo = new ObservableCollection<Catalogo>();
            Catalogo grupo;
            Catalogo detalleGrupo;


            var dtRegistros = generales.ToDataTable<Catalogo>(listaCatalogo);

            if (dtRegistros != null && dtRegistros.Rows.Count > 0)
            {
                foreach (DataRow MenuPadre in dtRegistros.Select("Nivel=1", "Orden ASC"))
                {
                    grupo = new Catalogo()
                    {
                        Id = MenuPadre["Id"].ToString()                        
                        ,
                        IdPadre = MenuPadre["IdPadre"].ToString()
                        ,
                        Nombre = MenuPadre["Nombre"].ToString()                        
                        ,
                        Nivel = MenuPadre["Nivel"] != null ? Convert.ToInt32(MenuPadre["Nivel"]) : 0
                        ,
                        Orden = MenuPadre["Orden"] != null ? Convert.ToInt32(MenuPadre["Orden"]) : 0
                        ,
                        EstaActivo = MenuPadre["EstaActivo"] != null && MenuPadre["EstaActivo"].ToString() == "1" ? 1 : 0                                                
                    };
                    if (dtRegistros.Select("IdPadre='" + grupo.Id + "'").Length > 0)
                    {
                        ldetalleGrupo = new ObservableCollection<Catalogo>();
                        ldetalleGrupo = await armaArbolHijos(dtRegistros, grupo.Id);
                        grupo.subCatalogos = ldetalleGrupo;
                    }
                    nodeImageInfo.Add(grupo);
                }
            }
            OpcionesImpresionInfo = nodeImageInfo;
        }
        private async Task<ObservableCollection<Catalogo>> armaArbolHijos(DataTable herencia, string IdPadre)
        {
            Catalogo detalleGrupo;
            var ldetalleGrupo = new ObservableCollection<Catalogo>();

            foreach (DataRow Menu in herencia.Select("IdPadre='" + IdPadre + "'", "Orden ASC"))
            {
                detalleGrupo = new Catalogo()
                {
                    Id = Menu["Id"].ToString()
                        ,
                    IdPadre = Menu["IdPadre"].ToString()
                        ,
                    Nombre = Menu["Nombre"].ToString()
                        ,
                    Nivel = Menu["Nivel"] != null ? Convert.ToInt32(Menu["Nivel"]) : 0
                        ,
                    Orden = Menu["Orden"] != null ? Convert.ToInt32(Menu["Orden"]) : 0
                        ,
                    EstaActivo = Menu["EstaActivo"] != null && Menu["EstaActivo"].ToString() == "1" ? 1 : 0
                };
                if (herencia.Select("IdPadre='" + detalleGrupo.Id + "'").Length > 0)
                {
                    var ldetalleGrupoHijos = new ObservableCollection<Catalogo>();
                    ldetalleGrupoHijos = await armaArbolHijos(herencia, detalleGrupo.Id);
                    detalleGrupo.subCatalogos = ldetalleGrupoHijos;
                }
                ldetalleGrupo.Add(detalleGrupo);
            }
            return ldetalleGrupo;
        }
        public async Task GoBack(Catalogo item)
        {
            EventoGuardar?.Invoke(true);
            //await NavigationService.NavigateToRootPage();
        }
        public async Task GoBackCancel()
        {
            EventoCancelar?.Invoke(true);
            //await NavigationService.NavigateToRootPage();
        }
        #endregion 6.Métodos Generales
    }
}
