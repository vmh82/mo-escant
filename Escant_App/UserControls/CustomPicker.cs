using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Escant_App.UserControls
{
    public class CustomPicker : Picker
    {
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(FontSize), typeof(Int32), typeof(CustomPicker),
                10 , BindingMode.Default);

        /// <summary>
        /// Added Icon to picker
        /// </summary>
        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(string), typeof(CustomPicker), string.Empty);

       public Int32 FontSize
        {
            get => (Int32)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        
        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }
}
