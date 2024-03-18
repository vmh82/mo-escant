using DataModel.DTO.Productos;
using DataModel.Helpers;
using Escant_App.ViewModels.Productos;
using Rg.Plugins.Popup.Pages;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Productos
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopupAddPrecioItemView : PopupPage
    {                
        public bool IsCancel { get; set; }
        public Precio PopupResult { get; set; }
        public PopupAddPrecioItemViewModel Context => this.BindingContext as PopupAddPrecioItemViewModel;
        public Task<Precio> PageClosedTask { get { return PageClosedTaskCompletionSource.Task; } }
        public TaskCompletionSource<Precio> PageClosedTaskCompletionSource { get; set; }
        public EventHandler CloseButtonEventHandler { get; set; }
        public PopupAddPrecioItemView()
		{
			InitializeComponent ();
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
            PageClosedTaskCompletionSource = new TaskCompletionSource<Precio>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            PageClosedTaskCompletionSource.SetResult(Context.PrecioSeleccionado);
        }

        private void eventosClick(EventArgs eventArgs)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsCancel = false;
                await Context.almacenaDatos();
                PopupResult = Context.PrecioSeleccionado;
                CloseButtonEventHandler?.Invoke(this, eventArgs);
            });
        }
        private void eventosCancel(EventArgs eventArgs)
        {
            IsCancel = true;
            CloseButtonEventHandler?.Invoke(this, eventArgs);
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
        

        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();

            FrameContainer.HeightRequest = -1;            
            //llenaCombos();
            //UsernameEntry.Text = "hola";
        }

        private void Ok_Clicked(object sender, EventArgs eventArgs)
        {
            IsCancel = false;
            //QuantityNumericUpDown.Unfocus();
            //NumericTextBoxPrice.Unfocus();
            //PopupResult = Context.ProductInSaleOrder;
            CloseButtonEventHandler?.Invoke(this, eventArgs);
        }

        async void Handle_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            int quantity = Convert.ToInt32(e.Value);
            /*bool isValid = await Context.IsQuantityValid(quantity);
            if (!isValid)
                QuantityNumericUpDown.Value = (int)e.OldValue;
            else
                CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(100));*/
        }
        private void UnidadSelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (Context != null)
            {
                
                //AsyncRunner.Run(Context.SearchOrders());
            }
        }

        private void ProductListCombobox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            if (Context != null)
            {
                

            }
        }

        private void CustomSfNumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {

        }
    }
}