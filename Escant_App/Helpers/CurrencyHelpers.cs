using ManijodaServicios.Resources.Texts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Escant_App.Helpers
{
    public static class CurrencyHelpers
    {
        public static string FormatPrice(this float price)
        {
            bool isUseDefaultCurrencyFormat = AppSettings.Settings.IsUseDefaultCurrencyFormat;
            string customFormat = AppSettings.Settings.CurrentCurrencyCustomFormat;
            string customCurrencyCode = AppSettings.Settings.CustomCurrencySymbol;

            if (!string.IsNullOrEmpty(customFormat) && !isUseDefaultCurrencyFormat)
            {
                if (!string.IsNullOrEmpty(customCurrencyCode))
                    return $"{customCurrencyCode} {price.ToString(customFormat)}";
                else
                    return price.ToString(customFormat);
            }
            else
            {
                var currentCulture = CultureInfo.CurrentCulture;
                var specName = CultureInfo.CreateSpecificCulture(currentCulture.Name).Name;
                return String.Format(CultureInfo.GetCultureInfo(specName), "{0:c}", price);
            }
        }
        public static string FormatPriceWithoutSymbol(this float price)
        {
            bool isUseDefaultCurrencyFormat = AppSettings.Settings.IsUseDefaultCurrencyFormat;
            string customFormat = AppSettings.Settings.CurrentCurrencyCustomFormat;

            if (!string.IsNullOrEmpty(customFormat) && !isUseDefaultCurrencyFormat)
            {
                return price.ToString(customFormat);
            }
            else
            {
                var currentCulture = CultureInfo.CurrentCulture;
                var specName = CultureInfo.CreateSpecificCulture(currentCulture.Name).Name;
                return String.Format(CultureInfo.GetCultureInfo(specName), "{0:N}", price);
            }
        }

        public static string FormatValor(this string stringValue)
        {
            CultureInfo ci = AppResources.Culture;
            try
            {
                if (string.IsNullOrEmpty(stringValue))
                    return "0";

                stringValue = System.Convert.ToString(stringValue, ci);
                // currency -> double (format to double)
                var currency = decimal.Parse(stringValue, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, ci);

                var formato=Escant_App.AppSettings.Settings.CurrentCurrencyCustomFormat;
                stringValue = currency.ToString(formato, ci);

                if (stringValue.Contains(".")|| stringValue.Contains(","))
                    stringValue = $"0{stringValue}";

                return stringValue;
            }
            catch
            {
                return "0";
            }

        }


    }
}
