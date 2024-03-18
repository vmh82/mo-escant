using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DataModel
{
    public static class LableName
    {

        public static Dictionary<string, Dictionary<LabelName, string>> LABELS = new Dictionary<string, Dictionary<LabelName, string>>
        {
            { "vi", new Dictionary<LabelName, string>
                {
                    {LabelName.ProductName,"Ten"},
                    {LabelName.Quantity,"SL"},
                    {LabelName.Price,"Gia"},
                    {LabelName.TotalPriceByItem,"T.Tien"},
                    {LabelName.ProvisionalSum,"TAM TINH"},
                    {LabelName.DiscountAmount,"Giam gia"},
                    {LabelName.TotalPrice,"TONG TIEN"},
                    {LabelName.ReceivedMoney,"Khach dua"},
                    {LabelName.Refund,"Tien thoi"},
                    {LabelName.ReceiptDate,"Ngay: "},
                    {LabelName.Mobile,"DT: "},
                }
            },
            { "en", new Dictionary<LabelName, string>
                {
                    {LabelName.ProductName,"Item"},
                    {LabelName.Quantity,"Qty"},
                    {LabelName.Price,"Price"},
                    {LabelName.TotalPriceByItem,"Total"},
                    {LabelName.ProvisionalSum,"PROVISIONAL SUMS"},
                    {LabelName.DiscountAmount,"Discount"},
                    {LabelName.TotalPrice,"TOTAL"},
                    {LabelName.ReceivedMoney,"Received"},
                    {LabelName.Refund,"Refund"},
                    {LabelName.ReceiptDate,"Date: "},
                    {LabelName.Mobile,"Mobile: "},
                }
            },
            { "es", new Dictionary<LabelName, string>
                {
                    {LabelName.ProductName,"Item"},
                    {LabelName.Quantity,"Cant"},
                    {LabelName.Price,"Precio"},
                    {LabelName.TotalPriceByItem,"Total"},
                    {LabelName.ProvisionalSum,"SUBTOTAL"},
                    {LabelName.DiscountAmount,"Descuento"},
                    {LabelName.TotalPrice,"TOTAL"},
                    {LabelName.ReceivedMoney,"Recibido"},
                    {LabelName.Refund,"Cambio"},
                    {LabelName.ReceiptDate,"Fecha: "},
                    {LabelName.Mobile,"Celular: "},
                }
            }

        };

        public static string GetText(LabelName key)
        {
            var langCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if (langCode.Equals("vi", StringComparison.CurrentCultureIgnoreCase))
                return LABELS["vi"][key];
            else
                return LABELS["en"][key];
        }

        public static string GetTextLanguage(LabelName key, string langCode)
        {
            //var langCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            if (langCode.Equals(langCode, StringComparison.CurrentCultureIgnoreCase))
                return LABELS[langCode][key];
            else
                return LABELS["en"][key];
        }
    }
    public enum LabelName
    {
        ProductName,
        Quantity,
        Price,
        TotalPriceByItem,
        ProvisionalSum,
        TotalPrice,
        DiscountAmount,
        ReceivedMoney,
        Refund,
        ReceiptDate,
        Mobile
    }
}
