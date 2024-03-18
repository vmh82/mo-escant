using DataModel.DTO.Administracion;
using DataModel.DTO.Iteraccion;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Online.Firebase.Administracion;
using ManijodaServicios.Resources.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManijodaServicios.Switch
{
    /// <summary>
    /// Clase para administrar los métodos online y offline del módulo de administración
    /// </summary>
    public class SwitchAdministracion
    {
        #region 0. DeclaracionServicios
        /// <summary>
        /// Declaración de Servicios
        /// </summary>
        private readonly IServicioAdministracion_Catalogo _catalogoServicio;
        private readonly IServicioAdministracion_Empresa _empresaServicio;
        private readonly IServicioAdministracion_Proveedor _proveedorServicio;
        #endregion 0. DeclaracionServicios

        #region 1.Constructor
        /// <summary>
        /// Constructor de Instanciación de Servicios
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public SwitchAdministracion(IServicioAdministracion_Catalogo catalogoServicio)
        {
            _catalogoServicio = catalogoServicio;
        }
        public SwitchAdministracion(IServicioAdministracion_Empresa empresaServicio)
        {
            _empresaServicio = empresaServicio;
        }
        public SwitchAdministracion(IServicioAdministracion_Proveedor proveedorServicio)
        {
            _proveedorServicio = proveedorServicio;
        }
        public SwitchAdministracion(IServicioAdministracion_Empresa empresaServicio,IServicioAdministracion_Proveedor proveedorServicio)
        {
            _empresaServicio = empresaServicio;
            _proveedorServicio = proveedorServicio;
        }
        public SwitchAdministracion(IServicioAdministracion_Catalogo catalogoServicio, IServicioAdministracion_Empresa empresaServicio)
        {
            _catalogoServicio = catalogoServicio;
            _empresaServicio = empresaServicio;
        }
        public SwitchAdministracion(IServicioAdministracion_Catalogo catalogoServicio, IServicioAdministracion_Empresa empresaServicio, IServicioAdministracion_Proveedor proveedorServicio)
        {
            _catalogoServicio = catalogoServicio;
            _empresaServicio = empresaServicio;
            _proveedorServicio = proveedorServicio;
        }
        #endregion 1.Constructor

        #region 2.Objetos
        #region 2.1.Catálogo
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<Catalogo>> ConsultaCatalogos(Catalogo iRegistro, bool estaConectado)
        {
            List<Catalogo> retorno = new List<Catalogo>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Catalogo firebaseHelper = new Firebase_Catalogo();
                retorno = await firebaseHelper.GetRegistrosCatalogo(iRegistro);
                if (retorno == null)
                    retorno = (await _catalogoServicio.GetAllCatalogoAsync(x => x.EstaActivo==1 && ((x.CodigoCatalogo == iRegistro.CodigoCatalogo && iRegistro.CodigoCatalogo!=null)
                                                                                                || (x.EsCatalogo == iRegistro.EsCatalogo && iRegistro.EsCatalogo ==1)
                                                                                                || (x.EsCentral == iRegistro.EsCentral && iRegistro.EsCentral == 1)
                                                                                                   ) 
                    ))?.ToList();                    
            }
            else
            {
                retorno = (await _catalogoServicio.GetAllCatalogoAsync(x => x.EstaActivo == 1 && ((x.CodigoCatalogo == iRegistro.CodigoCatalogo && iRegistro.CodigoCatalogo != null)
                                                                                               || (x.EsCatalogo == iRegistro.EsCatalogo && iRegistro.EsCatalogo == 1)
                                                                                               || (x.EsCentral == iRegistro.EsCentral && iRegistro.EsCentral ==1)
                                                                                                  )
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
        public async Task<Catalogo> ConsultaCatalogo(Catalogo iRegistro, bool estaConectado)
        {
            Catalogo retorno = new Catalogo();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Catalogo firebaseHelper = new Firebase_Catalogo();
                retorno = await firebaseHelper.GetRegistro(iRegistro);
                if (retorno == null)
                    retorno = (await _catalogoServicio.GetAllCatalogoAsync(x => x.EstaActivo == 1 && ((x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                                                   )
                    ))?.FirstOrDefault();
            }
            else
            {
                retorno = (await _catalogoServicio.GetAllCatalogoAsync(x => x.EstaActivo == 1 && ((x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                                                   )
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
        public async Task<DatosProceso> GuardaRegistro_Catalogo(Catalogo iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_Catalogo firebaseHelper = new Firebase_Catalogo();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    await _catalogoServicio.UpdateCatalogoAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("StoreUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                    await _catalogoServicio.InsertCatalogoAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("StoreInfoSaved");
                }
            }
            catch(Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            datosProceso.Id = iRegistro.Id;
            datosProceso.mensaje = retorno;
            datosProceso.Tipo = existe?"actualiza":"adiciona";
            return datosProceso;
        }

        //await _categoryService.ImportCategoryAsync(categories);
        public async Task SincronizaDatos(string nombreTabla)
        {               
            switch(nombreTabla)
            {
                case "Catalogo":
                    Firebase_Catalogo firebaseHelper = new Firebase_Catalogo();
                    var retorno = (await firebaseHelper.GetAllRegistros()).OfType<Catalogo>().ToList();
                    await ImportarDatos_Catalogo(retorno, false);
                    break;
                case "Empresa":
                    Firebase_Empresa firebaseHelper_Empresa = new Firebase_Empresa();
                    var retorno_empresa = (await firebaseHelper_Empresa.GetAllRegistros()).OfType<Empresa>().ToList();
                    await ImportarDatos_Empresa(retorno_empresa, false);
                    break;
                case "Proveedor":
                    Firebase_Proveedor firebaseHelper_Proveedor = new Firebase_Proveedor();
                    var retorno_proveedor = (await firebaseHelper_Proveedor.GetAllRegistros()).OfType<Proveedor>().ToList();
                    await ImportarDatos_Proveedor(retorno_proveedor, false);
                    break;
            }
            
        }

        public async Task<DatosProceso> ImportarDatos_Catalogo(IEnumerable<Catalogo> iRegistro, bool estaConectado)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {                                    
                    await _catalogoServicio.ImportCatalogoAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("StoreInfoSaved");
                
            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }            
            datosProceso.mensaje = retorno;            
            return datosProceso;
        }
        public async Task<DatosProceso> ImportarDatos_Empresa(IEnumerable<Empresa> iRegistro, bool estaConectado)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                await _empresaServicio.ImportEmpresaAsync(iRegistro);
                retorno = TextsTranslateManager.Translate("StoreInfoSaved");

            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            datosProceso.mensaje = retorno;
            return datosProceso;
        }
        public async Task<DatosProceso> ImportarDatos_Proveedor(IEnumerable<Proveedor> iRegistro, bool estaConectado)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                await _proveedorServicio.ImportProveedorAsync(iRegistro);
                retorno = TextsTranslateManager.Translate("StoreInfoSaved");

            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            datosProceso.mensaje = retorno;
            return datosProceso;
        }


        #endregion 2.1.Catálogo

        #region 2.2.Empresa
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<Empresa>> ConsultaEmpresas(Empresa iRegistro, bool estaConectado)
        {
            List<Empresa> retorno = new List<Empresa>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Empresa firebaseHelper = new Firebase_Empresa();
                retorno = await firebaseHelper.GetRegistrosEmpresa(iRegistro);
                /*if (retorno == null)
                    retorno = (await _empresaServicio.GetAllEmpresaAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.Codigo == iRegistro.Codigo && iRegistro.Codigo != null)
                                                                           || (x.Codigo.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Descripcion.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                         )                                                                                                   


                    )?.ToList();*/
            }
            else
            {
                retorno = (await _empresaServicio.GetAllEmpresaAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.Codigo == iRegistro.Codigo && iRegistro.Codigo != null)
                                                                           || (x.Codigo.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Descripcion.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
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
        public async Task<DatosProceso> GuardaRegistro_Empresa(Empresa iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_Empresa firebaseHelper = new Firebase_Empresa();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                #region Offline
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;                    
                    retorno = TextsTranslateManager.Translate("EmpresaInfoUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;                    
                    retorno = TextsTranslateManager.Translate("EmpresaInfoSaved");
                }
                await _empresaServicio.InsertEmpresaAsync(iRegistro);
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
        #endregion 2.1.Empresa

        #region 2.3.Proveedor
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<Proveedor>> ConsultaProveedores(Proveedor iRegistro, bool estaConectado)
        {
            List<Proveedor> retorno = new List<Proveedor>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Proveedor firebaseHelper = new Firebase_Proveedor();
                retorno = await firebaseHelper.GetRegistrosProveedor(iRegistro);
                if (retorno == null)
                    retorno = (await _proveedorServicio.GetAllProveedorAsync(x => x.Deleted == 0 && ((x.IdEmpresa == iRegistro.IdEmpresa && iRegistro.IdEmpresa != null)                                                                                              
                                                                                                   )
                    ))?.ToList();
            }
            else
            {
                retorno = (await _proveedorServicio.GetAllProveedorAsync(x => x.Deleted == 0 && ((x.IdEmpresa == iRegistro.IdEmpresa && iRegistro.IdEmpresa != null)                                                                                            
                                                                                                  )
                   ))?.ToList();
            }
            return retorno;
        }
        public async Task<Proveedor> ConsultaProveedor(Proveedor iRegistro, bool estaConectado)
        {
            Proveedor retorno = new Proveedor();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Proveedor firebaseHelper = new Firebase_Proveedor();
                retorno = await firebaseHelper.GetRegistro(iRegistro);
                if (retorno == null)
                    retorno = (await _proveedorServicio.GetAllProveedorAsync(x => x.Deleted == 0 && ((x.IdEmpresa == iRegistro.IdEmpresa && iRegistro.IdEmpresa != null)
                                                                                                   )
                    ))?.FirstOrDefault();
            }
            else
            {
                retorno = (await _proveedorServicio.GetAllProveedorAsync(x => x.Deleted == 0 && ((x.IdEmpresa == iRegistro.IdEmpresa && iRegistro.IdEmpresa != null)
                                                                                                  )
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
        public async Task<DatosProceso> GuardaRegistro_Proveedor(Proveedor iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_Proveedor firebaseHelper = new Firebase_Proveedor();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    await _proveedorServicio.UpdateProveedorAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("ProveedorInfoUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                    await _proveedorServicio.InsertProveedorAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("ProveedorInfoSaved");
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
        #endregion 2.3.Proveedor
        #endregion 2.Objetos

    }
}
