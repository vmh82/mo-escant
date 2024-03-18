using Escant_App.ViewModels;
using Escant_App.ViewModels.Base;
using Escant_App.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ManijodaServicios.Resources.Texts;

namespace Escant_App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginView : ContentPage
	{        
        public LoginView ()
		{
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }
        public LoginViewModel Context => (LoginViewModel)this.BindingContext;
        protected override bool OnBackButtonPressed()
        {
            return false;
        }
        
        public class Image
        {
            public string Leyenda { get; set; }
            public string Url { get; set; }
        }
        
    }
}