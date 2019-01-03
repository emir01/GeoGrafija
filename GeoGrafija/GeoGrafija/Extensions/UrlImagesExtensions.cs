using System.Web.Mvc;

namespace GeoGrafija.Helpers
{
    public static class UrlImagesExtensions
    {
        #region Images
        
        public static string MarkerImages(this UrlHelper helper,string iconString)
        {
            return helper.Content("~/Content/MarkerIcons/"+iconString);
        }

        public static string Image(this UrlHelper helper, string iconString)
        {
            return helper.Content("~/Content/images/" + iconString);
        }

        public static string  RankImages(this UrlHelper helper, string rankImage)
        {
            return helper.Content("~/Content/RankImages/" + rankImage);
        }

        public static string QuizImages(this UrlHelper helper, string quizImage)
        {
            return helper.Content("~/Content/QuizImages/" + quizImage);
        }

        public static string AboutImages(this UrlHelper helper, string aboutImage)
        {
            return helper.Content("~/Content/images/AboutImages/" + aboutImage);
        }

        #endregion
    }
}