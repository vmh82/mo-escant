using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escant_App.ViewModels.Configuracion;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Plugin.Multilingual;
using DataModel;

namespace Escant_App.Views.Configuracion
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AppSettingsView : ContentPage
	{
		public AppSettingsView ()
		{
			InitializeComponent ();
        }
        private AppSettingsViewModel Context => (AppSettingsViewModel)this.BindingContext;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            Task.Run(async () =>
            {
                await Context.LoadSettings();
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        }

        private void SfCustomCurencyFormat_Toggled(object sender, Syncfusion.XForms.Buttons.SwitchStateChangedEventArgs e)
        {
            Context.ShowCustomFormat = !e.NewValue;
        }
        private void CustomCurencyFormat_Toggled(object sender, ToggledEventArgs e)
        {
            Context.ShowCustomFormat = !e.Value;
        }
        private void CustomCurrencyFormatCombobox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            Context.CurrentCurrencyCustomFormat = e.Value.ToString();
        }

        private void LanguageSelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if(e.Value != null && e.Value is SelectItem selectedLanguage)
            {
                Context.LanguageSelected = selectedLanguage;
            }
        }

    }
}