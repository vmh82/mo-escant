using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace DataModel.Infraestructura.Offline.DB
{
    public interface IDBLite
    {
        string DatabasePath();
        SQLiteAsyncConnection GetConnection();
        bool CopyDBToSdCard();
        Dictionary<string, string> CopyDBToTarjeta();
        Dictionary<string, string> RestoreDBTarjeta();
    }
}
