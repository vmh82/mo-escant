using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using DataModel.DTO.Productos;
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
    public partial class PopupActualizaOrdenVentasView : PopupPage
    {
        public EventHandler CloseButtonEventHandler { get; set; }
        public Orden PopupResult { get; set; }
        public bool IsCancel { get; set; }
        public PopupActualizaOrdenVentasViewModel Context => this.BindingContext as PopupActualizaOrdenVentasViewModel;
        public Task<Orden> PageClosedTask { get { return PageClosedTaskCompletionSource.Task; } }

        public TaskCompletionSource<Orden> PageClosedTaskCompletionSource { get; set; }
        bool check = true;
        public PopupActualizaOrdenVentasView()
        {
            try
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
                Context.EventoGrid = ((obj) =>
                {
                    eventosGrid(new EventArgs());
                });
            }
            catch(Exception ex)
            {

            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            PageClosedTaskCompletionSource = new TaskCompletionSource<Orden>();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Context.OrdenSeleccionado.ContinuaNavegacion = 0;
            PageClosedTaskCompletionSource.SetResult(Context.OrdenSeleccionado);
        }        
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
        
        private void eventosClick(EventArgs eventArgs)
        {
            IsCancel = false;
            PopupResult = Context.OrdenSeleccionado;
            CloseButtonEventHandler?.Invoke(this, eventArgs);
        }
        private void eventosCancel(EventArgs eventArgs)
        {
            IsCancel = true;
            CloseButtonEventHandler?.Invoke(this, eventArgs);
        }
        private void Cancel_Clicked(object sender, EventArgs eventArgs)
        {
            eventosCancel(eventArgs);
        }

        private void Ok_Clicked(object sender, EventArgs eventArgs)
        {
            eventosClick(eventArgs);
        }

        private void eventosGrid(EventArgs eventArgs)
        {
            dgEntregaOrden.Refresh();
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

        private async void CustomSfNumericTextBox_Unfocused(object sender, FocusEventArgs e)
        {
            //var y = ((CustomSfNumericTextBox)sender).Parent;
            //((sender as Label).Parent.Parent as GridCell).DataColumn.ColumnIndex;             
            var row = ((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.RowIndex;
            var columna = 0;//((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.ColumnIndex;
            var valor=obtieneValorGrid(row,columna,1);
            await Context.CalculaValores(valor,1);

        }
        private string obtieneValorGrid(int row, int column, int origen)
        {
            string cellValue="";
            int rowIndex = row;
            switch (origen)
            {
                case 1:
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
                    break;
                case 2:
                    int columnIndex1 = dgEntregaOrden.ResolveToGridVisibleColumnIndex(column);
                    if (columnIndex1 < 0)
                        return "";
                    var mappingName1 = dgEntregaOrden.Columns[columnIndex1].MappingName;
                    var recordIndex1 = dgEntregaOrden.ResolveToRecordIndex(rowIndex);
                    if (dgEntregaOrden.View.TopLevelGroup != null)
                    {
                        var record = dgEntregaOrden.View.TopLevelGroup.DisplayElements[recordIndex1];
                        if (!record.IsRecords)
                            return "";
                        var data = (record as RecordEntry).Data;
                        cellValue = (data.GetType().GetProperty(mappingName1).GetValue(data, null).ToString());
                    }
                    else
                    {
                        var record1 = dgEntregaOrden.View.Records.GetItemAt(recordIndex1);
                        cellValue = (record1.GetType().GetProperty(mappingName1).GetValue(record1, null).ToString());
                    }
                    break;
            }
            return cellValue;
        }
        private void SfComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {          
            cambiaCombo(1,e);
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

        private void sfcmbFormaCancelacion_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            //cambiaCombo(2, e);
            List<Object> items = new List<Object>();
            /*if (e.Value != null && (((e.Value is string && (string)e.Value != string.Empty)) || (e.Value is IList && (e.Value as IList).Count > 0)) && check)
            {
                    for (int ii = 0; ii < (e.Value as List<Catalogo>).Count; ii++)
                    {
                        var collection = (e.Value as List<Catalogo>)[ii];
                        items.Add(collection);
                    }                
                Context.FormaCancelacionSeleccionado1 = items;                
            }
            else
            {
                Context.FormaCancelacionSeleccionado1 = null;
            }*/
            try
            {
                if (!Context.PoseeDatos)
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
            }
            catch (Exception ex) {
                var y = ex.Message.ToString();
            }

        }
        
        private void dgPagoOrden_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {            
                if (e.RowIndex != 0)
                {
                    //Calculates and sets height of the row based on its content 
                    e.Height = dgPagoOrden.GetRowHeight(e.RowIndex);
                    e.Handled = true;
                }
            
        }

        private void CustomSfNumericTextBox_Unfocused_1(object sender, FocusEventArgs e)
        {
            var row = ((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.RowIndex;
            var columna = ((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.ColumnIndex;
            var nombreColumna = dgEntregaOrden.Columns[columna].MappingName;
            var x = (GridCell)(((CustomSfNumericTextBox)sender).Parent);
            var data = (ItemEntrega)x.DataColumn.RowData;
            /*
            var row = ((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.RowIndex;
            var columna = ((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.ColumnIndex;
            //var valor = obtieneValorGrid(row, columna,2);
            //await Context.CalculaValores(valor);
            var nombreColumna = dgEntregaOrden.Columns[columna].MappingName;

            if (nombreColumna.Contains("cantidadEntrega"))
            {
                //dgEntregaOrden.EndEdit();
            }

            var x = (GridCell)(((CustomSfNumericTextBox)sender).Parent);
            var data = (ItemEntrega)x.DataColumn.RowData;
            Context.calculaValoresEntrega(data);
            */

        }

        private void dgEntregaOrden_CurrentCellEndEdit(object sender, GridCurrentCellEndEditEventArgs e)
        {
            var sfDataGrid = sender as SfDataGrid;
            var objeto = sfDataGrid.CurrentItem as ItemEntrega;
            //var z = e.RowColumnIndex;
            var columnIndex = dgEntregaOrden.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
            var nombreColumna = sfDataGrid.Columns[columnIndex].MappingName;
            var valorActual = e.OldValue.ToString();
            var valorNuevo = e.NewValue.ToString();            
        }

        private void MyCustomEntry_Completed(object sender, EventArgs e)
        {
            dgEntregaOrden.EndEdit();
        }

        private void CustomSfNumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
            var row = ((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.RowIndex;
            var columna = ((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.ColumnIndex;
            var nombreColumna = dgEntregaOrden.Columns[columna].MappingName;
            var x = (GridCell)(((CustomSfNumericTextBox)sender).Parent);
            var data = (ItemEntrega)x.DataColumn.RowData;
            dgEntregaOrden.EndEdit();            
            //var resultado = await Context.CalculaValoresEntrega(data);
                //dgEntregaOrden.Refresh();
            

        }

        private void CustomSfNumericTextBox_Unfocused_1(object sender, Syncfusion.SfNumericTextBox.XForms.FocusEventArgs e)
        {
            var row = ((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.RowIndex;
            var columna = ((GridCell)((CustomSfNumericTextBox)sender).Parent).DataColumn.ColumnIndex;
            var nombreColumna = dgEntregaOrden.Columns[columna].MappingName;
            var x = (GridCell)(((CustomSfNumericTextBox)sender).Parent);
            var data = (ItemEntrega)x.DataColumn.RowData;
        }

        
        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var row = ((GridCell)((Entry)sender).Parent).DataColumn.RowIndex;
            var columna = ((GridCell)((Entry)sender).Parent).DataColumn.ColumnIndex;
            var nombreColumna = dgEntregaOrden.Columns[columna].MappingName;
            var x = (GridCell)(((Entry)sender).Parent);
            var data = (ItemEntrega)x.DataColumn.RowData;
            /*Task.Run(async () =>
            {                
                var resultado = await Context.CalculaValoresEntrega(data);
                if(resultado)
                    dgEntregaOrden.Refresh();
            });*/
            
        }
    }
}