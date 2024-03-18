using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.DTO.Iteraccion
{
    public class MenuSistema
    {
        /*public MenuSistema(Type type, string icon, string title, string description, FuenteIconos iconoMenu, bool isEnable = true)
        {
            Type = type;
            Icon = icon;
            Title = title;
            Description = description;
            IconoMenu = iconoMenu;
            IsEnable = isEnable;
        }*/
        public bool IsEnable { get; set; }

        public Type Type { get; set; }

        public string Icon { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public FuenteIconos IconoMenu { get; set; }
    }
}
