using DataModel.DTO.Administracion;
using DataModel.DTO.Compras;
using DataModel.DTO.Iteraccion;
using DataModel.DTO.Productos;
using DataModel.Helpers;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Online.Firebase.Administracion;
using ManijodaServicios.Online.Firebase.Productos;
using ManijodaServicios.Resources.Texts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace ManijodaServicios.Switch
{
    public class SwitchProductos
    {
        #region 0. DeclaracionServicios
        /// <summary>
        /// Declaración de Servicios
        /// </summary>
        private readonly IServicioProductos_Item _itemServicio;
        private readonly IServicioProductos_Precio _precioServicio;
        #endregion 0. DeclaracionServicios

        #region 1.Constructor
        /// <summary>
        /// Constructor de Instanciación de Servicios
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public SwitchProductos(IServicioProductos_Item itemServicio)
        {
            _itemServicio = itemServicio;
        }
        public SwitchProductos(IServicioProductos_Precio precioServicio)
        {
            _precioServicio = precioServicio;
        }
        public SwitchProductos(IServicioProductos_Item itemServicio, IServicioProductos_Precio precioServicio)
        {
            _itemServicio = itemServicio;
            _precioServicio = precioServicio;
        }
        #endregion 1.Constructor

        #region 2.Objetos
        #region 2.0 General
        public async Task SincronizaDatos(string nombreTabla)
        {
            switch (nombreTabla)
            {
                case "Item":
                    Firebase_Item firebaseHelper = new Firebase_Item();
                    var retorno = (await firebaseHelper.GetAllRegistros()).OfType<Item>().ToList();
                    await ImportarDatos_Item(retorno, false);
                    break;
                case "Precio":
                    Firebase_Precio firebaseHelper_precio = new Firebase_Precio();
                    var retorno_precio = (await firebaseHelper_precio.GetAllRegistros()).OfType<Precio>().ToList();
                    await ImportarDatos_Precio(retorno_precio, false);
                    break;
            }

        }
        public async Task<DatosProceso> ImportarDatos_Item(IEnumerable<Item> iRegistro, bool estaConectado)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                await _itemServicio.ImportItemAsync(iRegistro);
                retorno = TextsTranslateManager.Translate("StoreInfoSaved");

            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            datosProceso.mensaje = retorno;
            return datosProceso;
        }
        public async Task<DatosProceso> ImportarDatos_Precio(IEnumerable<Precio> iRegistro, bool estaConectado)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                await _precioServicio.ImportPrecioAsync(iRegistro);
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
        #region 2.1.Item
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<Item>> ConsultaItems(Item iRegistro, bool estaConectado)
        {
            List<Item> retorno = new List<Item>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Item firebaseHelper = new Firebase_Item();
                retorno = await firebaseHelper.GetRegistrosItem(iRegistro);
                if (retorno == null)
                    retorno = (await _itemServicio.GetAllItemAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null && iRegistro.Id==null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.Codigo == iRegistro.Codigo && iRegistro.Codigo != null)
                                                                           || (x.Nombre == iRegistro.Nombre && iRegistro.Nombre != null)                                                                           
                                                                           || (x.Codigo.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Nombre.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Descripcion.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                         )


                    )?.ToList();
            }
            else
            {
                retorno = (await _itemServicio.GetAllItemAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.Codigo == iRegistro.Codigo && iRegistro.Codigo != null)
                                                                           || (x.Nombre == iRegistro.Nombre && iRegistro.Nombre != null)
                                                                           || (x.Codigo.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Nombre.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Descripcion.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                         )

                    )?.ToList();
            }
            return retorno;
        }
        public async Task<List<Item>> ConsultaTopItems(Item iRegistro, bool estaConectado)
        {
            List<Item> retorno = new List<Item>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Item firebaseHelper = new Firebase_Item();
                retorno = await firebaseHelper.GetRegistrosTopItem(iRegistro);
                if (retorno == null)
                    retorno = (await _itemServicio.GetAllItemAsync(x => x.Deleted == iRegistro.Deleted
                                                                           && x.CantidadVenta>0
                                                                         )


                    )?.ToList();
            }
            else
            {
                retorno = (await _itemServicio.GetAllItemAsync(x => x.Deleted == iRegistro.Deleted
                                                                           && x.CantidadVenta > 0
                                                                         )

                    )?.ToList();
            }
            return retorno;
        }
        public async Task<List<Item>> ConsultaStockItems(Item iRegistro, bool estaConectado)
        {
            List<Item> retorno = new List<Item>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Item firebaseHelper = new Firebase_Item();
                retorno = await firebaseHelper.GetRegistrosSaldosItem(iRegistro);
                if (retorno == null)
                    retorno = (await _itemServicio.GetAllItemAsync(x => x.Deleted == iRegistro.Deleted
                                                                           && x.CantidadSaldoFinal > 0
                                                                           && x.EsServicio==0
                                                                         )


                    )?.ToList();
            }
            else
            {
                retorno = (await _itemServicio.GetAllItemAsync(x => x.Deleted == iRegistro.Deleted
                                                                           && x.CantidadSaldoFinal > 0
                                                                           && x.EsServicio == 0
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
        public async Task<DatosProceso> GuardaRegistro_Item(Item iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_Item firebaseHelper = new Firebase_Item();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                #region Offline
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    retorno = TextsTranslateManager.Translate("ItemInfoUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                    retorno = TextsTranslateManager.Translate("ItemInfoSaved");
                }
                await _itemServicio.InsertItemAsync(iRegistro);
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
        /// <summary>
        /// Método para almacenar los precios en el Ítem
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <param name="estaConectado"></param>
        /// <param name="existe"></param>
        /// <returns></returns>
        public async Task<DatosProceso> GuardaRegistro_PreciosItem(Item iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_Item firebaseHelper = new Firebase_Item();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                #region Offline
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    retorno = TextsTranslateManager.Translate("ItemInfoUpdateSuccess");
                }                
                await _itemServicio.InsertItemAsync(iRegistro);
                #endregion Offline

            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            return datosProceso;
        }
        
        /// <summary>
        /// Método para modificar el stock de los ítems        
        /// </summary>
        /// <param name="iRegistroSeleccionado"></param>
        /// <param name="estaConectado"></param>
        /// <param name="existe"></param>
        /// <returns></returns>
        public async Task<DatosProceso> GuardaRegistro_StockOrden(Orden iRegistroSeleccionado, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            List<OrdenDetalleCompra> iRegistro=new List<OrdenDetalleCompra>();
            List<ItemEntrega> iRegistroEntregado = new List<ItemEntrega>();
            List<OrdenDetalleCompraDet> iDetalle = new List<OrdenDetalleCompraDet>();
            ObservableCollection<ItemStock> retornoStock = new ObservableCollection<ItemStock>();            
            Item itemProducto = new Item();            
            try
            {
                switch(iRegistroSeleccionado.TipoOrden)
                {
                    case string a when iRegistroSeleccionado.TipoOrden.ToUpper().Contains("OMPRA"):
                        iRegistro = iRegistroSeleccionado.OrdenDetalleCompras.ToList();
                        iDetalle = iRegistroSeleccionado.OrdenDetalleComprasRecibido.ToList();
                        foreach (var Item in iRegistro)
                        {

                            itemProducto = (await ConsultaItems(new DataModel.DTO.Productos.Item() { Id = Item.IdItem }, estaConectado)).FirstOrDefault();
                            retornoStock = !string.IsNullOrEmpty(itemProducto.ItemStockRegistradoSerializado) ? new ObservableCollection<ItemStock>(JsonConvert.DeserializeObject<List<ItemStock>>(itemProducto.ItemStockRegistradoSerializado)) : new ObservableCollection<ItemStock>();
                            var total = iDetalle.Where(x => x.IdItem == itemProducto.Id).ToList();
                            var z = total
                                .GroupBy(m => new
                                {
                                    IdOrden = m.IdOrden,
                                    IdItem = m.IdItem,
                                    NombreItem = m.NombreItem,
                                    Ubicacion = m.Ubicacion,
                                    Unidad = m.Unidad,
                                    Imagen = m.Imagen,
                                    ValorUnitario = m.ValorUnitario,
                                    FechaVencimiento = m.FechaVencimientoStr
                                }
                                        )
                                .Select(y => new ItemStock()
                                {
                                    Id=Generator.GenerateKey(),
                                    IdOrden = y.Key.IdOrden,
                                    NumeroOrden = iRegistroSeleccionado.NumeroOrden,
                                    IdItem = y.Key.IdItem,
                                    NombreItem = y.Key.NombreItem,
                                    FechaIngreso = DateTime.Now.ToString("dd/MM/yyyy"),
                                    FechaVencimiento = y.Key.FechaVencimiento,
                                    Ubicacion = y.Key.Ubicacion,
                                    Unidad = y.Key.Unidad,
                                    Imagen = y.Key.Imagen,
                                    ValorUnitario = y.Key.ValorUnitario,
                                    CantidadCompra = y.Sum(p=>p.Cantidad)
                                });
                                
                            z.ForEach(x => retornoStock.Add(x));
                            itemProducto.ItemStockRegistrado = retornoStock;
                            itemProducto.CantidadCompra = itemProducto.ItemStockRegistrado.Sum(x => x.CantidadCompra);
                            itemProducto.ItemStockRegistradoSerializado = JsonConvert.SerializeObject(itemProducto.ItemStockRegistrado);
                            datosProceso = await GuardaRegistro_Item(itemProducto, estaConectado, existe);
                        }
                        break;
                    case string a when iRegistroSeleccionado.TipoOrden.ToUpper().Contains("VENTARI"):
                        iRegistro = iRegistroSeleccionado.OrdenDetalleCompras.ToList();
                        iDetalle = iRegistroSeleccionado.OrdenDetalleComprasRecibido.ToList();
                        foreach (var Item in iRegistro)
                        {

                            itemProducto = (await ConsultaItems(new DataModel.DTO.Productos.Item() { Id = Item.IdItem }, estaConectado)).FirstOrDefault();
                            retornoStock = !string.IsNullOrEmpty(itemProducto.ItemStockRegistradoSerializado) ? new ObservableCollection<ItemStock>(JsonConvert.DeserializeObject<List<ItemStock>>(itemProducto.ItemStockRegistradoSerializado)) : new ObservableCollection<ItemStock>();
                            var total = iDetalle.Where(x => x.IdItem == itemProducto.Id).ToList();
                            var z = total
                                .GroupBy(m => new
                                {
                                    IdOrden = m.IdOrden,
                                    IdItem = m.IdItem,
                                    NombreItem = m.NombreItem,
                                    Ubicacion = m.Ubicacion,
                                    Unidad = m.Unidad,
                                    Imagen = m.Imagen,
                                    ValorUnitario = m.ValorUnitario,
                                    FechaVencimiento = m.FechaVencimientoStr
                                }
                                        )
                                .Select(y => new ItemStock()
                                {
                                    Id = Generator.GenerateKey(),
                                    IdOrden = y.Key.IdOrden,
                                    NumeroOrden = iRegistroSeleccionado.NumeroOrden,
                                    IdItem = y.Key.IdItem,
                                    NombreItem = y.Key.NombreItem,
                                    FechaIngreso = DateTime.Now.ToString("dd/MM/yyyy"),
                                    FechaVencimiento = y.Key.FechaVencimiento,
                                    Ubicacion = y.Key.Ubicacion,
                                    Unidad = y.Key.Unidad,
                                    Imagen = y.Key.Imagen,
                                    ValorUnitario = y.Key.ValorUnitario,
                                    CantidadInventarioInicial = y.Sum(p => p.Cantidad)
                                });

                            z.ForEach(x => retornoStock.Add(x));
                            itemProducto.ItemStockRegistrado = retornoStock;
                            itemProducto.CantidadInventarioInicial = itemProducto.ItemStockRegistrado.Sum(x => x.CantidadInventarioInicial);
                            itemProducto.PoseeInventarioInicial = 1;
                            itemProducto.ItemStockRegistradoSerializado = JsonConvert.SerializeObject(itemProducto.ItemStockRegistrado);
                            datosProceso = await GuardaRegistro_Item(itemProducto, estaConectado, existe);
                        }
                        break;
                    case string a when iRegistroSeleccionado.TipoOrden.ToUpper().Contains("ENTA"):
                        iRegistroEntregado = iRegistroSeleccionado.OrdenDetalleComprasEntregado.ToList();                        

                        float saldo = 0;                        
                        ObservableCollection<ItemEntrega> devuelveStockCancelado=new ObservableCollection<ItemEntrega>();
                        var iRegistroUnico = iRegistroEntregado
                                            //.Select(x => new { IdItem = x.IdItem, Cantidad = x.Cantidad })                                            
                                            //.GroupBy(x => x.IdItem)
                                            .GroupBy(x=> new { IdOrdenOrigen=x.IdOrdenOrigen
                                                             , IdItem = x.IdItem
                                                             , Unidad=x.Unidad
                                                             ,Ubicacion = x.Ubicacion
                                                             ,FechaVencimiento=x.FechaVencimiento
                                                      })
                                            .Select(c=> new
                                            {
                                                IdItem = c.Key,                                                
                                                Total = c.Sum(p => p.cantidad)
                                            });
                        //.Select(y => y.Sum(i => i.Cantidad));                                                               
                        foreach (var Item in iRegistroUnico)
                        {
                            //Obtengo el ítem
                            itemProducto = (await ConsultaItems(new DataModel.DTO.Productos.Item() { Id = Item.IdItem.IdItem }, estaConectado)).FirstOrDefault();                            
                            //Obtengo el stock que posee
                            retornoStock = !string.IsNullOrEmpty(itemProducto.ItemStockRegistradoSerializado) ? new ObservableCollection<ItemStock>(JsonConvert.DeserializeObject<List<ItemStock>>(itemProducto.ItemStockRegistradoSerializado)) : new ObservableCollection<ItemStock>();
                            var iretornoStock = retornoStock.Where(x => x.IdItem == Item.IdItem.IdItem
                                                                    && x.Ubicacion == Item.IdItem.Ubicacion
                                                                    && x.IdOrden==Item.IdItem.IdOrdenOrigen
                                                                    && x.FechaVencimiento==Item.IdItem.FechaVencimiento
                                                                    && x.CantidadSaldoFinal>0).OrderBy(y=>y.FechaVencimiento).ToList();

                            saldo = 0;                            
                            foreach (var rstock in iretornoStock)
                            {
                                var cantidadVendida = Item.Total-saldo;
                                var cantidadSaldo = rstock.CantidadSaldoFinal;
                                if (cantidadSaldo >= cantidadVendida)
                                {
                                    rstock.CantidadVenta += cantidadVendida;
                                    /*devuelveStockCancelado.Add(new ItemEntrega() { IdOrden=iRegistroSeleccionado.Id
                                                                                   ,NumeroOrden=iRegistroSeleccionado.NumeroOrden
                                                                                   ,IdItem=rstock.IdItem
                                                                                   ,NombreItem=rstock.NombreItem
                                                                                   ,Imagen=rstock.Imagen
                                                                                   ,Ubicacion=rstock.Ubicacion
                                                                                   ,FechaVencimiento=rstock.FechaVencimiento
                                                                                   ,cantidad= cantidadVendida
                                                                                   ,valorUnitario= valorUnitarioVenta
                                                                                   ,valorUnitarioCompra=rstock.ValorUnitario
                                    });*/
                                    break;
                                }
                                else
                                {
                                    rstock.CantidadVenta += cantidadSaldo;
                                    /*devuelveStockCancelado.Add(new ItemEntrega()
                                    {
                                        IdOrden = iRegistroSeleccionado.Id
                                                                                   ,
                                        NumeroOrden = iRegistroSeleccionado.NumeroOrden
                                                                                   ,
                                        IdItem = rstock.IdItem
                                                                                   ,
                                        NombreItem = rstock.NombreItem
                                                                                   ,
                                        Imagen = rstock.Imagen
                                                                                   ,
                                        Ubicacion = rstock.Ubicacion
                                                                                   ,
                                        FechaVencimiento = rstock.FechaVencimiento
                                                                                   ,
                                        cantidad = cantidadSaldo
                                                                                   ,
                                        valorUnitario = valorUnitarioVenta
                                                                                   ,
                                        valorUnitarioCompra = rstock.ValorUnitario
                                    });*/
                                    saldo += (cantidadSaldo);
                                }
                                
                            }                            
                            itemProducto.ItemStockRegistrado = retornoStock;
                            itemProducto.CantidadVenta = itemProducto.ItemStockRegistrado.Sum(x => x.CantidadVenta);
                            itemProducto.ItemStockRegistradoSerializado = JsonConvert.SerializeObject(itemProducto.ItemStockRegistrado);
                            datosProceso = await GuardaRegistro_Item(itemProducto, estaConectado, existe);
                            //datosProceso.objetoSerializado= JsonConvert.SerializeObject(devuelveStockCancelado);
                        }
                        break;
                }
                
                

            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            return datosProceso;
        }
        /*Especiales de complemento*/
        

        #endregion 2.1.Item

        #region 2.2.Precio
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<Precio>> ConsultaPrecios(Precio iRegistro, bool estaConectado)
        {
            List<Precio> retorno = new List<Precio>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Precio firebaseHelper = new Firebase_Precio();
                retorno = await firebaseHelper.GetRegistrosPrecio(iRegistro);
                if (retorno == null)
                    retorno = (await _precioServicio.GetAllPrecioAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.ValorConversion == iRegistro.ValorConversion && iRegistro.ValorConversion >0)
                                                                           || (x.IdItem == iRegistro.IdItem && iRegistro.Id != null)
                                                                           || (x.NombreItem == iRegistro.NombreItem && iRegistro.NombreItem != null)
                                                                           || (x.IdItem == iRegistro.IdItem && iRegistro.IdItem != null)
                                                                           || (x.CodigoItem.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.NombreItem.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                         )


                    )?.ToList();
            }
            else
            {
                retorno = (await _precioServicio.GetAllPrecioAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.ValorConversion == iRegistro.ValorConversion && iRegistro.ValorConversion > 0)
                                                                           || (x.IdItem == iRegistro.IdItem && iRegistro.Id != null)
                                                                           || (x.NombreItem == iRegistro.NombreItem && iRegistro.NombreItem != null)
                                                                           || (x.IdItem == iRegistro.IdItem && iRegistro.IdItem != null)
                                                                           || (x.CodigoItem.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.NombreItem.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
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
        public async Task<DatosProceso> GuardaRegistro_Precio(Precio iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_Precio firebaseHelper = new Firebase_Precio();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                #region Offline
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    retorno = TextsTranslateManager.Translate("PrecioInfoUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                    retorno = TextsTranslateManager.Translate("PrecioInfoSaved");
                }
                await _precioServicio.InsertPrecioAsync(iRegistro);
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

        public async Task<DatosProceso> GuardaRegistro_Precios(List<Precio> iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                foreach (var precio in iRegistro)
                {
                    datosProceso = await GuardaRegistro_Precio(precio, estaConectado, existe);
                }

            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            return datosProceso;
        }
        #endregion 2.1.Precio
        #endregion 2.Objetos
    }
}
