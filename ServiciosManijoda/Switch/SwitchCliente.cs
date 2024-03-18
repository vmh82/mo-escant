using DataModel.DTO.Administracion;
using DataModel.DTO.Clientes;
using DataModel.DTO.Iteraccion;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Online.Firebase.Administracion;
using ManijodaServicios.Online.Firebase.Clientes;
using ManijodaServicios.Resources.Texts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Switch
{
    /// <summary>
    /// Clase para administrar los métodos online y offline del módulo de Cliente
    /// </summary>
    public class SwitchCliente
    {
        #region 0. DeclaracionServicios
        /// <summary>
        /// Declaración de Servicios
        /// </summary>
        private readonly IServicioClientes_Cliente _clienteServicio;
        private readonly IServicioClientes_Persona _personaServicio;        
        #endregion 0. DeclaracionServicios

        #region 1.Constructor
        /// <summary>
        /// Constructor de Instanciación de Servicios
        /// </summary>
        /// <param name="catalogoServicio"></param>
        public SwitchCliente(IServicioClientes_Cliente clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }
        public SwitchCliente(IServicioClientes_Persona personaServicio)
        {
            _personaServicio = personaServicio;
        }
        
        public SwitchCliente(IServicioClientes_Cliente clienteServicio, IServicioClientes_Persona personaServicio)
        {
            _clienteServicio = clienteServicio;
            _personaServicio = personaServicio;
        }
        #endregion 1.Constructor

        #region 2.Objetos
        #region 2.0 General
        public async Task SincronizaDatos(string nombreTabla)
        {
            switch (nombreTabla)
            {
                case "Cliente":
                    Firebase_Cliente firebaseHelper = new Firebase_Cliente();
                    var retorno = (await firebaseHelper.GetAllRegistros()).OfType<Cliente>().ToList();
                    await ImportarDatos_Cliente(retorno, false);
                    break;
                case "Persona":
                    Firebase_Persona firebaseHelper_persona = new Firebase_Persona();
                    var retorno_persona = (await firebaseHelper_persona.GetAllRegistros()).OfType<Persona>().ToList();
                    await ImportarDatos_Persona(retorno_persona, false);
                    break;
            }

        }
        public async Task<DatosProceso> ImportarDatos_Cliente(IEnumerable<Cliente> iRegistro, bool estaConectado)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                await _clienteServicio.ImportClienteAsync(iRegistro);
                retorno = TextsTranslateManager.Translate("StoreInfoSaved");

            }
            catch (Exception ex)
            {
                retorno = ex.Message.ToString();
            }
            datosProceso.mensaje = retorno;
            return datosProceso;
        }
        public async Task<DatosProceso> ImportarDatos_Persona(IEnumerable<Persona> iRegistro, bool estaConectado)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                await _personaServicio.ImportPersonaAsync(iRegistro);
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

        #region 2.1.Cliente
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<Cliente>> ConsultaClientes(Cliente iRegistro, bool estaConectado)
        {
            List<Cliente> retorno = new List<Cliente>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Cliente firebaseHelper = new Firebase_Cliente();
                retorno = await firebaseHelper.GetRegistrosCliente(iRegistro);
                if (retorno == null)
                    retorno = (await _clienteServicio.GetAllClienteAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.Identificacion == iRegistro.Identificacion && iRegistro.Identificacion != null)
                                                                           || (x.Identificacion.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Nombre.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                                                   
                    ))?.ToList();
            }
            else
            {
                retorno = (await _clienteServicio.GetAllClienteAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.Identificacion == iRegistro.Identificacion && iRegistro.Identificacion != null)
                                                                           || (x.Identificacion.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)
                                                                           || (x.Nombre.ToLower().Contains(iRegistro.valorBusqueda) && iRegistro.valorBusqueda != null)

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
        public async Task<Cliente> ConsultaCliente(Cliente iRegistro, bool estaConectado)
        {
            Cliente retorno = new Cliente();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Cliente firebaseHelper = new Firebase_Cliente();
                retorno = await firebaseHelper.GetRegistro(iRegistro);
                /*if (retorno == null)
                    retorno = (await _clienteServicio.GetAllClienteAsync(x => ((x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                                                   )
                    ))?.FirstOrDefault();*/
            }
            else
            {
                retorno = (await _clienteServicio.GetAllClienteAsync(x => ((x.Id == iRegistro.Id && iRegistro.Id != null)
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
        public async Task<DatosProceso> GuardaRegistro_Cliente(Cliente iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_Cliente firebaseHelper = new Firebase_Cliente();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                //Para la parte Offline
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
                await _clienteServicio.InsertClienteAsync(iRegistro);
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
        #endregion 2.1.Cliente

        #region 2.2.Persona
        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>nº
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<List<Persona>> ConsultaPersonas(Persona iRegistro, bool estaConectado)
        {
            List<Persona> retorno = new List<Persona>();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Persona firebaseHelper = new Firebase_Persona();
                retorno = await firebaseHelper.GetRegistrosPersona(iRegistro);
                /*if (retorno == null)
                    retorno = (await _personaServicio.GetAllPersonaAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.Identificacion == iRegistro.Identificacion && iRegistro.Identificacion != null)                                                                           
                                                                         )


                    )?.ToList();*/
            }
            else
            {
                retorno = (await _personaServicio.GetAllPersonaAsync(x => (x.Deleted == iRegistro.Deleted && iRegistro.valorBusqueda == null)
                                                                           || (x.Id == iRegistro.Id && iRegistro.Id != null)
                                                                           || (x.Identificacion == iRegistro.Identificacion && iRegistro.Identificacion != null)
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
        public async Task<DatosProceso> GuardaRegistro_Persona(Persona iRegistro, bool estaConectado, bool existe)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                if (estaConectado) //Si la aplicación se encuentra Online
                {
                    Firebase_Persona firebaseHelper = new Firebase_Persona();
                    await firebaseHelper.InsertaRegistro(iRegistro);
                }
                #region Offline
                //Para la parte Offline
                if (existe)
                {
                    iRegistro.UpdatedBy = SettingsOnline.oAuthentication.Email;
                    retorno = TextsTranslateManager.Translate("PersonaInfoUpdateSuccess");
                }
                else
                {
                    iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                    retorno = TextsTranslateManager.Translate("PersonaInfoSaved");
                }
                await _personaServicio.InsertPersonaAsync(iRegistro);
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
        #endregion 2.1.Persona

        #endregion 2.Objetos
    }
}
