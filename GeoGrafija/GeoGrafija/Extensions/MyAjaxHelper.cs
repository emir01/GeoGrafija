using System;

namespace GeoGrafija.Extensions
{
    public static class MyAjaxHelper
    {
        public static void SlowRequest()
        {
            for (int i = 0; i < 10000000; i++)
            {
                var test = Math.Sqrt(i)/Math.Sin(i);
                var again = Math.Pow(test, 17);

                for (int j = 0; i < 50000000; i++)
                {
                    var testj = Math.Sqrt(j) / Math.Sin(j);
                    var againj = Math.Pow(testj, 17);
                }
            }
        }
    }
}