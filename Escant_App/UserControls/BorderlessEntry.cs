﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Escant_App.UserControls
{
    public class BorderlessEntry : Entry
    {
        private Color _lineColorToApply;
        public BorderlessEntry()
        {
            Focused += OnFocused;
            Unfocused += OnUnfocused;

            ResetLineColor();
        }

        public Color LineColorToApply
        {
            get { return _lineColorToApply; }
            private set
            {
                _lineColorToApply = value;
                OnPropertyChanged(nameof(LineColorToApply));
            }
        }

        public static readonly BindableProperty LineColorProperty =
            BindableProperty.Create("LineColor", typeof(Color), typeof(ExtendedEntry), Color.Default);

        public Color LineColor
        {
            get { return (Color)GetValue(LineColorProperty); }
            set { SetValue(LineColorProperty, value); }
        }

        public static readonly BindableProperty FocusLineColorProperty =
            BindableProperty.Create("FocusLineColor", typeof(Color), typeof(ExtendedEntry), Color.Default);

        public Color FocusLineColor
        {
            get { return (Color)GetValue(FocusLineColorProperty); }
            set { SetValue(FocusLineColorProperty, value); }
        }

        public static readonly BindableProperty IsValidProperty =
            BindableProperty.Create("IsValid", typeof(bool), typeof(ExtendedEntry), true);

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public static readonly BindableProperty InvalidLineColorProperty =
            BindableProperty.Create("InvalidLineColor", typeof(Color), typeof(ExtendedEntry), Color.Default);

        public Color InvalidLineColor
        {
            get { return (Color)GetValue(InvalidLineColorProperty); }
            set { SetValue(InvalidLineColorProperty, value); }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == IsValidProperty.PropertyName)
            {
                CheckValidity();
            }
        }

        private void OnFocused(object sender, FocusEventArgs e)
        {
            IsValid = true;
            LineColorToApply = FocusLineColor != Color.Default
                ? FocusLineColor
                : GetNormalStateLineColor();
            seleccionaTodo(sender);
        }
        private void seleccionaTodo(object sender)
        {
            var y = (Entry)sender;
            y.CursorPosition = 0;
            y.SelectionLength = y.Text.Length;
        }
        private void OnUnfocused(object sender, FocusEventArgs e)
        {
            ResetLineColor();
        }

        
        private void ResetLineColor()
        {
            LineColorToApply = GetNormalStateLineColor();
        }

        private void CheckValidity()
        {
            if (!IsValid)
            {
                LineColorToApply = InvalidLineColor;
            }
        }

        private Color GetNormalStateLineColor()
        {
            return LineColor != Color.Default
                    ? LineColor
                    : TextColor;
        }
    }
}
