using DataModel.DTO.Administracion;
using DataModel.DTO.Clientes;
using DataModel.DTO.Compras;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Gestion;
using DataModel.Helpers;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Switch;
using Escant_App.Validations;
using Escant_App.ViewModels.Administracion;
using Escant_App.ViewModels.Base;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using ManijodaServicios.AppSettings;
using Plugin.Messaging;

namespace Escant_App.ViewModels.Gestion
{
    public class AddGestionViewModel:ViewModelBase
    {
        #region 0.DeclaracionVariables
        private SwitchIteraccion _switchIteraccion;
        private SwitchCliente _switchCliente;
        private SwitchCompras _switchCompras;
        private Generales.Generales generales;

        private readonly IServicioClientes_Cliente _clienteServicio;
        private readonly IServicioClientes_Persona _personaServicio;
        private readonly IServicioIteraccion_Galeria _galeriaServicio;
        private readonly IServicioCompras_Orden _ordenServicio;

        /// 0.1 Referencias a instancias de objetos a utilizarse en la Clase        
        private GestionProceso _registro;
        private Galeria _galeria;


        private bool _isEditMode;

        /// <summary>
        /// 0.2 Declaración de las variables de lectura y escritura para los controles
        /// </summary>
        private ImageSource _fuenteImagenRegistro = ImageSource.FromStream(() =>
        {
            var stream = new MemoryStream();
            return stream;
        });        
        private DateTime _fechaNacimiento;        
        private Catalogo _tipoGestionSeleccionado;
        private Catalogo _respuestaGestionSeleccionado;
        private Orden _ordenSeleccionado;
        private string _descripcion;        
        private string _idGaleria;
        private string _imagen;
        private string _tipogestion;
        private string _respuesta;


        private GestionProceso _gestionSeleccionado;

        /// <summary>
        /// 0.4 Declaración de las variables de control de errores
        /// </summary>
        private bool _esVisibleFechas;

        public CultureInfo Cultura => Escant_App.AppSettings.Settings.CustomCurrencySymbol == "$" ? new System.Globalization.CultureInfo("en-US") : new System.Globalization.CultureInfo("en-US");
        #endregion DeclaracionVariables

