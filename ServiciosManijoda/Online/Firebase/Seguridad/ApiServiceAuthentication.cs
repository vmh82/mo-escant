using DataModel.DTO.Seguridad;
using DataModel.DTO.Seguridad.Online;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ManijodaServicios.AppSettings;
using DataModel.DTO.Iteraccion;
using Escant_App.Common;

namespace ManijodaServicios.Online.Firebase
{
    public class ApiServiceAuthentication
    {

        public static async Task<DatosProceso> TestConexion(UserAuthentication oUsuario)
        {
            var datosProceso = new DatosProceso();
            try
            {

                HttpClient client = new HttpClient();
                string jsonResult = "";
                var body = JsonConvert.SerializeObject(oUsuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(SettingsOnline.ApiAuthentication("LOGIN"), content);
                jsonResult = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {                    
                    datosProceso.mensaje="Conectado";
                }
                else
                {
                    ResponseError oResponseError = JsonConvert.DeserializeObject<ResponseError>(jsonResult);
                    datosProceso.mensaje = "Inexistente";                    
                }
            }
            catch (Exception ex)
            {
                datosProceso.mensaje = ex.Message;                
            }
            return datosProceso;
        }

        /// <summary>
        /// Método para loguearse al Método de Google
        /// </summary>
        /// <param name="oUsuario"></param>
        /// <returns></returns>
        public static async Task<DatosProceso> LoginOnline(UserAuthentication oUsuario)
        {
            DatosProceso respuesta=new DatosProceso();
            try
            {

                HttpClient client = new HttpClient();
                string jsonResult = "";
                var body = JsonConvert.SerializeObject(oUsuario);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(SettingsOnline.ApiAuthentication("LOGIN"), content);
                jsonResult = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {                    
                    ResponseAuthentication oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                    SettingsOnline.oAuthentication = oResponse;
                    GlobalSettings.User = new Usuario()
                    {
                        Id=oResponse.Email,
                        Nombre = oResponse.DisplayName,
                        Email=oResponse.Email,
                        Clave=oUsuario.password,
                        IdToken = oResponse.IdToken,
                        RefreshToken = oResponse.RefreshToken
                    };
                    respuesta.respuesta = true;
                    respuesta.conClaveSinEncriptar = false;                    
                }
                else
                {
                    if (await LoginOnlineSinEncriptar(oUsuario))
                    {
                        var oUser = new UserAuthentication()
                        {
                            email = oUsuario.email,
                            password = oUsuario.password,
                            returnSecureToken = true
                        };
                        body = JsonConvert.SerializeObject(oUser);
                        content = new StringContent(body, Encoding.UTF8, "application/json");
                        response = await client.PostAsync(SettingsOnline.ApiAuthentication("LOGIN"), content);
                        jsonResult = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            ResponseAuthentication oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                            SettingsOnline.oAuthentication = oResponse;
                            GlobalSettings.User = new Usuario()
                            {
                                Id = oResponse.Email,
                                Nombre = oResponse.DisplayName,
                                Email = oResponse.Email,
                                Clave = oUsuario.password,
                                IdToken = oResponse.IdToken,
                                RefreshToken = oResponse.RefreshToken
                            };
                            respuesta.respuesta = true;
                            respuesta.conClaveSinEncriptar = true;
                        }
                        else
                        {
                            respuesta.respuesta = false;
                            respuesta.conClaveSinEncriptar = false;
                        }
                    }
                    else
                    {
                        ResponseError oResponseError = JsonConvert.DeserializeObject<ResponseError>(jsonResult);
                        SettingsOnline.oError = oResponseError;
                        respuesta.respuesta = false;                        
                    }
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                respuesta.respuesta = false;
                
            }
            return respuesta;
        }

        public static async Task<bool> LoginOnlineSinEncriptar(UserAuthentication oUsuario)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            HttpResponseMessage responseActualiza = new HttpResponseMessage();
            UserAuthentication oUser;
            UserUpdate oUserUpdate;
            ResponseAuthentication oResponse;
            StringContent content;
            StringContent contentActualiza;
            string jsonResult = "";
            string body = "";
            string bodyActualiza = "";
            try
            {
                oUser = new UserAuthentication()
                {
                    email = oUsuario.email,
                    password = oUsuario.passwordSinEnc,
                    returnSecureToken = true
                };
                body = JsonConvert.SerializeObject(oUser);
                content = new StringContent(body, Encoding.UTF8, "application/json");
                response = await client.PostAsync(SettingsOnline.ApiAuthentication("LOGIN"), content);
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    jsonResult = await response.Content.ReadAsStringAsync();
                    oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                    oUserUpdate = new UserUpdate()
                    {
                        idToken = oResponse.IdToken,
                        password = Encriptar.encriptarRJC(oUser.password),
                        returnSecureToken = true
                    };
                    bodyActualiza = JsonConvert.SerializeObject(oUserUpdate);
                    contentActualiza = new StringContent(bodyActualiza, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(SettingsOnline.ApiAuthentication("UPDATE"), contentActualiza);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return false;
            }
        }


        /// <summary>
        /// Método para registrar un usuario al método de Google
        /// </summary>
        /// <param name="oUsuario"></param>
        /// <returns></returns>
        public static async Task<Usuario> RegistrarUsuario(Usuario oUsuario,bool existe)
        {            
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpResponseMessage responseActualiza = new HttpResponseMessage();
                UserAuthentication oUser;
                UserUpdate oUserUpdate;
                ResponseAuthentication oResponse;
                ResponseError oError;
                StringContent content;
                StringContent contentActualiza;
                string jsonResult = "";
                string body = "";
                string bodyActualiza = "";
                int nuevo = 0;
                if (!existe)
                {
                    oUser = new UserAuthentication()
                    {
                        email = oUsuario.Email,
                        password = oUsuario.Clave,
                        returnSecureToken = true
                    };
                    body = JsonConvert.SerializeObject(oUser);
                    content = new StringContent(body, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(SettingsOnline.ApiAuthentication("SIGNIN"), content);
                    nuevo = 1;
                    //var response = await client.PostAsync(SettingsOnline.ApiAuthentication("UPDATE"), content) : await client.PostAsync(SettingsOnline.ApiAuthentication("SIGNIN"), content);
                }
                else
                {
                    oUser = new UserAuthentication()
                    {
                        email = oUsuario.EmailAnterior,
                        password = oUsuario.ClaveAnterior,
                        returnSecureToken = true
                    };
                    body = JsonConvert.SerializeObject(oUser);
                    content = new StringContent(body, Encoding.UTF8, "application/json");
                    response = await client.PostAsync(SettingsOnline.ApiAuthentication("LOGIN"), content);
                    if (response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        jsonResult = await response.Content.ReadAsStringAsync();
                        oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                        oUserUpdate = new UserUpdate()
                        {
                            idToken = oResponse.IdToken,
                            password = oUsuario.Clave,
                            returnSecureToken = true
                        };
                        bodyActualiza = JsonConvert.SerializeObject(oUserUpdate);
                        contentActualiza = new StringContent(bodyActualiza, Encoding.UTF8, "application/json");
                        response= await client.PostAsync(SettingsOnline.ApiAuthentication("UPDATE"), contentActualiza);
                    }
                }
                jsonResult = await response.Content.ReadAsStringAsync();
                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {                    
                    oResponse = JsonConvert.DeserializeObject<ResponseAuthentication>(jsonResult);
                    if (oResponse != null)
                    {
                        if(oUsuario.Origen  !="")
                            SettingsOnline.oAuthentication = oResponse;
                        //Actualización de Token generado
                        oUsuario.IdToken = oResponse.IdToken;                        
                    }                    
                }
                else
                {
                    oError= JsonConvert.DeserializeObject<ResponseError>(jsonResult);
                    SettingsOnline.oError = oError;
                }
            }
            catch (Exception ex)
            {
                string t = ex.Message;                
            }

            return oUsuario;

        }
        

    }

}
