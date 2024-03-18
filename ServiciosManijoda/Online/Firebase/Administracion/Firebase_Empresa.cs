using DataModel.DTO.Administracion;
using Firebase.Database;
using Firebase.Database.Query;
using ManijodaServicios.AppSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ManijodaServicios.Online.Firebase.Administracion
{
    public class Firebase_Empresa
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
        public Firebase_Empresa()
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
        public async Task<List<Empresa>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("Empresa")
                  .OnceAsync<Empresa>()).Select(item => new Empresa
                  {
                      Id = item.Object.Id,                      
                      Codigo = item.Object.Codigo,
                      Descripcion = item.Object.Descripcion,
                      Identificacion = item.Object.Identificacion,
                      Direccion = item.Object.Direccion,
                      Celular = item.Object.Celular,
                      Email = item.Object.Email,
                      TelefonoFijo = item.Object.TelefonoFijo,
                      Representante = item.Object.Representante,
                      EsProveedor = item.Object.EsProveedor,
                      PorDefecto = item.Object.PorDefecto,
                      IdGaleria=item.Object.IdGaleria,
                      Imagen = item.Object.Imagen
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
        public async Task<Empresa> GetRegistro(Empresa oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("Empresa")
                  .OnceAsync<Empresa>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                            || (a.Codigo == oRegistro.Codigo && oRegistro.Codigo != null)).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Método para consultar un registro acorde a filtros
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<Empresa>> GetRegistrosEmpresa(Empresa oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Empresa")
                      .OnceAsync<Empresa>();
                    return allRegistros.Where(a => (string.IsNullOrEmpty(oRegistro.valorBusqueda)
                                                    && string.IsNullOrEmpty(oRegistro.Id)
                                                    && string.IsNullOrEmpty(oRegistro.Codigo)                                                    
                                                    && oRegistro.EsProveedor < 0
                                                    && a.IsDeleted==oRegistro.IsDeleted)
                                                || (oRegistro.EsProveedor>=0 && a.EsProveedor == oRegistro.EsProveedor)
                                                || (!string.IsNullOrEmpty(oRegistro.Id) && a.Id == oRegistro.Id)
                                                || (!string.IsNullOrEmpty(oRegistro.Codigo) && a.Codigo == oRegistro.Codigo)                                                
                                                || (!string.IsNullOrEmpty(oRegistro.valorBusqueda) 
                                                        && (a.Codigo.ToLower().Contains(oRegistro.valorBusqueda)
                                                        || a.Descripcion.ToLower().Contains(oRegistro.valorBusqueda)
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
        /// Método para mantenimiento de un nuevo registro
        /// </summary>
        /// <param name="iregistro"></param>
        /// <returns></returns>
        public async Task InsertaRegistro(Empresa iregistro)
        {
            Empresa registro = await GetRegistro(iregistro);
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
        public async Task AddRegistro(Empresa iregistro)
        {
            try
            {
                await firebase
                  .Child("Empresa")
                  .PostAsync(new Empresa()
                  {
                      Id = iregistro.Id,
                      Codigo = iregistro.Codigo,
                      Descripcion = iregistro.Descripcion,
                      Identificacion = iregistro.Identificacion,
                      Direccion= iregistro.Direccion,
                      Celular = iregistro.Celular,
                      Email = iregistro.Email,
                      TelefonoFijo = iregistro.TelefonoFijo,
                      Representante = iregistro.Representante,
                      EsProveedor = iregistro.EsProveedor,
                      PorDefecto = iregistro.PorDefecto,
                      IdGaleria = iregistro.IdGaleria,
                      Imagen = iregistro.Imagen,
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
        public async Task UpdateRegistro(Empresa iregistro)
        {
            try { 
            var toUpdateRegistro = (await firebase
              .Child("Empresa")
              .OnceAsync<Empresa>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("Empresa")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Empresa()
              {
                  Id = toUpdateRegistro.Object.Id,
                  Codigo = iregistro.Codigo,
                  Descripcion = iregistro.Descripcion,
                  Identificacion = iregistro.Identificacion,
                  Direccion = iregistro.Direccion,
                  Celular = iregistro.Celular,
                  Email = iregistro.Email,
                  TelefonoFijo = iregistro.TelefonoFijo,
                  Representante = iregistro.Representante,
                  EsProveedor = iregistro.EsProveedor,
                  PorDefecto = iregistro.PorDefecto,
                  IdGaleria = iregistro.IdGaleria,
                  Imagen = iregistro.Imagen,
                  //Logo = iRegistro.Logo
                  //Datos de Auditoría
                  UpdatedBy = SettingsOnline.oAuthentication.Email,
                  CreatedBy = toUpdateRegistro.Object.CreatedBy,
                  CreatedDateFormato = toUpdateRegistro.Object.CreatedDateFormato
              });
            }
            catch(Exception ex)
            {
                var y = ex.Message.ToString();
            }
        }
        /// <summary>
        /// Método para eliminar un registro de manera lógica
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task DeleteRegistro(Empresa oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("Empresa")
              .OnceAsync<Empresa>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("Empresa").Child(toDeleteRegistro.Key).DeleteAsync();

        }

        public Stream ByteArrayToStream(byte[] input)
        {
            return new MemoryStream(input);
        }
        #endregion 3.Métodos
    }
}
