using DataModel.DTO.Compras;
using Escant_App.ViewModels.Base;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Compras
{
    public class PopupActualizaItemOrdenComprasViewModel : ViewModelBase
    {
        #region 1. Declaración de Variables
        #region 1.1 Variables de Integración
        #endregion 1.1 Variables de Integración
        #region 1.2 Variables de Instancia de Objetos para la Clase
        private Orden _ordenSeleccionado;
        private OrdenDetalleCompra _ordenDetalleCompraSeleccionado;
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
        public OrdenDetalleCompra OrdenDetalleCompraSeleccionado
        {
            get => _ordenDetalleCompraSeleccionado;
            set
            {
                _ordenDetalleCompraSeleccionado = value;
                RaisePropertyChanged(() => OrdenDetalleCompraSeleccionado);
            }

        }
        #endregion 2.InstanciaAsignaVariablesControles

        #region 3.Constructor
        public PopupActualizaItemOrdenComprasViewModel()
        {

        }
        #endregion 3.Constructor

        #region 4.DefinicionMetodos
        public ICommand Ok_Command => new Command(async () => await Ok_CommandAsync());
        public ICommand Cancel_Command => new Command(async () => await Cancel_CommandAsync());
        #endregion 4.DefinicionMetodos

        #region 5.Métodos de Administración de Datos
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            if (navigationData is OrdenDetalleCompra item && item != null)
            {
                OrdenDetalleCompraSeleccionado = item;                
            }
            IsBusy = false;
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
            EventoGuardar?.Invoke(true);
        }
        #endregion 5.Métodos de Administración de Datos

        #region 6.Métodos Generales
        private async Task Cancel_CommandAsync()
        {
            EventoCancelar?.Invoke(true);
        }
        #endregion 6.Métodos Generales
    }
}
