using Escant_App.ViewModels.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Administracion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddEmpresaView : ContentPage
    {
        private AddEmpresaViewModel Context => (AddEmpresaViewModel)this.BindingContext;
        public AddEmpresaView()
        {
            InitializeComponent();
        }
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

        async void OnTipoProveedor_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            await Context.OpenTipoProveedorAsync();
        }

    }
}