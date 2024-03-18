using DataModel.DTO.Compras;
using ManijodaServicios.AppSettings;
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

namespace Escant_App.ViewModels.Inventario
{
    public class StockViewModel:ViewModelBase
    {
        #region 1. Declaración de Variables
        /// <summary>
        /// 1. Declaración de las variables                 
        /// </summary>
        #region 1.1 Variables de Integración
        private SwitchCompras _switchCompras;
        private Generales.Generales generales = new Generales.Generales();
        private readonly IServicioCompras_Orden _ordenServicio;
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase
        private ObservableCollection<Orden> _registros;
        private Orden _registroSeleccionado;
        #endregion 1.2 Variables de Instancia de Objetos para la Clase
        #region 1.3 Variables de Vinculación de Controles de Vista y Modelo
        internal SfListView ListView { get; set; }
        internal SearchBar BarraBusqueda { get; set; }
        #endregion 1.3 Variables de Vinculación de Controles de Vista y Modelo
        #region 1.4 Variables de Control de Datos
        private string _ordenamientoPorNombre = "-1";
        private string _textoBusqueda;
        private string _orientacion;
        private bool _isVisibleBotones;
        private string _menuImagen;
        #endregion 1.4 Variables de Control de Datos
        #region 1.5 Variables de Control de Errores
        #endregion 1.5 Variables de Control de Errores
        #region 1.6 Variables de Acciones
        #endregion 1.6 Variables de Acciones
        #endregion 1. Declaración de Variables

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

        public bool IsVisibleBotones
        {
            get => _isVisibleBotones;
            set
            {
                _isVisibleBotones = value;
                RaisePropertyChanged(() => IsVisibleBotones);
            }
        }
        public string MenuImagen
        {
            get => _menuImagen;
            set
            {
                _menuImagen = value;
                RaisePropertyChanged(() => MenuImagen);
            }
        }

        #endregion 2.InstanciaAsignaVariablesControles        

        #region 3.Constructor
        /// <summary>
        /// 1. Declaración de constructor
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public StockViewModel(IServicioSeguridad_Usuario seguridadServicio, IServicioCompras_Orden ordenServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);
            //Instanciación Acorde al model
            _ordenServicio = ordenServicio;
            _switchCompras = new SwitchCompras(_ordenServicio);
            startControles();

        }
        #endregion 3.Constructor       

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
                TipoOrden = "Inventario"            
            }, estaConectado);
            if (registros != null && registros.Count > 0)
            {
                Registros = generales.ToObservableCollection<Orden>(registros.OrderByDescending(x => x.NumeroOrden).ToList());
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
        public async Task AddRegistroAsync()
        {            
            await NavigationService.NavigateToAsync<DetalleInventarioViewModel>(RegistroSeleccionado);
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
                await NavigationService.NavigateToAsync<DetalleInventarioViewModel>(registro);
                ItemIndex = -1;
            }
        }
        private async Task DeleteRegistroAsync()
        {
            /*
            if (ItemIndex >= 0)
            {
                var supplier = Empresas.ElementAt(ItemIndex);
                bool result = await DialogService.ShowConfirmedAsync(string.Format(TextsTranslateManager.Translate("SupplierDeleteWarning"), supplier.Name), TextsTranslateManager.Translate("Warning"), "Ok", TextsTranslateManager.Translate("Cancel"));
                if (result && supplier != null)
                {
                    bool canNotDeleteSupplier = await _productService.IsSupplierExistsInPurchaseOrder(supplier.Id);
                    if (canNotDeleteSupplier)
                    {
                        await DialogService.ShowAlertAsync(TextsTranslateManager.Translate("InvalidSupplierDelete"), TextsTranslateManager.Translate("Warning"), "OK");
                    }
                    else
                    {
                        IsBusy = true;
                        Suppliers.RemoveAt(ItemIndex);
                        await _supplierService.DeleteSupplierAsync(supplier);
                        DialogService.ShowToast(string.Format(TextsTranslateManager.Translate("SupplierDeleted"), supplier.Name));
                        IsBusy = false;
                    }
                }
            }
            ItemIndex = -1;
            ListView?.ResetSwipe();
            */
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
        #endregion 5.MétodosGenerales

    }
}
