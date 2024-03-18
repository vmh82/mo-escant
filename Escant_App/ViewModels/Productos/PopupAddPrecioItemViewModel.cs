using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Escant_App.ViewModels.Base;
using Escant_App.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using DataModel;
using ManijodaServicios.Resources.Texts;
using DataModel.DTO;
using DataModel.DTO.Productos;
using DataModel.Helpers;
using System.Linq.Expressions;
using Escant_App.Extensions;
using ManijodaServicios.Switch;
using ManijodaServicios.Offline.Interfaz;
using System.Globalization;

namespace Escant_App.ViewModels.Productos
{
    public class PopupAddPrecioItemViewModel : ViewModelBase
    {

        #region 0. Declaración Variables
        // <summary>
        /// 0. Declaración de las variables         
        /// </summary>
        /// 0.0 Fijos en cada Model
        private SwitchProductos _switchProductos;
        private Generales.Generales generales = new Generales.Generales();        
        private readonly IServicioProductos_Precio _precioServicio;
        Precio _precioSeleccionado;
        Precio _precio;
        bool _isEditMode;
        bool _estaActivo;
        public CultureInfo Cultura => Escant_App.AppSettings.Settings.CustomCurrencySymbol == "$" ? new System.Globalization.CultureInfo("en-US") : new System.Globalization.CultureInfo("en-US");
        #region 0.6 Acciones
        public Action<bool> EventoGuardar { get; set; }
        public Action<bool> EventoCancelar { get; set; }
        #endregion 0.6 Acciones
        #endregion 0. Declaración Variables
        #region 1. Asignación valores a Control

        public bool EstaActivo
        {
            get => _estaActivo;
            set
            {
                _estaActivo = value;
                RaisePropertyChanged(() => EstaActivo);
            }
        }
        #endregion 1. Asignación valores a Control
        public Precio PrecioSeleccionado
        {
            get => _precioSeleccionado;
            set
            {
                _precioSeleccionado = value;
                RaisePropertyChanged(() => PrecioSeleccionado);
            }
        }
        #region 2. Constructor
        public PopupAddPrecioItemViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  , IServicioProductos_Precio precioServicio)
        {
            BindingContext = new ConectividadModelo(seguridadServicio);            
            _precioServicio = precioServicio;            
            _switchProductos = new SwitchProductos(_precioServicio);
            ResetForm();
        }
        #endregion 2. Constructor
        #region 3. Definición de Métodos
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());
        #endregion 3. Definición de Métodos
        #region 4. Métodos de Administración Datos
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            PageTitle = TextsTranslateManager.Translate("AddPriceTitle");
            //Bind data for edit mode           
            await Task.Run(() =>
            {
                if (navigationData is Precio precio && precio != null)
                {
                    _isEditMode = true;
                    _precio = precio;
                    PageTitle = string.Format(TextsTranslateManager.Translate("EditPriceTitle"), precio.NombreItem);// TextsTranslateManager.Translate("EditPriceTitle");
                    BindExistingData(_precio);
                }
            });
            //Inicia Información de la Página            
            IsBusy = false;
        }

        /// <summary>
        /// Método para almacenar información
        /// </summary>
        /// <returns></returns>
        private async Task Ok_CommandAsync()
        {
            EventoGuardar?.Invoke(true);

        }
        public async Task almacenaDatos()
        {
            IsBusy = true;            
            bool estaConectado = Convert.ToBoolean(generales.ConsultaSesion("EstaConectado") == "" ? "false" : generales.ConsultaSesion("EstaConectado"));            
            PrecioSeleccionado.EstaActivo = EstaActivo ? 1 : 0;
            PrecioSeleccionado.subCatalogos = null;
            var resultado = await _switchProductos.GuardaRegistro_Precio(PrecioSeleccionado, estaConectado, true);
            //MessagingCenter.Send<string>(string.Empty, "OnDataSourcePopupAddPrecioChanged");
            //string informedMessage = "";
            //informedMessage = resultado.mensaje;
            //DialogService.ShowToast(informedMessage);
            //ResetForm();
            IsBusy = false;                       
        }


        #endregion 4. Métodos de Administración Datos

        #region 5. Métodos Generales
        private async Task Cancel_CommandAsync()
        {
            EventoCancelar?.Invoke(true);
        }
        /// <summary>
        /// Reiniciar valores de controles
        /// </summary>
        private void ResetForm()
        {
            EstaActivo = true;
            PrecioSeleccionado = new Precio();
            PrecioSeleccionado.PrecioVenta = 0;
        }
        private void BindExistingData(Precio precio)
        {
            PrecioSeleccionado = precio;
        }
        
        #endregion 5. Métodos Generales


    }
}
