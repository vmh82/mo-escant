using DataModel.DTO.Iteraccion;
using Escant_App.ViewModels.Administracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Administracion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdministracionView : ContentPage
    {
        private AdministracionViewModel Context => (AdministracionViewModel)BindingContext;
        public AdministracionView()
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