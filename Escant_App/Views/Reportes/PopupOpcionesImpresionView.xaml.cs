using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using Escant_App.ViewModels.Reportes;
using Rg.Plugins.Popup.Pages;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Reportes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]    
    public partial class PopupOpcionesImpresionView : PopupPage    
    {
        Catalogo item;
        public EventHandler CloseButtonEventHandler { get; set; }
        public Orden PopupResult { get; set; }
        public bool IsCancel { get; set; }
        public PopupOpcionesImpresionViewModel Context => (PopupOpcionesImpresionViewModel)this.BindingContext;
        public Task<Orden> PageClosedTask { get { return PageClosedTaskCompletionSource.Task; } }

        public TaskCompletionSource<Orden> PageClosedTaskCompletionSource { get; set; }
        public PopupOpcionesImpresionView()
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
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
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
            Context.GoBackCancel();
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

        private async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollView scrollView = sender as ScrollView;
            double scrollingSpace = scrollView.ContentSize.Height - scrollView.Height;

        }

        private void treeView_ItemDoubleTapped(object sender, Syncfusion.XForms.TreeView.ItemDoubleTappedEventArgs e)
        {
            item = e.Node.Content as Catalogo;
            Context.OrdenSeleccionado.CodigoEstadoEnConsulta = item.Nombre;
            Context.GoBack(item);            
        }
    }
}