using Escant_App.ViewModels.Reportes;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Reportes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopupSelectDateView : PopupPage
	{
		public PopupSelectDateView ()
		{
			InitializeComponent ();
		}
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }

        public PopupSelectDateViewModel Context => (PopupSelectDateViewModel)this.BindingContext;

    }
}