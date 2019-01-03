using System.Web.Mvc;

namespace GeoGrafija.Helpers
{
    public static class UrlScriptsExtensions
    {
        #region Scripts

        public static string Scripts(this UrlHelper helper, string scriptName)
        {
            return helper.Content("~/Scripts/" + scriptName);
        }

        public static string PageScript(this UrlHelper helper, string scriptName)
        {
            return helper.Scripts("Page Scripts/" + scriptName);
        }
        
        public static string MiscScript(this UrlHelper helper, string scriptName)
        {
            return helper.Scripts("Misc/" + scriptName);
        }

        public static string MapScripts(this UrlHelper helper, string scriptName)
        {
            return helper.Scripts("Maps/" + scriptName);
        }

        public static string PluginScripts(this UrlHelper helper, string scriptName)
        {
            return helper.Scripts("Plugins/" + scriptName);
        }

        public static string GoogleMapsApi(this UrlHelper helper)
        {
            return "http://maps.google.com/maps/api/js?sensor=false&language=en";
        }

        public static string CommonScripts(this UrlHelper helper, string scriptName)
        {
            return helper.Scripts("Common/" + scriptName);
        }

        public static string GeoDisplay(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoDisplay.js");
        }

        public static string GeoClassroom(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoClassroom.js");
        }

        public static string GeoSingleDisplay(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoSingleDisplay.js");
        }

        public static string GeoSearch(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoSearch.js");
        }

        public static string GeoAjax(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoAjax.js");
        }

        public static string GeoUi(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoUI.js");
        }

        public static string GeoUtility(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoUtility.js");
        }

        public static string GeoMain(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoMain.js");
        }

        public static string GeoJqueryUiWrappers(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoJQueryUiWrappers.js");
        }

        public static string GeoDialogManager(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoDialogManager.js");
        }

        public static string GeoResources(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoResources.js");
        }

        public static string GeoResourcesCreate(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoResourcesCreate.js");
        }

        public static string GeoLearning(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoLearning.js");
        }

        public static string GeoExploration(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoExploration.js");
        }

        public static string GeoGeneralResourcesCreate(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoGeneralResourceCreate.js");
        }

        public static string GeoMarkup(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoMarkup.js");
        }

        public static string GeoDialogFactory(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoDialogFactory.js");
        }

        public static string GeoQuizesCreate(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoQuizesCreate.js");
        }

        public static string GeoQuizesTake(this UrlHelper helper)
        {
            return CommonScripts(helper, "GeoQuizesTake.js");
        }

        public static string ColorPickerScript(this UrlHelper helper)
        {
            #if DEBUG
                return PluginScripts(helper, "colorpicker.js");
            #else
                return PluginScripts(helper, "colorpicker.min.js");
            #endif
        }

        public static string RichEditorScript(this UrlHelper helper)
        {
            return PluginScripts(helper, "jquery.wysiwyg.js");
        }

        public static string JqueryTools(this UrlHelper helper)
        {
            return PluginScripts(helper, "jquery.tools.min.js");
        }

        public static string DropKickScript(this UrlHelper helper)
        {
            #if DEBUG
                return PluginScripts(helper, "jquery.dropkick-1.0.0.js");
            #else
                return PluginScripts(helper, "jquery.dropkick-1.0.0.min.js");
            #endif
        }

        public static string DataTablesScript(this UrlHelper helper)
        {
            return PluginScripts(helper, "jquery.dataTables.min.js");
        }

        public static string LoadMaskScript(this UrlHelper helper)
        {
            return PluginScripts(helper, "jquery.loadmask.min.js");
        }

        public static string JqueryCookiePlugin(this UrlHelper helper)
        {
            return PluginScripts(helper, "jquery.cookie.js");
        }

        public static string ScrollbarScript(this UrlHelper helper)
        {
            return PluginScripts(helper, "jquery.tinyscrollbar.min.js");
        }

        public static string JSON(this UrlHelper helper)
        {
            #if (DEBUG)
                return Scripts(helper, "json2.js");
            #else
                return Scripts(helper, "json2.min.js");
            #endif
        }

        #region ProfileScripts

        public static string ProfileScripts(this UrlHelper helper, string scriptName)
        {
            return Scripts(helper, "/Profiles/" + scriptName);
        }

        public static string ProfileGeneralScript(this UrlHelper helper)
        {
            return ProfileScripts(helper, "ProfileGeneral.js");
        }

        public static string ProfileAdminScript(this UrlHelper helper)
        {
            return ProfileScripts(helper, "ProfileAdmin.js");
        }

        public static string ProfileTeacherScript(this UrlHelper helper)
        {
            return ProfileScripts(helper, "ProfileTeacher.js");
        }

        public static string ProfileStudentScript(this UrlHelper helper)
        {
            return ProfileScripts(helper, "ProfileStudent.js");
        }

        #endregion

        #endregion
    }
}