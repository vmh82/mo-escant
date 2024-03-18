using DataModel.Infraestructura.Offline.DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Infraestructura.Offline.Interfaces
{
    interface ISyncTable<in TApiDto>
    {
        Task BindData(TApiDto apiDto, DbContext dbContext);
    }
}
