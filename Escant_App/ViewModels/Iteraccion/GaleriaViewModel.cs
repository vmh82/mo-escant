using DataModel.DTO.Administracion;
using DataModel.DTO.Iteraccion;
using Escant_App.ViewModels.Administracion;
using Escant_App.ViewModels.Base;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Iteraccion
{
    public class GaleriaViewModel : ViewModelBase
    {
        #region 0.Declaración Variables
        private ObservableCollection<FuenteIconos> _fuenteIconos;
        private ObservableCollection<FuenteLibreria> _fuenteLibreria;
        private FuenteLibreria _fuenteLibreriaSeleccionado;
        private Generales.Generales generales;
        private string _nombreIcono;
        private string _codigoIcono;
        private string _fuente;
        private Color _color;       
        private ColorAplicacion _colorSeleccionado;
        private FuenteIconos _fuenteIconosSeleccionada;
        internal SfListView ListView { get; set; }
        /// <summary>
        /// 0.3 Declaración variables control de datos
        /// </summary>
        private string _ordenamientoPorNombre = "-1";
        private string _textoBusqueda;        
        private string _orientacion;
        #endregion 0.Declaración Variables
        #region 1.Constructor
        public GaleriaViewModel()
        {
            generales = new Generales.Generales();                       
            MessagingCenter.Subscribe<ColorAplicacion>(this, "OnDataSourceChanged", (args) =>
            {
                ColorSeleccionado = args;
                ColorImagen = Color.FromHex(ColorSeleccionado.Codigo);
                App.Current.Resources["BlueColor"] = ColorImagen;
            });            
        }
        #endregion 1.Constructor
        #region 2.Asignación Variables
        public ObservableCollection<FuenteIconos> FuenteIconos
        {
            get => _fuenteIconos;
            set
            {
                _fuenteIconos = value;
                RaisePropertyChanged(() => FuenteIconos);
            }
        }
        public FuenteIconos FuenteIconoSeleccionado
        {
            get => _fuenteIconosSeleccionada;
            set
            {
                _fuenteIconosSeleccionada = value;
                RaisePropertyChanged(() => FuenteIconoSeleccionado);
            }
        }
        public ObservableCollection<FuenteLibreria> FuenteLibrerias
        {
            get => _fuenteLibreria;
            set
            {
                _fuenteLibreria = value;
                RaisePropertyChanged(() => FuenteLibrerias);
            }
        }
        public FuenteLibreria FuenteLibreriaSeleccionado
        {
            get => _fuenteLibreriaSeleccionado;
            set
            {
                _fuenteLibreriaSeleccionado = value;
                RaisePropertyChanged(() => FuenteLibreriaSeleccionado);
            }
        }
        public string Fuente
        {
            get => _fuente;
            set
            {
                _fuente = value;
                RaisePropertyChanged(() => Fuente);
            }
        }

        public string CodigoIcono
        {
            get => _codigoIcono;
            set
            {
                _codigoIcono = value;
                RaisePropertyChanged(() => CodigoIcono);
            }
        }
        public string NombreIcono
        {
            get => _nombreIcono;
            set
            {
                _nombreIcono = value;
                RaisePropertyChanged(() => NombreIcono);
            }
        }

        public FuenteLibreria FuenteLibreriaSeleccionada
        {
            get => _fuenteLibreriaSeleccionado;
            set
            {
                _fuenteLibreriaSeleccionado = value;
                RaisePropertyChanged(() => FuenteLibreriaSeleccionada);
                eventoLibreriaSeleccionada();
            }
        }
        public ColorAplicacion ColorSeleccionado
        {
            get => _colorSeleccionado;
            set
            {
                _colorSeleccionado = value;
                RaisePropertyChanged(() => ColorSeleccionado);
            }
        }
        public Color ColorImagen
        {
            get => _color;
            set
            {
                _color = value;
                RaisePropertyChanged(() => ColorImagen);
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
        public string Orientacion
        {
            get { return _orientacion; }
            set
            {
                _orientacion = value;
                RaisePropertyChanged(() => Orientacion);
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
        #endregion 2.Asignación Variables
        #region 3. Definición de eventos
        public ICommand AddRegistroCommand => new Command(async () => await AddRegistroAsync());
        public ICommand UpdateRegistroCommand => new Command(async () => await UpdateRegistroAsync());
        public ICommand DeleteRegistroCommand => new Command(async () => await DeleteRegistroAsync());
        public ICommand OpenColorCommand => new Command(async () => await OpenColorAsync());
        public ICommand NameSortingCommand => new Command(async () => await NameSortingCommandAsync());
        public ICommand ViewRegistroCommand => new Command<FuenteIconos>(async (fuenteIconos) => await RegistroViewTapped(fuenteIconos));
        #endregion 3. Definición de eventos
        #region 4. Métodos Administración Datos
        #endregion 4. Métodos Administración Datos
        #region 5.Métodos Generales
        /// <summary>
        /// Llamada a la Pantalla de Mantenimiento para un nuevo registros
        /// </summary>
        /// <returns></returns>
        private async Task AddRegistroAsync()
        {
            //await NavigationService.NavigateToAsync<AddEmpresaViewModel>(null);
        }
        public async Task UpdateRegistroAsync()
        {
            if (ItemIndex >= 0)
            {
            //    await NavigationService.NavigateToAsync<AddProductViewModel>(Products.ElementAt(ItemIndex));
            }
        }
        public async Task DeleteRegistroAsync()
        {
            if (ItemIndex >= 0)
            {
                //    await NavigationService.NavigateToAsync<AddProductViewModel>(Products.ElementAt(ItemIndex));
            }
        }
        public async Task RegistroViewTapped(FuenteIconos data)
        {
            //await NavigationService.NavigateToAsync<AddProductViewModel>(data);                        
        }
        /// <summary>
        /// Llamada a la Pantalla de Selección de Color
        /// </summary>
        /// <returns></returns>
        public async Task OpenColorAsync()
        {            
            await NavigationService.NavigateToPopupAsync<PaletaColoresViewModel>(ColorSeleccionado);
        }
        /// <summary>
        /// Método para iniciar valores
        /// </summary>
        /// <returns></returns>
        public async Task IniciaColor()
        {
            
            var z = new Fuentes().fuentes.GroupBy(p => new { p.Fuente, p.AliasFuente }).Select(g => g.First()).Select(e => new FuenteLibreria(e.Fuente, e.AliasFuente)).ToList();
            FuenteLibrerias = generales.ConvertObservable<FuenteLibreria>(z.Distinct());
            FuenteLibreriaSeleccionado = FuenteLibrerias.First();
            cambiaIconosFuente();
            ColorSeleccionado.Codigo = "#BC4C1B";
            App.Current.Resources["BlueColor"] = Color.FromHex(ColorSeleccionado.Codigo);
        }
        /// <summary>
        /// Método para regresar a la Pantalla de Llamada
        /// </summary>
        /// <returns></returns>
        public async Task GoBack()
        {
            await NavigationService.NavigateToRootPage();
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
        private void eventoLibreriaSeleccionada()
        {
            cambiaIconosFuente();
        }
        private void cambiaIconosFuente()
        {
            FuenteIconos = generales.ToObservableCollection<FuenteIconos>(new Fuentes().fuentes.Where(f => f.Fuente == FuenteLibreriaSeleccionado.Fuente).ToList());
            App.Current.Resources["Fontello"] = Preferences.Get("Value", FuenteLibreriaSeleccionado.AliasFuetne);
        }
        #endregion 5.Métodos Generales
    }
}
