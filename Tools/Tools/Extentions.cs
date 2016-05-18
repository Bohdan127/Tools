using System;
using System.Globalization;
using System.Linq;
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

        public static long? ConvertToLongOrNull(this object data)
        {
            if (IsBlank(data?.ToString())) return null;
            long result;

            if (long.TryParse(data.ToString().Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return result;

            return null;
        }

        public static int? ConvertToIntOrNull(this object data)
        {
            if (IsBlank(data?.ToString())) return null;
            int result;

            if (int.TryParse(data.ToString().Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return result;

            return null;
        }

        public static bool TryConvertStringToDateTime(ref DateTime objData, string sText)
        {
            if (IsBlank(sText)) return false;

            try
            {
                objData = DateTime.Parse(sText,
                    CultureInfo.InvariantCulture,
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

        public static string ConvertToStringOrNull(this object data)
            => data?.ToString().Length > 0 ? data.ToString() : null;

        public static decimal? ConvertToDecimalOrNull(this object data)
        {
            if (IsBlank(data?.ToString())) return null;

            decimal ret;
            if (decimal.TryParse(data.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out ret))
                return ret;
            return null;
        }

        public static double? ConvertToDoubleOrNull(this object data)
        {
            if (IsBlank(data?.ToString())) return null;
            double result;

            if (double.TryParse(data?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                return result;

            return null;
        }

        public static bool ConvertToBool(this object data)
        {
            var ret = false;
            if (IsNotBlank(data?.ToString()))
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

        public static DateTime ConvertStringToDateTime(string dt) => XmlConvert.ToDateTimeOffset(dt).DateTime;

        public static short GetStringSimilarityInPercent(string first, string second, bool clearSpecSymbols)
        {
            if (IsBlank(first) && IsBlank(second)) return 100;
            if (IsBlank(first) || IsBlank(second)) return 0;

            if (clearSpecSymbols)
            {
                Regex rgx = new Regex("[^a-z0-9]");
                first = rgx.Replace(first.ToLower(), "");
                second = rgx.Replace(second.ToLower(), "");
            }
            else
            {
                first = first.ToLower().Trim();
                second = second.ToLower().Trim();
            }

            double length = first.Length < second.Length ? first.Length : second.Length;
            var sameLength = 0;

            for (int i = 0; i < first.Length; i++)
            {
                for (int j = 0; j < second.Length; j++)
                {
                    if (i >= first.Length)
                        break;

                    while (first[i] == second[j])
                    {
                        i++;
                        j++;
                        if (i >= first.Length || j >= second.Length)
                            break;
                        if (first[i] == second[j])
                            sameLength++;
                    }
                }
            }
            if (sameLength != 0)
                sameLength++;

            return Convert.ToInt16(sameLength / length * 100);
        }

        public static double Round(double value, double step = .25)
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            => value % step == 0 ? value : value - value % step + step;
    }
}