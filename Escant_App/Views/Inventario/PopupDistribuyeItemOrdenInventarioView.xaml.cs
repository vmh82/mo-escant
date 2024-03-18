using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using Escant_App.ViewModels.Compras;
using Escant_App.ViewModels.Inventario;
using Plugin.Vibrate;
using Rg.Plugins.Popup.Pages;
using Syncfusion.Data;
using Syncfusion.SfDataGrid.XForms;
using Syncfusion.XForms.ComboBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Inventario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupDistribuyeItemOrdenInventarioView : PopupPage
    {
        public EventHandler CloseButtonEventHandler { get; set; }
        public OrdenDetalleCompra PopupResult { get; set; }
        public bool IsCancel { get; set; }
        public PopupDistribuyeItemOrdenInventarioViewModel Context => this.BindingContext as PopupDistribuyeItemOrdenInventarioViewModel;
        public Task<OrdenDetalleCompra> PageClosedTask { get { return PageClosedTaskCompletionSource.Task; } }

        public TaskCompletionSource<OrdenDetalleCompra> PageClosedTaskCompletionSource { get; set; }

        public PopupDistribuyeItemOrdenInventarioView()
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
                PopupResult = Context.OrdenDetalleCompraSeleccionado;
                CloseButtonEventHandler?.Invoke(this, eventArgs);
            });
        }
        private void eventosCancel(EventArgs eventArgs)
        {
            IsCancel = true;
            Context.borraDatos();
            CloseButtonEventHandler?.Invoke(this, eventArgs);
        }

        void Handle_ValueChanged(object sender, Syncfusion.SfNumericUpDown.XForms.ValueEventArgs e)
        {
            CrossVibrate.Current.Vibration(TimeSpan.FromMilliseconds(100));
        }

        /*
        private void catalogoCombobox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var combo = sender as SfComboBox;
            
            var record = combo.BindingContext as OrdenDetalleCompraDet;
            var recordRowIndex = Context.OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido.IndexOf(record);
            var rowIndex = dataGrid.ResolveToRowIndex(recordRowIndex);
            if(record!=null)
            {
                Context.AdministraGrid(record);
            }

        }

        private void catalogoCombobox_FocusChanged(object sender, FocusChangedEventArgs e)
        {
            var combo = sender as SfComboBox;

            var record = combo.BindingContext as OrdenDetalleCompraDet;
            var recordRowIndex = Context.OrdenDetalleCompraSeleccionado.OrdenDetalleComprasRecibido.IndexOf(record);
            var rowIndex = dataGrid.ResolveToRowIndex(recordRowIndex);
            if (record != null)
            {
                Context.AdministraGrid(record);
            }
        }
        
        private void dataGrid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            var selection = e.AddedItems[0];
        }
        */
        private void dataGrid_CurrentCellEndEdit(object sender, GridCurrentCellEndEditEventArgs e)
        {
            var sfDataGrid = sender as SfDataGrid;
            var objeto = sfDataGrid.CurrentItem as OrdenDetalleCompraDet;
            //var z = e.RowColumnIndex;
            var columnIndex = dataGrid.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
            var nombreColumna = sfDataGrid.Columns[columnIndex].MappingName;
            var valorActual = e.OldValue.ToString();
            var valorNuevo = e.NewValue.ToString();

            if (nombreColumna.Contains("TipoDescuento"))
            {
                /*Task.Run(async () =>
                {
                    //objeto.TipoDescuento = valorNuevo;
                    var retorno = await Context.AdministraGrid(objeto, valorActual, valorNuevo);
                    if (retorno)
                        e.Cancel = true;
                });*/
                //e.Cancel = true;
            }


        }

        private void comboBox_Completed(object sender, EventArgs e)
        {
            if (sender is SfComboBox comboBox)
            {
                if (comboBox.Text != null)
                {
                    var isNewItem = Context.TipoDescuentoList.Any(x => x.Nombre.ToLower().Contains(comboBox.Text.ToLower()));

                    //Skip adding duplicate items
                    if (!isNewItem) return;

                    Context.TipoDescuentoList.Add(new DataModel.DTO.Administracion.Catalogo() { Nombre = comboBox.Text });
                    comboBox.SelectedItem = Context.TipoDescuentoList[Context.TipoDescuentoList.Count - 1];
                }
            }
        }

        private void comboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            //var x = sender.Paren is SfComboBox;
            var x = (GridCell)(((SfComboBox)sender).Parent);
            var data = (OrdenDetalleCompraDet)x.DataColumn.RowData;
            if (e.Value != null && e.Value is Catalogo tipoDescuentoSeleccionado)
            {
                //Context.TipoDescuentoSeleccionado = tipoDescuentoSeleccionado;
                if (!validaSeleccion(tipoDescuentoSeleccionado.Nombre))
                {
                    data.TipoDescuentoSeleccion = Context.TipoDescuentoList.Where(y => y.Nombre.ToUpper().Contains("NO A")).FirstOrDefault();
                }
                /*Task.Run(async () =>
                {
                    IsBusy = true;
                    var retorno = await Context.AdministraGrid(data, "", tipoDescuentoSeleccionado.Nombre);
                    if (retorno)
                    {
                        var combo = (SfComboBox)sender;
                        combo.SelectedItem=Context.TipoDescuentoList.Where(y=>y.Nombre.ToUpper().Contains("NO A")).FirstOrDefault();
                        dataGrid.Refresh();
                        //Context.TipoDescuentoSeleccionado= Context.TipoDescuentoList.Where(y => y.Nombre.ToUpper().Contains("NO A")).FirstOrDefault();                        
                    }                        
                    IsBusy = false;
                });*/

            }

        }

        private void dataGrid_CurrentCellDropDownSelectionChanged(object sender, CurrentCellDropDownSelectionChangedEventArgs e)
        {

            var sfDataGrid = sender as SfDataGrid;
            var objeto = sfDataGrid.CurrentItem as OrdenDetalleCompraDet;
            //var z = e.RowColumnIndex;
            var columnIndex = dataGrid.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
            var nombreColumna = sfDataGrid.Columns[columnIndex].MappingName;

            if (nombreColumna.Contains("TipoDescuento"))
            {
                dataGrid.EndEdit();
            }
        }

        private void dataGrid_CurrentCellEndEdit_1(object sender, GridCurrentCellEndEditEventArgs e)
        {
            var sfDataGrid = sender as SfDataGrid;
            var objeto = sfDataGrid.CurrentItem as OrdenDetalleCompraDet;
            //var z = e.RowColumnIndex;
            var columnIndex = dataGrid.ResolveToGridVisibleColumnIndex(e.RowColumnIndex.ColumnIndex);
            var nombreColumna = sfDataGrid.Columns[columnIndex].MappingName;
            //var valorActual = e.OldValue.ToString();
            var valorNuevo = e.NewValue.ToString();

            if (nombreColumna.Contains("TipoDescuento"))
            {
                if (!validaSeleccion(valorNuevo))
                {
                    e.Cancel = true;
                }
                else
                {
                    Context.RecalculaCantidad(objeto);
                }
            }
        }
        private bool validaSeleccion(string valor)
        {
            bool retorno = true;
            if (valor.Contains("+"))
            {
                string[] pValorNuevo = valor.Split('+');
                var cantidadDescontada = Convert.ToInt32(pValorNuevo[1]);
                var cantidadRequerida = Convert.ToInt32(pValorNuevo[0]);
                if (cantidadRequerida != Context.OrdenDetalleCompraSeleccionado.Cantidad)
                {
                    retorno = false;
                }
            }
            return retorno;
        }

        private void comboBox_SelectionChanging(object sender, SelectionChangingEventArgs e)
        {
            var x = (GridCell)(((SfComboBox)sender).Parent);
            var data = (OrdenDetalleCompraDet)x.DataColumn.RowData;
            if (e.Value != null && e.Value is Catalogo tipoDescuentoSeleccionado)
            {
                //Context.TipoDescuentoSeleccionado = tipoDescuentoSeleccionado;
                if (!validaSeleccion(tipoDescuentoSeleccionado.Nombre))
                {
                    e.Cancel = true;
                }

            }
        }

        private void CustomSfNumericTextBox_ValueChanged(object sender, Syncfusion.SfNumericTextBox.XForms.ValueEventArgs e)
        {
            dataGrid.EndEdit();
        }

    }
}