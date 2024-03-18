using System;

using Escant_App.ViewModels.Seguridad;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Seguridad
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordOlvidadoView : ContentPage
    {
        public PasswordOlvidadoView()
        {
            InitializeComponent();
        }
        public PasswordOlvidadoViewModel Context => (PasswordOlvidadoViewModel)this.BindingContext;
        private async void ButtonSendLink_Clicked(object sender, EventArgs e)
        {
            string email = TxtEmail.Text;
            if (await Context.ResetearClave(email))
                await Navigation.PopModalAsync();
            //if(respuesta)
              //  await Navigation.PopModalAsync();

        }
    }
}