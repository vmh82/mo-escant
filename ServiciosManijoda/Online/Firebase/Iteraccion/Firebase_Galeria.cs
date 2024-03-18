using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataModel.DTO.Iteraccion;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using ManijodaServicios.AppSettings;

namespace ManijodaServicios.Online.Firebase.Iteraccion
{
    /// <summary>
    /// Servicio para administración de datos online en Firebase del Objeto Galeria
    /// </summary>
    public class Firebase_Galeria
    {
        #region 1.Declaraciones
        /// <summary>
        /// 1. Declaración de instancias de conexión
        /// </summary>
        Switch.SwitchSeguridad switchSeguridad = new Switch.SwitchSeguridad();
        FirebaseClient firebase = new FirebaseClient(SettingsOnline.ApiFirebase, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(SettingsOnline.oAuthentication.IdToken) });
        #endregion 1.Declaraciones

        #region 2.Constructor
        /// <summary>
        /// 2. Constructor del Servicio
        /// </summary>
        public Firebase_Galeria()
        {
            Task.Run(async () =>
            {
                firebase = await switchSeguridad.Conectar(true);
            });

        }
        #endregion 2.Constructor

        #region 3.Métodos
        /// <summary>
        /// Consultar todos los registros
        /// </summary>
        /// <returns></returns>
        public async Task<List<Galeria>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("Galeria")
                  .OnceAsync<Galeria>()).Select(item => new Galeria
                  {
                      Id = item.Object.Id,
                      Nombre= item.Object.Nombre,
                      UrlImagen = item.Object.UrlImagen,
                      UrlImagenOffline = item.Object.UrlImagenOffline,                      
                      Image = item.Object.Image
                  }).ToList() : null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Método para consultar un registro
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<Galeria> GetRegistro(Galeria oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("Galeria")
                  .OnceAsync<Galeria>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)                                         
                                            || (a.Nombre == oRegistro.Nombre && oRegistro.Nombre != null)
                                         ).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Método para consultar un registro acorde a filtros
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<Galeria>> GetRegistrosGaleria(Galeria oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Galeria")
                      .OnceAsync<Galeria>();
                    return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                             ).ToList();
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                var y = ex.Message.ToString();
                return null;
            }

        }
        /// <summary>
        /// Método para mantenimiento de un nuevo registro
        /// </summary>
        /// <param name="iregistro"></param>
        /// <returns></returns>
        public async Task InsertaRegistro(Galeria iregistro)
        {
            Galeria registro = await GetRegistro(iregistro);
            if (registro != null || iregistro.EsEdicion == 1)
            {
                await UpdateRegistro(iregistro);
            }
            else
            {
                iregistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                await AddRegistro(iregistro);
            }
        }
        /// <summary>
        /// Método para adicionar un registro nuevo
        /// </summary>
        /// <param name="iregistro"></param>
        /// <returns></returns>
        public async Task AddRegistro(Galeria iregistro)
        {
            try
            {
                await firebase
                  .Child("Galeria")
                  .PostAsync(new Galeria()
                  {
                      Id = iregistro.Id,
                      Directorio = iregistro.Directorio,
                      Nombre = iregistro.Nombre,
                      UrlImagen = iregistro.UrlImagen,
                      UrlImagenOffline = iregistro.UrlImagenOffline,
                      Image = iregistro.Image,
                      //Logo = iregistro.Logo
                      //Datos de Auditoría
                      CreatedBy = SettingsOnline.oAuthentication.Email,
                      CreatedDateFormato = iregistro.CreatedDateFormato

                  });
            }
            catch (Exception ex)
            {
                var y = ex.Message.ToString();
            }
        }

        /// <summary>
        /// Método para actualizar un registro existente
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <returns></returns>
        public async Task UpdateRegistro(Galeria iRegistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("Galeria")
              .OnceAsync<Galeria>()).Where(a => a.Object.Id == iRegistro.Id).FirstOrDefault();

            await firebase
              .Child("Galeria")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Galeria()
              {
                  Id = toUpdateRegistro.Object.Id,
                  Directorio = toUpdateRegistro.Object.Directorio,
                  Nombre = iRegistro.Nombre,
                  Image=iRegistro.Image,
                  UrlImagen = toUpdateRegistro.Object.UrlImagen,
                  UrlImagenOffline = toUpdateRegistro.Object.UrlImagenOffline,
                  //Logo = iRegistro.Logo
                  //Datos de Auditoría
                  UpdatedBy = SettingsOnline.oAuthentication.Email,
                  CreatedBy = toUpdateRegistro.Object.CreatedBy,
                  CreatedDateFormato = toUpdateRegistro.Object.CreatedDateFormato
              });
        }
        /// <summary>
        /// Método para eliminar un registro de manera lógica
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task DeleteRegistro(Galeria oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("Galeria")
              .OnceAsync<Galeria>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("Galeria").Child(toDeleteRegistro.Key).DeleteAsync();

        }
        /// <summary>
        /// Inserta objeto Galeria en Storage
        /// </summary>
        /// <param name="iregistro"></param>
        /// <returns></returns>
        public async Task<string> InsertaObjeto(Galeria iregistro)
        {
            string url = "";
            try
            {
                Stream stream = iregistro.fStream==null?new MemoryStream(Convert.FromBase64String(iregistro.Image)):iregistro.fStream;
                string nombreObjeto = "";
                switch (iregistro.Extension)
                {
                    case ".pdf":nombreObjeto = iregistro.Nombre+iregistro.Extension;
                        break;
                    default:nombreObjeto = iregistro.Id + iregistro.Extension;
                        break;
                }
                var task = await new FirebaseStorage(SettingsOnline.StorageFirebase)
                    .Child(iregistro.Directorio)
                    .Child(nombreObjeto)
                    .PutAsync(stream);
                url = task;
            }
            catch (Exception ex)
            {
                var y = ex.Message.ToString();
            }
            return url;
        }

        #endregion 3.Métodos
    }
}
