using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Helpers
{
    public static class Generator
    {
        public static string GenerateOrderNo()
        {
            var now = DateTime.Now;
            var timeInMilliSecond = now.Ticks.ToString();
            var last4Digit = timeInMilliSecond.Substring(timeInMilliSecond.Length - 4);
            var orderNo = $"{now.ToString("yyyy")}{now.ToString("MM")}{now.ToString("dd")}{last4Digit}";
            return orderNo;
        }
        public static string GenerateProductCode()
        {
            var now = DateTime.Now;
            var timeInMilliSecond = now.Ticks.ToString();
            var last4Digit = timeInMilliSecond.Substring(timeInMilliSecond.Length - 4);
            var code = $"{now.ToString("yyyy")}{now.ToString("MM")}{now.ToString("dd")}{last4Digit}";
            return code;
        }
        public static string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }
        public static string GenerarOrdenNo(string tipo,string numero)
        {
            string tabs = new string('0', 10 - numero.Length);
            return tipo + "-" + tabs + numero;
        }
    }
}
