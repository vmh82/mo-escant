using DataModel.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ManijodaServicios.Offline.Interfaz
{
    public interface ISettingsService
    {
        Task<Setting> GetSettingAsync(Expression<Func<Setting, bool>> expression);
        Task<IEnumerable<Setting>> GetAllSettingsAsync();
        //Task<bool> IsSettingExists(SettingType settingType);
        Task<int> ImportSettingAsync(IEnumerable<Setting> objetos);
        Task<bool> InsertSettingAsync(Setting setting);
        Task UpdateSettingAsync(Setting setting);
    }
}
