using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escant_App.ViewModels.Configuracion;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Configuracion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StoreSettingView : ContentPage
	{
		public StoreSettingView()
		{
			InitializeComponent ();
            Task.Run(async () =>
            {
                await Context.LoadSetting();
            });
        }
        private StoreSettingViewModel Context => (StoreSettingViewModel)this.BindingContext;

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
        private void OnShowAmountBeforeTaxSelected(object sender, ItemTappedEventArgs e)
        {

        }

        private void OnPrintBeforeTaxSelected(object sender, ItemTappedEventArgs e)
        {

        }
    }
}