using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Escant_App.UserControls
{
    public class Escant_AppButton : Button
    {
        public Escant_AppButton() : base()
        {
            const int _animationTime = 100;
            Clicked += async (sender, e) =>
            {
                var btn = (Escant_AppButton)sender;
                await btn.ScaleTo(1.2, _animationTime);
                btn.ScaleTo(1, _animationTime);
            };
        }
    }
}
