using System.Web.Mvc;

namespace GeoGrafija.Helpers
{
    public static class UrlCssExtensions
    {
        #region CSS

        public static string Css(this UrlHelper helper, string stylesheet)
        {
            return helper.Content("~/Content/Css/" + stylesheet);
        }

        public static string CsssMainSite(this UrlHelper helper)
        {
            return helper.Css("Site.css");
        }

        public static string CssLocations(this UrlHelper helper)
        {
            return helper.Css("Locations.css");
        }

        public static string CssJqueryUi(this UrlHelper helper)
        {
            return helper.Css("jQueryUi/jquery-ui-1.8.17.custom.css");
        }

        public static string CssColorPicker(this UrlHelper helper)
        {
            return helper.Css("ColorPicker/colorpicker.css");
        }

        public static string CssUtility(this UrlHelper helper)
        {
            return helper.Css("Utility.css");
        }

        public static string CssResources(this UrlHelper helper)
        {
            return helper.Css("Resources.css");
        }

        public static string CssQuizes(this UrlHelper helper)
        {
            return helper.Css("Quizes.css");
        }

        public static string CssClassroom(this UrlHelper helper)
        {
            return helper.Css("Classroom.css");
        }

        public static string CssForms(this UrlHelper helper)
        {
            return helper.Css("Forms.css");
        }

        public static string CssRichEditor(this UrlHelper helper)
        {
            return helper.Css("jwysiwyg/jquery.wysiwyg.css");
        }

        public static string CssProfiles(this UrlHelper helper)
        {
            return helper.Css("Profiles.css");
        }

        public static  string CssDropKick(this UrlHelper helper)
        {
            return helper.Css("DropKick/dropkick.css");
        }

        #region Data Tables

        public static string CssDataTables(this UrlHelper helper)
        {
            return helper.Css("DataTables/demo_table.css");
        }

        public static string CssDataTablesJui(this UrlHelper helper)
        {
            return helper.Css("DataTables/table_jui.css");
        }

        public static string CssLoadMask(this UrlHelper helper)
        {
            return helper.Css("Mask/jquery.loadmask.css");
        }

        public static string CssScrollbars(this UrlHelper helper)
        {
            return helper.Css("Scrollbar/scroll.css");
        }

        public static string CssLearning(this UrlHelper helper)
        {
            return helper.Css("Learning.css");
        }

        public static string CssExploration(this UrlHelper helper)
        {
            return helper.Css("Exploration.css");
        }

        #endregion

        #endregion
    }
}