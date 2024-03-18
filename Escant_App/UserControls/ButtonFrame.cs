using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Escant_App.UserControls
{
    public class ButtonFrame : Frame
    {
        public ButtonFrame()
        {
            HasShadow = true;
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == ContentProperty.PropertyName)
            {
                ContentUpdated();
            }
        }

        private void ContentUpdated()
        {
            if (Device.RuntimePlatform != Device.UWP)
            {
                BackgroundColor = Content.BackgroundColor;
            }
        }
    }
}
