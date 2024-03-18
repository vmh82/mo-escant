﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Escant_App.Behaviors
{
    public class LengthValidateBehavior : Behavior<Entry>
    {
        public static BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(LengthValidateBehavior), 2/* default value*/);

        public int MaxLength
        {
            get
            {
                return (int)GetValue(MaxLengthProperty);
            }
            set
            {
                SetValue(MaxLengthProperty, value);
            }
        }


        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            if (sender is Entry entry)
            {
                if (args.NewTextValue.Length > MaxLength)// write your logic here
                {
                    entry.Text = args.OldTextValue;
                }
            }
        }
    }

}
