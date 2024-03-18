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
    public partial class PopupActualizaOrdenComprasView : PopupPage
    {
        public EventHandler CloseButtonEventHandler { get; set; }
        public Orden PopupResult { get; set; }
        public bool IsCancel { get; set; }
        public PopupActualizaOrdenComprasViewModel Context => this.BindingContext as PopupActualizaOrdenComprasViewModel;
        public Task<Orden> PageClosedTask { get { return PageClosedTaskCompletionSource.Task; } }

        public TaskCompletionSource<Orden> PageClosedTaskCompletionSource { get; set; }
        public PopupActualizaOrdenComprasView()
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
            PageClosedTaskCompletionSource = new TaskCompletionSource<Orden>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            PageClosedTaskCompletionSource.SetResult(Context.OrdenSeleccionado);
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
        private void eventosClick(EventArgs eventArgs)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsCancel = false;
                //await Context.almacenaDatos();
                PopupResult = Context.OrdenSeleccionado;
                CloseButtonEventHandler?.Invoke(this, eventArgs);
            });
        }
        private void eventosCancel(EventArgs eventArgs)
        {
            IsCancel = true;
            CloseButtonEventHandler?.Invoke(this, eventArgs);
        }
        

        void Handle_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(100));
        }
        private async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            /*if (scrollingSpace <= e.ScrollY) // Touched bottom
                                             // Do the things you want to do
                await PopupNavigation.Instance.PopAsync();*/
        }

        private void CustomSfNumericTextBox_Unfocused(object sender, FocusEventArgs e)
        {
            Context.CalculaValores();
        }
    }
}