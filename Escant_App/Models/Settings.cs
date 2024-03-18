using System;
using System.Collections.Generic;
using System.Text;
using ManijodaServicios.Resources.Texts;
using Escant_App.Views;

namespace Escant_App.Models
{
    public class Settings
    {
        public Settings(Type type, string icon, string title, string description, bool isEnable = true)
        {
            Type = type;
            Icon = icon;
            Title = title;
            Description = description;
            IsEnable = isEnable;
        }
        public bool IsEnable { get; set; }

        public Type Type { private set; get; }

        public string Icon { private set; get; }

        public string Title { private set; get; }

        public string Description { private set; get; }
    }
}
