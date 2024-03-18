using DataModel.DTO.Iteraccion;
using ManijodaServicios.Offline.Interfaz;
using Escant_App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Iteraccion
{
    public class PaletaColoresViewModel : ViewModelBase
    {
                
        #region 1.Constructor
        public PaletaColoresViewModel(IServicioSeguridad_Usuario seguridadServicio
                                  )
        {
            //Fijo en cada Model
            BindingContext = new ConectividadModelo(seguridadServicio);        
        }
        #endregion 1.Constructor

        
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
        /// Método de Retorno a Pantalla de Llamada
        /// </summary>
        /// <returns></returns>
        public async Task GoReturn()
        {
           await NavigationService.RemovePopupAsync();
        }
        #endregion 5.MétodosGenerales
    }
}
