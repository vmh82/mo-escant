using Escant_App.ViewModels;
using Escant_App.ViewModels.Configuracion;
using Plugin.InputKit.Shared.Controls;
using Syncfusion.XForms.Buttons;
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
	public partial class ReceiptPrinterView : ContentPage
	{
		public ReceiptPrinterView()
		{
			InitializeComponent ();
		}

        public ReceiptPrinterViewModel Context => (ReceiptPrinterViewModel)this.BindingContext;

        //void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        //{
        //    if(e.Value)
        //    {
        //        Context.LoadPrinters();
        //    }
        //}

        private void SfSwitch_StateChanged(object sender, SwitchStateChangedEventArgs e)
        {
            if (Context.IsEnableConnectToPrinter)
            {
                Context.LoadPrinters();
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            Context.ComboBox = devicesCombobox;
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
        private void Bluetooth_StateChanged(object sender, StateChangedEventArgs e)
        {
            if(bluetooth.IsChecked.HasValue && bluetooth.IsChecked.Value)
            {
                Context.IsWifiChecked = false;
            }
        }

        void Wifi_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (wifi.IsChecked.HasValue && wifi.IsChecked.Value)
            {
                Context.IsBluetoothChecked = false;
            }
        }

        private void SizeA_StateChanged(object sender, EventArgs e)
        {
            if (sizeA.IsChecked.HasValue && sizeA.IsChecked.Value)
            {
                Context.IsSizeBChecked = false;
            }
        }

        void SizeB_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (sizeB.IsChecked.HasValue && sizeB.IsChecked.Value)
            {
                Context.IsSizeAChecked = false;
            }
        }

        void OnDevice_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if(devicesCombobox.SelectedItem != null)
            {
                Context.SelectedDevice = (BluetoothDeviceViewModel)devicesCombobox.SelectedItem;
                Context.DefaultDevice = Context.SelectedDevice.Address;
            }
        }
    }
}