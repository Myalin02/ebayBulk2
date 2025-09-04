using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EbayBulk_Generator.Helpers
{
    public static class SkuNormalizer
    {
        public static string Normalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            string upper = input.ToUpperInvariant();
            upper = upper.Replace("Ä", "AE").Replace("Ö", "OE").Replace("Ü", "UE");
            upper = upper.Replace("ß", "SS");
            upper = Regex.Replace(upper, "[^A-Z0-9]", "");
            return upper;
        }
    }

    public static class TitleShortener
    {
        public static string Shorten(string input, int maxLength = 80)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Length <= maxLength ? input : input.Substring(0, maxLength);
        }
    }

    public static class HeaderFinder
    {
        public static int FindHeaderLine(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].TrimStart().StartsWith("*Action(", StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }
    }
}
