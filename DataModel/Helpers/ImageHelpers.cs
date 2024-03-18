using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DataModel.Helpers
{
    public static class ImageHelpers
    {
        private static object locker = new object();
        public static byte[] GetByteFromImageSource(this ImageSource imageSource)
        {
            if (imageSource is StreamImageSource)
                return GetByteFromStreamImageSource(imageSource);
            else
                return GetByteFromFileImageSource(imageSource);

        }
        private static byte[] GetByteFromStreamImageSource(ImageSource imageSource)
        {
            StreamImageSource streamImageSource = (StreamImageSource)imageSource;
            System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            var stream = task.Result;
            byte[] data = new byte[stream.Length];
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                stream.Close();
                return memoryStream.ToArray();
            }
        }
        private static byte[] GetByteFromFileImageSource(ImageSource imageSource)
        {
            lock (locker)
            {
                FileImageSource fileImageSource = (FileImageSource)imageSource;
                

                using (var file = File.Open(fileImageSource.File, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] data = new byte[file.Length];
                    file.Read(data, 0, data.Length);
                    file.Close();
                    return data;
                }
            }
        }
        public static byte[] GenerateEmptyImage()
        {
            var imageSource = ImageSource.FromStream(() =>
            {
                var stream = new MemoryStream();
                return stream;
            });
            return imageSource.GetByteFromImageSource();
        }
    }
}
