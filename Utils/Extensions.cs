using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace Secret_Style_Things.Utils
{
    public static class Extensions
    {
        public static T2 GetPrivateField<T1, T2>(this T1 instance, string fieldName)
        {
            var field = typeof(T1).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return (T2) field?.GetValue(instance);
        }
        public static List<T> FindAll<T>(this IEnumerable<T> c, System.Predicate<T> predicate)
        {
            var r = new List<T>();
            foreach (var v in c)
                if (predicate(v))
                    r.Add(v);
            return r;
        }
        public static bool Exists<T>(this IEnumerable<T> c, System.Predicate<T> predicate)
        {
            foreach (var v in c)
                if (predicate(v))
                    return true;
            return false;
        }
        public static int IndexOf<T>(this IEnumerable<T> array, System.Predicate<T> predicate)
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