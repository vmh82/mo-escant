﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Escant_App.UserControls
{
    public class AdBanner : View
    {
        public enum Sizes { Standardbanner, LargeBanner, MediumRectangle, FullBanner, Leaderboard, SmartBannerPortrait }
        public Sizes Size { get; set; }
        public AdBanner()
        {
            this.BackgroundColor = Color.Transparent;
        }
    }
}
