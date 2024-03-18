using Escant_App.ViewModels.Base;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Escant_App.ViewModels.Reportes
{
    public class PopupSelectDateViewModel : ViewModelBase
    {
        public ICommand CancelCommand => new Command(async () => await CancelAsync());
        private DateTime _selectDate;

        private async Task CancelAsync()
        {
            await PopupNavigation.Instance.PopAsync();
        }
        public DateTime SelectDate
        {
            get => _selectDate;
            set
            {
                _selectDate = value;
                RaisePropertyChanged(() => SelectDate);
            }
        }

        public ICommand OkCommand => new Command(async () => await OkAsync());
        private async Task OkAsync()
        {
            await PopupNavigation.Instance.PopAsync();
            await NavigationService.NavigateToAsync<DailySaleReportViewModel>(_selectDate);
        }
        public override async Task InitializeAsync(object navigationData)
        {
            SelectDate = DateTime.Today;
        }
    }
}
