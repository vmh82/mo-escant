using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Escant_App.ViewModels.Reportes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Reportes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReportSummaryView : ContentPage
	{
		public ReportSummaryView ()
		{
			InitializeComponent ();
		}

        public ReportSummaryViewModel Context => (ReportSummaryViewModel)this.BindingContext;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Context.InitializeAsync(null);
        }
    }
}