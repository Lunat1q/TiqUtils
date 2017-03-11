using System.Collections.Generic;

namespace TiqUtils.Conversion
{
    public static class Collection
    {
        public static int IndexOf<T>(this IEnumerable<T> array, T element)
        {
            if (array == null) return -1;
            var i = 0;
            foreach (var item in array)
            {
                if (item.Equals(element))
                    return i;
                i++;
            }
            return -1;
        }
    }
}
