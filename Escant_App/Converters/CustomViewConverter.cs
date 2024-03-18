using System;
using System.Globalization;
using Xamarin.Forms;

namespace Escant_App.Converters
{
    public class CustomViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Device.RuntimePlatform == "Android")
            {
                if (parameter != null && parameter is string)
                    return "NestedTab.ttf#" + parameter.ToString();
                else
                    return "NestedTab.ttf";
            }
            else if (Device.RuntimePlatform == "iOS")
            {

                return "NestedTab";

            }
            else
            {
                var ff = "/Assets/NestedTab.ttf#NestedTab";
                var nested = "NestedTab.ttf#NestedTab";
                return ff;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
