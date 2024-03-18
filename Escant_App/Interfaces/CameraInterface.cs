using DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Interfaces
{
    public interface CameraInterface
    {
        void LaunchCamera(FileFormatEnum imageType, string imageId = null);
        void LaunchGallery(FileFormatEnum imageType, string imageId = null);
    }
}
