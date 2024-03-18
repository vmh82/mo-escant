using System;
using System.Globalization;
using ManijodaServicios.Resources.Texts;
using Xamarin.Forms;

namespace Escant_App.Converters
{
    public class StoreMobileNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(!string.IsNullOrEmpty(value?.ToString()))
            {
                return string.Format(TextsTranslateManager.Translate("Phone"), value.ToString());
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
