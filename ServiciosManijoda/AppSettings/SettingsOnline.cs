using System;
using System.Collections.Generic;
using System.Text;
using DataModel.DTO.Administracion;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Productos;
using DataModel.DTO.Seguridad;
using DataModel.DTO.Seguridad.Online;
using FirebaseAdmin;

namespace ManijodaServicios.AppSettings
{
    public class SettingsOnline
    {
        /// <summary>
        /// Constantes que se utilizan en todo el proyecto para definir datos de conexión de la Base de Datos Online
        /// </summary>      
        /*
        public static string ApiFirebase = "https://escantapp-default-rtdb.firebaseio.com/";
        public static string StorageFirebase = "escantapp.appspot.com";
        public static string FirestoreFirebase = "escantapp";
        public static string IdProyecto = "escantapp";
        public static string KeyAplication = "AIzaSyAymmcDQjKwAJ-Kf0zpLSRl4bDNF6X8p2w";
        */
        public static string ApiFirebase = "https://app-scant-882a1-default-rtdb.firebaseio.com/";//
        public static string StorageFirebase = "//app-scant-882a1.appspot.com";   //
        public static string FirestoreFirebase = "app-scant-882a1"; //
        public static string IdProyecto = "app-scant-882a1"; //
        public static string KeyAplication = " AIzaSyDYW5uaOZQeNbbN79DqyFKmlbdiGuJYxvY"; //

        public static Aplicacion oAplicacion { get; set; }
        public static ResponseAuthentication oAuthentication { get; set; }
        public static ResponseError oError { get; set; }
        public static Catalogo oCatalogo { get; set; }
        public static List<Precio> oLPrecio { get; set; }
        public static List<Catalogo> oLCatalogo { get; set; }
        public static List<PerfilMenu> oPerfilMenu { get; set; }
        public static bool EsMenuVacio { get; set; }

        private static string ApiUrlGoogleApis = "https://identitytoolkit.googleapis.com/v1/";        
        public static string ApiAuthentication(string tipo)
        {
            if (tipo == "LOGIN")
                return ApiUrlGoogleApis + "accounts:signInWithPassword?key=" + KeyAplication;
            else if (tipo == "SIGNIN")
                return ApiUrlGoogleApis + "accounts:signUp?key=" + KeyAplication;
            else if (tipo == "UPDATE")
                return ApiUrlGoogleApis + "accounts:update?key=" + KeyAplication;
            else if (tipo == "CONSULTA")
                return ApiUrlGoogleApis + "projects/" + IdProyecto + "/accounts";
            else
                return "";

        }
    }
}
