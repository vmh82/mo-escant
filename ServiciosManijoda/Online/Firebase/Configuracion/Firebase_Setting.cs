using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DTO;
using DataModel.DTO.Configuracion;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using ManijodaServicios.AppSettings;

namespace ManijodaServicios.Online.Firebase.Configuracion
{
    public class Firebase_Setting
    {                
        FirebaseClient firebase = new FirebaseClient(SettingsOnline.ApiFirebase, new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(SettingsOnline.oAuthentication.IdToken) });
        public async Task<List<Setting>> GetAllRegistros()
        {            
            return SettingsOnline.oAuthentication != null ? (await firebase
              .Child("Setting")
              .OnceAsync<Setting>()).Select(item => new Setting
              {                  
                  Id = item.Object.Id,
                  SettingType= item.Object.SettingType,
                  Data = item.Object.Data,
                  Logo = item.Object.Logo
              }).ToList() : null;
        }
        public async Task InsertaRegistro(Setting iregistro)
        {
            Setting registro = await GetRegistro(iregistro);
            if (registro != null)
            {
                await UpdateRegistro(iregistro);
            }
            else
            {
                iregistro.CreatedBy = SettingsOnline.oAuthentication.Email;
                await AddRegistro(iregistro);
            }
        }

        public async Task AddRegistro(Setting iregistro)
        {
            try
            {
                await firebase
                  .Child("Setting")
                  .PostAsync(new Setting()
                  {
                      Id = iregistro.Id,
                      SettingType = iregistro.SettingType,
                      Data = iregistro.Data,
                      Logo = iregistro.Logo,
                  //Datos de Auditoría
                      CreatedBy= SettingsOnline.oAuthentication.Email,                      
                      CreatedDateFormato=iregistro.CreatedDateFormato

              });
            }
            catch(Exception ex)
            {
                var y = ex.Message.ToString();
            }
        }

        public async Task<Setting> GetRegistro(Setting oRegistro)
        {
            var allRegistros = await GetAllRegistros();
            if (allRegistros != null)
            {
                await firebase
                  .Child("Setting")
                  .OnceAsync<Setting>();
                return allRegistros.Where(a => (a.Id == oRegistro.Id && oRegistro.Id != null)
                                            || (a.SettingType == oRegistro.SettingType && oRegistro.SettingType >=0)).FirstOrDefault();
            }
            else
                return null;
        }

        public async Task UpdateRegistro(Setting iRegistro)
        {
            var toUpdateRegistro = (await firebase
              .Child("Setting")
              .OnceAsync<Setting>()).Where(a => a.Object.Id == iRegistro.Id).FirstOrDefault();

            await firebase
              .Child("Setting")
              .Child(toUpdateRegistro.Key)
              .PutAsync(new Setting()
              {
                  Id = iRegistro.Id,
                  SettingType = iRegistro.SettingType,
                  Data = iRegistro.Data,
                  Logo = iRegistro.Logo,
                  //Datos de Auditoría
                  UpdatedBy = SettingsOnline.oAuthentication.Email,
                  CreatedBy= toUpdateRegistro.Object.CreatedBy,
                  CreatedDateFormato = toUpdateRegistro.Object.CreatedDateFormato
              });
        }

        public async Task DeleteRegistro(Setting oRegistro)
        {
            var toDeleteRegistro = (await firebase
              .Child("Setting")
              .OnceAsync<Setting>()).Where(a => a.Object.Id == oRegistro.Id).FirstOrDefault();
            await firebase.Child("Setting").Child(toDeleteRegistro.Key).DeleteAsync();

        }
    }
}
