using Escant_App.ViewModels.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Configuracion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecuencialesSettingView : ContentPage
    {
        public SecuencialesSettingView()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                await Context.LoadSetting();
            });
        }
        private SecuencialesSettingViewModel Context => (SecuencialesSettingViewModel)this.BindingContext;

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
    }
}