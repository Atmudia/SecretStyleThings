using System;
using System.Collections.Generic;

namespace Secret_Style_Things.Utils
{
    public static class Extensions
    {
        public static int IndexOf<T>(this IEnumerable<T> array, Predicate<T> predicate)
        {
            int i = 0;
            foreach (var item in array)
            {
                if (predicate(item))
                    return i;
                i++;
            }
            return -1;
        }
    }
}