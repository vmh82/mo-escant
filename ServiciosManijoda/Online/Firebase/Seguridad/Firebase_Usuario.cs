using DataModel.DTO.Productos;
using DataModel.DTO.Seguridad;
using Firebase.Database;
using Firebase.Database.Query;
using ManijodaServicios.AppSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ManijodaServicios.Online.Firebase.Seguridad
{
    public class Firebase_Usuario
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
        public Firebase_Usuario()
        {
            Task.Run(async () =>
            {
                firebase = await switchSeguridad.Conectar(true);
            });

        }
        public Firebase_Usuario(int tipo)
        {
            Task.Run(async () =>
            {
                if(tipo==1)
                    firebase = await switchSeguridad.Conectar(true);
            });

        }
        #endregion 2.Constructor

        #region 3.Métodos
        /// <summary>
        /// Consultar todos los registros
        /// </summary>
        /// <returns></returns>
        public async Task<List<Usuario>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("usuarios")
                  .OnceAsync<Usuario>()).Select(item => new Usuario
                  {
                      Id = item.Object.Id,
                      IdPersona = item.Object.IdPersona,
                      Nombre = item.Object.Nombre,
                      Nombres = item.Object.Nombres,
                      Apellidos = item.Object.Apellidos,
                      Identificacion = item.Object.Identificacion,
                      Celular=item.Object.Celular,
                      Email = item.Object.Email,
                      Clave = item.Object.Clave,
                      PinDigitos = item.Object.PinDigitos,
                      IdToken = item.Object.IdToken,
                      IdGaleria = item.Object.IdGaleria,
                      Imagen = item.Object.Imagen,
                      IdPerfil = item.Object.IdPerfil,
                      Perfil = item.Object.Perfil,
                      EstaActivo = item.Object.EstaActivo,
                      //Logo = item.Object.Logo
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
        public async Task<Usuario> GetRegistro(Usuario oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("usuarios")
                  .OnceAsync<Usuario>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                            || (a.Email == oRegistro.Email && oRegistro.Email != null && oRegistro.Clave==null)
                                            || (a.Email == oRegistro.Email && oRegistro.Email != null && a.Clave==oRegistro.Clave && oRegistro.Clave != null)
                                            || (a.PinDigitos == oRegistro.PinDigitos && oRegistro.PinDigitos !=null)
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
        public async Task<List<Usuario>> GetRegistrosUsuario(Usuario oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("usuarios")
                      .OnceAsync<Usuario>();
                    return allRegistros.Where(a => (a.IsDeleted == oRegistro.IsDeleted 
                                                  && string.IsNullOrEmpty(oRegistro.valorBusqueda)
                                                  && string.IsNullOrEmpty(oRegistro.Id)
                                                  && string.IsNullOrEmpty(oRegistro.Email)
                                                  && string.IsNullOrEmpty(oRegistro.Clave)
                                                  && string.IsNullOrEmpty(oRegistro.PinDigitos))
                                                || (!string.IsNullOrEmpty(oRegistro.Id) && a.Id == oRegistro.Id)
                                                || (!string.IsNullOrEmpty(oRegistro.Email) && a.Email == oRegistro.Email
                                                    && (string.IsNullOrEmpty(oRegistro.Clave)
                                                        || (!string.IsNullOrEmpty(oRegistro.Clave) && a.Clave == oRegistro.Clave)
                                                        )
                                                    )
                                                || (!string.IsNullOrEmpty(oRegistro.PinDigitos) && a.PinDigitos == oRegistro.PinDigitos )
                                                || (!string.IsNullOrEmpty(oRegistro.valorBusqueda) 
                                                 && (a.Perfil.ToLower().Contains(oRegistro.valorBusqueda) 
                                                    || a.Nombre.ToLower().Contains(oRegistro.valorBusqueda)
                                                    || a.Email.ToLower().Contains(oRegistro.valorBusqueda)
                                                    )
                                                    )
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
        public async Task InsertaRegistro(Usuario iregistro)
        {
            Usuario registro = await GetRegistro(iregistro);
            if (registro != null || iregistro.EsEdicion == 1)
            {
                await UpdateRegistro(iregistro);
            }
            else
            {
                iregistro.CreatedBy = SettingsOnline.oAuthentication != null ? SettingsOnline.oAuthentication.Email : iregistro.Email;
                await AddRegistro(iregistro);
            }
        }
        /// <summary>
        /// Método para adicionar un registro nuevo
        /// </summary>
        /// <param name="iregistro"></param>
        /// <returns></returns>
        public async Task AddRegistro(Usuario iregistro)
        {
            try
            {
                await firebase
                  .Child("usuarios")
                  .Child(SettingsOnline.oAuthentication.LocalId)
                  .PutAsync(new Usuario()
                  {
                      Id = iregistro.Id,                      
                      IdPersona = iregistro.IdPersona,
                      Nombre = iregistro.Nombre,
                      Nombres = iregistro.Nombres,
                      Apellidos = iregistro.Apellidos,
                      Identificacion = iregistro.Identificacion,
                      Celular = iregistro.Celular,
                      Email = iregistro.Email,
                      Clave = iregistro.Clave,
                      PinDigitos = iregistro.PinDigitos,
                      IdToken = iregistro.IdToken,
                      IdGaleria = iregistro.IdGaleria,
                      Imagen = iregistro.Imagen,
                      IdPerfil = iregistro.IdPerfil,
                      Perfil = iregistro.Perfil,
                      EstaActivo = iregistro.EstaActivo,
                      //Logo = iregistro.Logo
                      //Datos de Auditoría
                      CreatedBy = SettingsOnline.oAuthentication != null ? SettingsOnline.oAuthentication.Email : iregistro.Email,
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
        public async Task UpdateRegistro(Usuario iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("usuarios")
              .OnceAsync<Usuario>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("usuarios")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Usuario()
              {
                  Id = toUpdateRegistro.Object.Id,
                  IdPersona = toUpdateRegistro.Object.IdPersona,
                  Nombre = iregistro.Nombre,
                  Nombres = iregistro.Nombres,
                  Apellidos = iregistro.Apellidos,
                  Identificacion = iregistro.Identificacion,
                  Celular = iregistro.Celular,
                  Email = toUpdateRegistro.Object.Email,
                  Clave = iregistro.Clave,
                  PinDigitos = iregistro.PinDigitos,
                  IdToken = iregistro.IdToken,
                  IdGaleria = iregistro.IdGaleria,
                  Imagen = iregistro.Imagen,
                  IdPerfil = iregistro.IdPerfil,
                  Perfil = iregistro.Perfil,
                  EstaActivo = iregistro.EstaActivo,
                  //Logo = iRegistro.Logo
                  //Datos de Auditoría
                  UpdatedBy = SettingsOnline.oAuthentication != null ? SettingsOnline.oAuthentication.Email : iregistro.Email,
                  CreatedBy = toUpdateRegistro.Object.CreatedBy,
                  CreatedDateFormato = toUpdateRegistro.Object.CreatedDateFormato
              });
        }
        /// <summary>
        /// Método para eliminar un registro de manera lógica
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task DeleteRegistro(Usuario oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("usuarios")
              .OnceAsync<Usuario>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("usuarios").Child(toDeleteRegistro.Key).DeleteAsync();

        }
        #endregion 3.Métodos
    }
}
