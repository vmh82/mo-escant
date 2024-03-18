using DataModel;
using DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Escant_App.Common
{
    public class SystemConstants
    {
        public static string VN_CURRENTCY_FORMAT = "#,##0";
        //public static string VN_CURRENTCY_FORMAT = "#,##.00";
        public static string DATE_FORMAT_DDMM = "dd/MM";
        public static string DATE_FORMAT_DDMMYYYY = "dd/MM/yyyy";
        public static Dictionary<PaperSize, Dictionary<LableNameConfigPostion, int[]>> POSITION_CONFIG = new Dictionary<PaperSize, Dictionary<LableNameConfigPostion, int[]>>
        {
            { PaperSize.Size_58, new Dictionary<LableNameConfigPostion, int[]>
                {
                    { LableNameConfigPostion.NameLable, new int[]{0, 16}},
                    { LableNameConfigPostion.QuantityLable, new int[]{18, 21}},
                    { LableNameConfigPostion.PriceLable, new int[]{22, 31}},
                    { LableNameConfigPostion.TotalPriceLable, new int[]{32, 42}}
                }
            },
            { PaperSize.Size_80, new Dictionary<LableNameConfigPostion, int[]>
                {
                    { LableNameConfigPostion.NameLable, new int[]{0, 20}},
                    { LableNameConfigPostion.QuantityLable, new int[]{21, 24}},
                    { LableNameConfigPostion.PriceLable, new int[]{24, 36}},
                    { LableNameConfigPostion.TotalPriceLable, new int[]{36, 48}}
                }
            }
        };
        public static Dictionary<PaperSize, int> NUM_CHARS_LARGE_FONT_CONFIG = new Dictionary<PaperSize, int>
        {
            { PaperSize.Size_58, 42},
            { PaperSize.Size_80, 64},
        };
        public static Dictionary<PaperSize, int> NUM_CHARS_NORMAL_FONT_CONFIG = new Dictionary<PaperSize, int>
        {
            { PaperSize.Size_58, 32},
            { PaperSize.Size_80, 48},
        };
        public static string NEW_LINE = "\n";

    }
}
