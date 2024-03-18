using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel.DTO.Administracion;
using Firebase.Database;
using Firebase.Database.Query;
using ManijodaServicios.AppSettings;

namespace ManijodaServicios.Online.Firebase.Administracion
{
    /// <summary>
    /// Servicio para administración de datos online en Firebase del Objeto Catálogo
    /// </summary>
    public class Firebase_Catalogo
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
        public Firebase_Catalogo()
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
        public async Task<List<Catalogo>> GetAllRegistros()
        {
            try
            {                
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("Catalogo")
                  .OnceAsync<Catalogo>()).Select(item => new Catalogo
                  {
                      Id = item.Object.Id,
                      IdPadre = item.Object.IdPadre,
                      Empresa = item.Object.Empresa,
                      CodigoCatalogo=item.Object.CodigoCatalogo,
                      EsCatalogo = item.Object.EsCatalogo,
                      EsJerarquico = item.Object.EsJerarquico,
                      EsMedida = item.Object.EsMedida,
                      EsCentral = item.Object.EsCentral,
                      IdConversion = item.Object.IdConversion,
                      ValorConversion = item.Object.ValorConversion,
                      Codigo = item.Object.Codigo,
                      Nombre = item.Object.Nombre,
                      EstaActivo = item.Object.EstaActivo,
                      EsConstantes= item.Object.EsConstantes,
                      ValorConstanteNumerico= item.Object.ValorConstanteNumerico,
                      ValorConstanteTexto = item.Object.ValorConstanteTexto,
                      EsMenu = item.Object.EsMenu,
                      EsModulo = item.Object.EsModulo,
                      PoseeFormulario= item.Object.PoseeFormulario,
                      NombreFormulario = item.Object.NombreFormulario,
                      LabelTitulo = item.Object.LabelTitulo,
                      LabelDescripcion = item.Object.LabelDescripcion,
                      ImageIcon=item.Object.ImageIcon,
                      Nivel=item.Object.Nivel,
                      Orden = item.Object.Orden
                      //Logo = item.Object.Logo
                  }).ToList() : null;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Método para consultar un registro
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<Catalogo> GetRegistro(Catalogo oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("Catalogo")
                  .OnceAsync<Catalogo>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                            || (a.Codigo == oRegistro.Codigo && oRegistro.Codigo != null
                                             && a.CodigoCatalogo==oRegistro.CodigoCatalogo && oRegistro.CodigoCatalogo!=null
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
        public async Task<List<Catalogo>> GetRegistrosCatalogo(Catalogo oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Catalogo")
                      .OnceAsync<Catalogo>();
                    return allRegistros.Where(a => ((a.Deleted == oRegistro.Deleted 
                                                    && oRegistro.Deleted >=0)
                                                    && string.IsNullOrEmpty(oRegistro.CodigoCatalogo)
                                                    && oRegistro.EsCatalogo<1
                                                    && oRegistro.EsCentral<1
                                                    )
                                                || (a.Id == oRegistro.Id && oRegistro.Id != null)
                                                || (a.CodigoCatalogo == oRegistro.CodigoCatalogo && oRegistro.CodigoCatalogo != null)
                                                || (a.EsCatalogo == oRegistro.EsCatalogo && oRegistro.EsCatalogo == 1)
                                                || (a.EsCentral == oRegistro.EsCentral && oRegistro.EsCentral == 1)).ToList();
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
        public async Task InsertaRegistro(Catalogo iregistro)
        {
            Catalogo registro = await GetRegistro(iregistro);
            if (registro != null || iregistro.EsEdicion==1)
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
        public async Task AddRegistro(Catalogo iregistro)
        {
            try
            {
                await firebase
                  .Child("Catalogo")
                  .PostAsync(new Catalogo()
                  {
                      Id = iregistro.Id,                      
                      IdPadre = iregistro.IdPadre,
                      Empresa = iregistro.Empresa,
                      CodigoCatalogo= iregistro.CodigoCatalogo,
                      EsCatalogo = iregistro.EsCatalogo,
                      EsJerarquico = iregistro.EsJerarquico,
                      EsMedida = iregistro.EsMedida,
                      EsCentral = iregistro.EsCentral,
                      IdConversion = iregistro.IdConversion,
                      ValorConversion = iregistro.ValorConversion,
                      Codigo = iregistro.Codigo,
                      Nombre = iregistro.Nombre,
                      EstaActivo = iregistro.EstaActivo,
                      EsConstantes = iregistro.EsConstantes,
                      ValorConstanteNumerico = iregistro.ValorConstanteNumerico,
                      ValorConstanteTexto = iregistro.ValorConstanteTexto,
                      EsMenu = iregistro.EsMenu,
                      EsModulo = iregistro.EsModulo,
                      PoseeFormulario = iregistro.PoseeFormulario,
                      NombreFormulario = iregistro.NombreFormulario,
                      LabelTitulo = iregistro.LabelTitulo,
                      LabelDescripcion = iregistro.LabelDescripcion,
                      ImageIcon=iregistro.ImageIcon,
                      Nivel=iregistro.Nivel,
                      Orden = iregistro.Orden,
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
        public async Task UpdateRegistro(Catalogo iRegistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("Catalogo")
              .OnceAsync<Catalogo>()).Where(a => a.Object.Id == iRegistro.Id).FirstOrDefault();

            await firebase
              .Child("Catalogo")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Catalogo()
              {                  
                  Id= toUpdateRegistro.Object.Id,
                  IdPadre = iRegistro.IdPadre,
                  CodigoCatalogo = toUpdateRegistro.Object.CodigoCatalogo,
                  Codigo= iRegistro.Codigo,
                  Nombre= iRegistro.Nombre,
                  EsCatalogo = toUpdateRegistro.Object.EsCatalogo,
                  EsJerarquico = toUpdateRegistro.Object.EsJerarquico,
                  EsMedida = toUpdateRegistro.Object.EsMedida,
                  EsCentral = toUpdateRegistro.Object.EsCentral,
                  IdConversion = toUpdateRegistro.Object.IdConversion,
                  ValorConversion = toUpdateRegistro.Object.ValorConversion,
                  EstaActivo = iRegistro.EstaActivo,
                  EsConstantes = iRegistro.EsConstantes,
                  ValorConstanteNumerico = iRegistro.ValorConstanteNumerico,
                  ValorConstanteTexto = iRegistro.ValorConstanteTexto,
                  EsMenu = iRegistro.EsMenu,
                  EsModulo = iRegistro.EsModulo,
                  PoseeFormulario = iRegistro.PoseeFormulario,
                  NombreFormulario = iRegistro.NombreFormulario,
                  LabelTitulo = iRegistro.LabelTitulo,
                  LabelDescripcion = iRegistro.LabelDescripcion,
                  ImageIcon=iRegistro.ImageIcon,
                  Nivel=iRegistro.Nivel,
                  Orden = iRegistro.Orden,
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
        public async Task DeleteRegistro(Catalogo oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("Catalogo")
              .OnceAsync<Catalogo>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("Catalogo").Child(toDeleteRegistro.Key).DeleteAsync();

        }
        #endregion 3.Métodos
    }
}
