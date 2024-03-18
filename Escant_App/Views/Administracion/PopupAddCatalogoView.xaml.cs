using Escant_App.ViewModels.Administracion;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Administracion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupAddCatalogoView : ContentPage
    {
        public PopupAddCatalogoView()
        {
            InitializeComponent();            
        }
        public PopupAddCatalogoViewModel Context => this.BindingContext as PopupAddCatalogoViewModel;
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }        
        async void OnFuenteIcono_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            await Context.OpenFuenteIconoAsync();
        }
    }
}