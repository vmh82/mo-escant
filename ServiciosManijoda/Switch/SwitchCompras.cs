using DataModel.DTO.Compras;
using DataModel.DTO.Iteraccion;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Online.Firebase.Compras;
using ManijodaServicios.Resources.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Switch
{
    public class SwitchCompras
    {
        #region 0. DeclaracionServicios
        /// <summary>
        /// Declaración de Servicios
        /// </summary>
        private readonly IServicioCompras_Orden _ordenServicio;
        private readonly IServicioCompras_OrdenDetalleCompra _ordenDetalleCompraServicio;
        private readonly IServicioCompras_OrdenEstado _ordenEstadoServicio;
        #endregion 0. DeclaracionServicios

        #region 1.Constructor
        /// <summary>
        /// Constructor de Instanciación de Servicios
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public SwitchCompras(IServicioCompras_Orden ordenServicio)
        {
            _ordenServicio = ordenServicio;
        }
        public SwitchCompras(IServicioCompras_OrdenDetalleCompra ordenDetalleCompraServicio)
        {
            _ordenDetalleCompraServicio = ordenDetalleCompraServicio;
        }
        public SwitchCompras(IServicioCompras_OrdenEstado ordenEstadoServicio)
        {
            _ordenEstadoServicio = ordenEstadoServicio;
        }
        public SwitchCompras(IServicioCompras_Orden ordenServicio, IServicioCompras_OrdenDetalleCompra ordenDetalleCompraServicio, IServicioCompras_OrdenEstado ordenEstadoServicio)
        {
            _ordenServicio = ordenServicio;
            _ordenDetalleCompraServicio = ordenDetalleCompraServicio;
            _ordenEstadoServicio = ordenEstadoServicio;
        }
        #endregion 1.Constructor

        #region 2.Objetos
        #region 2.1.Orden
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<Orden>> ConsultaOrdenes(Orden iRegistro, bool estaConectado)
        {
            List<Orden> retorno = new List<Orden>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Orden firebaseHelper = new Firebase_Orden();
                retorno = await firebaseHelper.GetRegistrosOrden(iRegistro);
                /*if (retorno == null)
                    retorno = (await _ordenServicio.GetAllOrdenAsync(x => x.Deleted==iRegistro.Deleted
                                                                          ))?.ToList();*/
            }
            else
            {
                retorno = (await _ordenServicio.GetAllOrdenAsync(x => x.Deleted == iRegistro.Deleted
                                                                      ))?.ToList();
            }
            return retorno;
        }
        /// <summary>
        /// Consulta Órdenes por Período
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<Orden>> ConsultaPeriodoOrdenes(Orden iRegistro, bool estaConectado)
        {
            List<Orden> retorno = new List<Orden>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Orden firebaseHelper = new Firebase_Orden();
                retorno = await firebaseHelper.GetRegistrosPeriodoOrden(iRegistro);
                if (retorno == null)
                    retorno = (await _ordenServicio.GetAllOrdenAsync(x => x.Deleted == iRegistro.Deleted
                                                                          ))?.ToList();
            }
            else
            {
                retorno = (await _ordenServicio.GetAllOrdenAsync(x => x.Deleted == iRegistro.Deleted
                                                                      ))?.ToList();
            }
            return retorno;
        }
        public async Task<List<Orden>> ConsultaPeriodoClienteOrdenes(Orden iRegistro, bool estaConectado)
        {
            List<Orden> retorno = new List<Orden>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Orden firebaseHelper = new Firebase_Orden();
                retorno = await firebaseHelper.GetRegistrosPeriodoClienteOrden(iRegistro);
                if (retorno == null)
                    retorno = (await _ordenServicio.GetAllOrdenAsync(x => x.Deleted == iRegistro.Deleted
                                                                          ))?.ToList();
            }
            else
            {
                retorno = (await _ordenServicio.GetAllOrdenAsync(x => x.Deleted == iRegistro.Deleted
                                                                      ))?.ToList();
            }
            return retorno;
        }
        public async Task<List<Orden>> ConsultaFechaOrden(Orden iRegistro, bool estaConectado)
        {
            List<Orden> retorno = new List<Orden>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Orden firebaseHelper = new Firebase_Orden();
                retorno = await firebaseHelper.GetRegistrosFechaOrden(iRegistro);
                if (retorno == null)
                    retorno = (await _ordenServicio.GetAllOrdenAsync(x => x.Deleted == iRegistro.Deleted
                                                                          ))?.ToList();
            }
            else
            {
                retorno = (await _ordenServicio.GetAllOrdenAsync(x => x.Deleted == iRegistro.Deleted
                                                                      ))?.ToList();
            }
            return retorno;
        }
        public async Task<List<Orden>> ConsultaOrdenPorTipo(Orden iRegistro, bool estaConectado)
        {
            List<Orden> retorno = new List<Orden>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Orden firebaseHelper = new Firebase_Orden();
                retorno = await firebaseHelper.GetRegistrosTipoOrden(iRegistro);
                if (retorno == null)
                    retorno = (await _ordenServicio.GetAllOrdenAsync(x => x.Deleted == iRegistro.Deleted
                                                                          ))?.ToList();
            }
            else
            {
                retorno = (await _ordenServicio.GetAllOrdenAsync(x => x.Deleted == iRegistro.Deleted
                                                                      ))?.ToList();
            }
            return retorno;
        }
        /// <summary>
        /// Devuelve 1 registro de Catálogo
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<Orden> ConsultaOrden(Orden iRegistro, bool estaConectado)
        {
            Orden retorno = new Orden();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Orden firebaseHelper = new Firebase_Orden();
                retorno = await firebaseHelper.GetRegistro(iRegistro);
                if (retorno == null)
                    retorno = (await _ordenServicio.GetAllOrdenAsync(x => (!string.IsNullOrEmpty(iRegistro.Id) && x.Id==iRegistro.Id)
                                                                       || (!string.IsNullOrEmpty(iRegistro.NumeroOrden) && x.NumeroOrden == iRegistro.NumeroOrden)
                                                                    ))?.FirstOrDefault();
            }
            else
            {
                retorno = (await _ordenServicio.GetAllOrdenAsync(x => (!string.IsNullOrEmpty(iRegistro.Id) && x.Id == iRegistro.Id)
                                                                       || (!string.IsNullOrEmpty(iRegistro.NumeroOrden) && x.NumeroOrden == iRegistro.NumeroOrden)
                                                                    ))?.FirstOrDefault();
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
        public async Task<DatosProceso> GuardaRegistro_Orden(Orden iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_Orden firebaseHelper = new Firebase_Orden();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    await _ordenServicio.UpdateOrdenAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("OrdenUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                    await _ordenServicio.InsertOrdenAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("OrdenInfoSaved");
                }
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
        #endregion 2.1.Orden

        #region 2.2.OrdenDetalleCompra
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<OrdenDetalleCompra>> ConsultaOrdenesDetalleCompra(OrdenDetalleCompra iRegistro, bool estaConectado)
        {
            List<OrdenDetalleCompra> retorno = new List<OrdenDetalleCompra>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_OrdenDetalleCompra firebaseHelper = new Firebase_OrdenDetalleCompra();
                retorno = await firebaseHelper.GetRegistrosOrdenDetalleCompra(iRegistro);
                if (retorno == null)
                    retorno = (await _ordenDetalleCompraServicio.GetAllOrdenDetalleCompraAsync(x => x.Deleted == iRegistro.Deleted
                                                                          ))?.ToList();
            }
            else
            {
                retorno = (await _ordenDetalleCompraServicio.GetAllOrdenDetalleCompraAsync(x => x.Deleted == iRegistro.Deleted
                                                                          ))?.ToList();
            }
            return retorno;
        }
        /// <summary>
        /// Devuelve 1 registro de Catálogo
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<OrdenDetalleCompra> ConsultaOrdenDetalleCompra(OrdenDetalleCompra iRegistro, bool estaConectado)
        {
            OrdenDetalleCompra retorno = new OrdenDetalleCompra();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_OrdenDetalleCompra firebaseHelper = new Firebase_OrdenDetalleCompra();
                retorno = await firebaseHelper.GetRegistro(iRegistro);
                if (retorno == null)
                    retorno = (await _ordenDetalleCompraServicio.GetAllOrdenDetalleCompraAsync(x => (!string.IsNullOrEmpty(iRegistro.Id) && x.Id == iRegistro.Id)
                                                                       || (!string.IsNullOrEmpty(iRegistro.IdOrden) && x.IdOrden == iRegistro.IdOrden)
                                                                    ))?.FirstOrDefault();
            }
            else
            {
                retorno = (await _ordenDetalleCompraServicio.GetAllOrdenDetalleCompraAsync(x => (!string.IsNullOrEmpty(iRegistro.Id) && x.Id == iRegistro.Id)
                                                                       || (!string.IsNullOrEmpty(iRegistro.IdOrden) && x.IdOrden == iRegistro.IdOrden)
                                                                    ))?.FirstOrDefault();
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
        public async Task<DatosProceso> GuardaRegistro_OrdenDetalleCompra(OrdenDetalleCompra iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_OrdenDetalleCompra firebaseHelper = new Firebase_OrdenDetalleCompra();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    await _ordenDetalleCompraServicio.UpdateOrdenDetalleCompraAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("OrdenDetalleCompraUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                    await _ordenDetalleCompraServicio.InsertOrdenDetalleCompraAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("OrdenDetalleCompraInfoSaved");
                }
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
        /// Método para almacenar los registros de Catálogo
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <param name="estaConectado"></param>
        /// <param name="existe"></param>
        /// <returns></returns>
        public async Task<DatosProceso> GuardaRegistro_ListaOrdenDetalleCompra(List<OrdenDetalleCompra> iRegistro, bool estaConectado, bool existe)
        {

            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                foreach (var ordenDetalleCompra in iRegistro)
                {
                    datosProceso = await GuardaRegistro_OrdenDetalleCompra(ordenDetalleCompra, estaConectado, existe);
                }

            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            return datosProceso;
            
        }

        

        #endregion 2.2.OrdenDetalleCompra

        #region 2.3.OrdenEstado
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<OrdenEstado>> ConsultaOrdenesEstado(OrdenEstado iRegistro, bool estaConectado)
        {
            List<OrdenEstado> retorno = new List<OrdenEstado>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_OrdenEstado firebaseHelper = new Firebase_OrdenEstado();
                retorno = await firebaseHelper.GetRegistrosOrdenEstado(iRegistro);
                if (retorno == null)
                    retorno = (await _ordenEstadoServicio.GetAllOrdenEstadoAsync(x => x.Deleted == iRegistro.Deleted
                                                                          ))?.ToList();
            }
            else
            {
                retorno = (await _ordenEstadoServicio.GetAllOrdenEstadoAsync(x => x.Deleted == iRegistro.Deleted
                                                                          ))?.ToList();
            }
            return retorno;
        }
        /// <summary>
        /// Devuelve 1 registro de Catálogo
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<OrdenEstado> ConsultaOrdenEstado(OrdenEstado iRegistro, bool estaConectado)
        {
            OrdenEstado retorno = new OrdenEstado();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_OrdenEstado firebaseHelper = new Firebase_OrdenEstado();
                retorno = await firebaseHelper.GetRegistro(iRegistro);
                if (retorno == null)
                    retorno = (await _ordenEstadoServicio.GetAllOrdenEstadoAsync(x => (!string.IsNullOrEmpty(iRegistro.Id) && x.Id == iRegistro.Id)
                                                                       || (!string.IsNullOrEmpty(iRegistro.IdOrden) && x.IdOrden == iRegistro.IdOrden)
                                                                    ))?.FirstOrDefault();
            }
            else
            {
                retorno = (await _ordenEstadoServicio.GetAllOrdenEstadoAsync(x => (!string.IsNullOrEmpty(iRegistro.Id) && x.Id == iRegistro.Id)
                                                                       || (!string.IsNullOrEmpty(iRegistro.IdOrden) && x.IdOrden == iRegistro.IdOrden)
                                                                    ))?.FirstOrDefault();
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
        public async Task<DatosProceso> GuardaRegistro_OrdenEstado(OrdenEstado iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_OrdenEstado firebaseHelper = new Firebase_OrdenEstado();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    await _ordenEstadoServicio.UpdateOrdenEstadoAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("OrdenEstadoUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                    await _ordenEstadoServicio.InsertOrdenEstadoAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("OrdenEstadoInfoSaved");
                }
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
        /// Método para guardar registros de Orden Estado
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <param name="estaConectado"></param>
        /// <param name="existe"></param>
        /// <returns></returns>
        public async Task<DatosProceso> GuardaRegistro_ListaOrdenEstado(List<OrdenEstado> iRegistro, bool estaConectado, bool existe)
        {

            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                foreach (var ordenEstado in iRegistro)
                {
                    datosProceso = await GuardaRegistro_OrdenEstado(ordenEstado, estaConectado, existe);
                }

            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            return datosProceso;

        }
        #endregion 2.3.OrdenEstado


        #endregion 2.Objetos
    }
}
