using DataModel.Base;
using DataModel.DTO.Configuracion;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO
{
    public class Setting : BaseModel
    {
        public int SettingType { get; set; }
        public string Data { get; set; }
        public byte[] Logo { get; set; }        
    }    
}
