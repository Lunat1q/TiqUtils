using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
