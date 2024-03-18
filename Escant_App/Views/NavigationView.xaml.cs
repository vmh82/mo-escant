using Escant_App.Models;
using Escant_App.ViewModels;
using Escant_App.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Windows.Input;
using ManijodaServicios.Resources.Texts;
using Syncfusion.ListView.XForms;
//using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;
using Page = Xamarin.Forms.Page;
using DataModel.DTO.Iteraccion;
using Escant_App.Views.Administracion;
using Escant_App.Views.Ventas;
using Escant_App.Views.Principal;

namespace Escant_App.Views
{
    public partial class NavigationView : MasterDetailPage
    {
        public List<MainMenuItem> menuList { get; set; }
        private NavigationViewModel _context => (NavigationViewModel)BindingContext;

        public NavigationView()
        {
            try
            {
                InitializeComponent();
                //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(MainView)));
                //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(OrdenMaestroVentaView)));
                _context.AbreInicio = ((obj) =>
                {
                    iniciaDetalle();
                });
                iniciaDetalle();
            }
            catch (Exception ex)
            { }
        }
        private void iniciaDetalle()
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(PrincipalView)));
        }
        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
            _context.CheckLogin();
            }
            catch (Exception ex)
            { }
        }
    }    
}