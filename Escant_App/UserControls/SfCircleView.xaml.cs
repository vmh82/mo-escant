using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.UserControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SfCircleView : ContentView
    {
        public SfCircleView()
        {
            InitializeComponent();
            Container.BindingContext = this;
        }

        public static BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(SfCircleView), Color.Black);
        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(SfCircleView), Color.White);
        public new static BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SfCircleView), Color.White);
        public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(int), typeof(SfCircleView), 14);
        public static BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(SfCircleView), 50);
        public static BindableProperty AbbrTextProperty = BindableProperty.Create(nameof(AbbrText), typeof(string), typeof(SfCircleView), string.Empty,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            if (bindable is SfCircleView initialsView)
                initialsView.UpdateTextWithName(newVal?.ToString());
        });

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set
            {
                SetValue(BorderColorProperty, value);
            }
        }

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set
            {
                SetValue(BackgroundColorProperty, value);
            }
        }
        public Color TextColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set
            {
                SetValue(BorderColorProperty, value);
            }
        }
        public int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set
            {
                SetValue(FontSizeProperty, value);
            }
        }

        public int CornerRadius
        {
            get => (int)GetValue(CornerRadiusProperty);
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        public string AbbrText
        {
            get => (string)GetValue(AbbrTextProperty);
            set
            {
                SetValue(AbbrTextProperty, value);
            }
        }

        private void UpdateTextWithName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            var separateWords = name.Split(' ');
            if (separateWords.Length > 0)
            {
                var initialsArray = separateWords.Where(x => !string.IsNullOrEmpty(x)).Select(word => word[0].ToString().ToUpper()).ToArray(); // array of string of initials upper cased
                if (initialsArray.Length > 1)
                {
                    // grab the first and last
                    initialsArray = new string[2] { initialsArray[0], initialsArray[initialsArray.Length - 1] };
                }
                var initialsString = string.Join(string.Empty, initialsArray);
                AbbrLabel.Text = initialsString;
            }
            else
            {
                AbbrLabel.Text = "MN";
            }
        }

    }
}
