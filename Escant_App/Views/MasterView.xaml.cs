using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel.DTO.Iteraccion;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Resources.Texts;
using Escant_App.ViewModels;
using Escant_App.Views.Configuracion;
using Escant_App.Views.Seguridad;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
//using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Escant_App.Views
{
    public partial class MasterView : ContentPage
    {
        public List<MainMenuItem> menuList { get; set; }
        private Generales.Generales generales = new Generales.Generales();
        protected Xamarin.Forms.Page CurrentDetail
        {
            get { return (Xamarin.Forms.Application.Current.MainPage as NavigationView).Detail; }
        }
        public MasterView()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            MessagingCenter.Subscribe<string>(this, "OnDataSourceChanged", (args) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    IsBusy = true;
                    await Context.LoadSetting();
                    // Adding menu items to menuList and you can define title ,page and icon
                    armaMenu();                    
                    IsBusy = false;                    
                });
            });
            Task.Run(async () =>
            {
                await Context.LoadSetting();
                // Adding menu items to menuList and you can define title ,page and icon
                armaMenu();
            });            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string>(this, "StoreLogoChanged", async (args) =>
            {
                await Context.LoadSetting();
            });
        }
        public MasterViewModel Context => (MasterViewModel)this.BindingContext;

        async void Handle_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            if (!e.AddedItems.Any())
                return;

            var item = (MainMenuItem)e.AddedItems[0];
            if(item.TargetType == typeof(LoginView))
            {
                await Context.Logout();
            }
            else
            {
                var page = (Xamarin.Forms.Page)Activator.CreateInstance(item.TargetType);
                Xamarin.Forms.NavigationPage navigatedPage = new Xamarin.Forms.NavigationPage(page);
                (Xamarin.Forms.Application.Current.MainPage as NavigationView).Detail = navigatedPage;
                if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                    (Xamarin.Forms.Application.Current.MainPage as NavigationView).IsPresented = false;
            }
        }

        private void OnGotoStoreSettingTapped(object sender, EventArgs args)
        {
            var page = new Xamarin.Forms.NavigationPage((Xamarin.Forms.Page)Activator.CreateInstance(typeof(StoreSettingView)));

            (Xamarin.Forms.Application.Current.MainPage as NavigationView).Detail = page;

            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                (Xamarin.Forms.Application.Current.MainPage as NavigationView).IsPresented = false;

            //(Xamarin.Forms.Application.Current.MainPage as NavigationView).Navigation.PushAsync(page);
            //CurrentDetail.Navigation.PushAsync(page);
            //Navigation.PushAsync(new Xamarin.Forms.NavigationPage(page));
            //Context.NavigateToStoreSetting();
            //if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            //    (Xamarin.Forms.Application.Current.MainPage as NavigationView).IsPresented = false;
        }

        async void armaMenu()
        {
            menuList = new List<MainMenuItem>();
            var menu = await generales.devuelveMenu(1, "");
            if (menu == null)
            {
                SettingsOnline.EsMenuVacio = true;
                //await Context.Logout();
            }
            else
            {
                SettingsOnline.EsMenuVacio = false;
                menuList = menu.OrderBy(z=>z.Orden).Select(x => new MainMenuItem()
                {
                    Title = TextsTranslateManager.Translate(x.LabelTitulo)
                    ,
                    Icon = ""
                    ,
                    IconoMenu = x.ImageIcon != null && x.ImageIcon.ToString() != "" ? JsonConvert.DeserializeObject<FuenteIconos>(x.ImageIcon.ToString()) : new FuenteIconos()
                    {
                        NombreIcono = "Configurar"
                                ,
                        CodigoImagen = "&#xf0e8;"

                    }
                    ,
                    TargetType = Type.GetType(x.NombreFormulario)                    
                }).ToList();
                // Setting our list to be ItemSource for ListView in MainPage.xaml
                navigationDrawerList.ItemsSource = menuList;
                navigationDrawerList.SelectedItem = menuList.FirstOrDefault();
            }
        }
    }
}
