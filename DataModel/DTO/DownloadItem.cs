using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO
{
    public class DownloadItem
    {
        public DownloadItem(string id, byte[] data)
        {
            Id = id;
            Data = data;
        }
        public byte[] Data { get; set; }
        public string Id { get; set; }
    }
}
