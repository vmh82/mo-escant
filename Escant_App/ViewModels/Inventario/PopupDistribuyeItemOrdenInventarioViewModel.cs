using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using DataModel.Helpers;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using Escant_App.ViewModels.Base;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Inventario
{
    public class PopupDistribuyeItemOrdenInventarioViewModel:ViewModelBase
    {
        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración
        private CultureInfo _cultura;//=new System.Globalization.CultureInfo("ja-JP")
        private Generales.Generales generales;
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase
        private OrdenDetalleCompra _ordenDetalleCompraSeleccionado;
        private ObservableCollection<string> _oTipoDescuento = new ObservableCollection<string>();
        private ObservableCollection<string> _oUbicacion = new ObservableCollection<string>();
        private List<Catalogo> _tipoDescuento = new List<Catalogo>();
        private Catalogo _tipoDescuentoSeleccionado;
        #endregion 1.2 Variables de Instancia de Objetos para la Clase
        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #region 1.4 Variables de Control de Datos
        #endregion 1.4 Variables de Control de Datos
        #region 1.4 Variables de Control de Errores
        #endregion 1.4 Variables de Control de Errores
        #region 1.6 Acciones
        public Action<bool> EventoGuardar { get; set; }
        public Action<bool> EventoCancelar { get; set; }
        #endregion 1.6 Acciones
        #endregion 1. Declaración de Variables

        public CultureInfo Cultura => Escant_App.AppSettings.Settings.CustomCurrencySymbol == "$" ? new System.Globalization.CultureInfo("en-US") : new System.Globalization.CultureInfo("en-US");
        #region 2.InstanciaAsignaVariablesControles
        public OrdenDetalleCompra OrdenDetalleCompraSeleccionado
        {
            get => _ordenDetalleCompraSeleccionado;
            set
            {
                _ordenDetalleCompraSeleccionado = value;
                RaisePropertyChanged(() => OrdenDetalleCompraSeleccionado);
            }

        }
        public List<Catalogo> TipoDescuentoList
        {
            get { return _tipoDescuento; }
            set
            {
                _tipoDescuento = value;
                OnPropertyChanged(nameof(TipoDescuentoList));
            }
        }
        public ObservableCollection<string> OTipoDescuento
        {
            get { return _oTipoDescuento; }
            set
            {
                _oTipoDescuento = value;
                OnPropertyChanged(nameof(OTipoDescuento));
            }
        }
        public ObservableCollection<string> OUbicacion
        {
            get { return _oUbicacion; }
            set
            {
                _oUbicacion = value;
                OnPropertyChanged(nameof(OUbicacion));
            }
        }
        public Catalogo TipoDescuentoSeleccionado
        {
            get { return _tipoDescuentoSeleccionado; }
            set
            {
                _tipoDescuentoSeleccionado = value;
                OnPropertyChanged(nameof(TipoDescuentoSeleccionado));
            }
        }
        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        public PopupDistribuyeItemOrdenInventarioViewModel()
        {
            AddTipoDescuento();
            AddUbicacion();

        }
        public PopupDistribuyeItemOrdenInventarioViewModel(IServicioAdministracion_Catalogo catalogoServicio)
        {
            generales = new Generales.Generales(catalogoServicio);

        }
        #endregion 3.Constructor

        #region 4.DefinicionMetodos
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());
        public ICommand AddRowGridCommand => new Command(async () => await AddRowGridCommandAsync());
        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is OrdenDetalleCompra item && item != null)
            {
                OrdenDetalleCompraSeleccionado = item;
                cargarGrid();
            }
            IsBusy = false;
        }
        public async Task cargarGrid()
        {
            if (OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido == null)
            {
                OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido = new ObservableCollection<OrdenDetalleCompraDet>();
                TipoDescuentoSeleccionado = TipoDescuentoList.Where(x => x.Nombre.ToUpper().Contains("NO AP")).FirstOrDefault();
                adicionaDetalleRecibido(OrdenDetalleCompraSeleccionado.Cantidad);
            }
        }
        /*public override async Task InitializeAsync(object navigationData,object navigationData1)
        {
            IsBusy = true;
            if (navigationData is OrdenDetalleCompra item && item != null)
            {
                OrdenDetalleCompraSeleccionado = item;
                if(navigationData1 is Orden item1 && item1 != null) 
                {
                    OrdenSeleccionado = item1;
                }
            }
            IsBusy = false;
        }*/
        private async Task Ok_CommandAsync()
        {
            if (validaRecepcion())
            {
                EventoGuardar?.Invoke(true);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Importante", "Debe registrar la recepción de todos los ítems solicitados", "OK");
            }
        }
        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales        
        private async Task Cancel_CommandAsync()
        {
            EventoCancelar?.Invoke(true);
        }
        public void borraDatos()
        {
            OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido = null;
        }
        private void AddTipoDescuento()
        {
            OTipoDescuento = new ObservableCollection<string>(SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "tipodescuentocompra"
                                                                                                && y.EsCatalogo == 0).Select(x => x.Nombre).ToList());
            /*TipoDescuentoList = SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "tipodescuentocompra"
                                                                                                && y.EsCatalogo == 0).ToList();*/
        }
        private void AddUbicacion()
        {
            OUbicacion = new ObservableCollection<string>(SettingsOnline.oLCatalogo.Where(y => y.CodigoCatalogo == "ubicacionmercaderia"
                                                                                                && y.EsCatalogo == 0).Select(x => x.Nombre).ToList());
        }
        private void adicionaDetalleRecibido(float Cantidad)
        {
            try
            {
                OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido.Add(new OrdenDetalleCompraDet()
                {
                    Id = Generator.GenerateKey()
                    ,
                    IdOrden = OrdenDetalleCompraSeleccionado.IdOrden
                    ,
                    IdItem = OrdenDetalleCompraSeleccionado.IdItem
                    ,
                    IdUnidad = OrdenDetalleCompraSeleccionado.IdUnidad
                    ,
                    Unidad = OrdenDetalleCompraSeleccionado.Unidad
                        ,
                    CodigoExterno = ""
                        ,
                    NombreItem = OrdenDetalleCompraSeleccionado.NombreItem
                        ,
                    Cantidad = Cantidad
                        ,
                    ValorUnitario = OrdenDetalleCompraSeleccionado.ValorUnitario
                        ,
                    ValorCompra = OrdenDetalleCompraSeleccionado.ValorCompra
                        ,
                    TipoDescuento = "No Aplica"
                    ,
                    TipoDescuentoSeleccion = TipoDescuentoList.Where(x => x.Nombre.ToUpper().Contains("NO AP")).FirstOrDefault()
                    ,
                    Ubicacion = "Local"
                        ,
                    ValorDescuento = OrdenDetalleCompraSeleccionado.ValorDescuento
                        ,
                    FechaVencimientoStr = String.Format("{0:d}", DateTime.Now)

                });
            }
            catch (Exception ex)
            {

            }
        }
        public async Task<bool> AdministraGrid(OrdenDetalleCompraDet registro, string valorActual, string valorNuevo)
        {
            bool retorno = false;
            if (valorNuevo.Contains("+"))
            {
                string[] pValorNuevo = valorNuevo.Split('+');
                var cantidadDescontada = Convert.ToInt32(pValorNuevo[1]);
                var cantidadRequerida = Convert.ToInt32(pValorNuevo[0]);
                if (cantidadRequerida == OrdenDetalleCompraSeleccionado.Cantidad)
                {
                    /*registro.TipoDescuento = valorNuevo;
                    registro.Cantidad = cantidadDescontada;
                    registro.ValorDescuento = registro.Cantidad * registro.ValorUnitario;
                    var encontrado = OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido.FirstOrDefault(x => x.Id == registro.Id);
                    int i = OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido.IndexOf(encontrado);
                    OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido[i].TipoDescuento = valorNuevo;//registro;*/
                }
                else
                {
                    //var defecto= TipoDescuentoList.Where(x => x.Nombre.ToUpper().Contains("NO AP")).FirstOrDefault();                                        
                    //registro.TipoDescuento = "No Aplica";                    
                    //await App.Current.MainPage.DisplayAlert("Importante", "La cantidad de compra requerida para aplicar el descuento es: " + cantidadRequerida, "OK");
                    retorno = true;
                }
            }
            return retorno;
        }
        private async Task AddRowGridCommandAsync()
        {
            var cantidadOriginal = OrdenDetalleCompraSeleccionado.Cantidad;
            var cantidadRecibida = OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido.Where(y => (string.IsNullOrEmpty(y.TipoDescuento) || y.TipoDescuento.Contains("Apli"))).Sum(x => x.Cantidad);
            var cantidad = cantidadRecibida - cantidadOriginal;
            if (cantidadOriginal != cantidadRecibida)
            {
                if (cantidad < 0)
                {
                    adicionaDetalleRecibido(Math.Abs(cantidad));
                }
            }
        }
        private bool validaRecepcion()
        {
            bool retorno = true;
            var cantidadOriginal = OrdenDetalleCompraSeleccionado.Cantidad;
            var cantidadRecibida = OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido.Where(y => (string.IsNullOrEmpty(y.TipoDescuento) || y.TipoDescuento.Contains("Apli"))).Sum(x => x.Cantidad);
            if (cantidadOriginal != cantidadRecibida)
            {
                retorno = false;
            }
            return retorno;
        }

        public void RecalculaCantidad(OrdenDetalleCompraDet item)
        {
            var itemmodifica = OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido.Where(x => x.Id == item.Id).FirstOrDefault();
            var cantidadOriginal = OrdenDetalleCompraSeleccionado.Cantidad;
            var cantidadRecibida = OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido.Where(y => (string.IsNullOrEmpty(y.TipoDescuento) || y.TipoDescuento.Contains("Apli"))).Sum(x => x.Cantidad);
            var cantidadactual = item.Cantidad;
            var cantidad = cantidadOriginal - cantidadRecibida;
            if (cantidad > 0)
                itemmodifica.Cantidad = cantidad;

        }

        #endregion 6.Métodos Generales
    }

}

