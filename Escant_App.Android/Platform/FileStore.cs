using System.IO;
using Escant_App.Interfaces;

namespace Escant_App.Droid.Platform
{
    public class FileStore : IFileStore
    {
        public string GetFilePath()
        {
            return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "image.png");
        }
    }
}