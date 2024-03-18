using DataModel.DTO.Compras;
using DataModel.Helpers;
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
    public class Firebase_Orden
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
        public Firebase_Orden()
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
        public async Task<List<Orden>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("Orden")
                  .OnceAsync<Orden>()).Select(item => new Orden
                  {
                      Id = item.Object.Id,
                      IdProveedor = item.Object.IdProveedor,
                      NombreProveedor = item.Object.NombreProveedor,
                      IdCliente = item.Object.IdCliente,
                      NombreCliente = item.Object.NombreCliente,
                      Identificacion = item.Object.Identificacion,
                      Direccion = item.Object.Direccion,
                      Telefono = item.Object.Telefono,
                      IdTipoOrden = item.Object.IdTipoOrden,
                      TipoOrden = item.Object.TipoOrden,
                      NumeroOrden = item.Object.NumeroOrden,
                      NumeroOrdenPeriodo = item.Object.NumeroOrdenPeriodo,
                      IdEstado = item.Object.IdEstado,
                      CodigoEstado = item.Object.CodigoEstado,
                      IdCondicion= item.Object.IdCondicion,
                      Condicion = item.Object.Condicion,
                      DiasCredito= item.Object.DiasCredito,
                      ImporteCredito = item.Object.ImporteCredito,
                      Fecha = item.Object.Fecha,
                      Cantidad=item.Object.Cantidad,
                      ValorIncremento = item.Object.ValorIncremento,
                      ValorTotal = item.Object.ValorTotal,
                      IdTipoDescuento = item.Object.IdTipoDescuento,
                      TipoDescuento = item.Object.TipoDescuento,
                      ValorDescuento = item.Object.ValorDescuento,
                      ValorImpuesto = item.Object.ValorImpuesto,
                      ValorFinal = item.Object.ValorFinal,
                      Observaciones = item.Object.Observaciones,                      
                      OrdenDetalleCompraSerializado=item.Object.OrdenDetalleCompraSerializado,
                      OrdenEstadoCompraSerializado=item.Object.OrdenEstadoCompraSerializado,
                      OrdenDetalleCompraRecibidoSerializado = item.Object.OrdenDetalleCompraRecibidoSerializado,
                      OrdenPagoSerializado = item.Object.OrdenPagoSerializado,
                      OrdenTablaPagoSerializado =item.Object.OrdenTablaPagoSerializado,
                      OrdenDetalleCompraEntregadoSerializado=item.Object.OrdenDetalleCompraEntregadoSerializado,
                      OrdenDetalleCompraCanceladoSerializado = item.Object.OrdenDetalleCompraCanceladoSerializado,
                      FacturacionSerializado=item.Object.FacturacionSerializado,
                      NumeroSecuencialFactura=item.Object.NumeroSecuencialFactura,
                      NumeroFactura =item.Object.NumeroFactura,
                      AdjuntoFactura=item.Object.AdjuntoFactura,
                      ValorCancelado=item.Object.ValorCancelado,
                      AdjuntoCancelado= item.Object.AdjuntoCancelado,
                      FechaCancelado = item.Object.FechaCancelado,
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
        public async Task<Orden> GetRegistro(Orden oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("Orden")
                  .OnceAsync<Orden>();
                return allRegistros.Where(a => (!string.IsNullOrEmpty(oRegistro.Id) && a.Id == oRegistro.Id)
                                            || ((!string.IsNullOrEmpty(oRegistro.NumeroOrden) && a.NumeroOrden == oRegistro.NumeroOrden)
                                               && !string.IsNullOrEmpty(oRegistro.TipoOrden) && a.TipoOrden == oRegistro.TipoOrden)
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
        public async Task<List<Orden>> GetRegistrosOrden(Orden oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Orden")
                      .OnceAsync<Orden>();
                    return allRegistros.Where(a => (string.IsNullOrEmpty(oRegistro.valorBusqueda)
                                                    && string.IsNullOrEmpty(oRegistro.TipoOrden)
                                                    && string.IsNullOrEmpty(oRegistro.Identificacion)
                                                    && a.IsDeleted == oRegistro.IsDeleted)                    
                                                || (!string.IsNullOrEmpty(oRegistro.Id) && a.Id == oRegistro.Id)                                                
                                                || (!string.IsNullOrEmpty(oRegistro.NumeroOrden) && a.NumeroOrden == oRegistro.NumeroOrden)
                                                || (
                                                        (  !string.IsNullOrEmpty(oRegistro.TipoOrden) 
                                                        && !string.IsNullOrEmpty(oRegistro.Identificacion)
                                                          && a.TipoOrden == oRegistro.TipoOrden
                                                          && a.Identificacion == oRegistro.Identificacion
                                                        )
                                                    || (!string.IsNullOrEmpty(oRegistro.TipoOrden)
                                                        && string.IsNullOrEmpty(oRegistro.Identificacion)
                                                        && a.TipoOrden == oRegistro.TipoOrden)
                                                    || (!string.IsNullOrEmpty(oRegistro.Identificacion)
                                                        && string.IsNullOrEmpty(oRegistro.TipoOrden)
                                                        && a.Identificacion == oRegistro.Identificacion)
                                                    )                                                   
                                                || (!string.IsNullOrEmpty(oRegistro.valorBusqueda)
                                                 && (a.NumeroOrden.ToLower().Contains(oRegistro.valorBusqueda)
                                                    || a.TipoOrden.ToLower().Contains(oRegistro.valorBusqueda)
                                                    || a.NombreCliente.ToLower().Contains(oRegistro.valorBusqueda)
                                                    || a.NombreProveedor.ToLower().Contains(oRegistro.valorBusqueda)
                                                    || a.Identificacion.ToLower().Contains(oRegistro.valorBusqueda)
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
        /// Método para consultar un registro acorde a período
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<Orden>> GetRegistrosPeriodoOrden(Orden oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Orden")
                      .OnceAsync<Orden>();
                    return allRegistros.Where(a => a.IsDeleted == oRegistro.IsDeleted
                                                && a.TipoOrden==oRegistro.TipoOrden
                                                && a.Fecha >= DateTimeHelpers.GetDate(oRegistro.FechaDesde) && a.Fecha <= DateTimeHelpers.GetDate(oRegistro.FechaHasta)
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
        public async Task<List<Orden>> GetRegistrosPeriodoClienteOrden(Orden oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Orden")
                      .OnceAsync<Orden>();
                    return allRegistros.Where(a => a.IsDeleted == oRegistro.IsDeleted
                                                && a.TipoOrden == oRegistro.TipoOrden
                                                && a.Identificacion==oRegistro.Identificacion
                                                && a.Fecha >= DateTimeHelpers.GetDate(oRegistro.FechaDesde) && a.Fecha <= DateTimeHelpers.GetDate(oRegistro.FechaHasta)
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
        public async Task<List<Orden>> GetRegistrosFechaOrden(Orden oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Orden")
                      .OnceAsync<Orden>();
                    return allRegistros.Where(a => a.IsDeleted == oRegistro.IsDeleted
                                                && a.TipoOrden == oRegistro.TipoOrden
                                                && a.Fecha>= DateTimeHelpers.GetDate(oRegistro.FechaReporte)
                                                && a.Fecha<= DateTimeHelpers.GetDate(oRegistro.FechaReporte.AddDays(1))
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
        /// Consulta de órdenes por Tipo
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<Orden>> GetRegistrosTipoOrden(Orden oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Orden")
                      .OnceAsync<Orden>();
                    return allRegistros.Where(a => a.IsDeleted == oRegistro.IsDeleted
                                                && a.TipoOrden == oRegistro.TipoOrden                                                
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
        public async Task InsertaRegistro(Orden iregistro)
        {
            Orden registro = await GetRegistro(iregistro);
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
        public async Task AddRegistro(Orden iregistro)
        {
            try
            {
                await firebase
                  .Child("Orden")
                  .PostAsync(new Orden()
                  {
                      Id = iregistro.Id,
                      IdProveedor = iregistro.IdProveedor,
                      NombreProveedor = iregistro.NombreProveedor,
                      IdCliente = iregistro.IdCliente,
                      NombreCliente = iregistro.NombreCliente,
                      Identificacion = iregistro.Identificacion,
                      Direccion = iregistro.Direccion,
                      Telefono = iregistro.Telefono,
                      IdTipoOrden = iregistro.IdTipoOrden,
                      TipoOrden = iregistro.TipoOrden,
                      IdCondicion= iregistro.IdCondicion,
                      Condicion = iregistro.Condicion,
                      DiasCredito = iregistro.DiasCredito,
                      ImporteCredito = iregistro.ImporteCredito,
                      NumeroOrden = iregistro.NumeroOrden,
                      NumeroOrdenPeriodo = iregistro.NumeroOrdenPeriodo,
                      IdEstado = iregistro.IdEstado,
                      CodigoEstado = iregistro.CodigoEstado,
                      Fecha = iregistro.Fecha,
                      Cantidad=iregistro.Cantidad,
                      ValorIncremento = iregistro.ValorIncremento,
                      ValorTotal = iregistro.ValorTotal,
                      IdTipoDescuento = iregistro.IdTipoDescuento,
                      TipoDescuento = iregistro.TipoDescuento,
                      ValorDescuento = iregistro.ValorDescuento,
                      ValorImpuesto = iregistro.ValorImpuesto,
                      ValorFinal = iregistro.ValorFinal,
                      Observaciones = iregistro.Observaciones,         
                      OrdenEstadoCompraSerializado=iregistro.OrdenEstadoCompraSerializado,
                      OrdenDetalleCompraSerializado = iregistro.OrdenDetalleCompraSerializado,
                      OrdenDetalleCompraRecibidoSerializado = iregistro.OrdenDetalleCompraRecibidoSerializado,
                      OrdenPagoSerializado = iregistro.OrdenPagoSerializado,
                      OrdenTablaPagoSerializado = iregistro.OrdenTablaPagoSerializado,
                      OrdenDetalleCompraCanceladoSerializado = iregistro.OrdenDetalleCompraCanceladoSerializado,
                      OrdenDetalleCompraEntregadoSerializado = iregistro.OrdenDetalleCompraEntregadoSerializado,
                      FacturacionSerializado= iregistro.FacturacionSerializado,
                      NumeroSecuencialFactura = iregistro.NumeroSecuencialFactura,
                      NumeroFactura = iregistro.NumeroFactura,
                      AdjuntoFactura = iregistro.AdjuntoFactura,
                      ValorCancelado = iregistro.ValorCancelado,
                      AdjuntoCancelado = iregistro.AdjuntoCancelado,
                      FechaCancelado = iregistro.FechaCancelado,
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
        public async Task UpdateRegistro(Orden iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("Orden")
              .OnceAsync<Orden>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("Orden")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Orden()
              {
                  Id = toUpdateRegistro.Object.Id,
                  IdCliente = toUpdateRegistro.Object.IdCliente,
                  NombreCliente = toUpdateRegistro.Object.NombreCliente,
                  Direccion=iregistro.Direccion,
                  Telefono=iregistro.Telefono,
                  IdProveedor = toUpdateRegistro.Object.IdProveedor,
                  NombreProveedor = toUpdateRegistro.Object.NombreProveedor,
                  Identificacion = toUpdateRegistro.Object.Identificacion,
                  IdTipoOrden = toUpdateRegistro.Object.IdTipoOrden,
                  TipoOrden = toUpdateRegistro.Object.TipoOrden,
                  IdCondicion = toUpdateRegistro.Object.IdCondicion,
                  Condicion = toUpdateRegistro.Object.Condicion,
                  DiasCredito = toUpdateRegistro.Object.DiasCredito,
                  ImporteCredito = toUpdateRegistro.Object.ImporteCredito,
                  NumeroOrden = toUpdateRegistro.Object.NumeroOrden,
                  NumeroOrdenPeriodo = toUpdateRegistro.Object.NumeroOrdenPeriodo,
                  IdEstado = iregistro.IdEstado,
                  CodigoEstado = iregistro.CodigoEstado,
                  Fecha = toUpdateRegistro.Object.Fecha,
                  Cantidad= iregistro.Cantidad,
                  ValorIncremento = iregistro.ValorIncremento,
                  ValorTotal = iregistro.ValorTotal,
                  IdTipoDescuento = iregistro.IdTipoDescuento,
                  TipoDescuento = iregistro.TipoDescuento,
                  ValorDescuento = iregistro.ValorDescuento,
                  ValorImpuesto = iregistro.ValorImpuesto,
                  ValorFinal = iregistro.ValorFinal,
                  Observaciones = iregistro.Observaciones,
                  OrdenDetalleCompraSerializado = iregistro.OrdenDetalleCompraSerializado,
                  OrdenEstadoCompraSerializado = iregistro.OrdenEstadoCompraSerializado,
                  OrdenDetalleCompraRecibidoSerializado = iregistro.OrdenDetalleCompraRecibidoSerializado,
                  OrdenPagoSerializado = iregistro.OrdenPagoSerializado,
                  OrdenTablaPagoSerializado = iregistro.OrdenTablaPagoSerializado,
                  OrdenDetalleCompraCanceladoSerializado = iregistro.OrdenDetalleCompraCanceladoSerializado,
                  OrdenDetalleCompraEntregadoSerializado = iregistro.OrdenDetalleCompraEntregadoSerializado,
                  FacturacionSerializado = iregistro.FacturacionSerializado,
                  NumeroSecuencialFactura = iregistro.NumeroSecuencialFactura,
                  NumeroFactura = iregistro.NumeroFactura,
                  AdjuntoFactura = iregistro.AdjuntoFactura,
                  ValorCancelado = iregistro.ValorCancelado,
                  AdjuntoCancelado = iregistro.AdjuntoCancelado,
                  FechaCancelado = iregistro.FechaCancelado,
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
        public async Task DeleteRegistro(Orden oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("Orden")
              .OnceAsync<Orden>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("Orden").Child(toDeleteRegistro.Key).DeleteAsync();

        }

        public Stream ByteArrayToStream(byte[] input)
        {
            return new MemoryStream(input);
        }
        #endregion 3.Métodos
    }
}
