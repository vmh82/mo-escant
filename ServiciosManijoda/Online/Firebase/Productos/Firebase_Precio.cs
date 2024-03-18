using DataModel.DTO.Productos;
using Firebase.Database;
using Firebase.Database.Query;
using ManijodaServicios.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManijodaServicios.Online.Firebase.Productos
{
    public class Firebase_Precio
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
        public Firebase_Precio()
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
        public async Task<List<Precio>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("Precio")
                  .OnceAsync<Precio>()).Select(item => new Precio
                  {
                      Id = item.Object.Id,
                      IdItem = item.Object.IdItem,
                      CodigoItem = item.Object.CodigoItem,
                      NombreItem = item.Object.NombreItem,
                      IdUnidad = item.Object.IdUnidad,
                      Unidad = item.Object.Unidad,
                      ValorUnidadOriginal = item.Object.ValorUnidadOriginal,
                      IdUnidadPadre = item.Object.IdUnidadPadre,
                      IdUnidadConvertida = item.Object.IdUnidadConvertida,
                      UnidadConvertida = item.Object.UnidadConvertida,
                      ValorConversion = item.Object.ValorConversion,
                      PrecioVentaSugerido1 = item.Object.PrecioVentaSugerido1,
                      PrecioVentaSugerido2 = item.Object.PrecioVentaSugerido2,
                      PrecioVenta = item.Object.PrecioVenta,
                      FechaDesde = item.Object.FechaDesde,                      
                      FechaHasta = item.Object.FechaHasta,
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
        public async Task<Precio> GetRegistro(Precio oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("Precio")
                  .OnceAsync<Precio>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                            || (
                                               (a.CodigoItem == oRegistro.CodigoItem && oRegistro.CodigoItem != null)
                                            && (a.NombreItem == oRegistro.NombreItem && oRegistro.NombreItem != null)
                                            && (a.IdUnidadConvertida == oRegistro.IdUnidadConvertida && oRegistro.IdUnidadConvertida != null)
                                               )).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Método para consultar un registro acorde a filtros
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<Precio>> GetRegistrosPrecio(Precio oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Precio")
                      .OnceAsync<Precio>();
                    return allRegistros.Where(a => (a.IsDeleted == oRegistro.IsDeleted && oRegistro.valorBusqueda == null 
                                                    && string.IsNullOrEmpty(oRegistro.IdItem)
                                                    && oRegistro.ValorConversion==0)
                                                || (a.Id == oRegistro.Id && oRegistro.Id != null)
                                                || (a.IdItem == oRegistro.IdItem && !string.IsNullOrEmpty(oRegistro.IdItem)
                                                        && (
                                                            string.IsNullOrEmpty(oRegistro.IdUnidad)
                                                        || (!string.IsNullOrEmpty(oRegistro.IdUnidad)
                                                            && a.IdUnidad==oRegistro.IdUnidad)
                                                           )
                                                    )
                                                || (a.ValorConversion == oRegistro.ValorConversion && oRegistro.ValorConversion ==1)
                                                //|| (a.NombreItem == oRegistro.NombreItem && !string.IsNullOrEmpty(oRegistro.NombreItem))
                                                //|| (a.CodigoItem.ToLower().Contains(oRegistro.valorBusqueda) && oRegistro.valorBusqueda != null)
                                                //|| (a.NombreItem.ToLower().Contains(oRegistro.valorBusqueda) && oRegistro.valorBusqueda != null) 
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
        public async Task InsertaRegistro(Precio iregistro)
        {
            Precio registro = await GetRegistro(iregistro);
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
        public async Task AddRegistro(Precio iregistro)
        {
            try
            {
                await firebase
                  .Child("Precio")
                  .PostAsync(new Precio()
                  {
                      Id = iregistro.Id,
                      IdItem = iregistro.IdItem,
                      CodigoItem = iregistro.CodigoItem,
                      NombreItem = iregistro.NombreItem,
                      IdUnidad = iregistro.IdUnidad,
                      Unidad = iregistro.Unidad,
                      ValorUnidadOriginal = iregistro.ValorUnidadOriginal,
                      IdUnidadPadre = iregistro.IdUnidadPadre,
                      IdUnidadConvertida = iregistro.IdUnidadConvertida,
                      UnidadConvertida = iregistro.UnidadConvertida,
                      ValorConversion = iregistro.ValorConversion,
                      PrecioVentaSugerido1 = iregistro.PrecioVentaSugerido1,
                      PrecioVentaSugerido2 = iregistro.PrecioVentaSugerido2,
                      PrecioVenta = iregistro.PrecioVenta,
                      FechaDesde = iregistro.FechaDesde,
                      FechaHasta = iregistro.FechaHasta,                      
                      EstaActivo = iregistro.EstaActivo,                      
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
        public async Task UpdateRegistro(Precio iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("Precio")
              .OnceAsync<Precio>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("Precio")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Precio()
              {
                  Id = toUpdateRegistro.Object.Id,
                  IdItem = toUpdateRegistro.Object.IdItem,
                  CodigoItem = toUpdateRegistro.Object.CodigoItem,
                  NombreItem = toUpdateRegistro.Object.NombreItem,
                  IdUnidad = toUpdateRegistro.Object.IdUnidad,
                  Unidad = toUpdateRegistro.Object.Unidad,
                  ValorUnidadOriginal = toUpdateRegistro.Object.ValorUnidadOriginal,
                  IdUnidadPadre = iregistro.IdUnidadPadre,
                  IdUnidadConvertida = toUpdateRegistro.Object.IdUnidadConvertida,
                  UnidadConvertida = toUpdateRegistro.Object.UnidadConvertida,
                  ValorConversion = toUpdateRegistro.Object.ValorConversion,
                  PrecioVentaSugerido1 = iregistro.PrecioVentaSugerido1,
                  PrecioVentaSugerido2 = iregistro.PrecioVentaSugerido2,
                  PrecioVenta = iregistro.PrecioVenta,
                  FechaDesde = iregistro.FechaDesde,
                  FechaHasta = iregistro.FechaHasta,
                  EstaActivo = iregistro.EstaActivo,                  
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
        public async Task DeleteRegistro(Precio oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("Precio")
              .OnceAsync<Precio>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("Precio").Child(toDeleteRegistro.Key).DeleteAsync();

        }
        #endregion 3.Métodos


    }
}
