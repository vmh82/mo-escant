using System;
using Escant_App.Models;
using Escant_App.ViewModels.Reportes;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace Escant_App.Views.Reportes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PopupSelectDateRangeView : PopupPage
    {
		public PopupSelectDateRangeView ()
		{
			InitializeComponent ();
		}
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            FillData();
        }
        private void FillData()
        {
            switch(Context.PeriodType)
            {
                case PeriodType.Today:
                    today.IsChecked = true;
                    break;
                case PeriodType.Yesterday:
                    yesterday.IsChecked = true;
                    break;
                case PeriodType.LastWeek:
                    lastweek.IsChecked = true;
                    break;
                case PeriodType.ThisMonth:
                    thismonth.IsChecked = true;
                    break;
                case PeriodType.LastMonth:
                    lastmonth.IsChecked = true;
                    break;
                case PeriodType.Option:
                    optional.IsChecked = true;
                    break;
                default:
                    today.IsChecked = true;
                    break;
            }
        }
        public PopupSelectDateRangeViewModel Context => (PopupSelectDateRangeViewModel)this.BindingContext;

        private void DateRange_SelectedItemChanged(object sender, EventArgs e)
        {
            if (today.IsChecked == true)
            {
                Context.PeriodType = (PeriodType)0;
                return;
            }
            else if (yesterday.IsChecked == true)
            {
                Context.PeriodType = (PeriodType)1;
                return;
            }
            else if (lastweek.IsChecked == true)
            {
                Context.PeriodType = (PeriodType)2;
                return;
            }
            else if (thismonth.IsChecked == true)
            {
                Context.PeriodType = (PeriodType)3;
                return;
            }
            else if (lastmonth.IsChecked == true)
            {
                Context.PeriodType = (PeriodType)4;
                return;
            }
            else if (optional.IsChecked == true)
            {
                Context.PeriodType = (PeriodType)5;
                Context.IsEnableDateTimePicker = Context.PeriodType == PeriodType.Option;
                return;
            }
            else
            {
                today.IsChecked = true;
                Context.PeriodType = (PeriodType)0;
                return;
            }
            //Context.PeriodType = (DataModel.Enums.PeriodType)index;
            //Context.IsEnableDateTimePicker = Context.PeriodType == DataModel.Enums.PeriodType.Option;
        }
    }
}