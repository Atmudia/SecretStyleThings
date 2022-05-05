using System;

namespace Secret_Style_Things
{
    public static class StringExtensions
    {
        public static string ToTitle(this string str)
        {
            if (str != null)
                return char.ToUpper(str[0]) + str.Substring(1).ToLower();
            throw new Exception("String can't be null");
        }
        public static string RemoveAfterLast(this string value, string delimeter) => !string.IsNullOrEmpty(delimeter) && !string.IsNullOrEmpty(value) && value.Contains(delimeter) ? value.Remove(value.LastIndexOf(delimeter)) : value;
        public static string RemoveBefore(this string value, string delimeter) => !string.IsNullOrEmpty(delimeter) && !string.IsNullOrEmpty(value) && value.Contains(delimeter) ? value.Remove(0,value.IndexOf(delimeter) + delimeter.Length) : value;

        public static string BasicName(this Identifiable.Id id) => id.ToString().ToLowerInvariant().RemoveAfterLast("_");
    }
}