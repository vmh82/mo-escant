using Syncfusion.XForms.ProgressBar;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Escant_App.Converters
{
    public class StatusConverter : IValueConverter
    {
        string checkString = string.Empty;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StepStatus status=StepStatus.NotStarted;
            if (value != null)
            {

                if (parameter != null)
                {
                    if (parameter.GetType().Name.Contains("String"))
                    {
                        var cadena = (string)value;
                        switch (cadena)
                        {
                            case var s when cadena.Contains("Completed"):
                                status = StepStatus.Completed;
                                break;
                            case var s when cadena.Contains("Progress"):
                                status = StepStatus.InProgress;
                                break;
                            default: status = StepStatus.NotStarted;
                                break;
                        }
                    }
                    else
                    {
                        status = StepStatus.NotStarted;
                    }

                }
                else
                {
                    status = StepStatus.NotStarted;

                }

            }
            return status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return StepStatus.NotStarted;
        }
    }
}
