using DataModel.DTO.Clientes;
using Firebase.Database;
using Firebase.Database.Query;
using ManijodaServicios.AppSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ManijodaServicios.Online.Firebase.Clientes
{
    public class Firebase_Cliente
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
        public Firebase_Cliente()
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
        public async Task<List<Cliente>> GetAllRegistros()
        {
            try
            {
                return SettingsOnline.oAuthentication != null ? (await firebase
                  .Child("Cliente")
                  .OnceAsync<Cliente>()).Select(item => new Cliente
                  {
                      Id = item.Object.Id,
                      IdEmpresa = item.Object.IdEmpresa,
                      Empresa = item.Object.Empresa,
                      IdPersona = item.Object.IdPersona,
                      Identificacion = item.Object.Identificacion,
                      Nombre = item.Object.Nombre,
                      IdUnidadOrganizacion = item.Object.IdUnidadOrganizacion,
                      UnidadOrganizacion = item.Object.UnidadOrganizacion,
                      IdGenero = item.Object.IdGenero,
                      Genero = item.Object.Genero,
                      FechaNacimiento = item.Object.FechaNacimiento,
                      Direccion = item.Object.Direccion,
                      Celular = item.Object.Celular,
                      TelefonoFijo = item.Object.TelefonoFijo,
                      Email = item.Object.Email,
                      IdPorcentajeDescuento=item.Object.IdPorcentajeDescuento,
                      PorcentajeDescuento = item.Object.PorcentajeDescuento,
                      Descripcion = item.Object.Descripcion,
                      EsContacto = item.Object.EsContacto,
                      EsPorDefecto = item.Object.EsPorDefecto,
                      IdGaleria = item.Object.IdGaleria,
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
        public async Task<Cliente> GetRegistro(Cliente oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("Cliente")
                  .OnceAsync<Cliente>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                            || (a.IdPersona == oRegistro.IdPersona && oRegistro.IdPersona != null)
                                            || (a.Identificacion == oRegistro.Identificacion && oRegistro.Identificacion != null)).FirstOrDefault();
            }
            else
                return null;
        }
        /// <summary>
        /// Método para consultar un registro acorde a filtros
        /// </summary>
        /// <param name="oRegistro"></param>
        /// <returns></returns>
        public async Task<List<Cliente>> GetRegistrosCliente(Cliente oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            try
            {
                if (allRegistros != null)
                {
                    await firebase
                      .Child("Cliente")
                      .OnceAsync<Cliente>();
                    return allRegistros.Where(a => (a.IsDeleted == oRegistro.IsDeleted && oRegistro.valorBusqueda == null)
                                                || (a.Id == oRegistro.Id && oRegistro.Id != null)
                                                || (a.IdPersona == oRegistro.IdPersona && oRegistro.IdPersona != null)
                                                || (a.Identificacion == oRegistro.Identificacion && oRegistro.Identificacion != null)
                                                || (a.Identificacion.ToLower().Contains(oRegistro.valorBusqueda) && oRegistro.valorBusqueda != null)
                                                || (a.Descripcion.ToLower().Contains(oRegistro.valorBusqueda) && oRegistro.valorBusqueda != null)
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
        public async Task InsertaRegistro(Cliente iregistro)
        {
            Cliente registro = await GetRegistro(iregistro);
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
        public async Task AddRegistro(Cliente iregistro)
        {
            try
            {
                await firebase
                  .Child("Cliente")
                  .PostAsync(new Cliente()
                  {
                      Id = iregistro.Id,
                      IdEmpresa=iregistro.IdEmpresa,
                      Empresa = iregistro.Empresa,
                      IdPersona = iregistro.IdPersona,
                      Identificacion = iregistro.Identificacion,
                      Nombre = iregistro.Nombre,
                      IdUnidadOrganizacion = iregistro.IdUnidadOrganizacion,
                      UnidadOrganizacion = iregistro.UnidadOrganizacion,
                      IdGenero = iregistro.IdGenero,
                      Genero = iregistro.Genero,
                      FechaNacimiento = iregistro.FechaNacimiento,
                      Direccion = iregistro.Direccion,
                      Celular = iregistro.Celular,
                      TelefonoFijo = iregistro.TelefonoFijo,
                      Email = iregistro.Email,
                      IdPorcentajeDescuento=iregistro.IdPorcentajeDescuento,
                      PorcentajeDescuento = iregistro.PorcentajeDescuento,
                      Descripcion = iregistro.Descripcion,
                      EsContacto = iregistro.EsContacto,
                      EsPorDefecto = iregistro.EsPorDefecto,
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
        public async Task UpdateRegistro(Cliente iregistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("Cliente")
              .OnceAsync<Cliente>()).Where(a => a.Object.Id == iregistro.Id).FirstOrDefault();

            await firebase
              .Child("Cliente")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Cliente()
              {
                  Id = toUpdateRegistro.Object.Id,
                  IdEmpresa = toUpdateRegistro.Object.IdEmpresa,
                  Empresa = toUpdateRegistro.Object.Empresa,
                  IdPersona = toUpdateRegistro.Object.IdPersona,
                  Identificacion = toUpdateRegistro.Object.Identificacion,
                  Nombre = iregistro.Nombre,
                  IdUnidadOrganizacion = iregistro.IdUnidadOrganizacion,
                  UnidadOrganizacion = iregistro.UnidadOrganizacion,
                  IdGenero = iregistro.IdGenero,
                  Genero = iregistro.Genero,
                  FechaNacimiento = iregistro.FechaNacimiento,
                  Direccion = iregistro.Direccion,
                  Celular = iregistro.Celular,
                  TelefonoFijo = iregistro.TelefonoFijo,
                  Email = iregistro.Email,
                  IdPorcentajeDescuento=iregistro.IdPorcentajeDescuento,
                  PorcentajeDescuento = iregistro.PorcentajeDescuento,
                  Descripcion = iregistro.Descripcion,
                  EsContacto = iregistro.EsContacto,
                  EsPorDefecto = iregistro.EsPorDefecto,
                  IdGaleria = iregistro.IdGaleria,
                  Imagen = iregistro.Imagen,
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
        public async Task DeleteRegistro(Cliente oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("Cliente")
              .OnceAsync<Cliente>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("Cliente").Child(toDeleteRegistro.Key).DeleteAsync();

        }        
        #endregion 3.Métodos
    }
}
