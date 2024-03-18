using DataModel.DTO.Compras;
using Escant_App.ViewModels.Inventario;
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
    public partial class StockView : ContentPage
    {
        #region 0.InstanciaVinculaClase
        /// <summary>
        /// Instanciación de la clase con el FrontEnd
        /// </summary>
        public StockViewModel Context => (StockViewModel)this.BindingContext;
        #endregion 0.InstanciaVinculaClase

        #region 1.Constructor
        /// <summary>
        /// Constructor del código FrontEnd
        /// </summary>
        public StockView()
        {

            InitializeComponent();
        }
        #endregion 1.Constructor
        #region 2.MétodosFrontEnd
        /// <summary>
        /// Método Sobreescrito del evento aparecer de la Pantalla
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Context.ListView = registroListView;
            Context.MenuImagen = "\uf067";
            registroListView?.ResetSwipe();
            Task.Run(async () =>
            {
                IsBusy = true;
                await Context.LoadData();
                IsBusy = false;
            });
        }

        /// <summary>
        /// Método para buscar un valor en la pantalla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //await ((EmpresaViewModel)this.BindingContext).SearchRegistro(e.NewTextValue);
            if (registroListView.DataSource != null)
            {
                registroListView.DataSource.Filter = FilterContacts;
                registroListView.DataSource.RefreshFilter();
            }
            registroListView.RefreshView();
        }
        /// <summary>
        /// Método para disparar el inicio del botón deslizar para edición y/o mantenimiento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Handle_SwipeStarted(object sender, Syncfusion.ListView.XForms.SwipeStartedEventArgs e)
        {
            Context.ItemIndex = -1;
        }
        /// <summary>
        /// Método para configurar el fin del botón deslizar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        void Handle_SwipeEnded(object sender, Syncfusion.ListView.XForms.SwipeEndedEventArgs e)
        {
            Context.ItemIndex = e.ItemIndex;
        }
        /// <summary>
        /// Método para manejar el cambio de pantallas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void Handle_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var pages = Navigation.NavigationStack.ToList();
            var previousPage = pages.ElementAt(pages.Count - 2);
        }
        /// <summary>
        /// método para filtrar los datos a buscar en pantalla
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        private bool FilterContacts(object obj)
        {
            if (barraBusqueda == null || barraBusqueda.Text == null)
                return true;

            var taskInfo = obj as Orden;
            return (taskInfo.NombreCliente.ToLower().Contains(barraBusqueda.Text.ToLower())
                || taskInfo.CodigoEstado.ToLower().Contains(barraBusqueda.Text.ToLower())
                || taskInfo.Condicion.ToLower().Contains(barraBusqueda.Text.ToLower())
                || taskInfo.NumeroOrden.ToLower().Contains(barraBusqueda.Text.ToLower()));
        }

        async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (menu.Text == "\uf067")
            {
                Context.MenuImagen = "\uf057";
                Context.IsVisibleBotones = true;
            }
            else
            {
                Context.MenuImagen = "\uf067";
                Context.IsVisibleBotones = false;
            }
            /* Image image = sender as Image;
             string filename = image.Source.ToString();
             if (filename == "File: cross_512.png")
             {
                 contentPage.Opacity = 1;
                 grid.BackgroundColor = Color.Transparent;
                 contentPage.BackgroundColor = Color.White;
                 menu.Source = "share.png";
                 menu.WidthRequest = 50;
                 menu.HeightRequest = 50;
                 await Task.WhenAll(
                     fb.TranslateTo(0, 0),
                     twit.TranslateTo(0, 0)
                     );

                 fb.IsVisible = false;
                 twit.IsVisible = false;
                 contentPage.InputTransparent = false;
             }
             else if (filename == "File: share.png")
             {
                 fb.IsVisible = true;
                 twit.IsVisible = true;
                 //contentPage.InputTransparent = true;

                 //contentPage.Opacity = 0.3;
                 //grid.BackgroundColor = Color.LightBlue;
                // contentPage.BackgroundColor = Color.Transparent;
                 menu.Source = "cross_512.png";
                 menu.WidthRequest = 30;
                 menu.HeightRequest = 30;

                 await Task.WhenAll(
                     fb.TranslateTo(-80, 0),
                     twit.TranslateTo(-150, 0)
                     );


             }*/
        }
        async void AdicionarInventarioInicial_Tapped(object sender, EventArgs e)
        {
            Context.RegistroSeleccionado=
            new Orden()
            {
                TipoInventario="Inicial"
                ,EsEdicion = 0
            };
            await Context.AddRegistroAsync();

        }
        async void AdicionarInventarioConcilia_Tapped(object sender, EventArgs e)
        {
            /*
            Context.RegistroSeleccionado =
            new Orden()
            {
                TipoInventario = "Concilia"
                ,
                EsEdicion = 0
            };
            await Context.AddRegistroAsync();
            */
        }

        async void AdicionarInventarioTransferencia_Tapped(object sender, EventArgs e)
        {
            /*
            Context.RegistroSeleccionado =
            new Orden()
            {
                TipoInventario = "Transferencia"
                ,
                EsEdicion = 0
            };
            await Context.AddRegistroAsync();
            */
        }


        #endregion 2.MétodosFrontEnd
    }
}
