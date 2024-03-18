using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Escant_App.Common.Extensions
{
    public static class CommonExtensions
    {
        static Dictionary<string, string> VN_CHARACTERS = new Dictionary<string, string>
        {
            { "ÀÁÂÃĂẢẠẦẪẨẬẰẮẴẲẶА", "A" },
            { "àáâãăảạầấẫẩậằắẵẳặа", "a" },
            { "Ð", "D" },
            { "đ", "d" },
            { "ÈÉÊĔĚẼẺẸỀẾỄỂỆЕ", "E" },
            { "èéêĕẽẻẹềếễểệе", "e" },
            { "ÌÍĨĬǏΙỈỊ", "I" },
            { "ìíîĩĭǐỉị", "i" },
            { "ÒÓÔÕŎǑƠΌỎỌỒỐỖỔỘỜỚỠỞỢО", "O" },
            { "òóôõŏǒơοόỏọồốỗổộờớỡởợо", "o" },
            { "ÙÚŨƯŨỦỤỪỨỮỬỰ", "U" },
            { "ùúûũưủụừứữửự", "u" },
            { "ÝΎỲỸỶỴ", "Y" },
            { "ýyỳỹỷỵ", "y" }
        };
        //public static char RemoveDiacritics(this char c)
        //{
        //    foreach (KeyValuePair<string, string> entry in VN_CHARACTERS)
        //    {
        //        if (entry.Key.IndexOf(c) != -1)
        //        {
        //            return entry.Value[0];
        //        }
        //    }
        //    return c;
        //}
        public static string RemoveDiacritics(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;

            s = s.Normalize(NormalizationForm.FormD);
            var chars = s.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            s = new string(chars).Normalize(NormalizationForm.FormC);

            //StringBuilder sb = new StringBuilder ();
            string text = "";

            foreach (char c in s)
            {
                int len = text.Length;

                foreach (KeyValuePair<string, string> entry in VN_CHARACTERS)
                {
                    if (entry.Key.IndexOf(c) != -1)
                    {
                        text += entry.Value;
                        break;
                    }
                }
                if (len == text.Length)
                {
                    text += c;
                }
            }
            return text;
        }
        public static string[] CutString(this string source, int length)
        {
            if (source == null || source.Length < length)
            {
                return new string[2] { source, "" };
            }
            int nextSpace = source.LastIndexOf(" ", length);
            string[] results = new string[2];
            results[0] = string.Format("{0}", source.Substring(0, (nextSpace > 0) ? nextSpace : length).Trim());
            results[1] = source.Substring(nextSpace + 1);
            return results;
        }
        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
