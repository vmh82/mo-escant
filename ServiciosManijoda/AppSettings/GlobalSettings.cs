using DataModel.DTO.Seguridad;
using ManijodaServicios.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManijodaServicios.AppSettings
{
    public static class GlobalSettings
    {
        static string defaultServerEndpoint;
        public static int TimeForSync = 120; //in second
        static GlobalSettings()
        {
            //Change your API Server here
            defaultServerEndpoint = "http://192.168.1.1:8080";
        }
        public static string SignInEndpoint => $"{defaultServerEndpoint}/connect/token";
        public static string CategoriesEndpoint => $"{defaultServerEndpoint}/api/Escant_App/categories";
        public static string ProductsEndpoint => $"{defaultServerEndpoint}/api/Escant_App/products";
        public static string UnitsEndpoint => $"{defaultServerEndpoint}/api/Escant_App/units";
        public static string CustomersEndpoint => $"{defaultServerEndpoint}/api/Escant_App/customers";
        public static string SuppliersEndpoint => $"{defaultServerEndpoint}/api/Escant_App/suppliers";
        public static string SyncDataEndpoint => $"{defaultServerEndpoint}/api/Escant_App/pullData";
        public static string PushLocalDataEndpoint => $"{defaultServerEndpoint}/api/Escant_App/pushData";
        public static string ZReportEndpoint => $"{defaultServerEndpoint}/api/Escant_App/zReport";
        public static Usuario User
        {
            get => PreferencesHelpers.Get(nameof(User), default(Usuario));
            set => PreferencesHelpers.Set(nameof(User), value);
        }
    }
}
