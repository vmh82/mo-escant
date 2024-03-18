using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using Escant_App.ViewModels.Base;
using Escant_App.Models;

namespace Escant_App.ViewModels.Reportes
{
    public class PopupSelectDateRangeViewModel : ViewModelBase
    {
        private PeriodType _periodType;
        public ICommand CancelCommand => new Command(async () => await CancelAsync());
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _isEnableDateTimePicker;

        private async Task CancelAsync()
        {
            await PopupNavigation.PopAsync();
        }

        public PeriodType PeriodType
        {
            get
            {
                return _periodType;
            }
            set
            {
                _periodType = value;
                RaisePropertyChanged(() => PeriodType);
            }
        }
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                RaisePropertyChanged(() => StartDate);
            }
        }
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                RaisePropertyChanged(() => EndDate);
            }
        }
        public bool IsEnableDateTimePicker
        {
            get
            {
                return _isEnableDateTimePicker;
            }
            set
            {
                _isEnableDateTimePicker = value;
                RaisePropertyChanged(() => IsEnableDateTimePicker);
            }
        }
        public ICommand OkCommand => new Command(async () => await OkAsync());
        private async Task OkAsync()
        {
            var period = new SelectReportPeriod { PeriodType = PeriodType };

            switch (PeriodType)
            {
                case PeriodType.Today:
                    period.FromDate = DateTime.Today;
                    period.ToDate = DateTime.Today.AddDays(1);
                    break;
                case PeriodType.Yesterday:
                    period.FromDate = DateTime.Now.AddDays(-1).Date;
                    period.ToDate = period.FromDate.AddDays(1);
                    break;
                case PeriodType.LastWeek:
                    DateTime date = DateTime.Now.AddDays(-7);
                    while (date.DayOfWeek != DayOfWeek.Monday)
                    {
                        date = date.AddDays(-1);
                    }
                    period.FromDate = date;
                    period.ToDate = date.AddDays(6);
                    break;
                case PeriodType.ThisMonth:
                    period.FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    int lastDayInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    period.ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, lastDayInMonth);
                    break;
                case PeriodType.LastMonth:
                    DateTime previousMonth = DateTime.Now.AddMonths(-1);
                    period.FromDate = new DateTime(previousMonth.Year, previousMonth.Month, 1);
                    int lastDayInLastMonth = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
                    period.ToDate = new DateTime(previousMonth.Year, previousMonth.Month, lastDayInLastMonth);
                    break;
                case PeriodType.Option:
                    period.FromDate = StartDate;
                    period.ToDate = EndDate;
                    break;
                default:
                    period.FromDate = DateTime.Today;
                    period.ToDate = DateTime.Today.AddDays(1);
                    break;
            }
            MessagingCenter.Send(period, nameof(SelectReportPeriod));
            await PopupNavigation.Instance.PopAsync();
        }
        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData != null && navigationData is SelectReportPeriod reportPeriod)
            {
                PeriodType = reportPeriod.PeriodType;
                StartDate = reportPeriod.FromDate;
                EndDate = reportPeriod.ToDate;
                if (PeriodType == PeriodType.Option)
                {
                    IsEnableDateTimePicker = true;
                }
            }
            else
            {
                StartDate = DateTime.Today;
                EndDate = DateTime.Today.AddDays(1);
            }
        }
    }
}
