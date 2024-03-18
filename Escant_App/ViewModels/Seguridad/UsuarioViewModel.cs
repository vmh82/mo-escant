using DataModel.DTO.Administracion;
using DataModel.DTO.Seguridad;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.Switch;
using Escant_App.ViewModels.Administracion;
using Escant_App.ViewModels.Base;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace Escant_App.ViewModels.Seguridad
{
    public class UsuarioViewModel:ViewModelBase
    {
        #region 0.DeclaracionVariables
        /// <summary>
        /// 0. Declaración de las variables                 
        /// </summary>
        /// 0.0 Referencias a otras clases
        private SwitchSeguridad _switchSeguridad;
        private Generales.Generales generales = new Generales.Generales();

        private readonly IServicioSeguridad_Usuario _usuarioServicio;

        /// 0.1 Referencias a instancias de objetos a utilizarse en la Clase
        private ObservableCollection<Usuario> _registros;
        private Usuario _registroSeleccionado;


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
        /// <param name="usuarioServicio"></param>
        public UsuarioViewModel(IServicioSeguridad_Usuario usuarioServicio)
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(usuarioServicio);
            //Instanciación Acorde al model
            _usuarioServicio = usuarioServicio;
            _switchSeguridad = new SwitchSeguridad(_usuarioServicio);
        }
        #endregion 1.Constructor       

        #region 2.InstanciaAsignaVariablesControles
        /// 2. Asignacion y Lectura de variables vinculadas a los controles
        /// <summary>
        /// Método Asignación o lectura de CodigoEstablecimiento
        /// </summary>
        public ObservableCollection<Usuario> Registros
        {
            get => _registros;
            set
            {
                _registros = value;
                RaisePropertyChanged(() => Registros);
            }
        }

        public Usuario RegistroSeleccionado
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
            var registros = await _switchSeguridad.ConsultaUsuarios(new Usuario() { Deleted = 0 }, estaConectado);
            if (registros != null && registros.Count > 0)
            {
                Registros = generales.ToObservableCollection<Usuario>(registros);
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
            if (navigationData is Empresa registro && registro != null)
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
            var registros = await _switchSeguridad.ConsultaUsuarios(new Usuario() { valorBusqueda = searchKey }, estaConectado);
            if (registros != null && registros.Count > 0)
            {
                Registros = generales.ToObservableCollection<Usuario>(registros);
            }

        }

        #endregion 4.MétodosAdministraciónDatos

        #region 5.MétodosGenerales
        /// <summary>
        /// Retornar en navegación
        /// </summary>
        /// <returns></returns>
        public async Task GoBack()
        {
            await NavigationService.NavigateToRootPage();
        }
        /// <summary>
        /// Llamada a la Pantalla de Mantenimiento para un nuevo registros
        /// </summary>
        /// <returns></returns>
        private async Task AddRegistroAsync()
        {
            await NavigationService.NavigateToAsync<AddUsuarioViewModel>(null);
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
                await NavigationService.NavigateToAsync<AddUsuarioViewModel>(registro);
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
        #endregion 5.MétodosGenerales

    }
}
