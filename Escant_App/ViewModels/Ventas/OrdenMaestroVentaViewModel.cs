﻿using DataModel.DTO.Compras;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Switch;
using Escant_App.ViewModels.Base;
using Escant_App.ViewModels.Documentos;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using ManijodaServicios.AppSettings;
using Escant_App.ViewModels.Gestion;
using DataModel.DTO.Configuracion;
using Escant_App.ViewModels.Reportes;
using Escant_App.Views.Reportes;
using Rg.Plugins.Popup.Services;
using Newtonsoft.Json;
using System.Globalization;
using Escant_App.Common;

namespace Escant_App.ViewModels.Ventas
{
    public class OrdenMaestroVentaViewModel : ViewModelBase
    {
        #region 0.DeclaracionVariables
        /// <summary>
        /// 0. Declaración de las variables                 
        /// </summary>
        /// 0.0 Referencias a otras clases
        private SwitchCompras _switchCompras;
        private Generales.Generales generales = new Generales.Generales();
        private PrinterSettings printerSettings { get; set; } = new PrinterSettings();

        private readonly IServicioCompras_Orden _ordenServicio;

        /// 0.1 Referencias a instancias de objetos a utilizarse en la Clase
        private ObservableCollection<Orden> _registros;
        private Orden _registroSeleccionado;


        /// <summary>
        /// 0.2 Declaración de las variables de lectura y escritura para los controles
        /// </summary>
        internal SfListView ListView { get; set; }
        internal SearchBar BarraBusqueda { get; set; }


        /// <summary>
        /// 0.3 Declaración variables control de datos
        /// </summary>
        private string _ordenamientoPorNombre = "-1";
        private string _textoBusqueda;
        private string _orientacion;

        /// <summary>
        /// 0.4 Declaración de las variables de control de errores
        /// </summary>


        #endregion DeclaracionVariables

        #region 1.Constructor
        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public OrdenMaestroVentaViewModel(IServicioSeguridad_Usuario seguridadServicio, IServicioCompras_Orden ordenServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);
            //Instanciación Acorde al model
            _ordenServicio = ordenServicio;
            _switchCompras = new SwitchCompras(_ordenServicio);
            startControles();

        }
        #endregion 1.Constructor       

        #region 2.InstanciaAsignaVariablesControles
        /// 2. Asignacion y Lectura de variables vinculadas a los controles
        /// <summary>
        /// Método Asignación o lectura de CodigoEstablecimiento
        /// </summary>
        public ObservableCollection<Orden> Registros
        {
            get => _registros;
            set
            {
                _registros = value;
                RaisePropertyChanged(() => Registros);
            }
        }

        public Orden RegistroSeleccionado
        {
            get => _registroSeleccionado;
            set
            {
                _registroSeleccionado = value;
                RaisePropertyChanged(() => RegistroSeleccionado);
            }
        }

        public string OrdenamientoPorNombre
        {
            get => _ordenamientoPorNombre;
            set
            {
                _ordenamientoPorNombre = value;
                RaisePropertyChanged(() => OrdenamientoPorNombre);
            }
        }
        public string TextoBusqueda
        {
            get { return _textoBusqueda; }
            set
            {
                _textoBusqueda = value;
                RaisePropertyChanged(() => TextoBusqueda);
            }
        }
        public string Orientacion
        {
            get { return _orientacion; }
            set
            {
                _orientacion = value;
                RaisePropertyChanged(() => Orientacion);
            }
        }
        #endregion 2.InstanciaAsignaVariablesControles        

