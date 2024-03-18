using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using Escant_App.UserControls;
using Escant_App.ViewModels.Compras;
using Escant_App.ViewModels.Ventas;
using Plugin.Vibrate;
using Rg.Plugins.Popup.Pages;
using Syncfusion.Data;
using Syncfusion.SfDataGrid.XForms;
using Syncfusion.XForms.ComboBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Ventas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupValidaView : PopupPage
    {
        public EventHandler CloseButtonEventHandler { get; set; }
        public Orden PopupResult { get; set; }
        public bool IsCancel { get; set; }
        public PopupValidaViewModel Context => this.BindingContext as PopupValidaViewModel;
        public Task<Orden> PageClosedTask { get { return PageClosedTaskCompletionSource.Task; } }

        public TaskCompletionSource<Orden> PageClosedTaskCompletionSource { get; set; }

        bool check = true;
        public PopupValidaView()
        {
            InitializeComponent();
            var tapFinance = new TapGestureRecognizer();
            tapFinance.Tapped += async (s, e) =>
            {
                Context.IsVisibleCondicionesPago = true;
            };
            imgPagar.GestureRecognizers.Add(tapFinance);
            PageClosedTaskCompletionSource = new TaskCompletionSource<Orden>();
        }
        private async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

            /*if (scrollingSpace <= e.ScrollY) // Touched bottom
                                             // Do the things you want to do
                await PopupNavigation.Instance.PopAsync();*/
        }
        private void SfComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            cambiaCombo(1, e);
        }
        private void cambiaCombo(int tipo, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            bool isString = e.Value is string;
            switch (tipo)
            {
                case 1:
                    if (e.Value != null && (isString ? !string.IsNullOrEmpty((string)e.Value) : true))
                    {
                        Context.CondicionesVentaSeleccionado = (Catalogo)e.Value;
                        Context.EvaluaCondicion();
                    }
                    else
                        Context.CondicionesVentaSeleccionado = null;
                    break;
                case 2:
                    if (e.Value != null && (isString ? !string.IsNullOrEmpty((string)e.Value) : true))
                    {
                        Context.FormaCancelacionSeleccionado = (List<Catalogo>)e.Value;
                        //Context.EvaluaFormaCancelacion();
                    }
                    else
                        Context.FormaCancelacionSeleccionado = null;
                    break;
                case 3:
                    if (e.Value != null && (isString ? !string.IsNullOrEmpty((string)e.Value) : true))
                    {
                        Context.TipoDescuentoSeleccionado = (Catalogo)e.Value;
                        Context.EvaluaTipoDescuento();
                    }
                    else
                        Context.TipoDescuentoSeleccionado = null;
                    break;
            }

        }

        private void MyCustomEntry_Focused(object sender, FocusEventArgs e)
        {
            seleccionaTodo(sender);

        }
        private void seleccionaTodo(object sender)
        {
            var y = (Entry)sender;
            y.CursorPosition = 0;
            y.SelectionLength = y.Text.Length;
        }

        private void SfComboBox_SelectionChanged_1(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            cambiaCombo(3, e);
        }
        private void MyCustomEntry_Completed(object sender, EventArgs e)
        {
            Context.calculoDescuento(2);
        }
        private void sfcmbFormaCancelacion_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            //cambiaCombo(2, e);
            List<Object> items = new List<Object>();
           
            try
            {
                if (e.Value != null && (((e.Value is string && (string)e.Value != string.Empty)) || (e.Value is IList && (e.Value as IList).Count > 0)) && check)
                {
                    IList collection = (IList)e.Value;
                    Context.FormaCancelacionSeleccionado = collection.Cast<Catalogo>().ToList();
                }
                else
                {
                    Context.FormaCancelacionSeleccionado = new List<Catalogo>();
                }
                Context.EvaluaFormaCancelacion();
            }
            catch (Exception ex)
            {
                var y = ex.Message.ToString();
            }

        }
        private void dataGrid_CurrentCellEndEdit(object sender, GridCurrentCellEndEditEventArgs e)
        {
            var sfDataGrid = sender as SfDataGrid;
            var objeto = sfDataGrid.CurrentItem as OrdenPago;
            //var z = e.RowColumnIndex;
            var columnIndex = dataGrid.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
            var nombreColumna = sfDataGrid.Columns[columnIndex].MappingName;
            var valorActual = e.OldValue.ToString();
            var valorNuevo = e.NewValue.ToString();


        }
        private async void CustomSfNumericTextBox_Unfocused(object sender, FocusEventArgs e)
        {
            //var y = ((CustomSfNumericTextBox)sender).Parent;
            //((sender as Label).Parent.Parent as GridCell).DataColumn.ColumnIndex;             
            var row = ((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.RowIndex;
            var columna = 0;//((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.ColumnIndex;
            var valor = obtieneValorGrid(row, columna);
            await Context.CalculaValores(valor);

        }
        private string obtieneValorGrid(int row, int column)
        {
            string cellValue;
            int rowIndex = row;
            int columnIndex = dgPagoOrden.ResolveToGridVisibleColumnIndex(column);
            if (columnIndex < 0)
                return "";
            var mappingName = dgPagoOrden.Columns[columnIndex].MappingName;
            var recordIndex = dgPagoOrden.ResolveToRecordIndex(rowIndex);
            if (dgPagoOrden.View.TopLevelGroup != null)
            {
                var record = dgPagoOrden.View.TopLevelGroup.DisplayElements[recordIndex];
                if (!record.IsRecords)
                    return "";
                var data = (record as RecordEntry).Data;
                cellValue = (data.GetType().GetProperty(mappingName).GetValue(data, null).ToString());
            }
            else
            {
                var record1 = dgPagoOrden.View.Records.GetItemAt(recordIndex);
                cellValue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
            }
            return cellValue;
        }
    }
}