using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Runtime.CompilerServices;
using Escant_App.AppSettings.Anotaciones;
using Escant_App.Generales;
/*using FBGroupingApp.ServiceOffline.Services;
using FBGroupingApp.Switch;
*/
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Connectivity;
using ManijodaServicios.Switch;
using ManijodaServicios.Offline.Interfaz;

namespace Escant_App.ViewModels.Base
{
    internal class ConectividadModelo : INotifyPropertyChanged
    {

        private SwitchSeguridad _switchSeguridad;
        private Generales.Generales generales = new Generales.Generales();
        private string _conn;
        private string _tituloPagina;
        public string Conn
        {
            get => _conn;
            set
            {
                _conn = value;
                OnPropertyChanged();
            }
        }

        public string TituloPagina
        {
            get => _tituloPagina;
            set
            {
                _tituloPagina = value;
                OnPropertyChanged();
            }
        }
        public ConectividadModelo()
        {            
            Connectivity.ConnectivityChanged += ConnectivityChangedHandler;
            CheckInternetOnStart();
        }
        public ConectividadModelo(IServicioSeguridad_Usuario usuarioServicio)
        {
            _switchSeguridad = new SwitchSeguridad(usuarioServicio);
            Connectivity.ConnectivityChanged += ConnectivityChangedHandler;
            CheckInternetOnStart();
        }

        public async void CheckInternetOnStart()
        {
            //   Conn = CrossConnectivity.Current.IsConnected || Connectivity.NetworkAccess == NetworkAccess.Internet ? "online.png" : "offline.png";
            //bool conexion = true;//IsConexionInternet().Result;
            var resultado = await _switchSeguridad.TestConexion();
            bool conexion = resultado.mensaje.Contains("google") ? false : true;
            Conn = Connectivity.NetworkAccess.ToString() == "Internet" && conexion ? "online.png" : "offline.png";
            //Conn = "offline.png";
            TituloPagina = Conn == "online.png" ? "Firebase Realtime Database" : "SQLite Realtime Database";
            generales.GuardaSesion("EstaConectado", Conn == "online.png" ? "true" : "false");
           // _ = Generales.Generales.sincronizarDatos(0);
        }

        private async void ConnectivityChangedHandler(object sender, ConnectivityChangedEventArgs e)
        {
            //bool conexion = true;//IsConexionInternet().Result;
            var resultado = await _switchSeguridad.TestConexion();
            bool conexion = resultado.mensaje.Contains("google") ? false : true;
            Conn = e.NetworkAccess.ToString() == "Internet" && conexion ? "online.png" : "offline.png";
            //Conn = "offline.png";
            TituloPagina = Conn == "online.png" ? "Firebase Realtime Database" : "SQLite Realtime Database";
            generales.GuardaSesion("EstaConectado", Conn == "online.png" ? "true" : "false");
            //_ = Generales.Generales.sincronizarDatos(0);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Método para sincronizar datos
        /// </summary>

        public async Task<bool> IsConexionInternet()
        {
            var connectivity = CrossConnectivity.Current;
            var ping = await connectivity.IsRemoteReachable("google.es");
            return ping;
        }
    }
}
