using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Escant_App.Droid
{
    [Activity(Label = "Escant_App", MainLauncher = true, Theme = "@style/Theme.Splash", NoHistory = true,
        Icon = "@drawable/ic_launcher", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        ScreenOrientation = ScreenOrientation.Portrait | ScreenOrientation.Landscape)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;
                Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            }

            //Thread.Sleep(50);
            InvokeMainActivity();            
        }
        // Launches the startup task
        /*protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }*/

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            await Task.Delay(500); // AND AT HERE YOU CAN DECREASE YOUR SPLASH SCREEN TIME.
            InvokeMainActivity();// StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
        private void InvokeMainActivity()
        {
            var mainActivityIntent = new Intent(this, typeof(MainActivity));
            StartActivity(mainActivityIntent);
        }
    }
}