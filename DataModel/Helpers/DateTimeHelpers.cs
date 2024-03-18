using System;

namespace DataModel.Helpers
{
    public static class DateTimeHelpers
    {
        private static DateTime JanFirst1970 = new DateTime(1970, 1, 1);
        private static DateTime DefaultDate = new DateTime(1980, 1, 1);

        public static long GetDate(DateTime? dateTime = default(DateTime?))
        {
            if (!dateTime.HasValue)
                return (long)((DateTime.UtcNow.ToLocalTime() - JanFirst1970).TotalMilliseconds);
            else
                return (long)((dateTime.Value.ToLocalTime() - JanFirst1970).TotalMilliseconds);
        }
        public static int GetDateString(String fecha)
        {
                return (int)((DateTime.UtcNow.ToLocalTime() - JanFirst1970).TotalMilliseconds);            
        }
        public static DateTime GetDateLong(long? fecha)
        {            
            return !fecha.HasValue?new DateTime(1970, 1, 1).AddMilliseconds(Convert.ToDouble(fecha)): DefaultDate;
        }
        public static DateTime GetDateActual()
        {
            return DateTime.UtcNow;
        }
    }
}
