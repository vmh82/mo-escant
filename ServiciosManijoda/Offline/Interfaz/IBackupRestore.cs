using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataModel.DTO;

namespace ManijodaServicios.Offline.Interfaz
{
    public interface IBackupRestore
    {
        Task<bool> BackupDb(string backupFileName, string dbFileName);
        Task<bool> RestoreDb(string restoreFileName, string destDbFileName);
        Task<List<FileDto>> GetAllBackupFiles();
        Task<List<FileDto>> GetAllCertificados();
        Task<string> GetScriptTexts();
    }
}
