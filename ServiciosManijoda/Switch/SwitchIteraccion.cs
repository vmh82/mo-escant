using DataModel.DTO.Administracion;
using DataModel.DTO.Iteraccion;
using Firebase.Storage;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Online.Firebase.Administracion;
using ManijodaServicios.Online.Firebase.Clientes;
using ManijodaServicios.Online.Firebase.Configuracion;
using ManijodaServicios.Online.Firebase.Iteraccion;
using ManijodaServicios.Online.Firebase.Productos;
using ManijodaServicios.Online.Firebase.Seguridad;
using ManijodaServicios.Resources.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Switch
{
    public class SwitchIteraccion
    {
        #region 0. DeclaracionServicios
        /// <summary>
        /// Declaración de Servicios
        /// </summary>
        private readonly IServicioIteraccion_Galeria _galeriaServicio;
        private readonly IServicioIteraccion_TablasBdd _tablasBddServicio;        
        #endregion 0. DeclaracionServicios

        #region 1.Constructor
        /// <summary>
        /// Constructor de Instanciación de Servicios
        /// </summary>
        /// <param name="galeriaServicio"></param>
        public SwitchIteraccion(IServicioIteraccion_Galeria galeriaServicio)
        {
            _galeriaServicio = galeriaServicio;
        }
        public SwitchIteraccion(IServicioIteraccion_TablasBdd tablasBddServicio)
        {
            _tablasBddServicio = tablasBddServicio;
        }        
        public SwitchIteraccion(IServicioIteraccion_Galeria galeriaServicio, IServicioIteraccion_TablasBdd tablasBddServicio)
        {
            _galeriaServicio = galeriaServicio;
            _tablasBddServicio = tablasBddServicio;
        }
        #endregion 1.Constructor

        #region 2.Objetos
        #region 2.0 General
        public async Task SincronizaDatos(string nombreTabla)
        {
            switch (nombreTabla)
            {
                case "Galeria":
                    Firebase_Galeria firebaseHelper = new Firebase_Galeria();
                    var retorno = (await firebaseHelper.GetAllRegistros()).OfType<Galeria>().ToList();
                    await ImportarDatos_Galeria(retorno, false);
                    break;                
            }

        }
        public async Task<DatosProceso> ImportarDatos_Galeria(IEnumerable<Galeria> iRegistro, bool estaConectado)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                await _galeriaServicio.ImportGaleriaAsync(iRegistro);
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
        #region 2.1.Galeria
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<Galeria>> ConsultaGalerias(Galeria iRegistro, bool estaConectado)
        {
            List<Galeria> retorno = new List<Galeria>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Galeria firebaseHelper = new Firebase_Galeria();
                retorno = await firebaseHelper.GetRegistrosGaleria(iRegistro);
                if (retorno == null)
                    retorno = (await _galeriaServicio.GetAllGaleriaAsync(x => ((x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                               )
                    ))?.ToList();
            }
            else
            {
                retorno = (await _galeriaServicio.GetAllGaleriaAsync(x => ((x.Id == iRegistro.Id && iRegistro.Id != null)
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
        public async Task<Galeria> ConsultaGaleria(Galeria iRegistro, bool estaConectado)
        {
            Galeria retorno = new Galeria();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Galeria firebaseHelper = new Firebase_Galeria();
                retorno = await firebaseHelper.GetRegistro(iRegistro);
                if (retorno == null)
                    retorno = (await _galeriaServicio.GetAllGaleriaAsync(x => ((x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                                                   )
                    ))?.FirstOrDefault();
            }
            else
            {
                retorno = (await _galeriaServicio.GetAllGaleriaAsync(x => ((x.Id == iRegistro.Id && iRegistro.Id != null)
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
        public async Task<DatosProceso> GuardaRegistro_Galeria(Galeria iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            var url = "";
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    
                    if (iRegistro.Image!=null && iRegistro.GuardarStorage==1)
                    {
                        url=await GuardaObjetoGaleria(iRegistro, estaConectado);
                    }
                    Firebase_Galeria firebaseHelper = new Firebase_Galeria();
                    iRegistro.UrlImagen = url;
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    await _galeriaServicio.UpdateGaleriaAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("StoreUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                    await _galeriaServicio.InsertGaleriaAsync(iRegistro);
                    retorno = TextsTranslateManager.Translate("StoreInfoSaved");
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

        public async Task<string> GuardaObjetoGaleria(Galeria iRegistro, bool estaConectado)
        {
            var resultado = "";
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_Galeria firebaseHelper = new Firebase_Galeria();
                    resultado=await firebaseHelper.InsertaObjeto(iRegistro);
                }                
            }
            catch (Exception ex)
            {
                resultado = ex.Message.ToString();
            }
            return resultado;
        }

        #endregion 2.1.Galeria

        #region 2.2.TablasBdd
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<TablasBdd>> ConsultaTablasBddFull(TablasBdd iRegistro, bool estaConectado)
        {
            List<TablasBdd> retorno = new List<TablasBdd>();
            retorno = (await _tablasBddServicio.GetAllTablasBddFullAsync(iRegistro))?.ToList();
            if (retorno != null && retorno.Count() > 0)
            {
                foreach(var registro in retorno)
                {
                    registro.cantidadDatosPrincipal = await SELECT_ROWCOUNT(registro.nombreTabla);
                    registro.sincronizada = registro.cantidadDatosPrincipal == registro.cantidadDatosSecundaria ? 1 : 0;
                }
                /*retorno.ForEach(async i => i.cantidadDatosPrincipal = await SELECT_ROWCOUNT(i.nombreTabla));
                retorno.ForEach(i => i.sincronizada=i.cantidadDatosPrincipal ==i.cantidadDatosSecundaria?1:0);*/
                retorno.RemoveAll(s => s.sincronizada == 1);
            }
            return retorno;
        }
        
        public async Task DeleteTablasBddAllData(string nombreTabla)
        {            
            await _tablasBddServicio.DeleteTablasBddAllDataAsync(nombreTabla);                        
        }

        private async Task<int> SELECT_ROWCOUNT(string nombreTabla)
        {
            int retorno = 0;
            switch (nombreTabla)
            {
                case "Catalogo":
                    Firebase_Catalogo firebaseHelper = new Firebase_Catalogo();
                    retorno= (await firebaseHelper.GetAllRegistros()).Count;                    
                    break;
                case "Persona":
                    Firebase_Persona firebaseHelper_per = new Firebase_Persona();
                    retorno = (await firebaseHelper_per.GetAllRegistros()).Count;
                    break;
                case "Cliente":
                    Firebase_Cliente firebaseHelper_cli = new Firebase_Cliente();
                    retorno = (await firebaseHelper_cli.GetAllRegistros()).Count;
                    break;
                case "Proveedor":
                    Firebase_Proveedor firebaseHelper_prov = new Firebase_Proveedor();
                    retorno = (await firebaseHelper_prov.GetAllRegistros()).Count;
                    break;
                case "Empresa":
                    Firebase_Empresa firebaseHelper_emp = new Firebase_Empresa();
                    retorno = (await firebaseHelper_emp.GetAllRegistros()).Count;
                    break;
                case "Item":
                    Firebase_Item firebaseHelper_item = new Firebase_Item();
                    retorno = (await firebaseHelper_item.GetAllRegistros()).Count;
                    break;
                case "Precio":
                    Firebase_Precio firebaseHelper_precio = new Firebase_Precio();
                    retorno = (await firebaseHelper_precio.GetAllRegistros()).Count;
                    break;
                case "Setting":
                    Firebase_Setting firebaseHelper_setting = new Firebase_Setting();
                    retorno = (await firebaseHelper_setting.GetAllRegistros()).Count;
                    break;
                case "Galeria":
                    Firebase_Galeria firebaseHelper_galeria = new Firebase_Galeria();
                    retorno = (await firebaseHelper_galeria.GetAllRegistros()).Count;
                    break;
                case "Usuario":
                    Firebase_Usuario firebaseHelper_usuario = new Firebase_Usuario();
                    retorno = (await firebaseHelper_usuario.GetAllRegistros()).Count;
                    break;
            }
            return retorno;
        }

        #endregion 2.1.TablasBdd

        #endregion 2.Objetos
    }
}
