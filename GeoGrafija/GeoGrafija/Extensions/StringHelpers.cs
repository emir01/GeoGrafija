using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoGrafija.Extensions
{
    public static class StringHelpers
    {
        public static List<string> SplitStringToList(string String, char delimiter)
        {
            List<string> list;

            try
            {
                list = String.Split(delimiter).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
            
            return list;
        }

        public static string ShortenString(this string input, int desiredLength, char finalizer)
        {
            if (input.Length > desiredLength)
            {
                var substring = input.Substring(0, desiredLength - 3);
                var finString = finalizer.ToString();
                var finalized = String.Concat(substring, finString, finString, finString);
                return finalized;
            }
            else
            {
                return input;
            }
        }
    }
}