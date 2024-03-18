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
    public class Firebase_Item
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
        public Firebase_Item()
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
        public async Task<List<Item>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("Item")
                  .OnceAsync<Item>()).Select(item => new Item
                  {
                      Id = item.Object.Id,
                      EsServicio = item.Object.EsServicio,
                      Codigo = item.Object.Codigo,
                      Nombre = item.Object.Nombre,
                      IdMarca = item.Object.IdMarca,
                      Marca = item.Object.Marca,
                      IdCategoria = item.Object.IdCategoria,
                      Categoria = item.Object.Categoria,
                      IdUnidad = item.Object.IdUnidad,
                      Unidad = item.Object.Unidad,
                      IdTipoImpuesto = item.Object.IdTipoImpuesto,
                      TipoImpuesto = item.Object.TipoImpuesto,
                      PorcentajeImpuesto= item.Object.PorcentajeImpuesto,
                      ValorImpuesto = item.Object.ValorImpuesto,
                      IdTipoAplicacionImpuesto = item.Object.IdTipoAplicacionImpuesto,
                      TipoAplicacionImpuesto = item.Object.TipoAplicacionImpuesto,
                      IdPeriodoIncremento = item.Object.IdPeriodoIncremento,
                      PeriodoIncremento = item.Object.PeriodoIncremento,
                      ValorPeriodoIncremento = item.Object.ValorPeriodoIncremento,
                      IdPeriodoServicio = item.Object.IdPeriodoServicio,
                      PeriodoServicio = item.Object.PeriodoServicio,
                      CantidadPeriodoServicio = item.Object.CantidadPeriodoServicio,
                      ValorIncremento = item.Object.ValorIncremento,
                      ValorUnidadOriginal = item.Object.ValorUnidadOriginal,
                      EsParticionable = item.Object.EsParticionable,
                      IdGaleria = item.Object.IdGaleria,
                      Imagen = item.Object.Imagen,
                      GaleriaImagen = item.Object.GaleriaImagen,
                      StockMinimo = item.Object.StockMinimo,
                      Cantidad = item.Object.Cantidad,                      
                      SaldoExistente = item.Object.SaldoExistente,
                      PorcentajeRentabilidad = item.Object.PorcentajeRentabilidad,
                      PorcentajeGastoOperativo = item.Object.PorcentajeGastoOperativo,
                      PrecioBase = item.Object.PrecioBase,
                      PrecioCompra = item.Object.PrecioCompra,
                      PrecioVenta = item.Object.PrecioVenta,
                      PrecioVentaImpuesto = item.Object.PrecioVentaImpuesto,
                      ValorGastoOperativo = item.Object.ValorGastoOperativo,
                      ValorRentabilidad = item.Object.ValorRentabilidad,
                      ValorUtilidad = item.Object.ValorUtilidad,
                      Descripcion = item.Object.Descripcion,
                      CantidadInventarioInicial = item.Object.CantidadInventarioInicial,
                      CantidadCompra = item.Object.CantidadCompra,
                      CantidadEntrada = item.Object.CantidadEntrada,
                      CantidadInventarioConcilia = item.Object.CantidadInventarioConcilia,
                      CantidadDevuelta = item.Object.CantidadDevuelta,
                      CantidadVenta = item.Object.CantidadVenta,
                      CantidadOtros = item.Object.CantidadOtros,
                      CantidadSalida = item.Object.CantidadSalida,
                      CantidadSaldoFinal = item.Object.CantidadSaldoFinal,
                      ItemStockRegistradoSerializado=item.Object.ItemStockRegistradoSerializado,
                      PoseeStock=item.Object.PoseeStock,
                      PoseeInventarioInicial=item.Object.PoseeInventarioInicial
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
        public async Task<Item> GetRegistro(Item oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("Item")
                  .OnceAsync<Item>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                            || (a.Codigo == oRegistro.Codigo && oRegistro.Codigo != null)
                                            || (a.Nombre == oRegistro.Nombre && oRegistro.Nombre != null)).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Método para consultar un registro acorde a filtros
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<Item>> GetRegistrosItem(Item oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Item")
                      .OnceAsync<Item>();
                    return allRegistros.Where(a => (a.IsDeleted == oRegistro.IsDeleted 
                                                    && string.IsNullOrEmpty(oRegistro.valorBusqueda)
                                                    && string.IsNullOrEmpty(oRegistro.Id)
                                                    && ((oRegistro.PoseeStock>=0 && a.PoseeStock==oRegistro.PoseeStock)
                                                    || oRegistro.PoseeStock == -1)
                                                    && ((oRegistro.PoseeInventarioInicial>=0 && a.PoseeInventarioInicial==oRegistro.PoseeInventarioInicial)
                                                    || oRegistro.PoseeInventarioInicial == -1)
                                                    )
                                                || (a.Id == oRegistro.Id && !string.IsNullOrEmpty(oRegistro.Id))
                                                || (a.Codigo == oRegistro.Codigo && !string.IsNullOrEmpty(oRegistro.Codigo))
                                                || (a.Nombre == oRegistro.Nombre && !string.IsNullOrEmpty(oRegistro.Nombre))
                                                || (!string.IsNullOrEmpty(oRegistro.valorBusqueda) && 
                                                    (  a.Codigo.ToLower().Contains(oRegistro.valorBusqueda)
                                                    || a.Marca.ToLower().Contains(oRegistro.valorBusqueda)
                                                    || a.Nombre.ToLower().Contains(oRegistro.valorBusqueda)
                                                    )
                                                )).ToList();
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
        /// Selección de top Item
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<Item>> GetRegistrosTopItem(Item oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Item")
                      .OnceAsync<Item>();
                    return allRegistros.Where(a => a.IsDeleted == oRegistro.IsDeleted
                                                && a.CantidadVenta>0    
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
        public async Task<List<Item>> GetRegistrosSaldosItem(Item oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Item")
                      .OnceAsync<Item>();
                    return allRegistros.Where(a => a.IsDeleted == oRegistro.IsDeleted
                                                && a.CantidadSaldoFinal > 0
                                                && a.EsServicio == 0
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
        public async Task InsertaRegistro(Item iregistro)
        {
            Item registro = await GetRegistro(iregistro);
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
        public async Task AddRegistro(Item iregistro)
        {
            try
            {
                await firebase
                  .Child("Item")
                  .PostAsync(new Item()
                  {
                      Id = iregistro.Id,
                      EsServicio = iregistro.EsServicio,
                      Codigo = iregistro.Codigo,
                      Nombre = iregistro.Nombre,
                      IdMarca = iregistro.IdMarca,
                      Marca = iregistro.Marca,
                      IdCategoria = iregistro.IdCategoria,
                      Categoria = iregistro.Categoria,
                      IdUnidad = iregistro.IdUnidad,
                      Unidad = iregistro.Unidad,
                      IdTipoImpuesto = iregistro.IdTipoImpuesto,
                      TipoImpuesto = iregistro.TipoImpuesto,
                      PorcentajeImpuesto=iregistro.PorcentajeImpuesto,
                      IdTipoAplicacionImpuesto = iregistro.IdTipoAplicacionImpuesto,
                      TipoAplicacionImpuesto = iregistro.TipoAplicacionImpuesto,
                      ValorImpuesto = iregistro.ValorImpuesto,
                      IdPeriodoIncremento = iregistro.IdPeriodoIncremento,
                      PeriodoIncremento = iregistro.PeriodoIncremento,
                      ValorPeriodoIncremento = iregistro.ValorPeriodoIncremento,
                      IdPeriodoServicio = iregistro.IdPeriodoServicio,
                      PeriodoServicio = iregistro.PeriodoServicio,
                      CantidadPeriodoServicio = iregistro.CantidadPeriodoServicio,
                      ValorIncremento = iregistro.ValorIncremento,
                      ValorUnidadOriginal = iregistro.ValorUnidadOriginal,
                      EsParticionable = iregistro.EsParticionable,
                      IdGaleria = iregistro.IdGaleria,
                      Imagen = iregistro.Imagen,
                      GaleriaImagen = iregistro.GaleriaImagen,
                      StockMinimo = iregistro.StockMinimo,
                      PorcentajeRentabilidad=iregistro.PorcentajeRentabilidad,
                      PorcentajeGastoOperativo = iregistro.PorcentajeGastoOperativo,
                      PrecioCompra = iregistro.PrecioCompra,
                      PrecioVenta = iregistro.PrecioVenta,
                      PrecioBase = iregistro.PrecioBase,
                      PrecioVentaImpuesto = iregistro.PrecioVentaImpuesto,
                      ValorGastoOperativo = iregistro.ValorGastoOperativo,
                      ValorRentabilidad = iregistro.ValorRentabilidad,
                      ValorUtilidad = iregistro.ValorUtilidad,
                      Descripcion = iregistro.Descripcion,    
                      CantidadInventarioInicial=iregistro.CantidadInventarioInicial,
                      CantidadCompra = iregistro.CantidadCompra,
                      CantidadEntrada = iregistro.CantidadEntrada,
                      CantidadInventarioConcilia = iregistro.CantidadInventarioConcilia,
                      CantidadDevuelta = iregistro.CantidadDevuelta,
                      CantidadVenta = iregistro.CantidadVenta,
                      CantidadOtros = iregistro.CantidadOtros,
                      CantidadSalida = iregistro.CantidadSalida,
                      CantidadSaldoFinal = iregistro.CantidadSaldoFinal,
                      ItemStockRegistradoSerializado = iregistro.ItemStockRegistradoSerializado,
                      PoseeStock=iregistro.PoseeStock,
                      PoseeInventarioInicial=iregistro.PoseeInventarioInicial,
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
        public async Task UpdateRegistro(Item iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("Item")
              .OnceAsync<Item>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("Item")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Item()
              {
                  Id = toUpdateRegistro.Object.Id,
                  EsServicio = toUpdateRegistro.Object.EsServicio,                  
                  Codigo = toUpdateRegistro.Object.Codigo,
                  Nombre = toUpdateRegistro.Object.Nombre,
                  IdMarca = toUpdateRegistro.Object.IdMarca,
                  Marca = toUpdateRegistro.Object.Marca,
                  IdCategoria = toUpdateRegistro.Object.IdCategoria,
                  Categoria = toUpdateRegistro.Object.Categoria,
                  IdUnidad = toUpdateRegistro.Object.IdUnidad,
                  Unidad = toUpdateRegistro.Object.Unidad,                  
                  IdTipoImpuesto = iregistro.IdTipoImpuesto,
                  TipoImpuesto = iregistro.TipoImpuesto,
                  PorcentajeImpuesto = iregistro.PorcentajeImpuesto,
                  ValorImpuesto = iregistro.ValorImpuesto,
                  IdTipoAplicacionImpuesto = iregistro.IdTipoAplicacionImpuesto,
                  TipoAplicacionImpuesto = iregistro.TipoAplicacionImpuesto,
                  IdPeriodoIncremento = iregistro.IdPeriodoIncremento,
                  PeriodoIncremento = iregistro.PeriodoIncremento,
                  ValorPeriodoIncremento = iregistro.ValorPeriodoIncremento,
                  IdPeriodoServicio = iregistro.IdPeriodoServicio,
                  PeriodoServicio = iregistro.PeriodoServicio,
                  CantidadPeriodoServicio = iregistro.CantidadPeriodoServicio,
                  ValorIncremento = iregistro.ValorIncremento,
                  ValorUnidadOriginal = toUpdateRegistro.Object.ValorUnidadOriginal,
                  EsParticionable = iregistro.EsParticionable,
                  IdGaleria = iregistro.IdGaleria,
                  Imagen = iregistro.Imagen,
                  GaleriaImagen = iregistro.GaleriaImagen,
                  StockMinimo = iregistro.StockMinimo,
                  Descripcion = iregistro.Descripcion,
                  Cantidad = iregistro.Cantidad,
                  CantidadInventarioInicial = iregistro.CantidadInventarioInicial,
                  CantidadCompra = iregistro.CantidadCompra,
                  CantidadEntrada = iregistro.CantidadEntrada,
                  CantidadInventarioConcilia = iregistro.CantidadInventarioConcilia,
                  CantidadDevuelta = iregistro.CantidadDevuelta,
                  CantidadVenta = iregistro.CantidadVenta,
                  CantidadOtros = iregistro.CantidadOtros,
                  CantidadSalida = iregistro.CantidadSalida,
                  CantidadSaldoFinal = iregistro.CantidadSaldoFinal,                  
                  SaldoExistente = iregistro.SaldoExistente,
                  PorcentajeRentabilidad = iregistro.PorcentajeRentabilidad,
                  PorcentajeGastoOperativo = iregistro.PorcentajeGastoOperativo,
                  PrecioCompra = iregistro.PrecioCompra,
                  PrecioVenta = iregistro.PrecioVenta,
                  PrecioBase = iregistro.PrecioBase,
                  PrecioVentaImpuesto = iregistro.PrecioVentaImpuesto,
                  ValorGastoOperativo = iregistro.ValorGastoOperativo,
                  ValorRentabilidad = iregistro.ValorRentabilidad,
                  ValorUtilidad = iregistro.ValorUtilidad,
                  ItemStockRegistradoSerializado = iregistro.ItemStockRegistradoSerializado,
                  PoseeStock = toUpdateRegistro.Object.PoseeStock,
                  PoseeInventarioInicial = iregistro.PoseeInventarioInicial,
                  //Logo = iRegistro.Logo
                  //Datos de Auditoría
                  UpdatedBy = SettingsOnline.oAuthentication.Email,
                  CreatedBy = toUpdateRegistro.Object.CreatedBy,
                  CreatedDateFormato = toUpdateRegistro.Object.CreatedDateFormato
              });
        }
        /// <summary>
        /// Método para almacenar los precios en el ítem
        /// </summary>
        /// <param name="iregistro"></param>
        /// <returns></returns>
        public async Task UpdateRegistroPrecio(Item iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("Item")
              .OnceAsync<Item>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("Item")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Item()
              {
                  Id = toUpdateRegistro.Object.Id,
                  EsServicio = toUpdateRegistro.Object.EsServicio,
                  Codigo = toUpdateRegistro.Object.Codigo,
                  Nombre = toUpdateRegistro.Object.Nombre,
                  IdMarca = toUpdateRegistro.Object.IdMarca,
                  Marca = toUpdateRegistro.Object.Marca,
                  IdCategoria = toUpdateRegistro.Object.IdCategoria,
                  Categoria = toUpdateRegistro.Object.Categoria,
                  IdUnidad = toUpdateRegistro.Object.IdUnidad,
                  Unidad = toUpdateRegistro.Object.Unidad,
                  IdTipoImpuesto = toUpdateRegistro.Object.IdTipoImpuesto,
                  TipoImpuesto = toUpdateRegistro.Object.TipoImpuesto,
                  PorcentajeImpuesto = toUpdateRegistro.Object.PorcentajeImpuesto,
                  ValorImpuesto = toUpdateRegistro.Object.ValorImpuesto,
                  IdTipoAplicacionImpuesto = toUpdateRegistro.Object.IdTipoAplicacionImpuesto,
                  TipoAplicacionImpuesto = toUpdateRegistro.Object.TipoAplicacionImpuesto,
                  IdPeriodoIncremento = toUpdateRegistro.Object.IdPeriodoIncremento,
                  PeriodoIncremento = toUpdateRegistro.Object.PeriodoIncremento,
                  ValorPeriodoIncremento = toUpdateRegistro.Object.ValorPeriodoIncremento,
                  IdPeriodoServicio = toUpdateRegistro.Object.IdPeriodoServicio,
                  PeriodoServicio = toUpdateRegistro.Object.PeriodoServicio,
                  CantidadPeriodoServicio = iregistro.CantidadPeriodoServicio,
                  ValorIncremento = iregistro.ValorIncremento,
                  ValorUnidadOriginal = toUpdateRegistro.Object.ValorUnidadOriginal,
                  EsParticionable = toUpdateRegistro.Object.EsParticionable,
                  IdGaleria = toUpdateRegistro.Object.IdGaleria,
                  Imagen = toUpdateRegistro.Object.Imagen,
                  GaleriaImagen = toUpdateRegistro.Object.GaleriaImagen,
                  StockMinimo = toUpdateRegistro.Object.StockMinimo,
                  Descripcion = toUpdateRegistro.Object.Descripcion,
                  Cantidad = toUpdateRegistro.Object.Cantidad,
                  CantidadInventarioInicial = toUpdateRegistro.Object.CantidadInventarioInicial,
                  CantidadCompra = toUpdateRegistro.Object.CantidadCompra,
                  CantidadEntrada = toUpdateRegistro.Object.CantidadEntrada,
                  CantidadInventarioConcilia = toUpdateRegistro.Object.CantidadInventarioConcilia,
                  CantidadDevuelta = toUpdateRegistro.Object.CantidadDevuelta,
                  CantidadVenta = toUpdateRegistro.Object.CantidadVenta,
                  CantidadOtros = toUpdateRegistro.Object.CantidadOtros,
                  CantidadSalida = toUpdateRegistro.Object.CantidadSalida,
                  CantidadSaldoFinal = toUpdateRegistro.Object.CantidadSaldoFinal,
                  SaldoExistente = toUpdateRegistro.Object.SaldoExistente,
                  PorcentajeRentabilidad = toUpdateRegistro.Object.PorcentajeRentabilidad,
                  PorcentajeGastoOperativo = toUpdateRegistro.Object.PorcentajeGastoOperativo,
                  PrecioCompra = toUpdateRegistro.Object.PrecioCompra,
                  PrecioVenta = toUpdateRegistro.Object.PrecioVenta,
                  PrecioBase = toUpdateRegistro.Object.PrecioBase,
                  PrecioVentaImpuesto = toUpdateRegistro.Object.PrecioVentaImpuesto,
                  ValorGastoOperativo = toUpdateRegistro.Object.ValorGastoOperativo,
                  ValorRentabilidad = toUpdateRegistro.Object.ValorRentabilidad,
                  ValorUtilidad = toUpdateRegistro.Object.ValorUtilidad,
                  ItemStockRegistradoSerializado = toUpdateRegistro.Object.ItemStockRegistradoSerializado,

                  PreciosItemSerializado=iregistro.PreciosItemSerializado,
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
        public async Task DeleteRegistro(Item oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("Item")
              .OnceAsync<Item>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("Item").Child(toDeleteRegistro.Key).DeleteAsync();

        }
        #endregion 3.Métodos

    }
}
