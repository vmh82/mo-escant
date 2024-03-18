using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DataModel.Helpers
{
    public static class Comunes
    {
        public static string FormatoPrecioSinSimbolo(this float price)
        {
                        
                var currentCulture = CultureInfo.CurrentCulture;
                var specName = CultureInfo.CreateSpecificCulture(currentCulture.Name).Name;
                return String.Format(CultureInfo.GetCultureInfo(specName), "{0:N}", price);
            
        }
    }
}
