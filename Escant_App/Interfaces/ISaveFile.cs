using System;
using System.IO;
using System.Threading.Tasks;

namespace Escant_App.Interfaces
{
    public interface ISaveFile
    {
        Task<bool> WriteBackExcelFile(string filePath, string contentType, MemoryStream stream);
        Task<bool> Save(string filename, string contentType, MemoryStream stream);
        string SaveMemory(MemoryStream fileStream);
        string SaveMemory2(MemoryStream fileStream);
        void SaveFile(string filename, string contentType, MemoryStream stream);
        Task<byte[]> CaptureAsync();
    }
}