        #region 1.Constructor
        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public AddGestionViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioAdministracion_Catalogo catalogoServicio
                                  , IServicioClientes_Cliente clienteServicio
                                  , IServicioClientes_Persona personaServicio
                                  , IServicioIteraccion_Galeria galeriaServicio
                                  , IServicioCompras_Orden ordenServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);
            //Instanciación Acorde al model
            _clienteServicio = clienteServicio;
            _personaServicio = personaServicio;
            _galeriaServicio = galeriaServicio;
            _ordenServicio = ordenServicio;
            _switchCliente = new SwitchCliente(_clienteServicio, _personaServicio);
            _switchIteraccion = new SwitchIteraccion(_galeriaServicio);
            _switchCompras = new SwitchCompras(_ordenServicio);
            generales = new Generales.Generales(catalogoServicio, galeriaServicio);      
            MessagingCenter.Unsubscribe<Catalogo>(this, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddGestionView);
            MessagingCenter.Subscribe<Catalogo>(this, Escant_App.AppSettings.Settings.CatalogoSeleccionadoAddGestionView, (args) =>
            {
                TipoGestionSeleccionado = args.CodigoCatalogo == "tipogestion" ? args : TipoGestionSeleccionado;
                GestionSeleccionado.IdTipoGestion = TipoGestionSeleccionado != null && TipoGestionSeleccionado.Id != null ? TipoGestionSeleccionado.Id: GestionSeleccionado.IdTipoGestion;
                GestionSeleccionado.TipoGestion = TipoGestionSeleccionado != null && TipoGestionSeleccionado.Nombre != null ? TipoGestionSeleccionado.Nombre : GestionSeleccionado.TipoGestion;
                TipoGestion = GestionSeleccionado.TipoGestion;
                evaluaTipoGestion();
                RespuestaGestionSeleccionado = args.CodigoCatalogo == "respuestagestion" ? args : RespuestaGestionSeleccionado;
                GestionSeleccionado.IdRespuesta = RespuestaGestionSeleccionado != null && RespuestaGestionSeleccionado.Id != null ? RespuestaGestionSeleccionado.Id : GestionSeleccionado.IdRespuesta;
                GestionSeleccionado.Respuesta = RespuestaGestionSeleccionado != null && RespuestaGestionSeleccionado.Nombre != null ? RespuestaGestionSeleccionado.Nombre : GestionSeleccionado.Respuesta;
                Respuesta = GestionSeleccionado.Respuesta;
            });
        }
        #endregion 1.Constructor       

        #region 2.InstanciaAsignaVariablesControles
        /// 2. Asignacion y Lectura de variables vinculadas a los controles
        /// <summary>
        /// Método Asignación o lectura de CodigoEstablecimiento
        /// </summary>

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
        
                        
        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set
            {
                _fechaNacimiento = value;
                RaisePropertyChanged(() => FechaNacimiento);
            }
        }        

        public Catalogo TipoGestionSeleccionado
        {
            get => _tipoGestionSeleccionado;
            set
            {
                _tipoGestionSeleccionado = value;
                RaisePropertyChanged(() => TipoGestionSeleccionado);
            }
        }

        public Catalogo RespuestaGestionSeleccionado
        {
            get => _respuestaGestionSeleccionado;
            set
            {
                _respuestaGestionSeleccionado = value;
                RaisePropertyChanged(() => RespuestaGestionSeleccionado);
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

        public string Descripcion
        {
            get => _descripcion;
            set
            {
                _descripcion = value;
                RaisePropertyChanged(() => Descripcion);
            }
        }        
        public string IdGaleria
        {
            get => _idGaleria;
            set
            {
                _idGaleria = value;
                RaisePropertyChanged(() => IdGaleria);
            }
        }
        public string Imagen
        {
            get => _imagen;
            set
            {
                _imagen = value;
                RaisePropertyChanged(() => Imagen);
            }
        }

        public string TipoGestion
        {
            get => _tipogestion;
            set
            {
                _tipogestion = value;
                RaisePropertyChanged(() => TipoGestion);
            }
        }

        public string Respuesta
        {
            get => _respuesta;
            set
            {
                _respuesta = value;
                RaisePropertyChanged(() => Respuesta);
            }
        }

        public GestionProceso GestionSeleccionado
        {
            get => _gestionSeleccionado;
            set
            {
                _gestionSeleccionado = value;
                RaisePropertyChanged(() => GestionSeleccionado);
            }
        }
        public bool EsVisibleFechas
        {
            get => _esVisibleFechas;
            set
            {
                _esVisibleFechas = value;
                RaisePropertyChanged(() => EsVisibleFechas);
            }
        }

        #endregion 2.InstanciaAsignaVariablesControles        

        #region 3.DefinicionMetodos
        /// <summary>
        /// 3. Definición de métodos de la pantalla
        /// </summary>
        /// <summary>
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());
        public ICommand OpenTipoGestionCommand => new Command(async () => await OpenTipoGestionAsync());
        public ICommand OpenRespuestaGestionCommand => new Command(async () => await OpenRespuestaGestionAsync());


        #endregion 3.DefinicionMetodos

        #region 4.MétodosAdministraciónDatos
        /// <summary>
        /// 4. Métodos de administración de datos
        /// </summary>
        /// <returns></returns>
        /// 4.1. Método para Cargar información de pantalla, la llamada realizada desde el frontend al inicializar
        /// </summary>
        /// <returns></returns>
        public async Task Load()
        {
            IsBusy = true;
            await LoadInicio();
            IsBusy = false;
        }
        /// <summary>
        /// 4.2 Método iniciar valores
        /// </summary>
        /// <returns></returns>
        public async Task LoadInicio()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            ResetForm();
        }
        /// <summary>
        /// 4.3 Método iniciar Pantalla con 1 parámetro
        /// </summary>
        /// <param name="navigationData"></param>
        /// <returns></returns>
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;            
            //Bind data for edit mode
                if (navigationData is Orden registro && registro != null)
                {
                    OrdenSeleccionado = registro;
                    bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
                    var _cliente = await _switchCliente.ConsultaCliente(new Cliente() { Identificacion = registro.Identificacion },estaConectado);
                    _isEditMode = registro.EsEdicion == 1 ? true : false;
                    GestionSeleccionado = new GestionProceso() { 
                    IdOrden=registro.Id
                    ,IdClienteProveedor=_cliente.Id
                    ,Identificacion=_cliente.Identificacion
                    ,NombreClienteProveedor=registro.NombreCliente
                    ,NumeroTelefono=_cliente.Celular
                    ,EstaGestionado=0
                    ,TipoGestion= TextsTranslateManager.Translate("SelectTipoGestion")
                    ,Respuesta = TextsTranslateManager.Translate("SelectRespuestaGestion")
                    ,Observacion =""
                    };//registro;                    
                    EsVisibleFechas = false;
                    TipoGestion = TextsTranslateManager.Translate("SelectTipoGestion");
                    Respuesta = TextsTranslateManager.Translate("SelectRespuestaGestion");
                }
                else
                {
                    Load();
                }

            IsBusy = false;
        }

        /// <summary>
        /// Método para almacenar información
        /// </summary>
        /// <returns></returns>
        private async Task Ok_CommandAsync()
        {
            IsBusy = true;
            //1. Validar información
            bool result = await Validate();
            if (!result)
            {
                IsBusy = false;
                return;
            }
            GestionSeleccionado.Id = Generator.GenerateKey();
            GestionSeleccionado.EstaGestionado = 1;            
            string informedMessage = "";

            
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            //1. Añadir los datos de la gestión
            OrdenSeleccionado.ListaGestion=OrdenSeleccionado!=null && OrdenSeleccionado.GestionSerializado!=null?
                             JsonConvert.DeserializeObject<ObservableCollection<GestionProceso>>(OrdenSeleccionado.GestionSerializado):new ObservableCollection<GestionProceso>();
            OrdenSeleccionado.ListaGestion.Add(GestionSeleccionado);
            OrdenSeleccionado.GestionSerializado = JsonConvert.SerializeObject(OrdenSeleccionado.ListaGestion);
            var resultado = await _switchCompras.GuardaRegistro_Orden(OrdenSeleccionado, estaConectado, true);
            informedMessage = resultado.mensaje;            
            IsBusy = false;
            informedMessage = resultado.mensaje;
            DialogService.ShowToast(informedMessage);
        }
        private async Task<bool> Validate()
        {
            bool validado = true;
            if (string.IsNullOrEmpty(GestionSeleccionado.IdTipoGestion))
            {
                await App.Current.MainPage.DisplayAlert("Importante", "Debe seleccionar el Tipo de Gestión", "OK");
                return false;
            }
            else if (string.IsNullOrEmpty(GestionSeleccionado.IdRespuesta))
            {
                await App.Current.MainPage.DisplayAlert("Importante", "Debe seleccionar la Respuesta", "OK");
                return false;
            }                
            else if (string.IsNullOrEmpty(GestionSeleccionado.Observacion))
            {
                await App.Current.MainPage.DisplayAlert("Importante", "Debe seleccionar la Observación", "OK");
                return false;
            }                
            return validado;
        }

        /// <summary>
        /// Método de Asignación de valores recibidos por parámetros
        /// </summary>
        /// <param name="registro"></param>
        /// <returns></returns>
        private async Task BindExistingData(Cliente iregistro)
        {
            
            FechaNacimiento = DateTimeHelpers.GetDateLong(iregistro.FechaNacimiento);
            IdGaleria = iregistro.IdGaleria;
            
        }
        
        private void cargaDatosExternos()
        {
            Descripcion = "";
            

        }

        #endregion 4.MétodosAdministraciónDatos

        #region 5.MétodosGenerales
        /// <summary>
        /// Reiniciar valores de controles
        /// </summary>
        private void ResetForm()
        {
            
            Descripcion = "";
            IdGaleria = "";
            Imagen = "";

            FuenteImagenRegistro = ImageSource.FromStream(() =>
            {
                var stream = new MemoryStream();
                return stream;
            });

            _isEditMode = false;
        }
        
                
        /// <summary>
        /// Método para salir de la pantalla
        /// </summary>
        /// <returns></returns>
        private async Task Cancel_CommandAsync()
        {
            await NavigationService.RemoveLastFromBackStackAsync();
        }
        /// <summary>
        /// Evento cuando se escoge la opción EsProveedor
        /// </summary>
        /// <returns></returns>
        
        public async Task OpenTipoGestionAsync()
        {
            await NavigationService.NavigateToAsync<CatalogoViewModel>(new Catalogo()
            {
                CodigoCatalogo = "tipogestion"
            ,
                EsJerarquico = 0
            });
        }
        /// <summary>
        /// Abrir pantalla para selección de tipo de proveedor
        /// </summary>
        /// <returns></returns>
        public async Task OpenRespuestaGestionAsync()
        {
            await NavigationService.NavigateToAsync<CatalogoViewModel>(new Catalogo()
            {
                CodigoCatalogo = "respuestagestion"
            ,
                EsJerarquico = 0
            });
        }
        /// <summary>
        /// Método para realizar llamadas
        /// </summary>
        /// <param name="number"></param>
        public async Task PlacePhoneCall()
        {
            string number = GestionSeleccionado.NumeroTelefono;
            try
            {
                if(number.Length==10)
                {
                    var telefonoIso = "+593" + number.Substring(1);
                    // Make Phone Call
                    var phoneDialer = CrossMessaging.Current.PhoneDialer;
                    if (phoneDialer.CanMakePhoneCall)
                        phoneDialer.MakePhoneCall(telefonoIso);
                }
                    //PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
                await App.Current.MainPage.DisplayAlert("Importante", "Marcación Telefónica no está soportado en este dispositivo", "OK");
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        /// <summary>
        /// Envío Sms
        /// </summary>
        /// <param name="messageText"></param>
        /// <param name="recipient"></param>
        /// <returns></returns>
        public async Task SendSms()
        {       
            await devuelveMensaje();
            string messageText = GestionSeleccionado.Mensaje;
            string recipient = GestionSeleccionado.NumeroTelefono;
            try
            {

                var telefonoIso = "+593" + recipient.Substring(1);
                // Send Sms
                /*var smsMessenger = CrossMessaging.Current.SmsMessenger;
                if (smsMessenger.CanSendSms)
                    smsMessenger.SendSms(telefonoIso, messageText);
                */
                var smsMessenger = CrossMessaging.Current.SmsMessenger;
                if (smsMessenger.CanSendSmsInBackground)
                    smsMessenger.SendSmsInBackground(telefonoIso, messageText);
                //var message = new SmsMessage(messageText, new[] { recipient });
                //await Sms.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Sms is not supported on this device.
                await App.Current.MainPage.DisplayAlert("Importante", "Sms no está soportado en este dispositivo", "OK");
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        private async Task devuelveMensaje()
        {
            string mensaje = "";
            switch(GestionSeleccionado.Respuesta)
            {                
                case string a when GestionSeleccionado.Respuesta.ToString().ToUpper().Contains("ienvenida"):
                    mensaje="Bienvenido Sr/a " + GestionSeleccionado.NombreClienteProveedor + " , le saluda " + SettingsOnline.oAplicacion.Nombre;
                    break;
                default:
                    mensaje = "Estimado Sr/a " + GestionSeleccionado.NombreClienteProveedor + " , le saluda " + SettingsOnline.oAplicacion.Nombre;
                    break;
            }
            GestionSeleccionado.Mensaje = mensaje;
            //return mensaje;
        }

        private void evaluaTipoGestion()
        {
            OrdenSeleccionado.OrdenDetalleCompras = OrdenSeleccionado != null && OrdenSeleccionado.OrdenDetalleCompraSerializado != null ?
                             JsonConvert.DeserializeObject<ObservableCollection<OrdenDetalleCompra>>(OrdenSeleccionado.OrdenDetalleCompraSerializado) : new ObservableCollection<OrdenDetalleCompra>();
            var item = OrdenSeleccionado.OrdenDetalleCompras.Where(x => x.EsServicio == 1).FirstOrDefault();
            if (item != null)
            {            
                GestionSeleccionado.FechaInicio = DateTime.Now.ToString("dd/MM/yyyy");
                GestionSeleccionado.FechaFin = obtieneFechaServicio(item);
                EsVisibleFechas = true;
            }
        }

        public string obtieneFechaServicio(OrdenDetalleCompra OrdenDetalleCompraSeleccionado)
        {
            string retorno = DateTime.Now.ToString("dd/MM/yyyy");            
                switch (OrdenDetalleCompraSeleccionado.PeriodoServicio)
                {
                    case string a when OrdenDetalleCompraSeleccionado.PeriodoServicio.ToString().ToUpper().Contains("AÑO"):
                        var y = Convert.ToDateTime(retorno).AddYears((int)OrdenDetalleCompraSeleccionado.CantidadPeriodoServicio);
                        retorno = y.ToString("dd/MM/yyyy");
                        break;
                case string b when OrdenDetalleCompraSeleccionado.PeriodoServicio.ToString().ToUpper().Contains("MES"):
                    var y1 = Convert.ToDateTime(retorno).AddMonths((int)OrdenDetalleCompraSeleccionado.CantidadPeriodoServicio);
                    retorno = y1.ToString("dd/MM/yyyy");
                    break;
                case string c when OrdenDetalleCompraSeleccionado.PeriodoServicio.ToString().ToUpper().Contains("DIA"):
                    var y2 = Convert.ToDateTime(retorno).AddDays((int)OrdenDetalleCompraSeleccionado.CantidadPeriodoServicio);
                    retorno = y2.ToString("dd/MM/yyyy");
                    break;
            }            
            return retorno;
        }

        #endregion 5.MétodosGenerales

    }
}
