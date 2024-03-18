using System;
using System.Threading.Tasks;
using System.IO;

namespace Escant_App.Services
{
    public interface ISoundService
    {
        void PlayAudioFile(string fileName);
    }
}
