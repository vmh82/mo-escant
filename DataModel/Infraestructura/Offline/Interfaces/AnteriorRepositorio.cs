using DataModel;
using DataModel.Infraestructura.Offline.DB;
using DataModel.Infraestructura.Offline.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DataModel.DTO;

namespace DataModel.Infraestructura.Offline.Interfaces
{    

    public class SettingRepository : GenericSqliteRepository<Setting>, ISettingRepository
    {
        public SettingRepository(DbContext context) : base(context)
        {
        }
    } 

}
