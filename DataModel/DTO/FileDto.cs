using System;

namespace DataModel.DTO
{
    public class FileDto
    {
        public string FileName { get; set; }
        //public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string FileSize { get; set; }
    }
}
