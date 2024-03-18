using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Iteraccion
{
    public class MainMenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }
        public FuenteIconos IconoMenu { get; set; }
    }
}
