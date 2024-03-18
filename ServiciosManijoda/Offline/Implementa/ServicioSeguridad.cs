using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using DataModel.Helpers;
using DataModel.DTO.Seguridad;
using DataModel.Infraestructura.Offline.Interfaces;
using ManijodaServicios.Offline.Interfaz;
using ManijodaServicios.AppSettings;
using ManijodaServicios.Helpers;

namespace ManijodaServicios.Offline.Implementa
{
    #region Usuario
    public class ServicioSeguridad_Usuario : IServicioSeguridad_Usuario
    {        
        private readonly ISeguridadRepositorio _objetoRepository;

        public ServicioSeguridad_Usuario(ISeguridadRepositorio generalRepositorio, IRequestService requestService, IDialogService dialogService)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<Usuario>> GetAllUsuarioAsync(Expression<Func<Usuario, bool>> expression)
        {
            try
            {
                var objetos = await _objetoRepository.GetAsync(expression);

                if (objetos != null)
                    //return objetos.ToObservableCollection();
                    return new ObservableCollection<Usuario>(objetos);
                else
                    return new ObservableCollection<Usuario>();
            }
            catch(Exception ex)
            {
                var y = ex.Message.ToString();
                return new ObservableCollection<Usuario>();
            }
        }

        public async Task<bool> ConsultaUsuario(Usuario item)
        {
            
            IEnumerable<Usuario> resultado = await SELECT_FILTROS(item);

            return resultado.Count() > 0 ? true : false;
        }

        public async Task<string> InsertUsuarioAsync(Usuario objeto)
        {
            IEnumerable<Usuario> resultado = await SELECT_WHERE(objeto);
            if (resultado.Count() > 0)
            {
                var actualiza = (Usuario)resultado.First();
                actualiza.Clave=objeto.Clave;
                await _objetoRepository.UpdateAsync(actualiza);
                return "0";
            }
            else
            {
                return await _objetoRepository.AddAsync(objeto);
            }
        }
        public async Task UpdateUsuarioAsync(Usuario objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeleteUsuarioAsync(Usuario objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportUsuarioAsync(IEnumerable<Usuario> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        void SaveAuthenticationResult(TokenModel result)
        {
            var user = AuthenticationResultHelper.GetUserFromResult(result);
            GlobalSettings.User = user;
        }

        private async Task<IEnumerable<Usuario>> SELECT_WHERE(Usuario item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM Usuario";
            sql = sql + " where Email=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Email
                                                                             })
                                                                              ).AsEnumerable();


        }

        private async Task<IEnumerable<Usuario>> SELECT_FILTROS(Usuario item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM Usuario";
            sql = sql + " where Email=? and Clave=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Email
                                                                                ,item.Clave
                                                                             })
                                                                              ).AsEnumerable();
        }
    }
    #endregion Usuario

    #region PerfilMenu
    /// <summary>
    /// Métodos para la Administración de Cliente
    /// </summary>
    public class ServicioSeguridad_PerfilMenu : IServicioSeguridad_PerfilMenu
    {
        private readonly IPerfilMenuRepositorio _objetoRepository;

        public ServicioSeguridad_PerfilMenu(IPerfilMenuRepositorio generalRepositorio)
        {
            _objetoRepository = generalRepositorio;
        }

        public async Task<ObservableCollection<PerfilMenu>> GetAllPerfilMenuAsync(Expression<Func<PerfilMenu, bool>> expression)
        {
            var objetos = await _objetoRepository.GetAsync(expression);
            if (objetos != null)
            {
                //return objetos.ToObservableCollection();
                return new ObservableCollection<PerfilMenu>(objetos);
            }
            else
                return new ObservableCollection<PerfilMenu>();
        }
        
        public async Task<string> InsertPerfilMenuAsync(PerfilMenu objeto)
        {
            IEnumerable<PerfilMenu> resultado = await SELECT_WHERE(objeto);
            if (resultado.Count() > 0)
            {
                //Datos de Auditoría
                objeto.CreatedBy = resultado.FirstOrDefault().CreatedBy;

                await _objetoRepository.UpdateAsync(objeto);
                return "0";
            }
            else
            {
                return await _objetoRepository.AddAsync(objeto);
            }
        }
        public async Task UpdatePerfilMenuAsync(PerfilMenu objeto)
        {
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task DeletePerfilMenuAsync(PerfilMenu objeto)
        {
            objeto.Deleted = 1;
            objeto.UpdatedDate = DateTimeHelpers.GetDate(DateTime.UtcNow.ToLocalTime());
            await _objetoRepository.UpdateAsync(objeto);
        }
        public async Task<int> ImportPerfilMenuAsync(IEnumerable<PerfilMenu> objetos)
        {
            return await _objetoRepository.AddRangeAsync(objetos);
        }

        private async Task<IEnumerable<PerfilMenu>> SELECT_WHERE(PerfilMenu item)
        {
            string sql = "";
            sql = sql + "SELECT * FROM PerfilMenu";
            sql = sql + " where Id=?";
            return (await _objetoRepository.ExecuteScriptsWithParameterAsync(sql
                                                                            , new object[] {
                                                                                item.Id
                                                                             })
                                                                              ).AsEnumerable();


        }

    }
    #endregion PerfilMenu
}