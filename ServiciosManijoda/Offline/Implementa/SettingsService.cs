using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using DataModel.DTO;
using DataModel.Enums;
using DataModel.Infraestructura.Offline.Interfaces;
using ManijodaServicios.Offline.Interfaz;

namespace ManijodaServicios.Offline.Implementa
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingRepository _settingRepository;

        public SettingsService(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }
        public async Task<Setting> GetSettingAsync(Expression<Func<Setting, bool>> expression)
        {
            var setting = await _settingRepository.FindAsync(expression);
            return setting;
        }
        public async Task<IEnumerable<Setting>> GetAllSettingsAsync()
        {
            var settings = await _settingRepository.GetAsync(x => true);
            return settings;
        }
        //public async Task<bool> IsSettingExists(SettingType settingType)
        //{
        //    var setting = await _settingRepository.FindAsync(x => x.SettingType == (int)settingType);
        //    return setting != null;
        //}
        public async Task<bool> InsertSettingAsync(Setting setting)
        {
            var recordId = await _settingRepository.AddAsync(setting);
            return !string.IsNullOrEmpty(recordId);
        }
        public async Task UpdateSettingAsync(Setting setting)
        {
            await _settingRepository.UpdateAsync(setting);
        }
        public async Task<int> ImportSettingAsync(IEnumerable<Setting> objetos)
        {
            return await _settingRepository.AddRangeAsync(objetos);
        }
    }
}
