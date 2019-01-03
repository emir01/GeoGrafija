using System.Linq;

namespace GeoGrafija.Extensions
{
    public static class DisplayHelper
    {
        public static string ShortenString(string stringToShorten, int maxCharacters)
        {
            if(stringToShorten.Count() <= maxCharacters)
            {
                return stringToShorten;
            }
            else
            {
                string shorterString = stringToShorten.Substring(0, maxCharacters - 3);
                string trail = "...";
                shorterString += trail;
                return shorterString;
            }
        }
    }
}