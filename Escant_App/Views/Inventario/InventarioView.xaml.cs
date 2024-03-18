using DataModel.DTO.Iteraccion;
using Escant_App.ViewModels.Inventario;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Escant_App.Views.Inventario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventarioView : ContentPage
    {
        private InventarioViewModel Context => (InventarioViewModel)BindingContext;
        public InventarioView()
        {
            InitializeComponent();
        }
        async void Setting_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            //Enable Database management
            //if (e.ItemData != null && e.ItemData is Settings setting)

            //Disable Database managment features
            if (e.ItemData != null && e.ItemData is MenuSistema setting)
            {
                Page page = (Page)Activator.CreateInstance(setting.Type);
                NavigationPage.SetBackButtonTitle(page, "");
                await Navigation.PushAsync(page);
            }
            else
            {
                Context.ShowMessage();
            }
        }
    }
    
}