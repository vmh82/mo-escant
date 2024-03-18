using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Escant_App.Services
{
    public interface IDownloadImageService
    {
        Task DownloadImage(string url, string id);
    }
}
