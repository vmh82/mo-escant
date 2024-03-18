using DataModel.DTO.Clientes;
using Firebase.Database;
using Firebase.Database.Query;
using ManijodaServicios.AppSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ManijodaServicios.Online.Firebase.Clientes
{
    public class Firebase_Persona
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
        public Firebase_Persona()
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
        public async Task<List<Persona>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("Persona")
                  .OnceAsync<Persona>()).Select(item => new Persona
                  {
                      Id = item.Object.Id,
                      Identificacion = item.Object.Identificacion,
                      EsNatural = item.Object.EsNatural
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
        public async Task<Persona> GetRegistro(Persona oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("Persona")
                  .OnceAsync<Persona>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)                                            
                                            || (a.Identificacion == oRegistro.Identificacion && oRegistro.Identificacion != null)).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Método para consultar un registro acorde a filtros
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<Persona>> GetRegistrosPersona(Persona oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Persona")
                      .OnceAsync<Persona>();
                    return allRegistros.Where(a => (a.IsDeleted == oRegistro.IsDeleted 
                                                    && string.IsNullOrEmpty(oRegistro.valorBusqueda)
                                                    && string.IsNullOrEmpty(oRegistro.Id)
                                                    && string.IsNullOrEmpty(oRegistro.Identificacion))
                                                || (a.Id == oRegistro.Id && !string.IsNullOrEmpty(oRegistro.Id))
                                                || (a.Identificacion == oRegistro.Identificacion && !string.IsNullOrEmpty(oRegistro.Identificacion))
                                                || (a.Identificacion.ToLower().Contains(oRegistro.valorBusqueda) && !string.IsNullOrEmpty(oRegistro.valorBusqueda))                                                
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
        public async Task InsertaRegistro(Persona iregistro)
        {
            Persona registro = await GetRegistro(iregistro);
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
        public async Task AddRegistro(Persona iregistro)
        {
            try
            {
                await firebase
                  .Child("Persona")
                  .PostAsync(new Persona()
                  {
                      Id = iregistro.Id,
                      Identificacion = iregistro.Identificacion,
                      EsNatural = iregistro.EsNatural,
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
        public async Task UpdateRegistro(Persona iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("Persona")
              .OnceAsync<Persona>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("Persona")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Persona()
              {
                  Id = toUpdateRegistro.Object.Id,
                  Identificacion = toUpdateRegistro.Object.Identificacion,
                  EsNatural = iregistro.EsNatural,
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
        public async Task DeleteRegistro(Persona oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("Persona")
              .OnceAsync<Persona>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("Persona").Child(toDeleteRegistro.Key).DeleteAsync();

        }
        #endregion 3.Métodos

    }
}
