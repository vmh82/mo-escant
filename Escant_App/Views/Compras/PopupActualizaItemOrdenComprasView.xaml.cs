using DataModel.DTO.Compras;
using Escant_App.ViewModels.Compras;
using Plugin.Vibrate;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Compras
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupActualizaItemOrdenComprasView : PopupPage
    {
        public EventHandler CloseButtonEventHandler { get; set; }
        public OrdenDetalleCompra PopupResult { get; set; }
        public bool IsCancel { get; set; }
        public PopupActualizaItemOrdenComprasViewModel Context => this.BindingContext as PopupActualizaItemOrdenComprasViewModel;
        public Task<OrdenDetalleCompra> PageClosedTask { get { return PageClosedTaskCompletionSource.Task; } }

        public TaskCompletionSource<OrdenDetalleCompra> PageClosedTaskCompletionSource { get; set; }

        public PopupActualizaItemOrdenComprasView()
        {
            InitializeComponent();
            Context.EventoGuardar = ((obj) =>
            {
                eventosClick(new EventArgs());
            });
            Context.EventoCancelar = ((obj) =>
            {
                eventosCancel(new EventArgs());
            });            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PageClosedTaskCompletionSource = new TaskCompletionSource<OrdenDetalleCompra>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            PageClosedTaskCompletionSource.SetResult(Context.OrdenDetalleCompraSeleccionado);
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
        private void Cancel_Clicked(object sender, EventArgs eventArgs)
        {
            eventosCancel(eventArgs);
        }

        private void Ok_Clicked(object sender, EventArgs eventArgs)
        {
            eventosClick(eventArgs);

        }
        private void eventosCancel(EventArgs eventArgs)
        {
            IsCancel = true;
            CloseButtonEventHandler?.Invoke(this, eventArgs);
        }

        private void eventosClick(EventArgs eventArgs)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsCancel = false;
                NumericTextBoxPrice.Unfocus();
                NumericTextBoxCantidad.Unfocus();
                NumericTextBoxSaldo.Unfocus();
                PopupResult = Context.OrdenDetalleCompraSeleccionado;
                CloseButtonEventHandler?.Invoke(this, eventArgs);
            });
        }        

        void Handle_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(100));
        }

        private void MyCustomEntry_Unfocused(object sender, FocusEventArgs e)
        {
            CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(100));
        }
    }
}