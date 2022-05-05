using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable MemberCanBePrivate.Global
namespace Secret_Style_Things.Utils
{
    public static class SRObjects
    {
        public static T Get<T>(string name) where T : Object
        {
            foreach (T found in Resources.FindObjectsOfTypeAll<T>())
            {
                if (found.name.Equals(name))
                    return found;
            }

            return null;
        }
        public static List<T> GetAll<T>() where T : Object
        {
            return new List<T>(Resources.FindObjectsOfTypeAll<T>());
        }
        
    }
}