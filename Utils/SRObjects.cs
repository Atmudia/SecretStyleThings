using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Secret_Style_Things.Utils
{
    internal static class SRObjects
    {
        private static readonly Dictionary<Type, Object[]> cache = new Dictionary<Type, Object[]>();
        public static T Get<T>(string name) where T : Object
        {
            Type selected = typeof(T);
            if (!cache.ContainsKey(selected))
                cache.Add(selected, Resources.FindObjectsOfTypeAll<T>());
 
            T found = (T)cache[selected].FirstOrDefault(x => x && x.name == name);
            if (found == null)
            {
                cache[selected] = Resources.FindObjectsOfTypeAll<T>();
                found = (T)cache[selected].FirstOrDefault(x => x && x.name == name);
            }
 
            return found;
        }
        
    }
}