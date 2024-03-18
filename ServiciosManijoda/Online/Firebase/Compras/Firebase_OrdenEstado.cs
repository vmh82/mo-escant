using DataModel.DTO.Compras;
using Firebase.Database;
using Firebase.Database.Query;
using ManijodaServicios.AppSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Online.Firebase.Compras
{
    public class Firebase_OrdenEstado
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
        public Firebase_OrdenEstado()
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
        public async Task<List<OrdenEstado>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("OrdenEstado")
                  .OnceAsync<OrdenEstado>()).Select(item => new OrdenEstado
                  {
                      Id = item.Object.Id,
                      IdOrden = item.Object.IdOrden,
                      NumeroOrden = item.Object.NumeroOrden,
                      IdEstado = item.Object.IdEstado,
                      Estado = item.Object.Estado,
                      FechaProceso = item.Object.FechaProceso,                      
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
        public async Task<OrdenEstado> GetRegistro(OrdenEstado oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("OrdenEstado")
                  .OnceAsync<OrdenEstado>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                            || (a.NumeroOrden == oRegistro.NumeroOrden && oRegistro.NumeroOrden != null)).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Método para consultar un registro acorde a filtros
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<OrdenEstado>> GetRegistrosOrdenEstado(OrdenEstado oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("OrdenEstado")
                      .OnceAsync<OrdenEstado>();
                    return allRegistros.Where(a => (string.IsNullOrEmpty(oRegistro.valorBusqueda)
                                                    && a.IsDeleted == oRegistro.IsDeleted)
                                                || (!string.IsNullOrEmpty(oRegistro.Id) && a.Id == oRegistro.Id)
                                                || (!string.IsNullOrEmpty(oRegistro.NumeroOrden) && a.NumeroOrden == oRegistro.NumeroOrden)                                                
                                                || (!string.IsNullOrEmpty(oRegistro.valorBusqueda)
                                                 && (a.NumeroOrden.ToLower().Contains(oRegistro.valorBusqueda)                                                    
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
        public async Task InsertaRegistro(OrdenEstado iregistro)
        {
            OrdenEstado registro = await GetRegistro(iregistro);
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
        public async Task AddRegistro(OrdenEstado iregistro)
        {
            try
            {
                await firebase
                  .Child("OrdenEstado")
                  .PostAsync(new OrdenEstado()
                  {
                      Id = iregistro.Id,
                      IdOrden = iregistro.IdOrden,
                      NumeroOrden = iregistro.NumeroOrden,
                      IdEstado = iregistro.IdEstado,
                      Estado = iregistro.Estado,
                      FechaProceso = iregistro.FechaProceso,
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
        public async Task UpdateRegistro(OrdenEstado iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("OrdenEstado")
              .OnceAsync<OrdenEstado>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("OrdenEstado")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new OrdenEstado()
              {
                  Id = toUpdateRegistro.Object.Id,
                  IdOrden = toUpdateRegistro.Object.IdOrden,
                  NumeroOrden = toUpdateRegistro.Object.NumeroOrden,
                  IdEstado = toUpdateRegistro.Object.IdEstado,
                  Estado = toUpdateRegistro.Object.Estado,
                  FechaProceso = iregistro.FechaProceso,
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
        public async Task DeleteRegistro(OrdenEstado oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("OrdenEstado")
              .OnceAsync<OrdenEstado>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("OrdenEstado").Child(toDeleteRegistro.Key).DeleteAsync();

        }

        public Stream ByteArrayToStream(byte[] input)
        {
            return new MemoryStream(input);
        }
        #endregion 3.Métodos
    }
}
