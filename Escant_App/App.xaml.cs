using Escant_App.Services;
using Escant_App.Views;
using ManijodaServicios.Resources.Texts;
using Plugin.Multilingual;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Bluetooth.Xamarin.Abstractions;
using System.Linq;
using DataModel.DTO;
using Escant_App.Models;
using System.Threading;

namespace Escant_App
{
    public partial class App : Application
    {
        public static IMPosBluetooth BluetoothAdaper;
        public static bool IsInBackgrounded { get; private set; }
        public static bool IsOrderCompleted = false;
        public App()
        {
            try
            {
                //Syncfusion.Licensing 19.2.0.44                
                //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDg5MjIzQDMxMzkyZTMyMmUzMFRTMS9IVVMvZURSTWhMcjlxQ1V4NWQyK2JnNzUzaG1CRjNaQjFIU09MN1E9");
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzUzMkAzMTM5MmUzMzJlMzBGM3NvdU9HbWduN2c2S3NDSmlaTU9kQ0E5R1BlejhLUG1PQkd4TnE1cm5rPQ==");
                InitializeComponent();

                var appLanguageCode = string.Empty;
                if (string.IsNullOrEmpty(Escant_App.AppSettings.Settings.LanguageSelected))
                {
                    /*
                    var systemLanguageCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                    if (Escant_App.AppSettings.Settings.Languages.Any(x => x.Code == systemLanguageCode))
                        appLanguageCode = systemLanguageCode;
                    else
                        appLanguageCode = Escant_App.AppSettings.Settings.DefaultLanguage;
                    */
                    appLanguageCode = Escant_App.AppSettings.Settings.DefaultLanguage;
                }
                else
                    appLanguageCode = Escant_App.AppSettings.Settings.LanguageSelected;

                CultureInfo customCulture = new CultureInfo(appLanguageCode);
                customCulture.NumberFormat.NumberDecimalSeparator = ".";
                CrossMultilingual.Current.CurrentCultureInfo = customCulture;
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
                Thread.CurrentThread.CurrentCulture = CrossMultilingual.Current.CurrentCultureInfo;
                                              
                //CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("nl-NL");


                InitApp();
                DependencyService.Register<MockDataStore>();
                //MainPage = new AppShell();
                MainPage = new NavigationView();
            }
            catch (Exception ex)
            {

            }            

            
        }

        public static void SetBluetoothAdapter(IMPosBluetooth bluetoothAdaper)
        {
            BluetoothAdaper = bluetoothAdaper;
        }
        private void InitApp()
        {
            //sync data
            
        }
        

        protected override void OnStart()
        {
        }
        
        protected override void OnResume()
        {
            base.OnResume();
            App.IsInBackgrounded = false;
        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps
            base.OnSleep();
            App.IsInBackgrounded = true;
        }

    }
}
