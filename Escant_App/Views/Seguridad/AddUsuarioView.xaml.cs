using Escant_App.ViewModels.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Seguridad
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUsuarioView : ContentPage
    {
        #region 0.InstanciaVinculaClase
        /// <summary>
        /// Instanciación de la clase con el FrontEnd
        /// </summary>
        private AddUsuarioViewModel Context => (AddUsuarioViewModel)this.BindingContext;
        #endregion 0.InstanciaVinculaClase

        #region 1.Constructor
        /// <summary>
        /// Constructor del código FrontEnd
        /// </summary>
        public AddUsuarioView()
        {
            InitializeComponent();
        }
        #endregion 1.Constructor

        #region 2.MétodosFrontEnd
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        }        
        #endregion 2.MétodosFrontEnd
    }
}