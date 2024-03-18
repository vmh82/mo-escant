using DataModel.DTO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DataModel.DTO.Configuracion;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.Online.Firebase.Configuracion;
using ManijodaServicios.Resources.Texts;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Interfaces;
using DataModel.DTO.Iteraccion;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ManijodaServicios.Switch
{
    public class SwitchConfiguracion
    {
        private readonly ISettingsService _settingsServicio;
        private readonly IServicioFirestore _servicioFirestore;
        public SwitchConfiguracion(ISettingsService settingsServicio)
        {
            _settingsServicio = settingsServicio;
        }
        public SwitchConfiguracion(IServicioFirestore servicioFirestore)
        {
            _servicioFirestore = servicioFirestore;
        }
        public SwitchConfiguracion(ISettingsService settingsServicio,IServicioFirestore servicioFirestore)
        {
            _settingsServicio = settingsServicio;
            _servicioFirestore = servicioFirestore;
        }

        #region 2.0 General
        public async Task SincronizaDatos(string nombreTabla)
        {
            switch (nombreTabla)
            {
                case "Setting":
                    Firebase_Setting firebaseHelper = new Firebase_Setting();
                    var retorno = (await firebaseHelper.GetAllRegistros()).OfType<Setting>().ToList();
                    await ImportarDatos_Setting(retorno, false);
                    break;
            }

        }
        public async Task<DatosProceso> ImportarDatos_Setting(IEnumerable<Setting> iRegistro, bool estaConectado)
        {
            string retorno = "";
            var datosProceso = new DatosProceso();
            try
            {
                await _settingsServicio.ImportSettingAsync(iRegistro);
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

        /// <summary>
        /// Método para consultar configuración de manera Online u Offline
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <param name="estaConectado"></param>
        /// <returns></returns>
        public async Task<Setting> ConsultaConfiguracion(Setting iRegistro,bool estaConectado)
        {
            Setting retorno=new Setting();
            if (estaConectado) //Si la aplicación se encuentra Online
            {
                Firebase_Setting firebaseHelper = new Firebase_Setting();
                retorno= await firebaseHelper.GetRegistro(iRegistro);                
                if (retorno==null || retorno.Data==null)
                    retorno = await _settingsServicio.GetSettingAsync(x => x.SettingType == iRegistro.SettingType);
            }
            else
            {
                retorno= await _settingsServicio.GetSettingAsync(x => x.SettingType == iRegistro.SettingType);
            }
            return retorno;
        }
        /// <summary>
        /// Método para guardar la configuración 
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <param name="estaConectado"></param>
        /// <param name="existe"></param>
        /// <returns></returns>
        public async Task<string> GuardaConfiguracion(Setting iRegistro,bool estaConectado,bool existe)
        {
            string retorno = "";
            if (estaConectado) //Si la aplicación se encuentra Online
            {                
                Firebase_Setting firebaseHelper = new Firebase_Setting();                
                await firebaseHelper.InsertaRegistro(iRegistro);                
            }
            //Para la parte Offline
            if (existe)
            {
                iRegistro.UpdatedBy= SettingsOnline.oAuthentication.Email;
                await _settingsServicio.UpdateSettingAsync(iRegistro);
                retorno = TextsTranslateManager.Translate("StoreUpdateSuccess");
            }
            else
            {
                iRegistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                await _settingsServicio.InsertSettingAsync(iRegistro);
                retorno = TextsTranslateManager.Translate("StoreInfoSaved");
            }            
            return retorno;
        }

        public async Task<SecuencialNumerico> ActualizaConfiguracionSecuencial(string TipoSecuencial)
        {            
            return await _servicioFirestore.GetIncremental(TipoSecuencial);
        }

    }
}
