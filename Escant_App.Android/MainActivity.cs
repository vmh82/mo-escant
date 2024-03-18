using System;
using System.IO;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Gms.Ads;
using Android.Content;

using FFImageLoading.Forms.Platform;
using Acr.UserDialogs;
using Plugin.CurrentActivity;

using DataModel.Infraestructura.Offline.DB;
using Escant_App.Droid.Helpers;
using Android;
using AndroidX.Core.Content;
using AndroidX.Core.App;

namespace Escant_App.Droid
{   
    [Activity(Label = "Escant_App", Icon = "@drawable/ic_launcher", Theme = "@style/MainTheme"
        , ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
        | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize
        , ScreenOrientation = ScreenOrientation.Portrait | ScreenOrientation.Landscape)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        readonly string[] permissionGroup =
       {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Camera,
            Manifest.Permission.ReadContacts,
            Manifest.Permission.SendSms,
            Manifest.Permission.WriteSms,
            Manifest.Permission.ReadSms,
            Manifest.Permission.BroadcastSms,
            Manifest.Permission.ReceiveSms
        };
        const int requestApp = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            DbContext.LocalFilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var fileName = System.IO.Path.Combine(DbContext.LocalFilePath, DbContext.DB_NAME);
            //if (File.Exists(fileName)) File.Delete(fileName);
            if (!File.Exists(fileName))
            {
                using (var source = Assets.Open(DbContext.DB_NAME))
                using (var dest = OpenFileOutput(DbContext.DB_NAME, FileCreationMode.Append))
                {
                    source.CopyTo(dest);
                }
            }
            // Add FFLoadingImage
            CachedImageRenderer.Init(enableFastRenderer: true);

            RequestPermissions(permissionGroup, requestApp);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            UserDialogs.Init(this);
            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Syncfusion.XForms.Android.PopupLayout.SfPopupLayoutRenderer.Init();
            
            MobileAds.Initialize(ApplicationContext);
            /*RequestConfiguration.Builder builder = new RequestConfiguration.Builder();
            builder = builder.SetTestDeviceIds(new string[] { "ca-app-pub-3940256099942544~3347511713" });
            MobileAds.RequestConfiguration = builder.Build();
            */
            AppDataHelper.IniciaFirebase();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, 1);
            }


            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.ReadExternalStorage }, 1);
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}