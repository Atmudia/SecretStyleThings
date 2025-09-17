using System;
using System.Linq;

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
        public static string RemoveAfterLast(this string value, string delimiter, bool caseInsensitive = false) =>
            !string.IsNullOrEmpty(delimiter) && !string.IsNullOrEmpty(value) &&
            (caseInsensitive ? value.ToLowerInvariant().Contains(delimiter.ToLowerInvariant()) : value.Contains(delimiter))
                ? value.Remove(caseInsensitive 
                    ? value.ToLowerInvariant().LastIndexOf(delimiter.ToLowerInvariant(), StringComparison.InvariantCulture) 
                    : value.LastIndexOf(delimiter, StringComparison.InvariantCulture))
                : value;

        public static string RemoveBefore(this string value, string delimiter, bool caseInsensitive = false) =>
            !string.IsNullOrEmpty(delimiter) && !string.IsNullOrEmpty(value) &&
            (caseInsensitive ? value.ToLowerInvariant().Contains(delimiter.ToLowerInvariant()) : value.Contains(delimiter))
                ? value.Remove(0, (caseInsensitive 
                    ? value.ToLowerInvariant().IndexOf(delimiter.ToLowerInvariant(), StringComparison.InvariantCulture) 
                    : value.IndexOf(delimiter, StringComparison.InvariantCulture)) + delimiter.Length)
                : value;

        public static string BasicName(this Identifiable.Id id) => id.ToString().ToLowerInvariant().RemoveAfterLast("_");

        public static string[] MatchCase(this string[] o, string[] toMatch)
        {
            o = o.ToArray();
            var l1 = o.Count(x => x != "");
            var l2 = toMatch.Count(x => x != "");
            if (l1 == 0 || l2 == 0)
                return o;
            if (l1 == l2)
            {
                var i = 0;
                var j = 0;
                while (i < o.Length && j < toMatch.Length)
                {
                    if (o[i] == "")
                        i++;
                    else if (toMatch[j] == "")
                        j++;
                    else
                    {
                        o[i] = o[i].MatchCase(toMatch[j]);
                        i++;
                        j++;
                    }
                }
            }
            else if (l1 == 1 || l2 == 1)
            {
                var j = toMatch.First(x => x != "");
                for (var i = 0; i < o.Length; i++)
                    if (o[i] != "")
                        o[i] = o[i].MatchCase(j);
            }
            else
            {
                var k = 0;
                for (var i = 0; i < o.Length; i++)
                    if (o[i] != "")
                    {
                        var j = (int)Math.Round((double)k / l1 * l2);
                        o[i] = o[i].MatchCase(toMatch.First(x => x != "" && j-- == 0));
                        k++;
                    }
            }
            return o;
        }
        public static string MatchCase(this string o, string toMatch)
        {
            if (o.Length == 0 || toMatch.Length == 0)
                return o;
            var r = "";
            if (o.Length == toMatch.Length)
                for (int i = 0; i < o.Length; i++)
                    r += o[i].MatchCase(toMatch[i]);
            else
            {
                r += o[0].MatchCase(toMatch[0]);
                for (var i = 1; i < o.Length; i++)
                    r += o[i].MatchCase(toMatch[Math.Min((int)Math.Ceiling((double)i / o.Length * toMatch.Length), toMatch.Length - 1)]);
            }
            return r;
        }
        public static char MatchCase(this char o, char toMatch) => char.ToLowerInvariant(toMatch) == toMatch ? char.ToLowerInvariant(o) : char.ToUpperInvariant(o);
    }
}