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
    public class Firebase_OrdenDetalleCompra
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
        public Firebase_OrdenDetalleCompra()
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
        public async Task<List<OrdenDetalleCompra>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("OrdenDetalleCompra")
                  .OnceAsync<OrdenDetalleCompra>()).Select(item => new OrdenDetalleCompra
                  {
                      Id = item.Object.Id,
                      IdOrden = item.Object.IdOrden,
                      IdItem = item.Object.IdItem,
                      NombreItem = item.Object.NombreItem,
                      IdUnidad = item.Object.IdUnidad,
                      Unidad = item.Object.Unidad,
                      Cantidad = item.Object.Cantidad,
                      ValorUnitario = item.Object.ValorUnitario,
                      ValorCompra = item.Object.ValorCompra,
                      ValorDescuento = item.Object.ValorDescuento,
                      ValorFinal = item.Object.ValorFinal,
                      FechaVencimiento = item.Object.FechaVencimiento,                      
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
        public async Task<OrdenDetalleCompra> GetRegistro(OrdenDetalleCompra oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("OrdenDetalleCompra")
                  .OnceAsync<OrdenDetalleCompra>();
                return allRegistros.Where(a => (!string.IsNullOrEmpty(oRegistro.Id) && a.Id == oRegistro.Id)
                                            || (!string.IsNullOrEmpty(oRegistro.IdOrden) && a.IdOrden == oRegistro.IdOrden)).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Método para consultar un registro acorde a filtros
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<OrdenDetalleCompra>> GetRegistrosOrdenDetalleCompra(OrdenDetalleCompra oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("OrdenDetalleCompra")
                      .OnceAsync<OrdenDetalleCompra>();
                    return allRegistros.Where(a => (string.IsNullOrEmpty(oRegistro.valorBusqueda)
                                                    && a.IsDeleted == oRegistro.IsDeleted)
                                                || (!string.IsNullOrEmpty(oRegistro.Id) && a.Id == oRegistro.Id)
                                                || (!string.IsNullOrEmpty(oRegistro.IdOrden) && a.IdOrden == oRegistro.IdOrden)
                                                || (!string.IsNullOrEmpty(oRegistro.valorBusqueda)
                                                 && (a.NombreItem.ToLower().Contains(oRegistro.valorBusqueda)                                                    
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
        public async Task InsertaRegistro(OrdenDetalleCompra iregistro)
        {
            OrdenDetalleCompra registro = await GetRegistro(iregistro);
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
        public async Task AddRegistro(OrdenDetalleCompra iregistro)
        {
            try
            {
                await firebase
                  .Child("OrdenDetalleCompra")
                  .PostAsync(new OrdenDetalleCompra()
                  {
                      Id = iregistro.Id,
                      IdOrden = iregistro.IdOrden,
                      IdItem = iregistro.IdItem,
                      NombreItem = iregistro.NombreItem,
                      IdUnidad = iregistro.IdUnidad,
                      Unidad = iregistro.Unidad,
                      Cantidad = iregistro.Cantidad,
                      ValorUnitario = iregistro.ValorUnitario,
                      ValorCompra = iregistro.ValorCompra,
                      ValorDescuento = iregistro.ValorDescuento,
                      ValorFinal = iregistro.ValorFinal,
                      FechaVencimiento = iregistro.FechaVencimiento,
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
        public async Task UpdateRegistro(OrdenDetalleCompra iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("OrdenDetalleCompra")
              .OnceAsync<OrdenDetalleCompra>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("OrdenDetalleCompra")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new OrdenDetalleCompra()
              {
                  Id = toUpdateRegistro.Object.Id,
                  IdOrden = toUpdateRegistro.Object.IdOrden,
                  IdItem = toUpdateRegistro.Object.IdItem,
                  NombreItem = toUpdateRegistro.Object.NombreItem,
                  IdUnidad = toUpdateRegistro.Object.IdUnidad,
                  Unidad = toUpdateRegistro.Object.Unidad,
                  Cantidad = iregistro.Cantidad,
                  ValorUnitario = toUpdateRegistro.Object.ValorUnitario,
                  ValorCompra = iregistro.ValorCompra,
                  ValorDescuento = iregistro.ValorDescuento,
                  ValorFinal = iregistro.ValorFinal,
                  FechaVencimiento = iregistro.FechaVencimiento,
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
        public async Task DeleteRegistro(OrdenDetalleCompra oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("OrdenDetalleCompra")
              .OnceAsync<OrdenDetalleCompra>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("OrdenDetalleCompra").Child(toDeleteRegistro.Key).DeleteAsync();

        }

        public Stream ByteArrayToStream(byte[] input)
        {
            return new MemoryStream(input);
        }
        #endregion 3.Métodos
    }
}
