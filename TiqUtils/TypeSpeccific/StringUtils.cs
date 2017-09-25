using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TiqUtils.TypeSpeccific
{
    public static class StringUtils
    {
        private static readonly Random Random = new Random();
        public static string RandomString(int length, string chars = "abcdefghijklmnopqrstuvwxyz0123456789")
        {
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public static IEnumerable<string> SplitByRows(this string text)
        {
            string[] splitChars = {"\r", "\r\n", "\r"};
            var result = text.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            return result;
        }

        public static int GetNumbers(this string text)
        {
            var num = Regex.Match(text, @"\d+").Value;
            int res;
            int.TryParse(num, out res);
            return res;
        }

        public static bool Empty(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static bool MatchPattern(this string text, string pattern)
        {
            var regex = new Regex(pattern);
            var match = regex.Match(text);
            return match.Success;
        }

        public static string GetByPattern(this string text, string pattern)
        {
            var regex = new Regex(pattern);
            var match = regex.Match(text);
            return match.Value;
        }

        public static string RemoveUnwantedChars(this string input, char[] unwanted)
        {
            var builder = new StringBuilder(input.Length);
            foreach (var t in input)
            {
                if (unwanted.Contains(t)) continue;
                builder.Append(t);
            }
            return builder.ToString();
        }

        public static string WrapTimeStamp(this string text)
        {
            return $"[{DateTime.Now:HH:mm}] {text}";
        }

        private static string Conversion(string str1, string str2)
        {

            if (str1.Contains(".") && (str2 != "."))
                return str1.Replace('.', ',');
            if (str1.Contains(",") && (str2 != ","))
                return str1.Replace(',', '.');
            return str1;
        }

        public static bool GetVal<T>(this string value, out T resultVal) where T : IConvertible
        {
            resultVal = default(T);
            if (resultVal == null) return false;
            var typeCode = resultVal.GetTypeCode();
            switch (typeCode)
            {
                case TypeCode.Double:
                    {
                        double result;
                        var nfi = NumberFormatInfo.CurrentInfo;
                        var currentDecimalSeparator = nfi.CurrencyDecimalSeparator;
                        value = Conversion(value, currentDecimalSeparator);
                        var res = double.TryParse(value, out result);
                        if (!res) return false;
                        var changeType = Convert.ChangeType(result, typeCode);
                        if (changeType != null)
                            resultVal = (T)changeType;
                        return true;
                    }
                case TypeCode.Single:
                    {
                        float result;
                        var nfi = NumberFormatInfo.CurrentInfo;
                        var currentDecimalSeparator = nfi.CurrencyDecimalSeparator;
                        value = Conversion(value, currentDecimalSeparator);
                        var res = float.TryParse(value, out result);
                        if (!res) return false;
                        var changeType = Convert.ChangeType(result, typeCode);
                        if (changeType != null)
                            resultVal = (T)changeType;
                        return true;
                    }
                case TypeCode.Int32:
                    {
                        int result;
                        var res = int.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result)
                                  || int.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result)
                                  || int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
                        if (!res) return false;
                        var changeType = Convert.ChangeType(result, typeCode);
                        if (changeType != null)
                            resultVal = (T)changeType;
                        return true;
                    }
                case TypeCode.Boolean:
                    {
                        bool result;
                        var res = bool.TryParse(value, out result);
                        if (!res) return false;
                        var changeType = Convert.ChangeType(result, typeCode);
                        if (changeType != null)
                            resultVal = (T)changeType;
                        return true;
                    }
            }
            return false;
        }

        public static int ComputeStringDifference(this string s, string t, bool ignoreCase = false)
        {
            if (ignoreCase)
            {
                s = s.ToLower();
                t = t.ToLower();
            }
            int n = s.Length, m = t.Length;
            var d = new int[n + 1, m + 1];

            if (n == 0)
                return m;

            if (m == 0)
                return n;


            for (var i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (var j = 0; j <= m; d[0, j] = j++)
            {
            }

            for (var i = 1; i <= n; i++)
            for (var j = 1; j <= m; j++)
            {
                var cost = t[j - 1] == s[i - 1] ? 0 : 1;

                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
            }
            return d[n, m];
        }

        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
