using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Escant_App.UserControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InitialsCircleView : ContentView
	{
        public static BindableProperty CircleColorProperty = BindableProperty.Create(nameof(CircleColor), typeof(Color), typeof(InitialsCircleView), Color.White);
        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(InitialsCircleView), Color.White);
        public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(int), typeof(InitialsCircleView), 14);
        public static BindableProperty FontProperty = BindableProperty.Create(nameof(Font), typeof(Font), typeof(InitialsCircleView), Font.Default);
        public static BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(InitialsCircleView), 0.0, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var initialsView = bindable as InitialsCircleView;
            if (initialsView != null)
                initialsView.UpdateCornerRadius((double)newVal);
        });
        public static BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(InitialsCircleView), string.Empty,
        propertyChanged: (bindable, oldVal, newVal) =>
        {
            var initialsView = bindable as InitialsCircleView;
            if (initialsView != null)
                initialsView.UpdateTextWithName(newVal?.ToString());
        });

        public double CornerRadius
        {
            get
            {
                return (double)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
        public string Name
        {
            get
            {
                return (string)GetValue(NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);
            }
        }
        public int FontSize
        {
            get
            {
                return (int)GetValue(FontSizeProperty);
            }
            set
            {
                SetValue(FontSizeProperty, value);
            }
        }
        public Font Font
        {
            get
            {
                return (Font)GetValue(FontProperty);
            }
            set
            {
                SetValue(FontProperty, value);
            }
        }
        public Color CircleColor
        {
            get
            {
                return (Color)GetValue(CircleColorProperty);
            }
            set
            {
                SetValue(CircleColorProperty, value);
            }
        }
        public Color TextColor
        {
            get
            {
                return (Color)GetValue(TextColorProperty);
            }
            set
            {
                SetValue(TextColorProperty, value);
            }
        }


        public InitialsCircleView()
        {
            InitializeComponent();
            Container.BindingContext = this;
        }

        /// <summary>
        /// Updates the name of the text with.
        /// </summary>
        /// <param name="name">Name.</param>
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
                InitialsLabel.Text = initialsString;
            }
            else
            {
                //InitialsLabel.Text = string.Empty;
                //Set default Avatar
                InitialsLabel.Text = "T";
            }
        }

        /// <summary>
        /// Updates the corner radius.
        /// </summary>
        /// <param name="radius">Radius.</param>
        private void UpdateCornerRadius(double radius)
        {
            Circle.CornerRadius = radius;
        }
    }
}