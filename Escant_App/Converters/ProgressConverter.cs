using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Escant_App.Converters
{
    /// <summary>
    /// Title color value converter
    /// </summary>
    public class TitleColorConverter : IValueConverter
    {
        string checkString = string.Empty;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Span span = parameter as Span;
            Color color = Color.FromHex("#6E6E6E");
            if ((StepStatus)value == StepStatus.Completed)
            {
                if (checkString == string.Empty && span.Text != string.Empty)
                {
                    checkString = span.Text;
                    return color;
                }
                switch (span.ClassId)
                {
                    case "1":
                        color = Color.Black;
                        break;
                }
            }
            else if ((StepStatus)value == StepStatus.NotStarted)
            {
                checkString = string.Empty;
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Color)value == Color.Black;
        }
    }

    /// <summary>
    /// Sub title color value converter
    /// </summary>
    public class SubTitleColorConverter : IValueConverter
    {
        string checkString = string.Empty;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Span span = parameter as Span;
            Color color = Color.Transparent;
            if ((StepStatus)value == StepStatus.Completed)
            {
                color = Color.Black;
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Color)value == Color.Black;
        }
    }
}
