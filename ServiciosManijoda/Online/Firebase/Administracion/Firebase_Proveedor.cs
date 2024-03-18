using DataModel.DTO.Administracion;
using Firebase.Database;
using Firebase.Database.Query;
using ManijodaServicios.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ManijodaServicios.Online.Firebase.Administracion
{
    public class Firebase_Proveedor
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
        public Firebase_Proveedor()
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
        public async Task<List<Proveedor>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("Proveedor")
                  .OnceAsync<Proveedor>()).Select(item => new Proveedor
                  {
                      Id = item.Object.Id,
                      IdEmpresa = item.Object.IdEmpresa,
                      IdTipoProveedor = item.Object.IdTipoProveedor,
                      EsContribuyenteEspecial = item.Object.EsContribuyenteEspecial,
                      PoseeFacturaElectronica = item.Object.PoseeFacturaElectronica
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
        public async Task<Proveedor> GetRegistro(Proveedor oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("Proveedor")
                  .OnceAsync<Proveedor>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                            || (a.IdEmpresa == oRegistro.IdEmpresa && oRegistro.IdEmpresa != null)).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Método para consultar un registro acorde a filtros
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<Proveedor>> GetRegistrosProveedor(Proveedor oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Proveedor")
                      .OnceAsync<Proveedor>();
                    return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                                || (a.IdTipoProveedor == oRegistro.IdTipoProveedor && oRegistro.IdTipoProveedor != null)                                                
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
        public async Task InsertaRegistro(Proveedor iregistro)
        {
            Proveedor registro = await GetRegistro(iregistro);
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
        public async Task AddRegistro(Proveedor iregistro)
        {
            try
            {
                await firebase
                  .Child("Proveedor")
                  .PostAsync(new Proveedor()
                  {
                      Id = iregistro.Id,
                      IdEmpresa = iregistro.IdEmpresa,
                      IdTipoProveedor = iregistro.IdTipoProveedor,
                      EsContribuyenteEspecial = iregistro.EsContribuyenteEspecial,
                      PoseeFacturaElectronica = iregistro.PoseeFacturaElectronica,                      
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
        public async Task UpdateRegistro(Proveedor iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("Proveedor")
              .OnceAsync<Proveedor>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("Proveedor")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Proveedor()
              {
                  Id = toUpdateRegistro.Object.Id,
                  IdEmpresa = iregistro.IdEmpresa,
                  IdTipoProveedor = iregistro.IdTipoProveedor,
                  EsContribuyenteEspecial = iregistro.EsContribuyenteEspecial,
                  PoseeFacturaElectronica = iregistro.PoseeFacturaElectronica,                                    
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
        public async Task DeleteRegistro(Proveedor oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("Proveedor")
              .OnceAsync<Proveedor>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("Proveedor").Child(toDeleteRegistro.Key).DeleteAsync();

        }
        #endregion 3.Métodos

    }
}
