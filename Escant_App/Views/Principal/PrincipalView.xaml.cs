using DataModel.DTO;
using Escant_App.UserControls;
using Escant_App.ViewModels.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Principal
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrincipalView : ContentPage
    {
        #region 0.InstanciaVinculaClase
        /// <summary>
        /// Instanciación de la clase con el FrontEnd
        /// </summary>        
        public PrincipalViewModel Context => this.BindingContext as PrincipalViewModel;
        #endregion 0.InstanciaVinculaClase

        #region 1.Constructor
        /// <summary>
        /// Constructor del código FrontEnd
        /// </summary>
        public PrincipalView()
        {
            InitializeComponent();
            //listView.ItemsSource = new TransactionModel().Get();            
        }
        #endregion 1.Constructor
        #region 2.MétodosFrontEnd
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Context.InitializeAsync(null);
            //listView.ItemsSource = await Context.Get();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            dt.Focus();
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            /*if (listView.SelectedItem != null)
            {
                listView.SelectedItem = null;
            }*/
        }

        private void dt_DateSelected(object sender, DateChangedEventArgs e)
        {
            lblDate.Text = dt.Date.ToString("dd MMM yyyy");
        }

        private async void btnNew_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("NewTransaction");
        }
        #endregion 2.MétodosFrontEnd
    }
}