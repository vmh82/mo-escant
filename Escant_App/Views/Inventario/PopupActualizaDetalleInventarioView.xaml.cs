using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Productos;
using Escant_App.ViewModels.Compras;
using Escant_App.ViewModels.Inventario;
using Escant_App.ViewModels.Ventas;
using Plugin.Vibrate;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Inventario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupActualizaDetalleInventarioView : PopupPage
    {
        public EventHandler CloseButtonEventHandler { get; set; }
        public OrdenDetalleCompra PopupResult { get; set; }
        public bool IsCancel { get; set; }
        public PopupActualizaDetalleInventarioViewModel Context => this.BindingContext as PopupActualizaDetalleInventarioViewModel;
        public Task<OrdenDetalleCompra> PageClosedTask { get { return PageClosedTaskCompletionSource.Task; } }

        public TaskCompletionSource<OrdenDetalleCompra> PageClosedTaskCompletionSource { get; set; }


        public PopupActualizaDetalleInventarioView()
        {
            InitializeComponent();
            var tapFinance = new TapGestureRecognizer();
            tapFinance.Tapped += async (s, e) =>
            {
                //Context.calculaLimiteFechas(2);
            };
            imgPagar.GestureRecognizers.Add(tapFinance);
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
        private void eventosClick(EventArgs eventArgs)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsCancel = false;
                await Context.almacenaDatos();
                PopupResult = Context.OrdenDetalleCompraSeleccionado;
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

        private void SfComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            bool isString = e.Value is string;
            if (e.Value != null && (isString ? !string.IsNullOrEmpty((string)e.Value) : true))
            {
                //Context.PrecioSeleccionado = (Precio)e.Value;
                //if (!Context._esActivadaCalculo)
                //{
                //    Context.calculoValores(1);
                //}
                //Context._esActivadaCalculo = false;
            }
            /*else
                Context.PrecioSeleccionado = null;*/
        }

        private void MyCustomEntry_Unfocused(object sender, FocusEventArgs e)
        {
            Context.calculoValores(1);
        }

        private void MyCustomEntry_Unfocused_1(object sender, FocusEventArgs e)
        {
            //Context.calculoValores(2);
        }

        
        private void SfComboBox_SelectionChanged_1(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            bool isString = e.Value is string;
            if (e.Value != null && (isString ? !string.IsNullOrEmpty((string)e.Value) : true))
            {
               // Context.FechaSeleccionado = (FechaItem)e.Value;
               // Context.CalculaExistentes();

            }
            //else
            //    Context.FechaSeleccionado = null;
        }

        private void SfComboBox_SelectionChanged_2(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            bool isString = e.Value is string;
            if (e.Value != null && (isString ? !string.IsNullOrEmpty((string)e.Value) : true))
            {
                //Context.PeriodoPagoSeleccionado = (Catalogo)e.Value;
               // Context.CalculaExtraServicio();
            }
            //else
            //    Context.PeriodoPagoSeleccionado = null;
        }

        private void SfComboBox_SelectionChanged_3(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            cambiaCombo(3, e);
        }
        private void cambiaCombo(int tipo, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            bool isString = e.Value is string;
            switch (tipo)
            {
                case 3:
                    if (e.Value != null && (isString ? !string.IsNullOrEmpty((string)e.Value) : true))
                    {
                        //Context.TipoDescuentoSeleccionado = (Catalogo)e.Value;
                        //Context.EvaluaTipoDescuento();
                    }
                    //else
                    //    Context.TipoDescuentoSeleccionado = null;
                    break;
            }
            //dgTablaPago.Refresh();
        }

        private void MyCustomEntry_Completed(object sender, EventArgs e)
        {
            //Context.calculoDescuento(2);
        }
        private async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;
        }
    }
}