using DataModel.DTO.Clientes;
using DataModel.DTO.Compras;
using Escant_App.ViewModels.Ventas;
using Rg.Plugins.Popup.Pages;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Ventas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupDetalleOrdenView : PopupPage
    {
        public EventHandler CloseButtonEventHandler { get; set; }
        public Orden PopupResult { get; set; }
        public bool IsCancel { get; set; }
        public PopupDetalleOrdenViewModel Context => this.BindingContext as PopupDetalleOrdenViewModel;
        public Task<Cliente> PageClosedTask { get { return PageClosedTaskCompletionSource.Task; } }

        public TaskCompletionSource<Cliente> PageClosedTaskCompletionSource { get; set; }
        bool check = true;
        public PopupDetalleOrdenView()
        {
            InitializeComponent();            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PageClosedTaskCompletionSource = new TaskCompletionSource<Cliente>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();            
            PageClosedTaskCompletionSource.SetResult(Context.ClienteSeleccionado);
        }

        private async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            /*if (scrollingSpace <= e.ScrollY) // Touched bottom
                                             // Do the things you want to do
                await PopupNavigation.Instance.PopAsync();*/
        }

        private void dgTablaOrden_ValueChanged(object sender, Syncfusion.SfDataGrid.XForms.ValueChangedEventArgs e)
        {            
            
                var sfDataGrid = sender as SfDataGrid;                                
                Context.OrdenSeleccionado = (Orden)e.RowData;                
                Context.MuestraDetalle();
            /*    var retorno = Context.calculaValores(1, (int)valorColumnaCuota);
                if (nombreColumna.ToUpper().Contains("SELECCIO"))
                {
                    //if (!retorno)
                    sfDataGrid.Refresh();
                }
            */

        }
        private void Cancel_Clicked(object sender, EventArgs eventArgs)
        {
            IsCancel = true;
            CloseButtonEventHandler?.Invoke(this, eventArgs);
        }

        private void Ok_Clicked(object sender, EventArgs eventArgs)
        {
            IsCancel = false;
            //Context.AlmacenaDatos();
            PopupResult = Context.OrdenSeleccionado;
            CloseButtonEventHandler?.Invoke(this, eventArgs);
        }

    }
}