        #region 3.DefinicionMetodos
        /// <summary>
        /// 3. Definición de métodos de la pantalla
        /// </summary>
        /// <summary>
        public ICommand AddRegistroCommand => new Command(async () => await AddRegistroAsync());
        public ICommand UpdateRegistroCommand => new Command(async () => await UpdateRegistroAsync());
        public ICommand DeleteRegistroCommand => new Command(async () => await DeleteRegistroAsync());
        public ICommand NameSortingCommand => new Command(async () => await NameSortingCommandAsync());
        public ICommand ViewDocumentoCommand => new Command(async () => await ViewDocumentoAsync());
        public ICommand ViewOpcionesImpresionCommand => new Command(async () => await ViewOpcionesImpresionAsync());

        #endregion 3.DefinicionMetodos

        #region 4.MétodosAdministraciónDatos
        /// <summary>
        /// 4. Métodos de administración de datos
        /// </summary>
        /// <returns></returns>
        /// 4.1. Método para Cargar información de pantalla, la llamada realizada desde el frontend al inicializar
        /// </summary>
        /// <returns></returns>
        public async Task LoadData()
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var registros = await _switchCompras.ConsultaOrdenes(new Orden() 
            { 
              TipoOrden = "Venta" 
            , Identificacion=SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("SITANT") || SettingsOnline.oAplicacion.PerfilConectado.ToUpper().Contains("LIENTE")
                            ? SettingsOnline.oAplicacion.IdentificacionConectado:null
            }, estaConectado);
            if (registros != null && registros.Count > 0)
            {
                Registros = generales.ToObservableCollection<Orden>(registros.OrderByDescending(x=>Convert.ToInt32(x.NumeroOrden)).ToList());
            }
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
                RegistroSeleccionado = Registros?.FirstOrDefault(x => x.Id == registro.Id);
            }

            IsBusy = false;

        }
        /// <summary>
        /// 4.4 Método para buscar registros
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public async Task SearchRegistro(string searchKey)
        {
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));
            var registros = await _switchCompras.ConsultaOrdenes(new Orden() { valorBusqueda = searchKey }, estaConectado);
            if (registros != null && registros.Count > 0)
            {
                Registros = generales.ToObservableCollection<Orden>(registros);
            }

        }

        #endregion 4.MétodosAdministraciónDatos

        #region 5.MétodosGenerales        
        private async void startControles()
        {
            Registros = new ObservableCollection<Orden>();
            RegistroSeleccionado = new Orden();
        }
        /// <summary>
        /// Llamada a la Pantalla de Mantenimiento para un nuevo registros
        /// </summary>
        /// <returns></returns>
        private async Task AddRegistroAsync()
        {
            var registro = new Orden()
            {
                EsEdicion = 0
            };
            await NavigationService.NavigateToAsync<IngresoVentasViewModel>(registro);
        }
        /// <summary>
        /// Llamada a la Pantalla de Mantenimiento para actualización del registro
        /// </summary>
        /// <returns></returns>
        public async Task UpdateRegistroAsync()
        {
            if (ItemIndex >= 0)
            {
                var registro = Registros.ElementAt(ItemIndex);
                registro.EsEdicion = 1;
                await NavigationService.NavigateToAsync<IngresoVentasViewModel>(registro);
                ItemIndex = -1;
            }
        }
        private async Task DeleteRegistroAsync()
        {
            if (ItemIndex >= 0)
            {
                var registro = Registros.ElementAt(ItemIndex);
                registro.EsEdicion = 0;
                await NavigationService.NavigateToAsync<AddGestionViewModel>(registro);
                ItemIndex = -1;
            }
            
        }
        /// <summary>
        /// Método para Ordenamiento por Nombre
        /// </summary>
        /// <returns></returns>
        private async Task NameSortingCommandAsync()
        {
            IsBusy = true;
            if (OrdenamientoPorNombre == "-1")
            {
                OrdenamientoPorNombre = "0";
            }
            else
            {
                OrdenamientoPorNombre = OrdenamientoPorNombre == "0" ? "1" : "0";
            }
            Orientacion = OrdenamientoPorNombre == "0" ? "Ascending" : "Descending";

            //await LoadData();
            IsBusy = false;
        }
        public async Task ViewDocumentoAsync()
        {
            IsBusy = true;
            try
            {
                if (ItemIndex >= 0)
                {
                    var registro = Registros.ElementAt(ItemIndex);
                    registro.EsEdicion = 1;
                    RegistroSeleccionado = registro;
                    if (!RegistroSeleccionado.CodigoEstado.Contains("Pendie"))
                    {
                        await NavigationService.NavigateToAsync<PdfViewerViewModel>(RegistroSeleccionado);
                    }
                    ItemIndex = -1;
                }
            }
            catch (Exception ex)
            {

            }
            IsBusy = false;
        }
        public async Task ViewOpcionesImpresionAsync()
        {
            if (ItemIndex >= 0)
            {
                var registro = Registros.ElementAt(ItemIndex);
                registro.EsEdicion = 1;
                RegistroSeleccionado = registro;
                if (!RegistroSeleccionado.CodigoEstado.Contains("Pendie"))
                {
                    navegacion();                  
                }
                ItemIndex = -1;
            }
            
        }
        private async void navegacion()
        {
            //await NavigationService.NavigateToPopupAsync<PopupOpcionesImpresionViewModel>(RegistroSeleccionado);
            var returnPage2 = await NavigationService.NavigateToPopupAsync<PopupOpcionesImpresionViewModel>(RegistroSeleccionado);
            var popupPage2 = returnPage2 as PopupOpcionesImpresionView;
            popupPage2.CloseButtonEventHandler += async (sender, obj) =>
            {
                var isCancel = ((PopupOpcionesImpresionView)sender).IsCancel;
                if (!isCancel)
                {
                    var resultado = ((PopupOpcionesImpresionView)sender).PopupResult;
                    if (resultado != null)
                    {
                        RegistroSeleccionado = resultado;
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (!RegistroSeleccionado.CodigoEstado.Contains("Pendie"))
                            {
                                if (RegistroSeleccionado.CodigoEstadoEnConsulta.ToUpper().Contains("RECIBO"))
                                    await ImprimirOrden();
                                else
                                    await NavigationService.NavigateToAsync<PdfViewerViewModel>(RegistroSeleccionado);
                            }
                        });
                    }
                }
                await PopupNavigation.Instance.PopAsync();
            };
            var result2 = await popupPage2.PageClosedTask;
        }
        private async Task ImprimirOrden()
        {

            var bluetooth = App.BluetoothAdaper;
            if (!bluetooth.IsBluetoothTurnOn)
                bluetooth.Enable();

            if (bluetooth.IsBluetoothTurnOn)
            //if(true==true)
            {

                var billInfo = RegistroSeleccionado;
                SettingsOnline.oAplicacion.Lenguaje = string.IsNullOrEmpty(Escant_App.AppSettings.Settings.LanguageSelected) ? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName : Escant_App.AppSettings.Settings.LanguageSelected;
                SettingsOnline.oAplicacion.FormatoActual = string.IsNullOrEmpty(Escant_App.AppSettings.Settings.CurrentCurrencyCustomFormat) ? SystemConstants.VN_CURRENTCY_FORMAT : Escant_App.AppSettings.Settings.CurrentCurrencyCustomFormat;
                billInfo.ParametrosImpresion = JsonConvert.SerializeObject(SettingsOnline.oAplicacion);
                //Facturación Electrónica
                //var y= await bluetooth.ProcesaFacturaElectronica(billInfo);
                printerSettings = JsonConvert.DeserializeObject<PrinterSettings>(SettingsOnline.oAplicacion.ParametrosImpresion);
                /*var result = await bluetooth.ConnectAndPrintOrden(printerSettings?.DefaultDevice, billInfo);
                if (!result)
                {
                    await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("PrintError"), TextsTranslateManager.Translate("Warning"), "OK");
                }*/
            }
        }
        #endregion 5.MétodosGenerales
    }

}
