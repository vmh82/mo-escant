using Escant_App.ViewModels.Clientes;
using Escant_App.ViewModels.Gestion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Gestion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddGestionView : ContentPage
    {
        #region 0.InstanciaVinculaClase
        /// <summary>
        /// Instanciación de la clase con el FrontEnd
        /// </summary>
        private AddGestionViewModel Context => (AddGestionViewModel)this.BindingContext;
        #endregion 0.InstanciaVinculaClase

        #region 1.Constructor
        /// <summary>
        /// Constructor del código FrontEnd
        /// </summary>
        public AddGestionView()
        {
            InitializeComponent();
        }
        #endregion 1.Constructor

        #region 2.MétodosFrontEnd
        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        }
        async void OnEmpresa_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            //await Context.OpenEmpresaAsync();
        }
        
        private void Ok_Clicked(object sender, EventArgs e)
        {
            Context.PlacePhoneCall();
        }
        
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Context.SendSms();
        }
        #endregion 2.MétodosFrontEnd
    }
}
