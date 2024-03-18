using DataModel.DTO.Seguridad;
using Firebase.Database;
using Firebase.Database.Query;
using ManijodaServicios.AppSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ManijodaServicios.Online.Firebase.Seguridad
{
    public class Firebase_PerfilMenu
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
        public Firebase_PerfilMenu()
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
        public async Task<List<PerfilMenu>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("PerfilMenu")
                  .OnceAsync<PerfilMenu>()).Select(item => new PerfilMenu
                  {
                      Id = item.Object.Id,
                      IdPerfil = item.Object.IdPerfil,
                      CodigoPerfil = item.Object.CodigoPerfil,
                      IdMenu = item.Object.IdMenu,
                      IdMenuPadre = item.Object.IdMenuPadre,
                      NombreMenu = item.Object.NombreMenu,
                      NombreFormulario = item.Object.NombreFormulario,
                      LabelTitulo = item.Object.LabelTitulo,
                      LabelDescripcion = item.Object.LabelDescripcion,
                      ImageIcon = item.Object.ImageIcon,
                      Nivel = item.Object.Nivel,
                      Orden = item.Object.Orden,
                      EstaActivo = item.Object.EstaActivo,                      
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
        public async Task<PerfilMenu> GetRegistro(PerfilMenu oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("PerfilMenu")
                  .OnceAsync<PerfilMenu>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                            || (a.IdPerfil == oRegistro.IdPerfil && oRegistro.IdPerfil != null                                             
                                            && a.IdMenu == oRegistro.IdMenu && oRegistro.IdMenu != null)
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
        public async Task<List<PerfilMenu>> GetRegistrosPerfilMenu(PerfilMenu oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("PerfilMenu")
                      .OnceAsync<PerfilMenu>();
                    return allRegistros.Where(a => (a.IsDeleted == oRegistro.IsDeleted 
                                                    && oRegistro.ValorBusqueda == null
                                                    && oRegistro.CodigoPerfil == null
                                                    && oRegistro.IdPerfil==null
                                                    && oRegistro.Id == null)
                                                || (a.Id == oRegistro.Id && oRegistro.Id != null)
                                                || (a.IdPerfil == oRegistro.IdPerfil && oRegistro.IdPerfil != null)
                                                || (a.CodigoPerfil == oRegistro.CodigoPerfil && oRegistro.CodigoPerfil != null)
                                                || (a.IdMenu == oRegistro.IdMenu && oRegistro.IdMenu != null)                                                
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
        public async Task InsertaRegistro(PerfilMenu iregistro)
        {
            PerfilMenu registro = await GetRegistro(iregistro);
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
        public async Task AddRegistro(PerfilMenu iregistro)
        {
            try
            {
                await firebase
                  .Child("PerfilMenu")
                  .PostAsync(new PerfilMenu()
                  {
                      Id = iregistro.Id,
                      IdPerfil = iregistro.IdPerfil,
                      CodigoPerfil = iregistro.CodigoPerfil,
                      IdMenu = iregistro.IdMenu,
                      IdMenuPadre = iregistro.IdMenuPadre,
                      NombreMenu = iregistro.NombreMenu,
                      NombreFormulario = iregistro.NombreFormulario,
                      LabelTitulo = iregistro.LabelTitulo,
                      LabelDescripcion = iregistro.LabelDescripcion,
                      ImageIcon = iregistro.ImageIcon,
                      Nivel = iregistro.Nivel,
                      Orden = iregistro.Orden,
                      EstaActivo = iregistro.EstaActivo,
                      //Logo = iregistro.Logo
                      //Datos de Auditoría
                      CreatedBy = SettingsOnline.oAuthentication.Email,
                      CreatedDateFormato = iregistro.CreatedDateFormato

                  });
            }
            catch (Exception ex)
            {
                //var y = ex.Message.ToString();
            }
        }

        /// <summary>
        /// Método para actualizar un registro existente
        /// </summary>
        /// <param name="iRegistro"></param>
        /// <returns></returns>
        public async Task UpdateRegistro(PerfilMenu iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("PerfilMenu")
              .OnceAsync<PerfilMenu>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("PerfilMenu")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new PerfilMenu()
              {
                  Id = toUpdateRegistro.Object.Id,                  
                  IdPerfil = toUpdateRegistro.Object.IdPerfil,
                  CodigoPerfil = toUpdateRegistro.Object.CodigoPerfil,
                  IdMenu = toUpdateRegistro.Object.IdMenu,
                  IdMenuPadre = toUpdateRegistro.Object.IdMenuPadre,
                  NombreMenu = toUpdateRegistro.Object.NombreMenu,
                  NombreFormulario = toUpdateRegistro.Object.NombreFormulario,
                  LabelTitulo = toUpdateRegistro.Object.LabelTitulo,
                  LabelDescripcion = toUpdateRegistro.Object.LabelDescripcion,
                  ImageIcon = toUpdateRegistro.Object.ImageIcon,
                  Nivel = toUpdateRegistro.Object.Nivel,
                  Orden = toUpdateRegistro.Object.Orden,
                  EstaActivo = iregistro.EstaActivo,                  
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
        public async Task DeleteRegistro(PerfilMenu oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("PerfilMenu")
              .OnceAsync<PerfilMenu>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("PerfilMenu").Child(toDeleteRegistro.Key).DeleteAsync();

        }
        #endregion 3.Métodos

    }
}
