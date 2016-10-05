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

            if (long.TryParse(data?.ToString().Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                return result;

            return null;
        }

        [Pure]
        public static int? ConvertToIntOrNull(this object data)
        {
            if (IsBlank(data?.ToString())) return null;
            int result;

            if (int.TryParse(data?.ToString().Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out result))
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
            if (IsBlank(data?.ToString()))
                return null;

            // ReSharper disable once PossibleNullReferenceException
            var value = data.ToString();
            value = value.Replace(",",
               CultureInfo.CurrentUICulture.NumberFormat.PercentDecimalSeparator);
            value = value.Replace(".",
                CultureInfo.CurrentUICulture.NumberFormat.PercentDecimalSeparator);

            decimal ret;
            if (decimal.TryParse(value,
                NumberStyles.Any,
                CultureInfo.CurrentCulture,
                out ret))
                return ret;
            return null;
        }

        [Pure]
        public static double? ConvertToDoubleOrNull(this object data)
        {
            if (IsBlank(data?.ToString())) return null;

            // ReSharper disable once PossibleNullReferenceException
            var value = data.ToString();
            value = value.Replace(",",
                CultureInfo.CurrentUICulture.NumberFormat.PercentDecimalSeparator);
            value = value.Replace(".",
                CultureInfo.CurrentUICulture.NumberFormat.PercentDecimalSeparator);

            double result;
            if (double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                return result;

            return null;
        }

        [Pure]
        public static bool ConvertToBool(this object data)
        {
            var ret = false;
            if (IsNotBlank(data?.ToString()))
                bool.TryParse(data?.ToString(), out ret);
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
                var rgx = new Regex(@"[\%\/\\\&\?\,\'\;\:\!\-\|\.\,\@\#\(\)\s]");
                first = rgx.Replace(first.ToLower(), "");
                second = rgx.Replace(second.ToLower(), "");
            }
            else
            {
                first = first.ToLower().Trim();
                second = second.ToLower().Trim();
            }

            var isFirst = first.Length < second.Length;
            var sameLength = 0;

            if (isFirst)
                for (var i = 0; i < first.Length; i++)
                {
                    for (var j = 0; j < second.Length; j++)
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
            else
                for (var i = 0; i < second.Length; i++)
                {
                    for (var j = 0; j < first.Length; j++)
                    {
                        if (i >= second.Length)
                            break;

                        while (second[i] == first[j])
                        {
                            i++;
                            j++;
                            if (i >= second.Length || j >= first.Length)
                                break;
                            if (second[i] == first[j])
                                sameLength++;
                        }
                    }
                }
            if (sameLength != 0)
                sameLength++;
            double length = (isFirst
                ? first.Length
                : second.Length);
            return Convert.ToInt16(sameLength / length * 100);
        }

        [Pure]
        public static short GetStringSimilarityForSportTeams(string first, string second, bool clearSpecSymbols, string dateFirst, string dateSecond)
        {
            if (dateSecond != dateFirst)
                return 0;
            var rgx2 = new Regex(@"U|O^[A-Za-z]?\d+");
            if ((rgx2.IsMatch(first) && !rgx2.IsMatch(second)) || (!rgx2.IsMatch(first) && rgx2.IsMatch(second)))
                return 0;

            return GetStringSimilarityInPercent(first,
                 second,
                 clearSpecSymbols);
        }

        [Pure]
        public static double Round(double value, double step = .25)
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            => value % step == 0 ? value : value - value % step + step;
    }
}