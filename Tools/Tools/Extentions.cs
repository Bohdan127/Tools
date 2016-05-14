using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Tools
{
    public static class Extentions
    {
        public static bool IsNotNullOrEmpty(this string str) => !string.IsNullOrEmpty(str);

        public static bool IsBlank(this string str)
            => string.IsNullOrEmpty(str) || str.Cast<char>().All(char.IsWhiteSpace);

        public static bool IsNotBlank(this string str)
            => !string.IsNullOrEmpty(str) && str.Cast<char>().Any(ch => char.IsWhiteSpace(ch) == false);

        public static long? ConvertToLongOrNull(this object ob)
        {
            long result;

            if (long.TryParse(ob.ToString(), out result))
                return result;

            return null;
        }

        public static int? ConvertToIntOrNull(this object ob)
        {
            int result;

            if (int.TryParse(ob.ToString(), out result))
                return result;

            return null;
        }

        public static bool ConvertDateTimeToString(ref DateTime objData, string sText)
        {
            if (string.IsNullOrEmpty(sText)) return false;

            try
            {
                objData = DateTime.Parse(sText,
                    CultureInfo.CurrentCulture,
                    DateTimeStyles.NoCurrentDateDefault);
                return true;
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is ArgumentNullException)
                    return false;
                throw;
            }
        }

        public static string ConvertToStringOrNull(this object data) => data?.ToString().Length > 0 ? data.ToString() : null;

        public static decimal? ConvertToDecimalOrNull(this object data)
        {
            if (string.IsNullOrEmpty(data?.ToString())) return null;

            decimal ret;
            if (decimal.TryParse(data.ToString(), out ret))
                return ret;
            return null;
        }

        public static double? ConvertToDoubleOrNull(this object ob)
        {
            double result;

            if (double.TryParse(ob.ToString(), out result))
                return result;

            return null;
        }

        public static bool ConvertToBool(this object data)
        {
            var ret = false;
            if (data != null)
                bool.TryParse(data.ToString(), out ret);
            return ret;
        }

        public static string ConvertDateTimeToString(DateTime dt)
        {
            switch (dt.Kind)
            {
                case DateTimeKind.Unspecified:
                    dt = new DateTime(dt.Ticks, DateTimeKind.Local);
                    break;
                case DateTimeKind.Utc:
                    dt = dt.ToLocalTime();
                    break;
            }
            return XmlConvert.ToString(dt);
        }

        public static DateTime ConvertStringToDateTime(string dt)   => XmlConvert.ToDateTimeOffset(dt).DateTime;
    }
}
