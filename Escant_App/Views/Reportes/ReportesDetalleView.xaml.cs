using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ManijodaServicios.Resources.Texts;
using Escant_App.ViewModels.Reportes;


namespace Escant_App.Views.Reportes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportesDetalleView : ContentPage
    {
        public ReportesDetalleView()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {

            }
            var segmentoTitulosLabel = new List<string>() { "Tendencia Productos", "Tendencia Clientes" };
            segmentTitulo.Children = segmentoTitulosLabel;
            var topsaleProducts = new List<string>() { TextsTranslateManager.Translate("BySale"), TextsTranslateManager.Translate("ByQuantity") };
            Context.TendenciaSeleccion = "Tendencia Clientes";
            //this.simTab.SelectionIndicatorSettings = new SelectionIndicatorSettings() { Position = SelectionIndicatorPosition.Fill, Color = Color.FromHex("#FF00AFF0") };
            //this.simTab1.SelectionChanged += SimTab_SelectionChanged;
            segmentProducto.Children = topsaleProducts;
            segmentProducto.ItemSelected = TextsTranslateManager.Translate("BySale");
            segmentClientes.Children = topsaleProducts;
            segmentClientes.ItemSelected = TextsTranslateManager.Translate("BySale");
        }

        
        async void Handle_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem.ToString() == TextsTranslateManager.Translate("BySale"))
            {
                await Context.LoadTop10ProductoPorVentaAsync();
            }
            else
            {
                await Context.LoadTop10ProductoPorCantidadAsync();
            }
        }

        public ReportesDetalleViewModel Context => (ReportesDetalleViewModel)this.BindingContext;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Context.InitializeAsync(null);
        }

        private async void segmentTitulo_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem.ToString() == "Tendencia Productos")
            {
                Context.EsVisibleTendenciaProductos = true;
                Context.EsVisibleTendenciaClientes = false;
            }
            else
            {
                Context.EsVisibleTendenciaProductos = false;
                Context.EsVisibleTendenciaClientes = true;
            }
        }

        private async void segmentClientes_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem.ToString() == TextsTranslateManager.Translate("BySale"))
            {
                await Context.LoadTop10ClientesPorVentaAsync();
            }
            else
            {
                await Context.LoadTop10ClientesPorCantidadAsync();
            }
        }
    }
}