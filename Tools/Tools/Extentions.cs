using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace ToolsPortable
{
    public static class Extentions
    {
        [Pure]
        public static bool IsNotNullOrEmpty(this string str) => !string.IsNullOrEmpty(str);

        [Pure]
        public static bool IsBlank(this string str)
            => string.IsNullOrEmpty(str) || str.Cast<char>().All(char.IsWhiteSpace);

        [Pure]
        public static bool IsNotBlank(this string str)
            => !string.IsNullOrEmpty(str) && str.Cast<char>().Any(ch => char.IsWhiteSpace(ch) == false);

        [Pure]
        public static long? ConvertToLongOrNull(this object data)
        {
            if (IsBlank(data?.ToString())) return null;
            long result;

            if (long.TryParse(data.ToString().Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                return result;

            return null;
        }

        [Pure]
        public static int? ConvertToIntOrNull(this object data)
        {
            if (IsBlank(data?.ToString())) return null;
            int result;

            if (int.TryParse(data.ToString().Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                return result;

            return null;
        }

        [Pure]
        public static bool TryConvertStringToDateTime(ref DateTime objData, string sText)
        {
            if (IsBlank(sText)) return false;

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

        [Pure]
        public static string ConvertToStringOrNull(this object data)
            => data?.ToString().Length > 0 ? data.ToString() : null;

        [Pure]
        public static decimal? ConvertToDecimalOrNull(this object data)
        {
            if (IsBlank(data?.ToString())) return null;

            decimal ret;
            if (decimal.TryParse(data.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out ret))
                return ret;
            return null;
        }

        [Pure]
        public static double? ConvertToDoubleOrNull(this object data)
        {
            if (IsBlank(data?.ToString())) return null;
            double result;

            if (double.TryParse(data?.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                return result;

            return null;
        }

        [Pure]
        public static bool ConvertToBool(this object data)
        {
            var ret = false;
            if (IsNotBlank(data?.ToString()))
                bool.TryParse(data.ToString(), out ret);
            return ret;
        }

        [Pure]
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

        [Pure]
        public static DateTime ConvertStringToDateTime(string dt) => XmlConvert.ToDateTimeOffset(dt).DateTime;

        [Pure]
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

        [Pure]
        public static double Round(double value, double step = .25)
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            => value % step == 0 ? value : value - value % step + step;
    }
}