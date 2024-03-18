using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Escant_App.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(Escant_App.Droid.AlertViewAndroid))]
namespace Escant_App.Droid
{
    public class AlertViewAndroid : IAlertView
    {
        public void Show(string message)
        {
            ContentPage page = new ContentPage();
            page.DisplayAlert("", message, "Ok");
        }
    }
}