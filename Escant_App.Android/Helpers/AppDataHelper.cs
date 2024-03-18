using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Firestore;
using ManijodaServicios.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escant_App.Droid.Helpers
{
    public static class AppDataHelper
    {
        public static void IniciaFirebase()
        {


            //var app = FirebaseApp.InitializeApp(Application.Context);
            try
            {
                var option = new FirebaseOptions.Builder()
                        .SetApplicationId(SettingsOnline.IdProyecto)
                        .SetApiKey(SettingsOnline.KeyAplication)
                        .SetDatabaseUrl(SettingsOnline.ApiFirebase)
                        .SetStorageBucket(SettingsOnline.StorageFirebase)
                        .SetProjectId(SettingsOnline.IdProyecto)
                        .Build();
                FirebaseApp.InitializeApp(Application.Context /* Context */, option);
                //app = FirebaseApp.InitializeApp(Application.Context, option);
            }
            catch (Exception ex)
            { }

        }
        public static FirebaseApp IniciarFirebase()
        {

            //FirebaseApp app=null;
            FirebaseApp app = FirebaseApp.InitializeApp(Application.Context);

            if (app == null)
            {
                var option = new FirebaseOptions.Builder()
                        .SetApplicationId(SettingsOnline.IdProyecto)
                        .SetApiKey(SettingsOnline.KeyAplication)
                        .SetDatabaseUrl(SettingsOnline.ApiFirebase)
                        .SetStorageBucket(SettingsOnline.StorageFirebase)
                        .SetProjectId(SettingsOnline.IdProyecto)
                        .Build();
                app = FirebaseApp.InitializeApp(Application.Context, option);
            }
            return app;
        }

        public static FirebaseFirestore GetDataBase()
        {
            FirebaseFirestore database = null;
            try
            {
                var app = IniciarFirebase();
                database = FirebaseFirestore.GetInstance(app);
            }
            catch (Exception ex)
            {

            }
            return database;
        }
    }
}