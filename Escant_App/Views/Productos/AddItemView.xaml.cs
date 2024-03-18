using Escant_App.ViewModels.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Productos
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemView : ContentPage
    {
        
        private AddItemViewModel Context => (AddItemViewModel)this.BindingContext;
        public AddItemView()
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

        private void CustomSfNumericTextBox_Unfocused(object sender, FocusEventArgs e)
        {
            Context.CalculaValores(1);
        }

        private void CustomSfNumericTextBox_Unfocused_1(object sender, FocusEventArgs e)
        {
            Context.CalculaValores(2);
        }
    }
}