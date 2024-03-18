using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Escant_App.UserControls
{
    public class Checkbox : View
    {
        public event EventHandler OnCheckChanged;

        public static BindableProperty OutlineColorProperty = BindableProperty.Create(nameof(OutlineColor), typeof(Color), typeof(Checkbox), Color.Black);
        public static BindableProperty InnerColorProperty = BindableProperty.Create(nameof(InnerColor), typeof(Color), typeof(Checkbox), Color.White);
        public static BindableProperty CheckColorProperty = BindableProperty.Create(nameof(CheckColor), typeof(Color), typeof(Checkbox), Color.Black);
        public static BindableProperty CheckedOutlineColorProperty = BindableProperty.Create(nameof(CheckedOutlineColor), typeof(Color), typeof(Checkbox), Color.Black);
        public static BindableProperty CheckedInnerColorProperty = BindableProperty.Create(nameof(CheckedInnerColor), typeof(Color), typeof(Checkbox), Color.White);
        public static BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(Checkbox), false, BindingMode.TwoWay);
        public static BindableProperty CheckedCommandProperty = BindableProperty.Create(nameof(CheckedCommand), typeof(ICommand), typeof(Checkbox), null);
        public static BindableProperty CheckedCommandParameterProperty = BindableProperty.Create(nameof(CheckedCommandParameter), typeof(object), typeof(Checkbox), null);

        public object CheckedCommandParameter
        {
            get
            {
                return GetValue(CheckedCommandParameterProperty);
            }
            set
            {
                SetValue(CheckedCommandParameterProperty, value);
            }
        }
        public ICommand CheckedCommand
        {
            get
            {
                return (ICommand)GetValue(CheckedCommandProperty);
            }
            set
            {
                SetValue(CheckedCommandProperty, value);
            }
        }
        public bool IsChecked
        {
            get
            {
                return (bool)GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }
        public Color CheckColor
        {
            get
            {
                return (Color)GetValue(CheckColorProperty);
            }
            set
            {
                SetValue(CheckColorProperty, value);
            }
        }
        public Color InnerColor
        {
            get
            {
                return (Color)GetValue(InnerColorProperty);
            }
            set
            {
                SetValue(InnerColorProperty, value);
            }
        }
        public Color OutlineColor
        {
            get
            {
                return (Color)GetValue(OutlineColorProperty);
            }
            set
            {
                SetValue(OutlineColorProperty, value);
            }
        }
        public Color CheckedInnerColor
        {
            get
            {
                return (Color)GetValue(CheckedInnerColorProperty);
            }
            set
            {
                SetValue(CheckedInnerColorProperty, value);
            }
        }
        public Color CheckedOutlineColor
        {
            get
            {
                return (Color)GetValue(CheckedOutlineColorProperty);
            }
            set
            {
                SetValue(CheckedOutlineColorProperty, value);
            }
        }

        public void FireCheckChange()
        {
            OnCheckChanged?.Invoke(this, new CheckChangedArgs
            {
                IsChecked = IsChecked
            });
        }

        public class CheckChangedArgs : EventArgs
        {
            public bool IsChecked { get; set; }
        }
    }

    /*
    public class Checkbox : View
    {
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create<CheckboxCustom, bool>
            (p => p.IsChecked, true, propertyChanged: (s, o, n) => { (s as CheckboxCustom).OnChecked(new EventArgs()); });
        public static readonly BindableProperty ColorProperty = BindableProperty.Create<CheckboxCustom, Color>
            (p => p.Color, defaultValue:Color.Default);

        public bool IsChecked
        {
            get
            {
                return (bool)GetValue(IsCheckedProperty);
            }
            set
            {
                SetValue(IsCheckedProperty, value);
            }
        }

        public Color Color
        {
            get
            {
                return (Color)GetValue(ColorProperty);
            }
            set
            {
                SetValue(ColorProperty, value);
            }
        }

        public event EventHandler Checked;

        protected virtual void OnChecked(EventArgs e)
        {
            if (Checked != null)
                Checked?.Invoke(this, e);
        }
    }
    */
}
