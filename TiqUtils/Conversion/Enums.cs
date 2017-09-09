using System;

namespace TiqUtils.Conversion
{
    public static class Enums
    {
        public static int SwitchEnumInt<T>(this Enum g)
        {
            var next = Convert.ToInt32(g) + 1;

            return Enum.IsDefined(typeof(T), next) ? next : 0;
        }
        public static T SwitchEnum<T>(this T g)
        {
            var nt = SwitchEnumInt<T>(g as Enum);
            return (T) (object)nt;
        }
    }
}