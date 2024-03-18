using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using DataModel.DTO.Configuracion;
using Escant_App.ViewModels.Cajas;
using Newtonsoft.Json;
using Plugin.Vibrate;
using Rg.Plugins.Popup.Pages;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Cajas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupOrdenTablaPagoView : PopupPage
    {
        public EventHandler CloseButtonEventHandler { get; set; }
        public Orden PopupResult { get; set; }
        public bool IsCancel { get; set; }
        public PopupOrdenTablaPagoViewModel Context => this.BindingContext as PopupOrdenTablaPagoViewModel;
        public Task<Orden> PageClosedTask { get { return PageClosedTaskCompletionSource.Task; } }
        public TaskCompletionSource<Orden> PageClosedTaskCompletionSource { get; set; }
        public PopupOrdenTablaPagoView()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            /*var tapRetornar = new TapGestureRecognizer();
            tapRetornar.Tapped += async (s, e) =>
            {
                await Context.Regresar();
            };
            imgRetornar.GestureRecognizers.Add(tapRetornar);*/
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
            IsCancel = true;
            CloseButtonEventHandler?.Invoke(this, eventArgs);
        }

        private void Ok_Clicked(object sender, EventArgs eventArgs)
        {
            IsCancel = false;
            Context.AlmacenaDatos();
            PopupResult = Context.OrdenSeleccionado;
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
        private void dataGrid_CurrentCellEndEdit(object sender, GridCurrentCellEndEditEventArgs e)
        {
            var sfDataGrid = sender as SfDataGrid;
            var objeto = sfDataGrid.CurrentItem as TablaPago;
            //var z = e.RowColumnIndex;
            var columnIndex = dgTablaPago.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
            var nombreColumna = sfDataGrid.Columns[columnIndex].MappingName;
            var valorActual = e.OldValue.ToString();
            var valorNuevo = e.NewValue.ToString();


        }
        

        private void dgTablaPago_ValueChanged(object sender, Syncfusion.SfDataGrid.XForms.ValueChangedEventArgs e)
        {
            /*
            var column = e.Column;
            var newValue = e.NewValue;
            var rowColIndex = e.RowColIndex;
            var rowData = e.RowData;
            */
            if (!Context.EsApertura)
            {
                var sfDataGrid = sender as SfDataGrid;
                var objeto = sfDataGrid.CurrentItem as TablaPago;
                var columnIndex = dgTablaPago.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
                var nombreColumna = sfDataGrid.Columns[columnIndex].MappingName;
                var valorNuevo = e.NewValue.ToString();
                var rowData = e.RowData;
                var columnaCuota = 0;
                var nombreColumnaCuota = sfDataGrid.Columns[columnaCuota].MappingName;
                var valorColumnaCuota = dgTablaPago.GetCellValue(rowData, nombreColumnaCuota);
                var retorno = Context.calculaValores(1, (int)valorColumnaCuota);
                if (nombreColumna.ToUpper().Contains("SELECCIO"))
                {
                    //if (!retorno)
                    sfDataGrid.Refresh();
                }
            }
            
        }        
    }
}