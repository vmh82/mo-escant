using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Escant_App.UserControls
{
    public class FilterPicker : Picker
    {
        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(string), typeof(FilterPicker), string.Empty );

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(Int32), typeof(FilterPicker), 10, BindingMode.Default);

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public Int32 FontSize
        {
            get => (Int32)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
    }
}
