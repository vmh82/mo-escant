using ManijodaServicios.Resources.Texts;
using Escant_App.ViewModels.GestionBDD;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.GestionBDD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ManageDatabaseView : ContentPage
	{
        public ManageDatabaseViewModel Context => (ManageDatabaseViewModel)this.BindingContext;

        public ManageDatabaseView ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Task.Run(async () =>
            {
                await Context.LoadData();
            });
        }
        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            
        }
    }
}