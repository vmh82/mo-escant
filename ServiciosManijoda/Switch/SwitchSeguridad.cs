using DataModel.DTO.Iteraccion;
using DataModel.DTO.Seguridad;
using DataModel.Helpers;
using DataModel.Infraestructura.Offline.DB;
using Firebase.Database;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Interfaces;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Online.Firebase;
using ManijodaServicios.Online.Firebase.Seguridad;
using ManijodaServicios.Resources.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ManijodaServicios.Switch
{
    /// <summary>
    /// Switch de métodos de Seguridad para orquestar la llamada online u offline
    /// </summary>
    public class SwitchSeguridad 
    {
        #region 0. DeclaracionServicios
        private readonly IServicioSeguridad_Usuario _seguridadServicio;        
        private readonly IServicioSeguridad_PerfilMenu _PerfilMenuServicio;
        private readonly IServicioFirestore _servicioFirestore;
        #endregion 0. DeclaracionServicios

        #region 1. Constructor
        public SwitchSeguridad(IServicioSeguridad_Usuario seguridadServicio)
        {            
            _seguridadServicio = seguridadServicio;
        }
        public SwitchSeguridad(IServicioSeguridad_Usuario seguridadServicio,IServicioFirestore servicioFirestore)
        {
            _seguridadServicio = seguridadServicio;
            _servicioFirestore = servicioFirestore;
        }
        public SwitchSeguridad(IServicioSeguridad_PerfilMenu PerfilMenuServicio)
        {
            _PerfilMenuServicio = PerfilMenuServicio;
        }
        public SwitchSeguridad(IServicioSeguridad_Usuario seguridadServicio,IServicioSeguridad_PerfilMenu PerfilMenuServicio)
        {
            _seguridadServicio = seguridadServicio;
            _PerfilMenuServicio = PerfilMenuServicio;
        }
        public SwitchSeguridad(IServicioSeguridad_Usuario seguridadServicio, IServicioSeguridad_PerfilMenu PerfilMenuServicio
                            ,IServicioFirestore servicioFirestore)
        {
            _seguridadServicio = seguridadServicio;
            _PerfilMenuServicio = PerfilMenuServicio;
            _servicioFirestore = servicioFirestore;
        }
        public SwitchSeguridad()
        {
         
        }
        #endregion 1. Constructor
        #region 2.Objetos
        #region 2.0 General
        public async Task SincronizaDatos(string nombreTabla)
        {
            switch (nombreTabla)
            {
                case "Usuario":
                    Firebase_Usuario firebaseHelper = new Firebase_Usuario();
                    var retorno = (await firebaseHelper.GetAllRegistros()).OfType<Usuario>().ToList();
                    await ImportarDatos_Usuario(retorno, false);
                    break;                
            }

        }
        public async Task<DatosProceso> ImportarDatos_Usuario(IEnumerable<Usuario> iRegistro, bool estaConectado)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                await _seguridadServicio.ImportUsuarioAsync(iRegistro);
                retorno = TextsTranslateManager.Translate("StoreInfoSaved");

            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            datosProceso.mensaje = retorno;
            return datosProceso;
        }
        
        #endregion 2.0 General

        #region 2.1.Usuario
        public async Task<DatosProceso> GuardaRegistro_Usuario(Usuario iRegistro, bool estaConectado,bool existe)
        {           
            string retorno = "";
            var datosProceso = new DatosProceso();
            Usuario oResponse = new Usuario();
            try
            {                
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    if(!existe
                        || (existe && iRegistro.Clave!=iRegistro.ClaveAnterior))
                    { 
                        oResponse = await ApiServiceAuthentication.RegistrarUsuario(iRegistro,existe);
                    }
                    if (oResponse.IdToken != null)
                    {
                        iRegistro.IdToken = oResponse.IdToken;                        
                    }
                    /*if (iRegistro.Origen != "")
                    {
                        UserAuthentication oUsuario = new UserAuthentication()
                        {
                            email = iRegistro.Email,
                            password = iRegistro.Clave,
                            returnSecureToken = true
                        };
                        bool isAuth = await Login(oUsuario, estaConectado);
                    }*/
                    if (SettingsOnline.oError == null) { 
                        Firebase_Usuario firebaseHelper = new Firebase_Usuario();
                        await firebaseHelper.InsertaRegistro(iRegistro);
                    }
                }
                #region Offline
                if (SettingsOnline.oError == null)
                {
                    //Para la parte Offline
                    if (existe)
                    {
                        iRegistro.UpdatedBy = SettingsOnline.oAuthentication != null ? SettingsOnline.oAuthentication.Email : iRegistro.Email;
                        retorno = TextsTranslateManager.Translate("UsuarioInfoUpdateSuccess");
                    }
                    else
                    {
                        iRegistro.CreatedBy = SettingsOnline.oAuthentication != null ? SettingsOnline.oAuthentication.Email : iRegistro.Email;
                        retorno = TextsTranslateManager.Translate("UsuarioInfoSaved");
                    }
                    await _seguridadServicio.InsertUsuarioAsync(iRegistro);
                }
                #endregion Offline
                //Logout
                if (iRegistro.Origen != "")
                {                    
                    GlobalSettings.User = null;
                }                
                SettingsOnline.oError = null;
            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            datosProceso.Id = iRegistro.Id;
            datosProceso.mensaje = retorno;
            datosProceso.Tipo = existe ? "actualiza" : "adiciona";
            return datosProceso;

        }

        
        public async Task<bool> Login(UserAuthentication oUsuario,bool estaConectado)
        {
            bool respuesta = false;
            DatosProceso datosProceso = new DatosProceso();
            
            if (estaConectado)
            {
                datosProceso = await ApiServiceAuthentication.LoginOnline(oUsuario);
                if (datosProceso.respuesta)
                {
                    await _servicioFirestore.LoginAsync(GlobalSettings.User.Email, GlobalSettings.User.Clave);
                    //await Autenticarse(GlobalSettings.User.Email, GlobalSettings.User.Clave);
                    if (datosProceso.conClaveSinEncriptar)
                    {
                        Firebase_Usuario firebaseHelper = new Firebase_Usuario(2);
                        var iRegistro = await ConsultaUsuario(new Usuario()
                        {
                            Email = oUsuario.email
                            ,TipoConstructor=2
                        }, estaConectado);
                        iRegistro.ClaveAnterior = iRegistro.Clave;
                        iRegistro.Clave = oUsuario.password;
                        await firebaseHelper.InsertaRegistro(iRegistro);
                    }
                }
                Usuario oUsuario1 = new Usuario()
                {
                    Id = Generator.GenerateKey(),
                    Email = oUsuario.email,
                    Clave = oUsuario.password                    
                };
                var devuelve = await _seguridadServicio.InsertUsuarioAsync(oUsuario1);
                //var devuelve = servicioSeguridad.InsertaUsuario(oUsuario1);
                //estaConectado = false;
                respuesta = datosProceso.respuesta;
            }
            if(estaConectado==false)
            {                
                respuesta = await _seguridadServicio.ConsultaUsuario(new Usuario()
                {
                    Email = oUsuario.email
                                                                            ,
                    Clave = oUsuario.password
                });
            }
            return respuesta;
                
        }
        /*Métodos para Autenticación directa
        public async Task<string> Autenticarse(string email, string password)
        {
            string respuesta = "";
            ApiUsuarioFirebase apiUsuarioFirebase = new ApiUsuarioFirebase();
            respuesta = await apiUsuarioFirebase.SignIn(email,password);            
            return respuesta;

        }*/
        public async Task<bool> ResetearPassword(UserAuthentication oUsuario, bool estaConectado)
        {
            bool respuesta = false;
            /*ApiUsuarioFirebase apiUsuarioFirebase = new ApiUsuarioFirebase();
            if (estaConectado)
            {
                respuesta = await apiUsuarioFirebase.ResetPassword(oUsuario.email);
            } */
            respuesta=await _servicioFirestore.ResetPasswordAsync(oUsuario.email);
            return respuesta;
            
        }
        /*
        public async Task<string> CambioPassword(UserAuthentication oUsuario, bool estaConectado)
        {
            string respuesta = "";
            ApiUsuarioFirebase apiUsuarioFirebase = new ApiUsuarioFirebase();
            if (estaConectado)
            {
                respuesta = await apiUsuarioFirebase.ChangePassword(oUsuario.token,oUsuario.password);
            }
            return respuesta;

        }*/

        public async Task<DatosProceso> TestConexion()
        {

            return await ApiServiceAuthentication.TestConexion(new UserAuthentication() { email="test@test.com"
                                                                                          ,password="test"
                                                                                           ,returnSecureToken=true});            
            
        }
        public async Task<bool> RefrescaCredencialesOnline(bool estaConectado)
        {
            if (SettingsOnline.oAuthentication == null)
            {

                
                UserAuthentication oUsuario = new UserAuthentication()
                {
                    email = GlobalSettings.User.Email,
                    password = GlobalSettings.User.Clave,
                    returnSecureToken = true
                };                

                return await Login(oUsuario, estaConectado);
            }
            else
                return true;
        }
        public async Task<FirebaseClient> Conectar(bool estaConectado)
        {
            if (estaConectado) { 
                UserAuthentication oUsuario = new UserAuthentication()
                {
                    email = GlobalSettings.User.Email,
                    password = GlobalSettings.User.Clave,
                    returnSecureToken = true
                };

                await Login(oUsuario, estaConectado);
            }
            return estaConectado?new FirebaseClient(SettingsOnline.ApiFirebase, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(SettingsOnline.oAuthentication.IdToken) }):null;

        }
        public async Task<List<Usuario>> ConsultaUsuarios(Usuario iRegistro, bool estaConectado)
        {
            List<Usuario> retorno = new List<Usuario>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Usuario firebaseHelper = new Firebase_Usuario();
                retorno = await firebaseHelper.GetRegistrosUsuario(iRegistro);
                if (retorno == null)
                    retorno = (await _seguridadServicio.GetAllUsuarioAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.Nombre == iRegistro.Nombre && iRegistro.Nombre != null)
                                                                           || (x.Email == iRegistro.Email && iRegistro.Email != null)                                                                           
                                                                           || (x.Nombre.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Perfil.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Email.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                         )


                    )?.ToList();
            }
            else
            {
                retorno = (await _seguridadServicio.GetAllUsuarioAsync(a => (a.IsDeleted == iRegistro.IsDeleted
                                                  && string.IsNullOrEmpty(iRegistro.valorBusqueda)
                                                  && string.IsNullOrEmpty(iRegistro.Id)
                                                  && string.IsNullOrEmpty(iRegistro.Email)
                                                  && string.IsNullOrEmpty(iRegistro.Clave)
                                                  && string.IsNullOrEmpty(iRegistro.PinDigitos))
                                                || (!string.IsNullOrEmpty(iRegistro.Id) && a.Id == iRegistro.Id)
                                                || (!string.IsNullOrEmpty(iRegistro.Email) && a.Email == iRegistro.Email
                                                 && !string.IsNullOrEmpty(iRegistro.Clave) && a.Clave == iRegistro.Clave)
                                                || (!string.IsNullOrEmpty(iRegistro.PinDigitos) && a.PinDigitos == iRegistro.PinDigitos)
                                                || (!string.IsNullOrEmpty(iRegistro.valorBusqueda)
                                                 && (a.Perfil.ToLower().Contains(iRegistro.valorBusqueda)
                                                    || a.Nombre.ToLower().Contains(iRegistro.valorBusqueda)
                                                    || a.Email.ToLower().Contains(iRegistro.valorBusqueda)
                                                    )
                                                    )
                                                )

                    )?.ToList();
            }
            return retorno;
        }
        public async Task<Usuario> ConsultaUsuario(Usuario iRegistro, bool estaConectado)
        {
            Usuario retorno = new Usuario();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Usuario firebaseHelper = iRegistro.TipoConstructor==2?new Firebase_Usuario(2): new Firebase_Usuario();
                retorno = (await firebaseHelper.GetRegistrosUsuario(iRegistro)).FirstOrDefault();
                if (retorno == null)
                    retorno = (await _seguridadServicio.GetAllUsuarioAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.Nombre == iRegistro.Nombre && iRegistro.Nombre != null)
                                                                           || (x.Email == iRegistro.Email && iRegistro.Email != null)
                                                                           || (x.Nombre.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Perfil.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Email.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                         )


                    )?.FirstOrDefault();
            }
            else
            {
                var predicate = PredicateBuilder.True<Usuario>();
                predicate = PredicateBuilder.And(predicate, x => x.Deleted == (iRegistro.IsDeleted?1:0));
                retorno=(await _seguridadServicio.GetAllUsuarioAsync(predicate))?.FirstOrDefault();
                /*retorno = (await _seguridadServicio.GetAllUsuarioAsync(a => (a.IsDeleted == iRegistro.IsDeleted
                                                   && string.IsNullOrEmpty(iRegistro.valorBusqueda)
                                                   && string.IsNullOrEmpty(iRegistro.Id)
                                                   && string.IsNullOrEmpty(iRegistro.Email)
                                                   && string.IsNullOrEmpty(iRegistro.Clave)
                                                   && string.IsNullOrEmpty(iRegistro.PinDigitos))
                                                 || (!string.IsNullOrEmpty(iRegistro.Id) && a.Id == iRegistro.Id)
                                                 || (!string.IsNullOrEmpty(iRegistro.Email) && a.Email == iRegistro.Email
                                                  && !string.IsNullOrEmpty(iRegistro.Clave) && a.Clave == iRegistro.Clave)
                                                 || (!string.IsNullOrEmpty(iRegistro.PinDigitos) && a.PinDigitos == iRegistro.PinDigitos)
                                                 || (!string.IsNullOrEmpty(iRegistro.valorBusqueda)
                                                  && (a.Perfil.ToLower().Contains(iRegistro.valorBusqueda)
                                                     || a.Nombre.ToLower().Contains(iRegistro.valorBusqueda)
                                                     || a.Email.ToLower().Contains(iRegistro.valorBusqueda)
                                                     )
                                                   )
                                               )

                   )?.FirstOrDefault();*/
            }
            return retorno;
        }
        #endregion 2.1.Usuario

        #region 2.2.PerfilMenu
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<PerfilMenu>> ConsultaPerfilesMenu(PerfilMenu iRegistro, bool estaConectado)
        {
            List<PerfilMenu> retorno = new List<PerfilMenu>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_PerfilMenu firebaseHelper = new Firebase_PerfilMenu();
                retorno = await firebaseHelper.GetRegistrosPerfilMenu(iRegistro);
                if (retorno == null)
                    retorno = (await _PerfilMenuServicio.GetAllPerfilMenuAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.ValorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.IdPerfil == iRegistro.IdPerfil && iRegistro.IdPerfil != null)
                                                                           || (x.CodigoPerfil == iRegistro.CodigoPerfil && iRegistro.CodigoPerfil != null)
                                                                           || (x.IdMenu == iRegistro.IdMenu && iRegistro.IdMenu != null)
                                                                           || (x.CodigoPerfil.ToLower().Contains(iRegistro.ValorBusqueda) && iRegistro.ValorBusqueda != null)                                                                           
                                                                         )


                    )?.ToList();
            }
            else
            {
                retorno = (await _PerfilMenuServicio.GetAllPerfilMenuAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.ValorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.IdPerfil == iRegistro.IdPerfil && iRegistro.IdPerfil != null)
                                                                           || (x.CodigoPerfil == iRegistro.CodigoPerfil && iRegistro.CodigoPerfil != null)
                                                                           || (x.IdMenu == iRegistro.IdMenu && iRegistro.IdMenu != null)
                                                                           || (x.CodigoPerfil.ToLower().Contains(iRegistro.ValorBusqueda) && iRegistro.ValorBusqueda != null)                                                                           
                                                                         )

                    )?.ToList();
            }
            return retorno;
        }
        /// <summary>
        /// Método para almacenar los registros de Catálogo
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <param name="estaConectado"></param>
        /// <param name="existe"></param>
        /// <returns></returns>
        public async Task<DatosProceso> GuardaRegistro_PerfilMenu(PerfilMenu iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_PerfilMenu firebaseHelper = new Firebase_PerfilMenu();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                #region Offline
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    retorno = TextsTranslateManager.Translate("PerfilMenuInfoUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                    retorno = TextsTranslateManager.Translate("PerfilMenuInfoSaved");
                }
                await _PerfilMenuServicio.InsertPerfilMenuAsync(iRegistro);
                #endregion Offline
            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            datosProceso.Id = iRegistro.Id;
            datosProceso.mensaje = retorno;
            datosProceso.Tipo = existe ? "actualiza" : "adiciona";
            return datosProceso;
        }

        public async Task<DatosProceso> GuardaRegistro_PerfilesMenu(List<PerfilMenu> iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                foreach(var perfilMenu in iRegistro)
                {
                    datosProceso = await GuardaRegistro_PerfilMenu(perfilMenu,estaConectado,existe);
                }
                
            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }            
            return datosProceso;
        }

        #endregion 2.1.PerfilMenu

        #endregion 2.Objetos

    }
}
