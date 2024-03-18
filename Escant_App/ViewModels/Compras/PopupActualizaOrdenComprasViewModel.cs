using DataModel.DTO.Compras;
using DataModel.DTO.Iteraccion;
using DataModel.Enums;
using DataModel.Helpers;
using ManijodaServicios.Interfaces;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Switch;
using Escant_App.Helpers;
using Escant_App.ViewModels.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Compras
{
    public class PopupActualizaOrdenComprasViewModel : ViewModelBase
    {
        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración
        private SwitchCompras _switchCompras;
        private SwitchConfiguracion _switchConfiguracion;
        private SwitchProductos _switchProductos;
        private Generales.Generales generales;
        private Generales.GeneraDocumentos generaDocumentos;
        private readonly IServicioProductos_Item _itemServicio;
        private readonly IServicioCompras_Orden _ordenServicio;
        private readonly IServicioCompras_OrdenDetalleCompra _ordenDetalleCompraServicio;
        private readonly IServicioCompras_OrdenEstado _ordenEstadoServicio;
        private readonly IServicioFirestore _servicioFirestore;
        private readonly ISettingsService _settingsServicio;
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase
        private Orden _ordenSeleccionado;
        private List<OrdenDetalleCompra> _ordenDetalleCompraSeleccionado;
        private List<OrdenEstado> _ordenEstadoSeleccionado;
        #endregion 1.2 Variables de Instancia de Objetos para la Clase
        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        private int _listViewHeight;
        private bool _esVisibleAdjunto;
        private Galeria _galeria;
        private float _valorCancelado;
        public CultureInfo Cultura => Escant_App.AppSettings.Settings.CustomCurrencySymbol == "$" ? new System.Globalization.CultureInfo("en-US") : new System.Globalization.CultureInfo("en-US");
        private ImageSource _fuenteImagenRegistro = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #region 1.4 Variables de Control de Datos
        public bool _isVisibleIngresado;
        public bool _isVisibleRecibido;
        public bool _isVisibleCancelado;
        public bool _isVisibleCompartido;
        #endregion 1.4 Variables de Control de Datos
        #region 1.4 Variables de Control de Errores
        #endregion 1.4 Variables de Control de Errores
        #region 1.6 Acciones
        public Action<bool> EventoGuardar { get; set; }
        public Action<bool> EventoCancelar { get; set; }
        #endregion 1.6 Acciones
        #endregion 1. Declaración de Variables

        #region 2.InstanciaAsignaVariablesControles
        public Orden OrdenSeleccionado
        {
            get => _ordenSeleccionado;
            set
            {
                _ordenSeleccionado = value;
                RaisePropertyChanged(() => OrdenSeleccionado);
            }

        }
        public List<OrdenDetalleCompra> OrdenDetalleCompraSeleccionado
        {
            get => _ordenDetalleCompraSeleccionado;
            set
            {
                _ordenDetalleCompraSeleccionado = value;
                RaisePropertyChanged(() => OrdenDetalleCompraSeleccionado);
            }

        }
        public List<OrdenEstado> OrdenEstadoSeleccionado
        {
            get => _ordenEstadoSeleccionado;
            set
            {
                _ordenEstadoSeleccionado = value;
                RaisePropertyChanged(() => OrdenEstadoSeleccionado);
            }

        }
        public int ListViewHeight
        {
            get => _listViewHeight;
            set
            {
                _listViewHeight = value;
                RaisePropertyChanged(() => ListViewHeight);
            }
        }
        public bool EsVisibleAdjunto
        {
            get => _esVisibleAdjunto;
            set
            {
                _esVisibleAdjunto = value;
                RaisePropertyChanged(() => EsVisibleAdjunto);
            }
        }
        public bool IsVisibleIngresado
        {
            get => _isVisibleIngresado;
            set
            {
                _isVisibleIngresado = value;
                RaisePropertyChanged(() => IsVisibleIngresado);
            }
        }
        public bool IsVisibleRecibido
        {
            get => _isVisibleRecibido;
            set
            {
                _isVisibleRecibido = value;
                RaisePropertyChanged(() => IsVisibleRecibido);
            }
        }
        public bool IsVisibleCancelado
        {
            get => _isVisibleCancelado;
            set
            {
                _isVisibleCancelado = value;
                RaisePropertyChanged(() => IsVisibleCancelado);
            }
        }
        public bool IsVisibleCompartido
        {
            get => _isVisibleCompartido;
            set
            {
                _isVisibleCompartido = value;
                RaisePropertyChanged(() => IsVisibleCompartido);
            }
        }
        
        public ImageSource FuenteImagenRegistro
        {
            get
            {
                return _fuenteImagenRegistro;
            }
            set
            {
                _fuenteImagenRegistro = value;
                RaisePropertyChanged(() => FuenteImagenRegistro);
            }
        }

        public float ValorCancelado
        {
            get
            {
                return _valorCancelado;
            }
            set
            {
                _valorCancelado = value;
                RaisePropertyChanged(() => ValorCancelado);
            }
        }

        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        public PopupActualizaOrdenComprasViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioCompras_Orden ordenServicio
                                  , IServicioCompras_OrdenDetalleCompra ordenDetalleCompraServicio
                                  , IServicioCompras_OrdenEstado ordenEstadoServicio
                                  , ISettingsService settingsServicio
                                  , IServicioFirestore servicioFirestore
                                  , IServicioProductos_Item itemServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);
            //Instanciación Acorde al model
            _itemServicio = itemServicio;            
            _ordenServicio = ordenServicio;
            _servicioFirestore = servicioFirestore;
            _settingsServicio = settingsServicio;
            _ordenDetalleCompraServicio = ordenDetalleCompraServicio;
            _ordenEstadoServicio = ordenEstadoServicio;            
            _switchCompras = new SwitchCompras(_ordenServicio, _ordenDetalleCompraServicio, _ordenEstadoServicio);
            _switchConfiguracion = new SwitchConfiguracion(_settingsServicio, _servicioFirestore);
            _switchProductos = new SwitchProductos(_itemServicio);
            generales = new Generales.Generales(_settingsServicio,_servicioFirestore);
            generaDocumentos = new Generales.GeneraDocumentos();            
        }
        #endregion 3.Constructor

        #region 4.DefinicionMetodos
        public ICommand BrowseGalleryCommand => new Command(async () => await BrowseGalleryAsync());
        public ICommand OpenCameraCommand => new Command(async () => await OpenCameraAsync());
        public ICommand RemoveImageCommand => new Command(async () => await RemoveImageAsync());
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());        

        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            try { 
            if (navigationData is Orden item && item != null)
            {
                OrdenSeleccionado = item;
                    if (OrdenSeleccionado.CodigoEstadoEnConsulta == OrdenSeleccionado.CodigoEstadoEnDesarrollo)
                    {
                        switch (OrdenSeleccionado.CodigoEstadoEnConsulta)
                        {
                            case "Ingresado":
                                OrdenSeleccionado.Observaciones = string.Format(TextsTranslateManager.Translate("ObservacionCompra"), OrdenSeleccionado.NombreProveedor, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorTotal.FormatPrice());
                                break;
                            case "Recibido":
                                var cantpro = OrdenSeleccionado.OrdenDetalleComprasRecibido != null ? OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(y => !y.TipoDescuento.Contains("Aplica")).Sum(x => x.Cantidad) : 0;
                                var descto = OrdenSeleccionado.OrdenDetalleComprasRecibido != null ? OrdenSeleccionado.OrdenDetalleComprasRecibido.Where(y => !y.TipoDescuento.Contains("Aplica")).Sum(x => x.ValorDescuento) : 0;
                                OrdenSeleccionado.Observaciones = string.Format(TextsTranslateManager.Translate("ObservacionCompraRecibido"), OrdenSeleccionado.NombreProveedor, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorTotal.FormatPrice(), cantpro, descto.FormatPrice());
                                break;
                            case "Cancelado":
                                CalculaValores();                                
                                break;
                        }
                    }                    

                inicia();
                BindingData();
                CalculateListViewHeight();
            }
            }
            catch(Exception ex)
            {

            }
            IsBusy = false;
        }

        private void inicia()
        {
            IsVisibleIngresado = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Ingres") ? true : false;
            IsVisibleRecibido = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Recibi") ? true : false;
            IsVisibleCancelado = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Cancela") ? true : false;
            IsVisibleCompartido = OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Recibi") || OrdenSeleccionado.CodigoEstadoEnConsulta.Contains("Cancela") ? true : false;
        }

        private void BindingData()
        {
            switch (OrdenSeleccionado.CodigoEstadoEnConsulta)
            {
                case "Recibido":
                    CargaImagen(4);
                    break;
                case "Cancelado":
                    ValorCancelado = OrdenSeleccionado.ValorCancelado;
                    CargaImagen(5);
                    break;
            }
            //EsVisibleAdjunto = !string.IsNullOrEmpty(OrdenSeleccionado.IdGaleria) ? true : false;
            //OrdenSeleccionado.AdjuntoFactura= !String.IsNullOrEmpty(OrdenSeleccionado.AdjuntoFactura)? OrdenSeleccionado.AdjuntoFactura:
        }

        private async Task Ok_CommandAsync()
        {
            string informedMessage = "";            
            IsBusy = true;
            if (await validaAlmacenamiento())
            {
                bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                bool existe = false;
                //1.Recuperar datos de ALmacenamiento
                //OrdenDetalleCompraSeleccionado = new List<OrdenDetalleCompra>(OrdenSeleccionado.OrdenDetalleCompras);
                //OrdenEstadoSeleccionado = new List<OrdenEstado>(OrdenSeleccionado.OrdenEstado);
                if (string.IsNullOrEmpty(OrdenSeleccionado.NumeroOrden) || OrdenSeleccionado.NumeroOrden == "0")
                {
                    var secuenciales = await generales.obtieneSecuenciales("OrdenCompra");
                    OrdenSeleccionado.NumeroOrden = secuenciales.SecuencialReciboIncrementado;
                }                                
                OrdenSeleccionado.Imagen = OrdenSeleccionado.Imagen;
                await actualizaEstadoOrdenAsync();
                switch (OrdenSeleccionado.CodigoEstado)
                {
                    case "Ingresado":
                        OrdenSeleccionado.OrdenDetalleCompraSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.OrdenDetalleCompras);                        
                        break;
                    case "Recibido":OrdenSeleccionado.OrdenDetalleCompraRecibidoSerializado= JsonConvert.SerializeObject(OrdenSeleccionado.OrdenDetalleComprasRecibido);
                                    OrdenSeleccionado.AdjuntoFactura = _galeria != null ? _galeria.Image : OrdenSeleccionado.AdjuntoFactura;
                        break;
                    case "Cancelado":
                        OrdenSeleccionado.AdjuntoCancelado = _galeria != null ? _galeria.Image : OrdenSeleccionado.AdjuntoCancelado;
                        OrdenSeleccionado.ValorCancelado = ValorCancelado;                        
                        break;
                }
                
                OrdenSeleccionado.OrdenEstadoCompraSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.OrdenEstado);
                
                var resultado = await _switchCompras.GuardaRegistro_Orden(OrdenSeleccionado,estaConectado,existe);
                if (OrdenSeleccionado.CodigoEstado.Contains("Recibido") && OrdenSeleccionado.OrdenDetalleComprasRecibido.Count > 0)
                {
                    var resultado1 = await _switchProductos.GuardaRegistro_StockOrden(OrdenSeleccionado, estaConectado, false);
                }                
                informedMessage = resultado.mensaje;
                DialogService.ShowToast(informedMessage);
                MessagingCenter.Send<Orden>(OrdenSeleccionado, "OnDataSourcePopupActualizaOrdenComprasChanged");
                await NavigationService.RemovePopupAsync();
            }
            IsBusy = false;
            //EventoGuardar?.Invoke(true);
        }
        

        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales

        private async Task<bool> validaAlmacenamiento()
        {
            bool retorno = false;
            if(OrdenSeleccionado != null && OrdenSeleccionado.CodigoEstadoEnConsulta==OrdenSeleccionado.CodigoEstadoEnDesarrollo)
            {
                switch (OrdenSeleccionado.CodigoEstadoEnDesarrollo)
                {
                    case "Recibido":
                        if(!string.IsNullOrEmpty(OrdenSeleccionado.NumeroFactura))
                        {
                            if (!FuenteImagenRegistro.IsEmpty)
                            {
                                retorno = true;
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Importante", "Debe adjuntar el respaldo de factura y/o remisión", "OK");
                            }
                        }
                        else {
                            await App.Current.MainPage.DisplayAlert("Importante", "Debe registrar el número de factura y/o remisión", "OK");
                        }
                        break;
                    case "Cancelado":
                        if (OrdenSeleccionado.ValorCancelado>0)
                        {
                            if (!FuenteImagenRegistro.IsEmpty)
                            {
                                retorno = true;
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("Importante", "Debe adjuntar el respaldo de pago ", "OK");
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("Importante", "Debe registrar el valor Cancelado", "OK");
                        }
                        break;
                    default:
                        retorno = true;
                        break;
                }                
            }            


            return retorno;
        }

        private async Task actualizaEstadoOrdenAsync()
        {
            OrdenEstado estadoActualiza = new OrdenEstado();
            var IdEstadoActualiza = OrdenSeleccionado.IdEstado;
            var IdEstadoSeguimiento = OrdenSeleccionado.CodigoEstadoEnDesarrollo != null ? OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.Estado == OrdenSeleccionado.CodigoEstadoEnDesarrollo).IdEstado:"";
            var IdEstadoSiguiente = OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.StatusProgreso.ToString().Contains("Progre")).IdEstado;
            /*OrdenSeleccionado.CodigoEstado = OrdenSeleccionado.CodigoEstadoEnDesarrollo != null ? OrdenSeleccionado.CodigoEstadoEnDesarrollo : OrdenSeleccionado.CodigoEstado;
            OrdenSeleccionado.IdEstado = ((int)Enum.Parse(typeof(EstadoOrdenCompra), OrdenSeleccionado.CodigoEstado)).ToString();*/
            if (IdEstadoSeguimiento == IdEstadoSiguiente)
            {
                OrdenSeleccionado.IdEstado = IdEstadoSiguiente;
                OrdenSeleccionado.CodigoEstado = OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.IdEstado == IdEstadoSiguiente).Estado;
                foreach (OrdenEstado ordenEstado in OrdenSeleccionado.OrdenEstado)
                {
                    estadoActualiza = OrdenSeleccionado.OrdenEstado.FirstOrDefault(x => x.Id == ordenEstado.Id);
                    if (Convert.ToInt32(ordenEstado.IdEstado) >= Convert.ToInt32(OrdenSeleccionado.IdEstado))
                    {

                        if (Convert.ToInt32(ordenEstado.IdEstado) == (Convert.ToInt32(OrdenSeleccionado.IdEstado)))
                        {
                            estadoActualiza.StatusProgreso = Syncfusion.XForms.ProgressBar.StepStatus.Completed;
                            estadoActualiza.FechaProcesoStr = DateTime.Now.ToString("dd MMM yyyy");
                            estadoActualiza.Observaciones = OrdenSeleccionado.Observaciones;
                            switch (OrdenSeleccionado.CodigoEstado)
                            {
                                case "Recibido":estadoActualiza.Adjunto = OrdenSeleccionado.AdjuntoFactura;
                                    break;
                                case "Cancelado":
                                    estadoActualiza.Adjunto = OrdenSeleccionado.AdjuntoCancelado;
                                    break;
                            }
                        }
                        else
                        if (Convert.ToInt32(ordenEstado.IdEstado) == (Convert.ToInt32(OrdenSeleccionado.IdEstado) + 1))
                        {
                            estadoActualiza.StatusProgreso = Syncfusion.XForms.ProgressBar.StepStatus.InProgress;
                        }
                        else
                        {
                            estadoActualiza.StatusProgreso = Syncfusion.XForms.ProgressBar.StepStatus.NotStarted;
                        }

                    }

                }
            }

        }
        
        private async Task Cancel_CommandAsync()
        {
            await NavigationService.RemovePopupAsync();
            //EventoCancelar?.Invoke(true);
        }

        private void CalculateListViewHeight()
        {
            if (OrdenSeleccionado.OrdenDetalleCompras.Any())
            {
                int itemHeight = 0;
                switch (Xamarin.Forms.Device.Idiom)
                {
                    case TargetIdiom.Tablet:
                        itemHeight = 100;
                        break;
                    case TargetIdiom.Phone:
                        itemHeight = 60;
                        break;
                    default:
                        break;
                }
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                {
                    ListViewHeight = OrdenSeleccionado.OrdenDetalleCompras.Count() * itemHeight;
                });
            }
        }
        public string devuelveFecha(string name)
        {
            string fecha = "";
            switch ((int)Enum.Parse(typeof(EstadoOrdenCompra), name))
            {
                case 0:
                    fecha = DateTime.Now.ToString("dd MMM yyyy");
                    break;
                default:
                    fecha = "";
                    break;

            }
            return fecha;
        }
        public async Task BrowseGalleryAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "FacturaCompra",
                GuardarStorage = 0
            };
            galeria.Nombre = OrdenSeleccionado.NumeroOrden == "" ? galeria.Id : OrdenSeleccionado.NumeroOrden;
            _galeria = await generales.PickAndShowFile(galeria);
            await CargaImagen(2);
        }

        private async Task OpenCameraAsync()
        {
            var galeria = new Galeria()
            {
                Id = Generator.GenerateKey()
                                    ,
                Directorio = "FacturaCompra",
                GuardarStorage = 0
            };
            galeria.Nombre = OrdenSeleccionado.NumeroOrden == "" ? galeria.Id : OrdenSeleccionado.NumeroOrden;
            _galeria = await generales.TakePhoto(galeria);
            await CargaImagen(1);
        }
        private async Task RemoveImageAsync()
        {
            await CargaImagen(3);

        }
        private async Task CargaImagen(int Origen)
        {
            switch (Origen)
            {
                case 1:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(_galeria.Image));
                        return stream;
                    });
                    break;
                case 2:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(_galeria.Image));
                        return stream;
                    });
                    break;
                case 3:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = new MemoryStream();
                        return stream;
                    });
                    break;
                case 4:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(OrdenSeleccionado.AdjuntoFactura));
                        return stream;
                    });
                    break;
                case 5:
                    FuenteImagenRegistro = ImageSource.FromStream(() =>
                    {
                        var stream = generales.ByteArrayToStream(Convert.FromBase64String(OrdenSeleccionado.AdjuntoCancelado));
                        return stream;
                    });
                    break;
            }
        }

        public async void CalculaValores()
        {            
            var pago = ValorCancelado;
            float desctoPago = OrdenSeleccionado.ValorTotal>pago? OrdenSeleccionado.ValorTotal - pago:0;
            OrdenSeleccionado.ValorCancelado = ValorCancelado;
            OrdenSeleccionado.Observaciones = string.Format(TextsTranslateManager.Translate("ObservacionCompraCancelado"), OrdenSeleccionado.NombreProveedor, OrdenSeleccionado.Cantidad, OrdenSeleccionado.ValorTotal.FormatPrice(), pago.FormatPrice(), desctoPago.FormatPrice());
        }

           #endregion 6.Métodos Generales

    }
}
