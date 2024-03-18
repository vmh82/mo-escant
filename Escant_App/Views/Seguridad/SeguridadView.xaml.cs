using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DTO.Iteraccion;
using Escant_App.Models;
using Escant_App.ViewModels;
using Escant_App.ViewModels.Seguridad;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Seguridad
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeguridadView : ContentPage
    {
        public SeguridadView()
        {
            InitializeComponent();
        }
        private SeguridadViewModel Context => (SeguridadViewModel)BindingContext;

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